using AutoMapper;
using by.Reba.Business.Models;
using by.Reba.Business.ServicesImplementations.ArticleRecievers;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleInitializerService : IArticleInitializerService
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleInitializerService(
            ICategoryService categoryService,
            IUnitOfWork unitOfWork,
            IMapper mapper) =>

            (_categoryService, _unitOfWork, _mapper) = (categoryService, unitOfWork, mapper);

        public async Task<int> RemoveEmptyArticles()
        {
            var articles = await _unitOfWork.Articles
                .Get()
                .Where(a => a.HtmlContent != null && a.HtmlContent.Equals(string.Empty))
                .ToArrayAsync();

            foreach (var item in articles)
            {
                _unitOfWork.Articles.Remove(item);
            }

            return await _unitOfWork.Commit();
        }

        public async Task<int> CreateArticlesFromExternalSourcesAsync()
        {
            var sources = await _unitOfWork.Sources
                .Get()
                .AsNoTracking()
                .ToListAsync();

            Parallel.ForEach(sources, (source) => CreateArticlesFromSpecificSourceAsync(source.Id, source.Type, source.RssUrl).Wait());
            return await _unitOfWork.Commit();
        }

        public async Task AddTextToArticlesAsync(int articlesCount)
        {
            var articlesWithoutText = await _unitOfWork.Articles
                .FindBy(a => string.IsNullOrEmpty(a.HtmlContent), a => a.Source)
                .Take(articlesCount)
                .ToListAsync();

            if (articlesWithoutText is not null)
            {
                foreach (var article in articlesWithoutText)
                {
                    var text = GetTextForSpecificArticleAsync(article.Source.Type, article.SourceUrl);
                    article.HtmlContent = text;
                }

                await _unitOfWork.Commit();
            }
        }

        public async Task AddPositivityToArticlesAsync(int articlesCount)
        {
            var articlesId = await _unitOfWork.Articles
                .Get()
                .Where(a => a.RatingId == null && !string.IsNullOrEmpty(a.HtmlContent))
                .Select(a => a.Id)
                .Take(articlesCount)
                .ToArrayAsync();

            var affinData = await LoadAfinnData();

            var positivities = await _unitOfWork.Positivities
                .Get()
                .Select(p => new { Id = p.Id, Value = p.Value })
                .ToArrayAsync();

            var positivitiesTuples = positivities
                .Select(p => (Id: p.Id, Value: p.Value))
                .ToArray();

            foreach (var id in articlesId)
            {
                var result = await RateArticleAsync(id, affinData, positivitiesTuples);
            }
        }

        private async Task<int> RateArticleAsync(Guid id, Dictionary<string, int?> afinnData, (Guid Id, double Value)[] positivities)
        {
            try
            {
                var article = await _unitOfWork.Articles.Get().FirstOrDefaultAsync(a => a.Id.Equals(id));

                if (article is null)
                {
                    throw new ArgumentException($"Article with id = {id} doesn't exist", nameof(id));
                }

                using (var client = new HttpClient())
                {
                    var httpRequest = new HttpRequestMessage(HttpMethod.Post,
                        new Uri(@"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=736af07339c6209d5b43d9254ff8f67407be1c73"));
                    httpRequest.Headers.Add("Accept", "application/json");

                    var articleText = ExtractText(article.HtmlContent);
                    httpRequest.Content = JsonContent.Create(new[] { new TextRequestModel() { Text = articleText } });

                    var response = await client.SendAsync(httpRequest);
                    var responseStr = await response.Content.ReadAsStreamAsync();

                    using (var sr = new StreamReader(responseStr))
                    {
                        var isprassData = await sr.ReadToEndAsync();
                        var isprassResponse = JsonConvert.DeserializeObject<IsprassResponseObject[]>(isprassData);

                        var words = isprassResponse.First().Annotations.Lemma.Select(l => l.Value).ToArray();

                        var rating = words
                            .Select(w => afinnData.Where(a => a.Key.Contains(w, StringComparison.OrdinalIgnoreCase)).Select(a => a.Value).FirstOrDefault())
                            .Where(n => n != null)
                            .Average();

                        var positivityId = positivities
                            .OrderBy(p => Math.Abs((int)rating - p.Value))
                            .Select(p => p.Id)
                            .First();

                        var patchList = new List<PatchModel>()
                        {
                            new PatchModel()
                            {
                                PropertyName = nameof(article.RatingId),
                                PropertyValue = positivityId
                            }
                        };

                        await _unitOfWork.Articles.PatchAsync(id, patchList);
                        return await _unitOfWork.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task<Dictionary<string, int?>> LoadAfinnData()
        {
            var filePath = @"F:\GoodNewsAggregator\by.Reba\by.Reba.Business\afinn-ru.json";

            using (var sr = new StreamReader(filePath))
            {
                var data = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<Dictionary<string, int?>>(data);
            }
        }

        private static string ExtractText(string html)
        {
            if (html == null)
            {
                return string.Empty;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc.DocumentNode.InnerText;
        }

        private async Task CreateArticlesFromSpecificSourceAsync(Guid sourceId, ArticleSource sourceType, string? sourceRssUrl)
        {
            if (!string.IsNullOrEmpty(sourceRssUrl))
            {
                var articles = new List<CreateArticleFromRssDTO>();

                using (var reader = XmlReader.Create(sourceRssUrl))
                {
                    var receiver = GetReceiver(sourceType);

                    var feed = SyndicationFeed.Load(reader);

                    var categories = await _unitOfWork.Categories
                        .Get()
                        .AsNoTracking()
                        .Select(c => _mapper.Map<CategoryDTO>(c))
                        .ToListAsync();

                    foreach (var item in feed.Items.Distinct())
                    {
                        var categoryTitle = receiver.GetCategoryTitle(item);
                        var categoryDTO = categories.FirstOrDefault(c => c.Title.Equals(categoryTitle, StringComparison.OrdinalIgnoreCase));
                        if (categoryDTO is null)
                        {
                            categoryDTO = new CategoryDTO()
                            {
                                Id = Guid.NewGuid(),
                                Title = categoryTitle
                            };

                            await _categoryService.CreateAsync(categoryDTO);
                            categories.Add(categoryDTO);
                        }

                        var posterUrl = receiver.GetPosterUrl(item);

                        var article = new CreateArticleFromRssDTO()
                        {
                            Id = Guid.NewGuid(),
                            Title = item.Title.Text,
                            PublicationDate = item.PublishDate.UtcDateTime,
                            PosterUrl = posterUrl,
                            SourceUrl = item.Id,
                            SourceId = sourceId,
                            CategoryId = categoryDTO.Id,
                        };

                        articles.Add(article);
                    }

                    var oldArticlesUrls = await _unitOfWork.Articles
                        .Get()
                        .AsNoTracking()
                        .Select(article => article.SourceUrl)
                        .ToArrayAsync();

                    var newArticles = articles
                        .Where(dto => !oldArticlesUrls.Contains(dto.SourceUrl))
                        .Select(dto => _mapper.Map<T_Article>(dto))
                        .ToArray();

                    await _unitOfWork.Articles.AddRangeAsync(newArticles);
                }
            }
        }

        private static IArticleReceiver GetReceiver(ArticleSource sourceType) => sourceType switch
        {
            ArticleSource.Onliner => new Onliner(),
            ArticleSource.Devby => new Devby(),
            _ => throw new NotImplementedException("Trying to access a non-existing source"),
        };

        private string GetTextForSpecificArticleAsync(ArticleSource sourceType, string sourceUrl)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(sourceUrl);

            var receiver = GetReceiver(sourceType);
            var nodes = receiver.GetNodes(htmlDoc);

            return GetArticleTextWithStylization(nodes!);
        }

        private string GetArticleTextWithStylization(HtmlNode[] nodes)
        {
            if (nodes is null || !nodes.Any())
            {
                return string.Empty;
            }

            foreach (var node in nodes)
            {
                StylizateInnerNodes(node);
                node.AddClass("m-0 py-2");
            }

            var text = nodes
                .Select(node => node.OuterHtml)
                .Aggregate((i, j) => i + Environment.NewLine + j);

            text = Regex.Replace(text, @"(?<=\n)\n", "", RegexOptions.Compiled);

            return text ?? string.Empty;
        }

        private HtmlNode StylizateInnerNodes(HtmlNode node)
        {
            node.RemoveClass();
            node.Attributes["width"]?.Remove();
            node.Attributes["height"]?.Remove();
            node.Attributes["style"]?.Remove();

            switch (node.Name.ToLower())
            {
                case "iframe":
                    node.AddClass("mw-100 col-12 offset-0 col-lg-6 offset-lg-3");
                    node.SetAttributeValue("height", "500px");
                    break;

                case "img":
                    node.AddClass("mw-100 col-12 offset-0 col-lg-6 offset-lg-3");
                    break;

                case "table":
                    node.AddClass("table-responsive table table-hover text-center table-bordered");
                    break;
            }

            foreach (var item in node.ChildNodes)
            {
                StylizateInnerNodes(item);
            }

            return node;
        }

        public async Task Test()
        {
            var positivities = await _unitOfWork.Positivities
                .Get()
                .Select(p => new { Id = p.Id, Value = p.Value })
                .ToArrayAsync();

            var positivitiesTuples = positivities
                .Select(p => (Id: p.Id, Value: p.Value))
                .ToArray();

            await RateArticleAsync(new Guid("00CDFACB-298E-44E5-826C-000491BEBDAF"), await LoadAfinnData(), positivitiesTuples);
        }
    }
}
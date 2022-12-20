using AutoMapper;
using by.Reba.Business.Models;
using by.Reba.Business.ServicesImplementations.ArticleRecievers;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Data.Abstractions;
using by.Reba.Data.CQS.Commands;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ArticleInitializerService(
            ICategoryService categoryService,
            IMapper mapper,
            IConfiguration configuration,
            IMediator mediator) =>

            (_categoryService, _mapper, _configuration, _mediator) = 
            (categoryService, mapper, configuration, mediator);

        public async Task RemoveEmptyArticles()
        {
            await _mediator.Send(new RemoveEmptyArticlesCommand());
        }

        public async Task CreateArticlesFromExternalSourcesAsync()
        {
            var sources = await _mediator.Send(new GetSourcesQuery());

            foreach (var source in sources)
            {
                await CreateArticlesFromSpecificSourceAsync(source.Id, source.Type, source.RssUrl);
            }
        }

        public async Task AddTextToArticlesAsync(int articlesCount)
        {
            var articlesWithoutText = await _mediator.Send(new GetArticlesWithoutTextQuery() 
            { 
                ArticlesCount = articlesCount 
            });

            if (articlesWithoutText is not null)
            {
                foreach (var article in articlesWithoutText)
                {
                    var text = GetTextForSpecificArticleAsync(article.Source.Type, article.SourceUrl);
                    article.HtmlContent = text;
                }

                await _mediator.Send(new SaveChangesCommand());
            }
        }

        public async Task AddPositivityToArticlesAsync(int articlesCount)
        {
            var loadAfinnDataTask = LoadAfinnData();

            var articlesId = await _mediator.Send(new GetArticlesIdWithoutPositivityQuery()
            {
                ArticlesCount = articlesCount
            });

            var positivitiesTuples = await _mediator.Send(new GetPositivitiesIdAndValueQuery());
            var affinData = await loadAfinnDataTask;

            foreach (var id in articlesId)
            {
                await RateArticleAsync(id, affinData, positivitiesTuples);
            }
        }

        private async Task RateArticleAsync(Guid id, Dictionary<string, int?> afinnData, IEnumerable<(Guid Id, float Value)> positivities)
        {
            var article = await _mediator.Send(new GetTrackedArticleByIdQuery() { Id = id });

            if (article is null)
            {
                throw new ArgumentException($"Article with id = {id} doesn't exist", nameof(id));
            }

            using (var client = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_configuration["Ispras:Url"]));
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
                        .Where(w => !string.IsNullOrEmpty(w))
                        .Select(w => afinnData.FirstOrDefault(a => a.Key.Equals(w, StringComparison.OrdinalIgnoreCase)).Value)
                        .Average() ?? 0;

                    var positivityId = positivities
                        .OrderBy(p => Math.Abs(rating - p.Value))
                        .Select(p => p.Id)
                        .First();

                    var patchList = new List<PatchModel>()
                    {
                        new PatchModel()
                        {
                            PropertyName = nameof(article.PositivityId),
                            PropertyValue = positivityId
                        }
                    };

                    await _mediator.Send(new PatchArticleCommand()
                    {
                        Id = id,
                        PatchData = patchList
                    });
                }
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

            var text = doc.DocumentNode.InnerText;
            text = text.Replace("\n", " ");
            text = text.Replace("\r", " ");
            text = text.Replace("&nbsp;", " ", StringComparison.OrdinalIgnoreCase);

            return text;
        }

        private async Task CreateArticlesFromSpecificSourceAsync(Guid sourceId, ArticleSource sourceType, string? sourceRssUrl)
        {
            if (!string.IsNullOrEmpty(sourceRssUrl))
            {
                var articles = new List<CreateArticleFromRssDTO>();

                using (var reader = XmlReader.Create(sourceRssUrl))
                {
                    var articleSource = GetArticleSource(sourceType);
                    var feed = SyndicationFeed.Load(reader);

                    var categories = (await _mediator.Send(new GetNoTrackedCategoriesQuery()))
                        .Select(c => _mapper.Map<CategoryDTO>(c))
                        .ToList();

                    foreach (var item in feed.Items.Distinct())
                    {
                        var categoryTitle = articleSource.GetCategoryTitle(item);
                        var categoryDTO = categories.FirstOrDefault(c => c.Title.Equals(categoryTitle, StringComparison.OrdinalIgnoreCase));

                        if (categoryDTO is null)
                        {
                            categoryDTO = new CategoryDTO()
                            {
                                Id = Guid.NewGuid(),
                                Title = categoryTitle
                            };

                            var result = _categoryService.CreateAsync(categoryDTO);
                            categories.Add(categoryDTO);
                        }

                        var posterUrl = articleSource.GetPosterUrl(item);

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

                    var oldArticlesUrls = await _mediator.Send(new GetArticlesSourceUrlQuery());

                    var newArticles = articles
                        .Where(dto => !oldArticlesUrls.Contains(dto.SourceUrl))
                        .Select(dto => _mapper.Map<T_Article>(dto))
                        .ToArray();

                    await _mediator.Send(new AddArticlesRangeCommand() { Articles = newArticles });
                }
            }
        }

        private static IArticleSource GetArticleSource(ArticleSource sourceType) => sourceType switch
        {
            ArticleSource.Onliner => new Onliner(),
            ArticleSource.Devby => new Devby(),
            _ => throw new NotImplementedException("Trying to access a non-existing source"),
        };

        private string GetTextForSpecificArticleAsync(ArticleSource sourceType, string sourceUrl)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(sourceUrl);

            var receiver = GetArticleSource(sourceType);
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
    }
}
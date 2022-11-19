﻿using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace by.Reba.Business.ServicesImplementations
{
    public class ArticleInitializerService : IArticleInitializerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ArticleInitializerService
            (IUnitOfWork unitOfWork,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<int> CreateArticlesFromExternalSourcesAsync()
        {
            var sources = await _unitOfWork.Sources
                .Get()
                .AsNoTracking()
                .ToListAsync();

            Parallel.ForEach(sources, (source) => CreateArticlesFromSpecificSourceAsync(source.Id, source.SourceType, source.RssUrl).Wait());
            return await _unitOfWork.Commit();
        }

        public async Task AddTextToArticlesAsync()
        {
            var articlesWithoutText = await _unitOfWork.Articles
                .FindBy(a => a.Text == null, a => a.Source)
                .Take(30)
                .ToListAsync();

            if (articlesWithoutText is not null)
            {
                foreach (var article in articlesWithoutText)
                {
                    var text = GetTextForSpecificArticleAsync(article.Source.SourceType, article.SourceUrl);
                    article.Text = text;
                }

                await _unitOfWork.Commit();
            }
        }

        private async Task AddTextToArticlesWithEmptyText()
        {
            var articlesWithEmptyString = await _unitOfWork.Articles
                .FindBy(a => a.Text.Equals(""), a => a.Source)
                .Take(20)
                .ToListAsync();

            if (articlesWithEmptyString is not null)
            {
                foreach (var article in articlesWithEmptyString)
                {
                    var text = GetTextForSpecificArticleAsync(article.Source.SourceType, article.SourceUrl);
                    article.Text = text;
                }

                await _unitOfWork.Commit();
            }
        }

        private async Task CreateArticlesFromSpecificSourceAsync(Guid sourceId, SourceType sourceType, string? sourceRssUrl)
        {
            if (!string.IsNullOrEmpty(sourceRssUrl))
            {
                var articles = new List<CreateArticleFromRssDTO>();

                using (var reader = XmlReader.Create(sourceRssUrl))
                {
                    var feed = SyndicationFeed.Load(reader);

                    var categories = await _unitOfWork.Categories
                        .Get()
                        .AsNoTracking()
                        .Select(c => _mapper.Map<CategoryDTO>(c))
                        .ToListAsync();

                    foreach (var item in feed.Items.Distinct())
                    {
                        var categoryTitle = GetCategoryTitle(sourceType, item);
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

                        var posterUrl = GetPosterUrl(sourceType, item);

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

        private string GetCategoryTitle(SourceType sourceType, SyndicationItem item) => sourceType switch
        {
            SourceType.Onliner => item?.Categories?.FirstOrDefault()?.Name ?? "Общее",
            SourceType.Dev => item?.Categories?.FirstOrDefault()?.Name ?? "Информационные технологии",
            _ => "Общее",
        };

        private string GetPosterUrl(SourceType sourceType, SyndicationItem item) => sourceType switch
        {
            SourceType.Onliner => Regex.Match(item?.Summary?.Text, @"(?<=src="")(\S+)?(?="")", RegexOptions.Compiled).Value ?? "none",
            SourceType.Dev => item?.Links[1].GetAbsoluteUri().AbsoluteUri ?? "none",
            _ => "none",
        };

        private string GetTextForSpecificArticleAsync(SourceType sourceType, string sourceUrl)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(sourceUrl);

            var nodes = sourceType switch
            {
                SourceType.Onliner => GetNodesFrom_Onliner(htmlDoc),
                SourceType.Dev => GetNodesFrom_Dev(htmlDoc),
                _ => null,
            };

            return GetArticleTextWithStylization(nodes);
        }

        private HtmlNode[]? GetNodesFrom_Onliner(HtmlDocument htmlDoc)
        {
            var mainNode = htmlDoc.DocumentNode.Descendants()
                    .Where(n => n.HasClass("news-text"))
                    .FirstOrDefault();

            return mainNode is null || !mainNode.ChildNodes.Any()
                ? null
                : mainNode.ChildNodes
                .Where(node => !node.Name.Equals("script")
                                && char.IsLetter(node.Name[0])
                                && !string.IsNullOrEmpty(node.InnerHtml)
                                && node.Attributes["style"] is null
                                && !node.HasClass("news-reference")
                                && !node.HasClass("news-widget")
                                && !node.HasClass("news-incut")
                                && !node.HasClass("news-header")
                                && !node.HasClass("news-vote")
                                && !node.HasClass("news-media_3by2")
                                && !(node.HasClass("news-media") && node.InnerHtml.Contains("href", StringComparison.Ordinal)))
                .ToArray();
        }

        private HtmlNode[] GetNodesFrom_Dev(HtmlDocument htmlDoc)
        {
            var mainNode = htmlDoc.DocumentNode.Descendants()
                    .Where(n => Regex.IsMatch(n.GetAttributeValue("class", ""), @"\s*article__container\s*", RegexOptions.Compiled)
                                && n.ParentNode.HasClass("article__body"))
                    .FirstOrDefault();

            return mainNode is null || !mainNode.ChildNodes.Any()
                ? Array.Empty<HtmlNode>()
                : mainNode.ChildNodes
                .Where(node => !node.Name.Equals("script")
                                && char.IsLetter(node.Name[0])
                                && !string.IsNullOrEmpty(node.InnerHtml)
                                && node.Attributes["style"] is null
                                && !Regex.IsMatch(node.GetAttributeValue("class", ""), @"\s*global-incut\s*")
                                && !Regex.IsMatch(node.GetAttributeValue("class", ""), @"\s*incut\s*")
                                )
                .ToArray();
        }

        private string GetArticleTextWithStylization(HtmlNode[] nodes)
        {
            if (nodes is null)
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

            if (node.Name.ToLower().Equals("Iframe"))
            {
                node.AddClass("mw-100 col-12 offset-0 col-lg-10 offset-lg-1");
                node.SetAttributeValue("heigth", "500px");
            }

            if (node.Name.ToLower().Equals("img"))
            {
                node.AddClass("mw-100 col-12 offset-0 col-lg-10 offset-lg-1");
            }

            foreach (var item in node.ChildNodes)
            {
                StylizateInnerNodes(item);
            }

            return node;
        }
    }
}
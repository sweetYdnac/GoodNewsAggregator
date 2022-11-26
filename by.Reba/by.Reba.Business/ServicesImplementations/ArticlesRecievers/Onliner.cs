using by.Reba.Core.Abstractions;
using HtmlAgilityPack;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

namespace by.Reba.Business.ServicesImplementations.ArticleExternalSources
{
    public class Onliner : IArticleReciever
    {
        public HtmlNode[]? GetNodes(HtmlDocument htmlDoc)
        {
            var mainNode = htmlDoc.DocumentNode.Descendants()
                    .Where(n => n.HasClass("news-text"))
                    .FirstOrDefault();

            return mainNode is null || !mainNode.ChildNodes.Any()
                ? null
                : mainNode.ChildNodes
                .Where(node => char.IsLetter(node.Name[0])
                                && !string.IsNullOrEmpty(node.InnerHtml)
                                && node.Attributes["style"] is null
                                && !node.HasClass("news-reference")
                                && !node.HasClass("news-widget")
                                && !node.HasClass("news-incut")
                                && !node.HasClass("news-header")
                                && !node.HasClass("news-vote")
                                && !node.HasClass("news-media_3by2")
                                && !node.InnerHtml.ToLower().Contains("https://catalog.onliner.by", StringComparison.Ordinal))
                .ToArray();
        }
        public string GetCategoryTitle(SyndicationItem item)
        {
            return item?.Categories?.FirstOrDefault()?.Name ?? "Общее";
        }

        public string GetPosterUrl(SyndicationItem item)
        {
            return item.Summary.Text is null
                ? "none"
                : Regex.Match(item?.Summary?.Text, @"(?<=src="")(\S+)?(?="")", RegexOptions.Compiled).Value ?? "none";
        }
    }
}

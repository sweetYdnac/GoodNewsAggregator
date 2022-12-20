using by.Reba.Core.Abstractions;
using HtmlAgilityPack;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

namespace by.Reba.Business.ServicesImplementations.ArticleRecievers
{
    public class Onliner : IArticleSource
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
                                && (node.Name.Equals("iframe", StringComparison.OrdinalIgnoreCase) || node.Attributes["style"] is null)
                                && !node.HasClass("news-reference")
                                && !node.HasClass("news-widget")
                                && !node.HasClass("news-incut")
                                && !node.HasClass("news-header")
                                && !node.HasClass("news-vote")
                                && !node.HasClass("news-media_3by2")
                                && !node.OuterHtml.Contains("script", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("people.onliner.by", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("catalog.onliner.by", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("auto.onliner.by", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("tech.onliner.by", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("realt.onliner.by", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("money.onliner.by", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("t.elegram.ru", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("t.me/autoonliner", StringComparison.OrdinalIgnoreCase)
                                && !node.OuterHtml.Contains("b2bblog.onliner.by", StringComparison.OrdinalIgnoreCase))
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

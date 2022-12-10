using by.Reba.Core.Abstractions;
using HtmlAgilityPack;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

namespace by.Reba.Business.ServicesImplementations.ArticleRecievers
{
    public class Devby : IArticleReceiver
    {
        public HtmlNode[]? GetNodes(HtmlDocument htmlDoc)
        {
            var mainNode = htmlDoc.DocumentNode.Descendants()
                    .Where(n => n.HasClass("article__container") && n.ParentNode.HasClass("article__body"))
                    .FirstOrDefault();

            mainNode ??= htmlDoc.DocumentNode.Descendants()
                    .Where(n => n.HasClass("article__body"))
                    .FirstOrDefault();

            return mainNode is null || !mainNode.ChildNodes.Any()
                ? Array.Empty<HtmlNode>()
                : mainNode.ChildNodes
                .Where(node => char.IsLetter(node.Name[0])
                                && !string.IsNullOrEmpty(node.InnerHtml)
                                && node.Attributes["style"] is null
                                && !Regex.IsMatch(node.GetAttributeValue("class", ""), @"\s*global-incut\s*")
                                && !Regex.IsMatch(node.GetAttributeValue("class", ""), @"\s*incut\s*")
                                && !Regex.IsMatch(node.GetAttributeValue("class", ""), @"\s*article-aside\s*")
                                )
                .ToArray();
        }
        public string GetCategoryTitle(SyndicationItem item)
        {
            return item?.Categories?.FirstOrDefault()?.Name ?? "Информационные технологии";
        }

        public string GetPosterUrl(SyndicationItem item)
        {
            return item?.Links[1].GetAbsoluteUri().AbsoluteUri ?? "none";
        }
    }
}

using HtmlAgilityPack;
using System.ServiceModel.Syndication;

namespace by.Reba.Core.Abstractions
{
    public interface IArticleSource
    {
        HtmlNode[]? GetNodes(HtmlDocument htmlDoc);
        string GetCategoryTitle(SyndicationItem item);
        string GetPosterUrl(SyndicationItem item);
    }
}

using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;
using by.Reba.WebAPI.Models.Requests.QueryStringParameters.Pagination;

namespace by.Reba.WebAPI.Models.Requests.ArticlesPreview
{
    public class GetArticlesRequestModel : PaginationParameters
    {
        public ArticleFilterDTO Filter { get; set; }
        public ArticleSort SortOrder { get; set; } = ArticleSort.PublicationDate;
        public string SearchString { get; set; } = string.Empty;
    }
}

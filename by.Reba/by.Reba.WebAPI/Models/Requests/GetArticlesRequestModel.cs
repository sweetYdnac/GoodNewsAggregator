using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;

namespace by.Reba.WebAPI.Models.Requests
{
    public class GetArticlesRequestModel
    {
        public ArticleFilterDTO Filter { get; set; }
        public ArticleSort SortOrder { get; set; }
        public string SearchString { get; set; }

        public int Page = 1;
    }
}

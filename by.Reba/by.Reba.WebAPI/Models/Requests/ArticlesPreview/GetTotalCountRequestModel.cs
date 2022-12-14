using by.Reba.Core.DataTransferObjects.Article;

namespace by.Reba.WebAPI.Models.Requests.ArticlesPreview
{
    public class GetTotalCountRequestModel
    {
        public ArticleFilterDTO Filter { get; set; }
        public string SearchString { get; set; } = string.Empty;
    }
}

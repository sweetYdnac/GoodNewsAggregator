using by.Reba.Core.DataTransferObjects.Article;

namespace by.Reba.Application.Models.Article
{
    public class ArticlesGridVM
    {
        public IEnumerable<ArticlePreviewDTO> Articles { get; set; }
        public ArticleFilterDataVM FilterData { get; set; }
        public string SearchString { get; set; }
    }
}

using by.Reba.Core.DataTransferObjects;

namespace by.Reba.AdminPanel.Models.Article
{
    public class ArticlesListVM
    {
        public IEnumerable<ArticlePreviewDTO> Articles { get; set; }
    }
}

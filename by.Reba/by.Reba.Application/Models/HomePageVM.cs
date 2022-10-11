using by.Reba.Application.Models.Article;
using by.Reba.Core.DataTransferObjects.Article;

namespace by.Reba.Application.Models
{
    public class HomePageVM
    {
        public IEnumerable<ArticlePreviewDTO> Articles { get; set; }
        public ArticleFilterDataVM FilterData { get; set; }
        public string SearchString { get; set; }

        public bool IsAdmin = false;
    }
}

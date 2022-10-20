using by.Reba.Application.Models.Article;
using by.Reba.Core;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;

namespace by.Reba.Application.Models
{
    public class HomePageVM
    {
        public IEnumerable<ArticlePreviewDTO>? Articles { get; set; }
        public ArticleFilterDataVM? FilterData { get; set; }
        public string? SearchString { get; set; }
        public ArticleSort? SortOrder { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public bool IsAdmin = false;
    }
}

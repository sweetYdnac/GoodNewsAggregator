using by.Reba.Core.DataTransferObjects.Article;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace by.Reba.Application.Models.Article
{
    public class ArticleFilterDataVM
    {
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> PositivityRatings { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Sources { get; set; } = Enumerable.Empty<SelectListItem>();
        public ArticleFilterDTO CurrentFilter { get; set; }
    }
}

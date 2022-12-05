using by.Reba.Core;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.Core.SortTypes;

namespace by.Reba.WebAPI.Models.Responces
{
    public class GetArticlesResponseModel
    {
        public IEnumerable<ArticlePreviewDTO> Articles { get; set; } = Enumerable.Empty<ArticlePreviewDTO>();

        public string SearchString { get; set; }
        public ArticleSort SortOrder { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; } = Enumerable.Empty<CategoryDTO>();
        public IEnumerable<PositivityDTO> Positivities { get; set; } = Enumerable.Empty<PositivityDTO>();
        public IEnumerable<SourceDTO> Sources { get; set; } = Enumerable.Empty<SourceDTO>();
        public ArticleFilterDTO CurrentFilter { get; set; }
    }
}

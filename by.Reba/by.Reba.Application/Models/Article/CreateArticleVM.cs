using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Application.Models.Article
{
    public class CreateArticleVM
    {
        public IEnumerable<CategoryDTO> Categories { get; set; } = Enumerable.Empty<CategoryDTO>();
        public IEnumerable<SourceDTO> Sources { get; set; } = Enumerable.Empty<SourceDTO>();
        public IEnumerable<PositivityRatingDTO> Ratings { get; set; } = Enumerable.Empty<PositivityRatingDTO>();
    }
}

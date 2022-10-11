using by.Reba.Core.DataTransferObjects.Category;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.Core.DataTransferObjects.Source;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Article
{
    public class CreateArticleVM
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(512)]
        [DataType(DataType.ImageUrl)]
        public string PosterUrl { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid SourceId { get; set; }

        [Required]
        public Guid RatingId { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; } = Enumerable.Empty<CategoryDTO>();
        public IEnumerable<SourceDTO> Sources { get; set; } = Enumerable.Empty<SourceDTO>();
        public IEnumerable<PositivityRatingDTO> Ratings { get; set; } = Enumerable.Empty<PositivityRatingDTO>();
    }
}

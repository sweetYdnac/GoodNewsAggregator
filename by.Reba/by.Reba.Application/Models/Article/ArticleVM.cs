using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Article
{
    public class ArticleVM
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

    }
}

using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Article
{
    public class ArticleFilterVM
    {
        [Required]
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public Guid MinPositivityRating { get; set; }

        [Required]
        public IEnumerable<Guid> SourcesId { get; set; } = Enumerable.Empty<Guid>();
    }
}

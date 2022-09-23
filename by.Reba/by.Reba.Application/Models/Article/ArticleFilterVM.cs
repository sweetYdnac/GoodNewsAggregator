using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Article
{
    public class ArticleFilterVM
    {
        [Required]
        public List<Guid> Categories { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public Guid MinPositivityRating { get; set; }
    }
}

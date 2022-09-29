using System.ComponentModel.DataAnnotations;

namespace by.Reba.AdminPanel.Models.Article
{
    public class ArticleFilterVM
    {
        [Required]
        public IEnumerable<Guid> Categories { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public Guid MinPositivityRating { get; set; }
    }
}

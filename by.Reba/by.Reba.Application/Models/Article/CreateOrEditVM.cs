using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Article
{
    public class CreateOrEditArticleVM
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string PosterUrl { get; set; }

        public string? Text { get; set; }

        [Required]
        public string SourceUrl { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid SourceId { get; set; }

        [Required]
        public Guid RatingId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Sources { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Ratings { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Account
{
    public class CreateUserPreferenceVM
    {
        [Required]
        public Guid RatingId { get; set; }

        [Required]
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();

        public IEnumerable<SelectListItem> AllRatings { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> AllCategories { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

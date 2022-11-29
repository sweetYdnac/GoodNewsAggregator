using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Preference
{
    public class EditPreferenceVM
    {
        public Guid Id { get; set; }

        [Required]
        public Guid RatingId { get; set; }

        [Required]
        public IList<Guid> CategoriesId { get; set; } = new List<Guid>();

        public IEnumerable<SelectListItem> AllRatings { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> AllCategories { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

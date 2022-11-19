using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Account
{
    public class CreateUserPreferenceVM
    {
        [Required]
        public Guid RatingId { get; set; }

        [Required]
        public IEnumerable<Guid> CategoriesId { get; set; }

        public IEnumerable<SelectListItem>? AllRatings { get; set; }

        public IEnumerable<SelectListItem>? AllCategories { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Account
{
    public class CreateUserPreferenceVM
    {
        [Required]
        public Guid RatingId { get; set; }
        public IEnumerable<SelectListItem> Ratings { get; set; }

        [Required]
        public IEnumerable<Guid> CategoriesId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}

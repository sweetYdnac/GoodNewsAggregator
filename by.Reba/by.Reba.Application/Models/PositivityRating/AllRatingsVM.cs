using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.PositivityRating
{
    public class AllRatingsVM
    {
        [Required]
        public Guid MinPositivityRating { get; set; }

        public IEnumerable<SelectListItem> PositivityRatings { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.PositivityRating
{
    public class AllPositivitiesVM
    {
        [Required]
        public Guid MinPositivity { get; set; }

        public IEnumerable<SelectListItem> Positivities { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

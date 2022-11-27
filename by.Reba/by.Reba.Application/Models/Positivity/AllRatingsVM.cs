using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.PositivityRating
{
    public class AllPositivitiesVM
    {
        [Required]
        public string MinPositivityName { get; set; }

        public IEnumerable<SelectListItem> Positivities { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

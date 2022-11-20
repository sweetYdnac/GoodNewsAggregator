using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Category
{
    public class AllCategoriesVM
    {
        [Required]
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

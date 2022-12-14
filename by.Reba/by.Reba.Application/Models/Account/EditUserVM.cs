using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Account
{
    public class EditUserVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(50, ErrorMessage = "Никнейм должен быть не более чем 50 символов")]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }
        public bool IsAdmin { get; set; } = false;

        [Required]
        public Guid MinPositivity { get; set; }
        [Required]
        public IList<Guid> CategoriesId { get; set; } = new List<Guid>();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Ratings { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

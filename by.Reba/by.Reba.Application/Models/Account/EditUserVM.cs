using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace by.Reba.Application.Models.Account
{
    public class EditUserVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(30, ErrorMessage = "Никнейм должен быть не более чем 30 символов")]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }
        public bool IsAdmin { get; set; } = false;

        [Required]
        public Guid RatingId { get; set; }
        [Required]
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Ratings { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.User
{
    public class PatchUserRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(50, ErrorMessage = "Никнейм должен быть не более чем 50 символов")]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }

        [Required]
        public Guid MinPositivity { get; set; }

        [Required]
        public IList<Guid> CategoriesId { get; set; } = new List<Guid>();
    }
}

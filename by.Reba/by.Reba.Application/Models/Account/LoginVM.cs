using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(70, ErrorMessage = "Почта должена быть не более чем 50 символов")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(50, ErrorMessage = "Пароль должен быть не более 50-ти символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

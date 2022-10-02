using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace by.Reba.Application.Models.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Remote(controller: "Account", action: "VerifyNickname", HttpMethod = WebRequestMethods.Http.Post)]
        [MaxLength(30, ErrorMessage = "Никнейм должен быть не более чем 30 символов")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Remote(controller: "Account", action: "VerifyEmail", HttpMethod = WebRequestMethods.Http.Post)]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "Email должен быть не болеее чем 100 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(50, ErrorMessage = "Пароль должен быть не более 50-ти символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(50, ErrorMessage = "Пароль должен быть не более 50-ти символов")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароль не совпадает")]
        public string PasswordConfirm { get; set; }
    }
}

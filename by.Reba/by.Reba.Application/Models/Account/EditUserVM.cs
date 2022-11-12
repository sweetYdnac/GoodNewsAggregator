﻿using Microsoft.AspNetCore.Mvc;
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
        [Remote(controller: "Account", action: "VerifyNickname", HttpMethod = WebRequestMethods.Http.Post)]
        [MaxLength(30, ErrorMessage = "Никнейм должен быть не более чем 30 символов")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Remote(controller: "Account", action: "VerifyEmail", HttpMethod = WebRequestMethods.Http.Post)]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "Email должен быть не болеее чем 100 символов")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }
        public bool IsAdmin { get; set; } = false;

        [Required]
        public Guid RatingId { get; set; }
        [Required]
        public IList<Guid> CategoriesId { get; set; } = new List<Guid>();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> PositivityRatings { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

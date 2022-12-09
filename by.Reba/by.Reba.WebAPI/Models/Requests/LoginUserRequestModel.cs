using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests
{
    public class LoginUserRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

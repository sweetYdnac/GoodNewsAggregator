using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests
{
    public class RefreshTokenRequestModel
    {
        [Required]
        public Guid RefreshToken { get; set; }
    }
}

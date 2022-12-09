using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Positivity
{
    public class CreatePositivityRequestModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public float Value { get; set; }
    }
}

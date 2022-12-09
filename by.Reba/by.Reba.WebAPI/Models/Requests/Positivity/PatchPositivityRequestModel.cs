using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Positivity
{
    public class PatchPositivityRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public float Value { get; set; }
    }
}

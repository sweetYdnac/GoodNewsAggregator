using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Source
{
    public class CreateOrEditPositivityVM
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public float Value { get; set; }
    }
}

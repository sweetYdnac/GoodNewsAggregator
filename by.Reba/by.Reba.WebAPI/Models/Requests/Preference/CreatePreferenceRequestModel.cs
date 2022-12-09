using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Preference
{
    public class CreatePreferenceRequestModel
    {
        [Required]
        public Guid PositivityId { get; set; }

        [Required]
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();

        [Required]
        public Guid UserId { get; set; }
    }
}

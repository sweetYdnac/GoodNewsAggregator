using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Preference
{
    public class PatchPreferenceRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid PositivityId { get; set; }

        [Required]
        public IList<Guid> CategoriesId { get; set; } = new List<Guid>();
    }
}

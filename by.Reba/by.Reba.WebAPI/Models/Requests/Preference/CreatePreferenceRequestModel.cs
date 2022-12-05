namespace by.Reba.WebAPI.Models.Requests.Preference
{
    public class CreatePreferenceRequestModel
    {
        public Guid PositivityId { get; set; }
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();
        public string Email { get; set; }
    }
}

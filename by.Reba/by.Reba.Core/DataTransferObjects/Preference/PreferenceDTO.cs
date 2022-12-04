namespace by.Reba.Core.DataTransferObjects.UserPreference
{
    public class PreferenceDTO
    {
        public Guid Id { get; set; }
        public Guid PositivityId { get; set; }
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();
        public Guid UserId { get; set; }
    }
}

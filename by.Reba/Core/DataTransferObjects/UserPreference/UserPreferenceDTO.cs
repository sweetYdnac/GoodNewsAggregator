namespace by.Reba.Core.DataTransferObjects.UserPreference
{
    public class UserPreferenceDTO
    {
        public Guid RatingId { get; set; }
        public IEnumerable<Guid> CategoriesId { get; set; }
        public Guid UserId { get; set; }
    }
}

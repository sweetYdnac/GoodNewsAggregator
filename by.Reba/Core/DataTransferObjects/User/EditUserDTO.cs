namespace by.Reba.Core.DataTransferObjects.User
{
    public class EditUserDTO
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RatingId { get; set; }
        public IEnumerable<Guid> CategoriesId { get; set; }
    }
}

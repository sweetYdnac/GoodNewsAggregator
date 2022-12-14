namespace by.Reba.Core.DataTransferObjects.User
{
    public class EditUserDTO
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public Guid PositivityId { get; set; }
        public IEnumerable<Guid> CategoriesId { get; set; } = Enumerable.Empty<Guid>();
    }
}

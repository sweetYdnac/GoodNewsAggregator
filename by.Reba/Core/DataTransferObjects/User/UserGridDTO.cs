namespace by.Reba.Core.DataTransferObjects.User
{
    public class UserGridDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string Nickname { get; set; }
        public string RoleName { get; set; }
    }
}

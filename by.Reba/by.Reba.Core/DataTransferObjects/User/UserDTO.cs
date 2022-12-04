namespace by.Reba.Core.DataTransferObjects.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}

using by.Reba.Core.DataTransferObjects.User;

namespace by.Reba.Core.Abstractions
{
    public interface IUserService
    {
        Task<bool> VerifyEmail(string email);
        Task<bool> VerifyNickname(string nickname);

        Task<bool> IsUserExist(Guid userId);
        Task<int> RegisterUser(UserDTO dto);
        Task<bool> CheckUserPassword(string email, string password);
        Task<UserDTO> GetUserByEmailAsync(string email);
    }
}

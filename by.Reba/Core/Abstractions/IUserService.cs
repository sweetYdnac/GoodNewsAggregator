using by.Reba.Core.DataTransferObjects.User;

namespace by.Reba.Core.Abstractions
{
    public interface IUserService
    {
        Task<bool> VerifyEmailAsync(string email);
        Task<bool> VerifyNicknameAsync(string nickname);

        Task<bool> IsUserExistAsync(Guid userId);
        Task<int> RegisterUserAsync(UserDTO dto);
        Task<bool> CheckUserPasswordAsync(string email, string password);
        Task<UserDTO> GetUserByEmailAsync(string email);
    }
}

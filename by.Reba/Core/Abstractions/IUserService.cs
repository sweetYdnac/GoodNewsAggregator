using by.Reba.Core.DataTransferObjects.User;

namespace by.Reba.Core.Abstractions
{
    public interface IUserService
    {
        Task<bool> IsEmailExistAsync(string email);
        Task<bool> IsNicknameExistAsync(string nickname);
        Task<bool> IsEmailExistAsync(string email, string? currentEmail);
        Task<bool> IsNicknameExistAsync(string nickname, string? currentEmail);
        Task<bool> IsUserExistAsync(Guid userId);
        Task<int> RegisterUserAsync(UserDTO dto);
        Task<bool> CheckUserPasswordAsync(string email, string password);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserNavigationDTO> GetUserNavigationByEmailAsync(string email);
        Task<UserDetailsDTO> GetUserDetailsByEmailAsync(Guid id);
        Task<EditUserDTO> GetEditUserDTOByEmailAsync(string email);
        Task<Guid> GetIdByEmailAsync(string email);
        Task<int> AddOrUpdateArticleInUserHistoryAsync(Guid articleId, string userEmail);
        Task<UserPreviewDTO> GetUserPreviewByEmailAsync(string email);
        Task<int> UpdateAsync(Guid id, EditUserDTO dto);
    }
}

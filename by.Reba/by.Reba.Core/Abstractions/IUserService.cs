using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Core.SortTypes;

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
        Task<UserPreviewDTO> GetUserPreviewByEmailAsync(string email);
        Task<int> UpdateAsync(Guid id, EditUserDTO dto);
        Task<IEnumerable<UserGridDTO>> GetUsersGridAsync(int page, int pageSize, UserSort sortOrder, string searchString);
        Task<int> GetTotalCountAsync(string searchString);
        Task<int> RemoveAsync(Guid id);
        Task<UserDTO?> GetUserByRefreshTokenAsync(Guid token);
    }
}

using by.Reba.Core.DataTransferObjects.User;
using by.Reba.WebAPI.Models.Responces;

namespace by.Reba.WebAPI.Utils
{
    public interface IJwtUtil
    {
        Task<TokenResponse> GenerateTokenAsync(UserDTO dto);
        Task RemoveRefreshTokenAsync(Guid requestRefreshToken);
    }
}

using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Data.CQS.Commands;
using by.Reba.WebAPI.Models.Responces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace by.Reba.WebAPI.Utils
{
    public class JwtUtilSha256 : IJwtUtil
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public JwtUtilSha256(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<TokenResponse> GenerateTokenAsync(UserDTO dto)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:JwtSecret"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var nowUtc = DateTime.UtcNow;
            var exp = nowUtc.AddMinutes(double.Parse(_configuration["Token:ExpiryMinutes"])).ToUniversalTime();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Actor, dto.Email),
                new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString("D")),
                new Claim(ClaimTypes.Role, dto.RoleName),
            };

            var jwtToken = new JwtSecurityToken(_configuration["Token:Issuer"],
                _configuration["Token:Issuer"],
                claims,
                expires: exp,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshTokenValue = Guid.NewGuid();

            await _mediator.Send(new AddRefreshTokenCommand()
            {
                UserId = dto.Id,
                TokenValue = refreshTokenValue,
            });

            return new TokenResponse()
            {
                AccessToken = accessToken,
                Role = dto.RoleName,
                TokenExpiration = jwtToken.ValidTo,
                UserId = dto.Id,
                RefreshToken = refreshTokenValue,
            };
        }

        public async Task RemoveRefreshTokenAsync(Guid requestRefreshToken)
        {
            await _mediator.Send(new RemoveRefreshTokenCommand()
            {
                TokenValue = requestRefreshToken,
            });
        }
    }
}

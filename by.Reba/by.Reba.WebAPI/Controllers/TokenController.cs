using by.Reba.Core.Abstractions;
using by.Reba.WebAPI.Models.Requests;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with tokens
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtUtil _jwtUtil;

        public TokenController(
            IUserService userService,
            IJwtUtil jwtUtil)
        {
            _userService = userService;
            _jwtUtil = jwtUtil;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateJwtToken([FromBody] LoginUserRequestModel request)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(request.Email);

                if (user is null)
                {
                    return NotFound(new ErrorModel()
                    {
                        Message = $"User with email {request.Email} doesn't exist"
                    });
                }

                var isPasswordCorrect = await _userService.CheckUserPasswordAsync(request.Email, request.Password);

                if (!isPasswordCorrect)
                {
                    return NotFound(new ErrorModel()
                    {
                        Message = $"Password is incorrect"
                    });
                }

                var response = await _jwtUtil.GenerateTokenAsync(user);

                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Reauthorize user by creating new token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel request)
        {
            try
            {
                var user = await _userService.GetUserByRefreshTokenAsync(request.RefreshToken);
                var response = await _jwtUtil.GenerateTokenAsync(user);
                await _jwtUtil.RemoveRefreshTokenAsync(request.RefreshToken);

                return StatusCode(201, response);
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                return NotFound(new ErrorModel() { Message = ex.Message});
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Revoke refresh token with specified model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Revoke")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestModel request)
        {
            try
            {
                await _jwtUtil.RemoveRefreshTokenAsync(request.RefreshToken);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                return NotFound(new ErrorModel() { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }
    }
}

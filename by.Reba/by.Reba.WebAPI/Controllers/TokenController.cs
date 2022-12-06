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
            IJwtUtil jwtUtil) => 

            (_userService, _jwtUtil) = (userService, jwtUtil);

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateJwtToken([FromBody] LoginUserRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = await _userService.GetUserByEmailAsync(request.Email);

                if (dto is null)
                {
                    var message = $"User with email {request.Email} doesn't exist";

                    Log.Error(message);
                    return NotFound(new ErrorModel() { Message = message });
                }

                var isPasswordCorrect = await _userService.CheckUserPasswordAsync(request.Email, request.Password);

                if (!isPasswordCorrect)
                {
                    var message = $"Password is incorrect";

                    Log.Error(message);
                    return NotFound(new ErrorModel() { Message = message });
                }

                var response = await _jwtUtil.GenerateTokenAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Reauthorize user by creating new token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel request)
        {
            try
            {
                var user = await _userService.GetUserByRefreshTokenAsync(request.RefreshToken);
                var response = await _jwtUtil.GenerateTokenAsync(user);
                await _jwtUtil.RemoveRefreshTokenAsync(request.RefreshToken);

                return Ok(response);
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
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
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

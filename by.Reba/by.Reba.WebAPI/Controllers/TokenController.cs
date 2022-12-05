using by.Reba.Core.Abstractions;
using by.Reba.WebAPI.Models.Requests;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
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
        public async Task<IActionResult> CreateJwtToken([FromBody] LoginUserRequestModel request)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(request.Email);

                if (user is null)
                {
                    return BadRequest(new ErrorModel()
                    {
                        Message = $"User with email {request.Email} doesn't exist"
                    });
                }

                var isPasswordCorrect = await _userService.CheckUserPasswordAsync(request.Email, request.Password);

                if (!isPasswordCorrect)
                {
                    return BadRequest(new ErrorModel()
                    {
                        Message = $"Password is incorrect"
                    });
                }

                var response = await _jwtUtil.GenerateTokenAsync(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Reauthorize user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel request)
        {
            //try
            //{
            //    var user = await _userService.GetUserByRefreshTokenAsync(request.RefreshToken);

            //    var response = await _jwtUtil.GenerateTokenAsync(user);

            //    await _jwtUtil.RemoveRefreshTokenAsync(request.RefreshToken);

            //    return Ok(response);
            //}
            //catch (Exception e)
            //{
            //    Log.Error(e.Message);
            //    return StatusCode(500);
            //}

            return Ok();
        }
    }
}

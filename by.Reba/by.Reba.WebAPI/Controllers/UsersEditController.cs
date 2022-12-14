using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.WebAPI.Models.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with edit usee model
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersEditController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersEditController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Get user edit response model
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(EditUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserNavigation([FromRoute] string email)
        {
            try
            {
                var user = await _userService.GetEditUserDTOByEmailAsync(email);
                return Ok(user);
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

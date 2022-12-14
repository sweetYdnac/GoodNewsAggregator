using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.WebAPI.Models.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with user navigation model
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersNavigationController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersNavigationController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Get user navigation
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(UserNavigationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserNavigation([FromRoute] string email)
        {
            try
            {
                var user = await _userService.GetUserNavigationByEmailAsync(email);
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

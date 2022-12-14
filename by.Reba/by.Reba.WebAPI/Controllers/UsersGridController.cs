using by.Reba.Core.Abstractions;
using by.Reba.WebAPI.Models.Requests.Users;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Models.Responces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with users previews
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersGridController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersGridController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Get user previews by request filter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetUsersGridResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersGrid([FromQuery] GetUsersGridRequestModel request)
        {
            try
            {
                var users = await _userService.GetUsersGridAsync(request.PageNumber, request.PageSize, request.SortOrder, request.SearchString);
                return Ok(new GetUsersGridResponseModel() { Users = users });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Get total count of users
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCount([FromQuery] string searchString)
        {
            try
            {
                var count = await _userService.GetTotalCountAsync(searchString);
                return Ok(count);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }
    }
}

using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.WebAPI.Models.Requests.Comment;
using by.Reba.WebAPI.Models.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data;
using System.Security.Claims;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with comment
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CommentController(
            ICommentService commentService, 
            IMapper mapper, 
            IUserService userService, 
            IConfiguration configuration) =>
            (_commentService, _userService, _mapper, _configuration) = (commentService, userService, mapper, configuration);

        /// <summary>
        /// Get Comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommentShortSummaryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComment([FromRoute] Guid id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);
                return Ok(comment);
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

        /// <summary>
        /// Create comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<CreateCommentDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from CreateCommentRequestModel to CreateCommentDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                var authorId = HttpContext.User.Claims
                    .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                    .Select(c => new Guid(c.Value))
                    .FirstOrDefault();

                if (authorId.Equals(default))
                {
                    var message = "Token does not have correct credentials";

                    Log.Error(message);
                    return Unauthorized(new ErrorModel() { Message = message });
                }

                dto.AuthorId = authorId;

                var result = await _commentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetComment), new { id = dto.Id }, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Patch specific comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchComment([FromBody] PatchCommentRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var userId = HttpContext.User.Claims
                    .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                    .Select(c => c.Value)
                    .FirstOrDefault();

                var role = HttpContext.User.Claims
                    .Where(c => c.Type.Equals(ClaimTypes.Role))
                    .Select(c => c.Value)
                    .FirstOrDefault();

                if (userId is null || role is null)
                {
                    var message = "Token does not have correct credentials";

                    Log.Error(message);
                    return Unauthorized(new ErrorModel() { Message = message });
                }

                var authorId = await _commentService.GetAuthorIdAsync(request.Id);

                if (!role.Equals(_configuration["Roles:Admin"]) && !userId.Equals(authorId.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var message = $"User with id = {userId} do not have access right to patch comment for user with id = {authorId}";

                    Log.Error(message);
                    return StatusCode(403, new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<EditCommentDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from PatchCommentRequestModel to EditCommentDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                var result = await _commentService.UpdateAsync(request.Id, dto);
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

        /// <summary>
        /// Rate comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("Rate")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RateComment(RateCommentRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<RateEntityDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from RateCommentRequestModel to RateEntityDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }
                
                dto.AuthorId = HttpContext.User.Claims
                    .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                    .Select(c => new Guid(c.Value))
                    .FirstOrDefault();

                var result = await _commentService.RateAsync(dto);
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

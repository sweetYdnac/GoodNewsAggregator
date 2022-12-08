using by.Reba.Core.Abstractions;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Models.Responces.CommentsTrees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with comments tree
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsTreesController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsTreesController(ICommentService commentService) => _commentService = commentService;

        /// <summary>
        /// Get comments trees from specific article
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet("{articleId}")]
        [ProducesResponseType(typeof(GetCommentsTreesResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentsTrees([FromRoute] Guid articleId)
        {
            try
            {
                var commentsTrees = await _commentService.GetCommentsTreesByArticleIdAsync(articleId);
                return Ok(new GetCommentsTreesResponseModel() { CommentsTrees = commentsTrees });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }
    }
}

using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.WebAPI.Models.Requests.Article;
using by.Reba.WebAPI.Models.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with articles
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService articleService, IMapper mapper) =>
            (_articleService, _mapper) = (articleService, mapper);

        /// <summary>
        /// Get Article from storage with specified id. If used is authorised returned article have comments trees.
        /// </summary>
        /// <param name="id">Id of article</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArticleDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticle([FromRoute] Guid id)
        {
            try
            {
                var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;

                var article = isAuthenticated
                    ? await _articleService.GetWithCommentsByIdAsync(id)
                    : await _articleService.GetByIdAsync(id);

                return Ok(article);
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
        /// Create article
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<CreateOrEditArticleDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from CreateArticleRequestModel to CreateOrEditArticleDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                await _articleService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetArticle), new { id = dto.Id }, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Patch specific article
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchArticle([FromBody] PatchArticleRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<CreateOrEditArticleDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from PatchArticleRequestModel to CreateOrEditArticleDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                await _articleService.UpdateAsync(request.Id, dto);
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
        /// Delete article by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteArticle([FromRoute] Guid id)
        {
            try
            {
                await _articleService.RemoveAsync(id);
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
        /// Rate article
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("Rate")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RateArticle(RateArticleRequestModel request)
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
                    var message = "Invalid mapping from RateArticleRequestModel to RateEntityDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                dto.AuthorId = HttpContext.User.Claims
                    .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                    .Select(c => new Guid(c.Value))
                    .FirstOrDefault();

                await _articleService.RateAsync(dto);
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

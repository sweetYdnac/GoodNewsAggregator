using AutoMapper;
using by.Reba.Business.ServicesImplementations;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.WebAPI.Models.Requests.ArticlesPreview;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Models.Responces.Sources;
using by.Reba.WebAPI.Models.Responces.Users;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with articles preview
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesPreviewController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesPreviewController(IArticleService articleService) => _articleService = articleService;

        /// <summary>
        /// Get articles preview by request filter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ArticlePreviewDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticlesPreview([FromQuery] GetArticlesRequestModel request)
        {
            try
            {
                var articles = await _articleService.GetPreviewsByPageAsync(
                    request.PageNumber, request.PageSize, request.Filter, request.SortOrder, request.SearchString);

                return Ok(articles);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Get total count of articles
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCount([FromQuery] GetTotalCountRequestModel request)
        {
            try
            {
                var count = await _articleService.GetTotalCountAsync(request.Filter, request.SearchString);
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

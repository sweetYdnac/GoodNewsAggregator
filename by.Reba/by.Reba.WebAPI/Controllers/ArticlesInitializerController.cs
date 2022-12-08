using by.Reba.Core.Abstractions;
using by.Reba.WebAPI.Models.Responces;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for initialize articles recieve Hangfire Jobs
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesInitializerController : ControllerBase
    {
        private readonly IArticleInitializerService _articleInitializerService;

        public ArticlesInitializerController(IArticleInitializerService articleInitializerService) => _articleInitializerService = articleInitializerService;

        /// <summary>
        /// Initialize hangfire jobs for work with recieve articles from external resources logic
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddArticles()
        {
            try
            {
                RecurringJob.AddOrUpdate(() => _articleInitializerService.CreateArticlesFromExternalSourcesAsync(), "5 */1 * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.AddTextToArticlesAsync(30), "*/25 * * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.AddPositivityToArticlesAsync(10), "*/11 * * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.RemoveEmptyArticles(), "0 19 */1 * *");

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message});
            }
        }
    }
}

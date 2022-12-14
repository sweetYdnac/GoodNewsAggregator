using by.Reba.Core.Abstractions;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;

namespace by.Reba.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticleInitializerController : Controller
    {
        private readonly IArticleInitializerService _articleInitializerService;

        public ArticleInitializerController(IArticleInitializerService articleInitializerService) =>  
            _articleInitializerService = articleInitializerService;

        [HttpGet]
        public async Task<IActionResult> AddArticles()
        {
            try
            {
                RecurringJob.AddOrUpdate(() => _articleInitializerService.CreateArticlesFromExternalSourcesAsync(), "0 */1 * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.AddTextToArticlesAsync(30), "*/30 * * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.AddPositivityToArticlesAsync(10), "*/10 * * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.RemoveEmptyArticles(), "0 19 */1 * *");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

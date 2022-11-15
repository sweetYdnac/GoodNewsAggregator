using by.Reba.Core.Abstractions;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace by.Reba.Application.Controllers
{
    public class ArticleInitializerController : Controller
    {
        private readonly IArticleInitializerService _articleInitializerService;

        public ArticleInitializerController(
            IArticleInitializerService articleInitializerService)
        {
            _articleInitializerService = articleInitializerService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddArticles()
        {
            try
            {
                RecurringJob.AddOrUpdate(() => _articleInitializerService.CreateArticlesFromExternalSourcesAsync(), "*/15 * * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.AddTextToArticlesAsync(), "*/30 * * * *");

                //return Redirect("~/hangfire");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            //await _articleInitializerService.CreateArticlesFromAllSourcesRssAsync();
            await _articleInitializerService.AddTextToArticlesAsync();
            return Ok();
        }
    }
}

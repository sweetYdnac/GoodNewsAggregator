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
        [HttpGet]
        public async Task<IActionResult> AddArticles()
        {
            try
            {
                RecurringJob.AddOrUpdate(() => _articleInitializerService.CreateArticlesFromExternalSourcesAsync(), "*/30 * * * *");
                RecurringJob.AddOrUpdate(() => _articleInitializerService.AddTextToArticlesAsync(), "*/12 * * * *");

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
            //await _articleInitializerService.AddTextToArticlesAsync();
            //await _articleInitializerService.AddRatingAsync();
            return Ok();
        }
    }
}

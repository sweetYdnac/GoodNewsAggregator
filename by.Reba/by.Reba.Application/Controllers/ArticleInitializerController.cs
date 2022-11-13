using by.Reba.Core.Abstractions;
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

        [HttpPost]
        public async Task<IActionResult> AddArticles()
        {
            try
            {
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
            return Ok();
        }
    }
}

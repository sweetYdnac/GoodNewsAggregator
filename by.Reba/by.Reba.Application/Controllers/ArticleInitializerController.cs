using by.Reba.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace by.Reba.Application.Controllers
{
    public class ArticleInitializerController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleInitializerController(
            IArticleService articleService)
        {
            _articleService = articleService;
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
            await _articleService.CreateArticlesFromAllSourcesRssAsync();
            return Ok();
        }
    }
}

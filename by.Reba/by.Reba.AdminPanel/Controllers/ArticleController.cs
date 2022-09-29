using by.Reba.AdminPanel.Models;
using by.Reba.AdminPanel.Models.Article;
using by.Reba.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace by.Reba.AdminPanel.Controllers
{
    public class ArticleController : Controller
    {
        private const int PAGE_SIZE = 20;
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var articles = await _articleService.GetByPage(page, PAGE_SIZE);

            var model = new ArticlesListVM()
            {
                Articles = articles
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateArticleVM model)
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

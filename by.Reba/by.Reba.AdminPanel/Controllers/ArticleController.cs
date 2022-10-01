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
        private readonly ICategoryService _categoryService;
        public ArticleController(
            IArticleService articleService,
            ICategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var articles = await _articleService.GetArticleDTOsByPage(page, PAGE_SIZE);
            var categories = await _categoryService.GetAllCategories();
            var sources = await _articleService.GetAllSources();

            var model = new ArticlesListVM()
            {
                Articles = articles,
                Categories = categories,
                Sources = sources,
                From = DateTime.Now - TimeSpan.FromDays(7),
                To = DateTime.Now
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

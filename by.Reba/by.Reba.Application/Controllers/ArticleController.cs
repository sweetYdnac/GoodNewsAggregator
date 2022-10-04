using by.Reba.Application.Models;
using by.Reba.Application.Models.Article;
using by.Reba.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace by.Reba.Application.Controllers
{
    public class ArticleController : Controller
    {
        private const int COUNT_ON_PAGE = 9;

        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IRoleService _roleService;
        public ArticleController(
            IArticleService articleService,
            ICategoryService categoryService,
            IRoleService roleService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            var articles = await _articleService.GetByPage(page, COUNT_ON_PAGE);
            var categories = await _categoryService.GetAllCategories();
            var positivityRatings = await _articleService.GetAllPositivityRatings();
            var isAdmin = await _roleService.IsAdmin(HttpContext.User.Identity.Name);

            var model = new HomePageVM()
            {
                Articles = articles,
                Categories = categories,
                From = DateTime.Now - TimeSpan.FromDays(7),
                To = DateTime.Now,
                PositivityRatings = positivityRatings,
                IsAdmin = isAdmin
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(ArticleFilterVM model)
        {

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(Guid id)
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

using AutoMapper;
using by.Reba.Application.Models;
using by.Reba.Application.Models.Article;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace by.Reba.Application.Controllers
{
    public class ArticleController : Controller
    {
        private const int COUNT_ON_PAGE = 9;

        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IPositivityRatingService _positivityRatingService;
        private readonly ISourceService _sourceService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public ArticleController(
            IArticleService articleService,
            ICategoryService categoryService,
            IRoleService roleService,
            IPositivityRatingService positivityRatingService,
            ISourceService sourceService,
            IMapper mapper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _roleService = roleService;
            _positivityRatingService = positivityRatingService;
            _sourceService = sourceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            var articles = await _articleService.GetByPage(page, COUNT_ON_PAGE);
            var categories = await _categoryService.GetAllCategories();
            var positivityRatings = await _positivityRatingService.GetAll();
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Grid(int page, ArticleFilterVM filter, ArticleSort sort, string searchString)
        {
            var articles = await _articleService.GetFilteredAndOrderedByPage(page, 15, _mapper.Map<ArticleFilterDTO>(filter), sort, searchString);
            var categories = await _categoryService.GetAllCategories();
            var positivityRatings = await _positivityRatingService.GetAll();
            var sources = await _sourceService.GetAll();

            var model = new ArticlesGridVM()
            {
                Articles = articles,
                Categories = categories,
                From = DateTime.Now - TimeSpan.FromDays(7),
                To = DateTime.Now,
                PositivityRatings = positivityRatings,
                Sources = sources
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

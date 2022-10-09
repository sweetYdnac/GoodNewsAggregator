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
            var articles = await _articleService.GetByPageAsync(page, COUNT_ON_PAGE);
            var categories = await _categoryService.GetAllAsync();
            var positivityRatings = await _positivityRatingService.GetAllOrderedAsync();
            var isAdmin = await _roleService.IsAdminAsync(HttpContext.User.Identity.Name);

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
            var filterDTO = await _articleService.SetDefaultFilterAsync(_mapper.Map<ArticleFilterDTO>(filter));

            var articles = await _articleService.GetFilteredAndOrderedByPageAsync(page, 15, filterDTO, sort, searchString);
            var categories = await _categoryService.GetAllAsync();
            var positivityRatings = await _positivityRatingService.GetAllOrderedAsync();
            var sources = await _sourceService.GetAllAsync();

            var model = new ArticlesGridVM()
            {
                Articles = articles,
                Categories = categories,
                PositivityRatings = positivityRatings,
                Sources = sources,
                CurrentFilter = filterDTO,
                SearchString = searchString
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            var sources = await _sourceService.GetAllAsync();
            var ratings = await _positivityRatingService.GetAllOrderedAsync();

            var model = new CreateArticleVM()
            {
                Categories = categories,
                Sources = sources,
                Ratings = ratings
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ArticleVM model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Grid");
            }

            return View(model);
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

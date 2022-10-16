using AutoMapper;
using by.Reba.Application.Models;
using by.Reba.Application.Models.Article;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.SortTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using Serilog.Events;
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
        public async Task<IActionResult> Index(int page, ArticleFilterVM filter, ArticleSort sort, string searchString)
        {
            var filterDTO = await _articleService.SetDefaultFilterAsync(_mapper.Map<ArticleFilterDTO>(filter));

            var articles = await _articleService.GetFilteredAndOrderedByPageAsync(page, COUNT_ON_PAGE, filterDTO, sort, searchString);
            var categories = await _categoryService.GetAllAsync();
            var positivityRatings = await _positivityRatingService.GetAllOrderedAsync();
            var sources = await _sourceService.GetAllAsync();
            var isAdmin = await _roleService.IsAdminAsync(HttpContext.User.Identity.Name);

            var model = new HomePageVM()
            {
                Articles = articles,
                FilterData = new ArticleFilterDataVM
                {
                    Categories = categories,
                    PositivityRatings = positivityRatings,
                    Sources = sources,
                    CurrentFilter = filterDTO,
                },
                SearchString = searchString,
                SortOrder = sort,
                IsAdmin = isAdmin
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Grid(int page, ArticleFilterVM filter, ArticleSort sortOrder, string searchString)
        {
            var filterDTO = await _articleService.SetDefaultFilterAsync(_mapper.Map<ArticleFilterDTO>(filter));

            var articles = await _articleService.GetFilteredAndOrderedByPageAsync(page, 15, filterDTO, sortOrder, searchString);
            var categories = await _categoryService.GetAllAsync();
            var positivityRatings = await _positivityRatingService.GetAllOrderedAsync();
            var sources = await _sourceService.GetAllAsync();

            var model = new ArticlesGridVM()
            {
                Articles = articles,
                FilterData = new ArticleFilterDataVM
                {
                    Categories = categories,
                    PositivityRatings = positivityRatings,
                    Sources = sources,
                    CurrentFilter = filterDTO,
                },
                SearchString = searchString,
                sortOrder = sortOrder
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
                Categories = categories.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString())),
                Sources = sources.Select(dto => new SelectListItem(dto.Name, dto.Id.ToString())),
                Ratings = ratings.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()))
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateArticleVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _articleService.CreateAsync(_mapper.Map<CreateArticleDTO>(model));
                    return RedirectToAction("Grid");
                }

                return View(model);
            }
            catch(Exception ex)
            {
                Log.Write(LogEventLevel.Information, ex.Message);
                return StatusCode(500);
            }   
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var dto = HttpContext.User.Identity.IsAuthenticated
                    ? await _articleService.GetWithCommentsByIdAsync(id)
                    : await _articleService.GetByIdAsync(id);

                var model = _mapper.Map<ArticleDetailsVM>(dto);
                return View(model);
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
                return NotFound();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

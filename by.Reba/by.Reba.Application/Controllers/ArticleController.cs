using AutoMapper;
using by.Reba.Application.Models;
using by.Reba.Application.Models.Article;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
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
        private const int COUNT_PER_PAGE = 2;

        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IPositivityRatingService _positivityRatingService;
        private readonly ISourceService _sourceService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ArticleController(
            IArticleService articleService,
            ICategoryService categoryService,
            IRoleService roleService,
            IPositivityRatingService positivityRatingService,
            ISourceService sourceService,
            IMapper mapper,
            IUserService userService,
            IConfiguration configuration)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _roleService = roleService;
            _positivityRatingService = positivityRatingService;
            _sourceService = sourceService;
            _mapper = mapper;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet()]
        public async Task<IActionResult> Index(ArticleFilterVM filter, ArticleSort sortType, string searchString, int page = 1)
        {
            try
            {
                var filterDTO = _mapper.Map<ArticleFilterDTO>(filter);

                if (string.IsNullOrEmpty(HttpContext.Request.QueryString.Value))
                {
                    await _articleService.SetDefaultFilterAsync(filterDTO);
                }

                var model = new HomePageVM()
                {
                    Articles = await _articleService.GetPreviewsByPageAsync(page, COUNT_PER_PAGE, filterDTO, sortType, searchString),
                    FilterData = new ArticleFilterDataVM
                    {
                        Categories = await _categoryService.GetAllAsync(),
                        PositivityRatings = await _positivityRatingService.GetAllOrderedAsync(),
                        Sources = await _sourceService.GetAllAsync(),
                        CurrentFilter = filterDTO,
                    },
                    SearchString = searchString,
                    SortOrder = sortType,
                    PagingInfo = new PagingInfo()
                    {
                        TotalItems = await _articleService.GetTotalCount(filterDTO, searchString),
                        CurrentPage = page,
                        ItemsPerPage = COUNT_PER_PAGE,
                    },
                    IsAdmin = await _roleService.IsAdminAsync(HttpContext?.User?.Identity?.Name),
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Grid(ArticleFilterVM filter, ArticleSort sortOrder, string searchString, int page = 1)
        {
            try
            {
                var filterDTO = _mapper.Map<ArticleFilterDTO>(filter);

                if (string.IsNullOrEmpty(HttpContext.Request.QueryString.Value))
                {
                    await _articleService.SetDefaultFilterAsync(filterDTO);
                }

                var articles = await _articleService.GetPreviewsByPageAsync(page, 15, filterDTO, sortOrder, searchString);
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
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }      
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                var sources = await _sourceService.GetAllAsync();
                var ratings = await _positivityRatingService.GetAllOrderedAsync();

                var model = new CreateOrEditVM()
                {
                    Categories = categories.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString())),
                    Sources = sources.Select(dto => new SelectListItem(dto.Name, dto.Id.ToString())),
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateOrEditVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _articleService.CreateAsync(_mapper.Map<CreateOrEditArticleDTO>(model));
                    return RedirectToAction(nameof(Grid));
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }   
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    var result = await _userService.AddOrUpdateArticleInHistory(id, HttpContext.User.Identity.Name);
                }

                var dto = isAuthenticated
                    ? await _articleService.GetWithCommentsByIdAsync(id)
                    : await _articleService.GetByIdAsync(id);

                var isAdmin = await _roleService.IsAdminAsync(HttpContext.User.Identity.Name);

                var model = _mapper.Map<ArticleDetailsVM>(dto);
                model.isAdmin = isAdmin;
                model.isAuthenticated = isAuthenticated;

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Rate(RateArticleVM model)
        {
            try
            {
                var dto = _mapper.Map<RateEntityDTO>(model);
                dto.AuthorId = await _userService.GetIdByEmailAsync(HttpContext.User.Identity.Name);

                await _articleService.RateAsync(dto);
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var dto = await _articleService.GetEditArticleDTOByIdAsync(id);
                var model = _mapper.Map<CreateOrEditVM>(dto);

                var categories = await _categoryService.GetAllAsync();
                var sources = await _sourceService.GetAllAsync();
                var ratings = await _positivityRatingService.GetAllOrderedAsync();

                model.Categories = categories.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()));
                model.Sources = sources.Select(dto => new SelectListItem(dto.Name, dto.Id.ToString()));

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CreateOrEditVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<CreateOrEditArticleDTO>(model);
                    var result = await _articleService.UpdateAsync(model.Id, dto);
                    return RedirectToAction(nameof(Grid));
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _articleService.RemoveAsync(id);
                return RedirectToAction(nameof(Grid));
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

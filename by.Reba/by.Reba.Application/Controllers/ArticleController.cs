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
        private const int COUNT_PER_PAGE = 12;

        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IHistoryService _historyService;
        private readonly IPositivityService _positivityService;
        private readonly ISourceService _sourceService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ArticleController(
            IArticleService articleService,
            ICategoryService categoryService,
            IHistoryService historyService,
            IPositivityService positivityService,
            IRoleService roleService,
            ISourceService sourceService,
            IUserService userService,
            IMapper mapper,
            IConfiguration configuration) =>

            (_articleService, _categoryService, _historyService, _positivityService, _roleService, _sourceService, _userService, _mapper, _configuration) =
            (articleService, categoryService, historyService, positivityService, roleService, sourceService, userService, mapper, configuration);

        [HttpGet]
        public async Task<IActionResult> Index(ArticleFilterDTO filter, ArticleSort sortOrder, string searchString, int page = 1)
        {
            return await DisplayArticleView(nameof(Index), filter, sortOrder, searchString, page);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Grid(ArticleFilterDTO filter, ArticleSort sortOrder, string searchString, int page = 1)
        {
            return await DisplayArticleView(nameof(Grid), filter, sortOrder, searchString, page);
        }

        private async Task<IActionResult> DisplayArticleView(string viewName, ArticleFilterDTO filter, ArticleSort sortOrder, string searchString, int page = 1)
        {
            try
            {
                var userEmail = HttpContext?.User?.Identity?.Name;

                if (string.IsNullOrEmpty(HttpContext.Request.QueryString.Value))
                {
                    if (HttpContext.User.Identity.IsAuthenticated && !(await _roleService.IsAdminAsync(userEmail)))
                    {
                        var userId = await _userService.GetIdByEmailAsync(userEmail);
                        await _articleService.SetPreferenceInFilterAsync(userId, filter);
                    }
                    else
                    {
                        await _articleService.SetDefaultFilterAsync(filter);
                    }
                }

                var articles = await _articleService.GetPreviewsByPageAsync(page, COUNT_PER_PAGE, filter, sortOrder, searchString);

                var model = new HomePageVM()
                {
                    Articles = articles,
                    FilterData = new ArticleFilterDataVM
                    {
                        Categories = (await _categoryService.GetAllOrderedAsync()).Select(c => new SelectListItem(c.Title, c.Id.ToString(), filter.CategoriesId.Contains(c.Id))),
                        Positivities = (await _positivityService.GetAllOrderedAsync()).Select(r => new SelectListItem(r.Title, r.Id.ToString(), filter.MinPositivity.Equals(r.Id))),
                        Sources = (await _sourceService.GetAllAsync()).Select(s => new SelectListItem(s.Name, s.Id.ToString(), filter.SourcesId.Contains(s.Id))),
                        CurrentFilter = filter,
                    },
                    SearchString = searchString,
                    SortOrder = sortOrder,
                    PagingInfo = new PagingInfo()
                    {
                        TotalItems = await _articleService.GetTotalCount(filter, searchString),
                        CurrentPage = page,
                        ItemsPerPage = COUNT_PER_PAGE,
                    },
                    IsAdmin = await _roleService.IsAdminAsync(userEmail),
                };

                return View(viewName, model);
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
                var categories = await _categoryService.GetAllOrderedAsync();
                var sources = await _sourceService.GetAllAsync();
                var ratings = await _positivityService.GetAllOrderedAsync();

                var model = new CreateOrEditArticleVM()
                {
                    Categories = categories.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString())),
                    Sources = sources.Select(dto => new SelectListItem(dto.Name, dto.Id.ToString())),
                    Ratings = ratings.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString())),
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
        public async Task<IActionResult> Create(CreateOrEditArticleVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _articleService.CreateAsync(_mapper.Map<CreateOrEditArticleDTO>(model));
                    return RedirectToAction(nameof(Grid));
                }

                var categories = await _categoryService.GetAllOrderedAsync();
                var sources = await _sourceService.GetAllAsync();
                var ratings = await _positivityService.GetAllOrderedAsync();

                model.Categories = categories.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()));
                model.Sources = sources.Select(dto => new SelectListItem(dto.Name, dto.Id.ToString()));
                model.Ratings = ratings.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()));

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
                    var result = await _historyService.AddOrUpdateArticleInHistoryAsync(id, HttpContext.User.Identity.Name);
                }

                var dto = isAuthenticated
                    ? await _articleService.GetWithCommentsByIdAsync(id)
                    : await _articleService.GetByIdAsync(id);

                var isAdmin = await _roleService.IsAdminAsync(HttpContext.User.Identity.Name);

                var model = _mapper.Map<ArticleDetailsVM>(dto);
                model.IsAdmin = isAdmin;
                model.IsAuthenticated = isAuthenticated;

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
                var model = _mapper.Map<CreateOrEditArticleVM>(dto);

                var categories = await _categoryService.GetAllOrderedAsync();
                var sources = await _sourceService.GetAllAsync();
                var ratings = await _positivityService.GetAllOrderedAsync();

                model.Categories = categories.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()));
                model.Sources = sources.Select(dto => new SelectListItem(dto.Name, dto.Id.ToString()));
                model.Ratings = ratings.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()));

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
        public async Task<IActionResult> Edit(CreateOrEditArticleVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<CreateOrEditArticleDTO>(model);
                    var result = await _articleService.UpdateAsync(model.Id, dto);
                    return RedirectToAction(nameof(Grid));
                }

                var categories = await _categoryService.GetAllOrderedAsync();
                var sources = await _sourceService.GetAllAsync();
                var ratings = await _positivityService.GetAllOrderedAsync();

                model.Categories = categories.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()));
                model.Sources = sources.Select(dto => new SelectListItem(dto.Name, dto.Id.ToString()));
                model.Ratings = ratings.Select(dto => new SelectListItem(dto.Title, dto.Id.ToString()));

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

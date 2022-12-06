using AutoMapper;
using by.Reba.Application.Models.Preference;
using by.Reba.Business.ServicesImplementations;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.UserPreference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using Serilog.Events;

namespace by.Reba.Application.Controllers
{
    [Authorize]
    public class PreferenceController : Controller
    {
        private const int COUNT_PER_PAGE = 15;

        private readonly IPreferenceService _preferenceService;
        private readonly IPositivityService _positivityService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public PreferenceController(
            IPreferenceService preferenceService,
            IPositivityService positivityService,
            ICategoryService categoryService,
            IUserService userService,
            IMapper mapper) =>

            (_preferenceService, _positivityService, _categoryService, _userService, _mapper) =
            (preferenceService, positivityService, categoryService, userService, mapper);

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var dto = await _preferenceService.GetPreferenceByUserEmailAsync(HttpContext?.User?.Identity?.Name);
                var model = _mapper.Map<EditPreferenceVM>(dto);

                var ratings = await _positivityService.GetAllOrderedAsync();
                var categories = await _categoryService.GetAllOrderedAsync();

                model.AllCategories = categories.Select(c => new SelectListItem(c.Title, c.Id.ToString(), model.CategoriesId.Contains(c.Id)));
                model.AllRatings = ratings.Select(r => new SelectListItem(r.Title, r.Id.ToString(), model.RatingId.Equals(r.Id)));

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditPreferenceVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<PreferenceDTO>(model);
                    var result = await _preferenceService.UpdateAsync(model.Id ,dto);

                    return RedirectToAction("Index", "Article");
                }

                var ratings = await _positivityService.GetAllOrderedAsync();
                var categories = await _categoryService.GetAllOrderedAsync();

                model.AllCategories = categories.Select(c => new SelectListItem(c.Title, c.Id.ToString(), model.CategoriesId.Contains(c.Id)));
                model.AllRatings = ratings.Select(r => new SelectListItem(r.Title, r.Id.ToString(), model.RatingId.Equals(r.Id)));

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }
    }
}

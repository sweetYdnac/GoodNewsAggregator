using AutoMapper;
using by.Reba.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;
using by.Reba.Application.Models.Source;
using by.Reba.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using by.Reba.Core.DataTransferObjects.Source;

namespace by.Reba.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SourceController : Controller
    {
        private const int COUNT_PER_PAGE = 15;

        private readonly ISourceService _sourceService;
        private readonly IMapper _mapper;

        public SourceController(
            ISourceService sourceService,
            IMapper mapper) =>

            (_sourceService, _mapper) =
            (sourceService, mapper);

        [HttpGet]
        public async Task<IActionResult> Grid(string searchString, int page = 1)
        {
            try
            {
                var model = new SourcesGridVM()
                {
                    Sources = await _sourceService.GetAllByFilterAsync(page, COUNT_PER_PAGE, searchString),
                    SearchString = searchString,
                    PagingInfo = new PagingInfo()
                    {
                        TotalItems = await _sourceService.GetTotalCountAsync(searchString),
                        CurrentPage = page,
                        ItemsPerPage = COUNT_PER_PAGE
                    }
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
        public IActionResult Create()
        {
            try
            {
                var types = Enum.GetValues(typeof(ArticleSource));

                var model = new CreateOrEditSourceVM()
                {
                    SourceTypes = types.OfType<ArticleSource>().ToArray().Select(s => new SelectListItem() { Text = s.ToString(), Value = ((int)s).ToString() })
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
        public async Task<IActionResult> Create(CreateOrEditSourceVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _sourceService.CreateAsync(_mapper.Map<CreateOrEditSourceDTO>(model));
                    return RedirectToAction(nameof(Grid));
                }

                var types = Enum.GetValues(typeof(ArticleSource));
                model.SourceTypes = types.OfType<ArticleSource>().ToArray().Select(s => new SelectListItem() { Text = s.ToString(), Value = ((int)s).ToString() });

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var dto = await _sourceService.GetCreateOrEditDTObyIdAsync(id);
                var model = _mapper.Map<CreateOrEditSourceVM>(dto);

                var types = Enum.GetValues(typeof(ArticleSource));
                model.SourceTypes = types.OfType<ArticleSource>().ToList().Select(s => new SelectListItem() { Text = s.ToString(), Value = ((int)s).ToString() });

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateOrEditSourceVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<CreateOrEditSourceDTO>(model);
                    await _sourceService.UpdateAsync(model.Id, dto);
                    return RedirectToAction(nameof(Grid));
                }

                var types = Enum.GetValues(typeof(ArticleSource));
                model.SourceTypes = types.OfType<ArticleSource>().ToList().Select(s => new SelectListItem() { Text = s.ToString(), Value = ((int)s).ToString() });

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _sourceService.RemoveAsync(id);
                return RedirectToAction(nameof(Grid));
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }
    }
}

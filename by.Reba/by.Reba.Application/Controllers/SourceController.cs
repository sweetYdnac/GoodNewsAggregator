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
            IMapper mapper)
        {
            _sourceService = sourceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Grid(string searchString, int page = 1)
        {
            try
            {
                var model = new SourcesGridVM()
                {
                    Sources = await _sourceService.GetSourcesGridAsync(page, COUNT_PER_PAGE, searchString),
                    SearchString = searchString,
                    PagingInfo = new PagingInfo()
                    {
                        TotalItems = await _sourceService.GetTotalCount(searchString),
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
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CreateOrEditSourceVM()
                {
                    SourceTypes = (await _sourceService.GetAllAsync()).Select(s => new SelectListItem() { Text = s.Name, Value = ((int)s.SourceType).ToString() })
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
                    var result = _sourceService.CreateAsync(_mapper.Map<CreateOrEditSourceDTO>(model));
                    return RedirectToAction(nameof(Grid));
                }

                model.SourceTypes = (await _sourceService.GetAllAsync()).Select(s => new SelectListItem() { Text = s.Name, Value = s.SourceType.ToString() });
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

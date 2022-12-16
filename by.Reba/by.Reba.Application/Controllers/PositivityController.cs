using AutoMapper;
using by.Reba.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;
using by.Reba.Application.Models.Source;
using by.Reba.Application.Models.PositivityRating;
using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositivityController : Controller
    {
        private readonly IPositivityService _positivityService;
        private readonly IMapper _mapper;

        public PositivityController(
            IPositivityService positivityService,
            IMapper mapper) =>

            (_positivityService, _mapper) =
            (positivityService, mapper);

        [HttpGet]
        public async Task<IActionResult> Grid()
        {
            try
            {
                var model = new PositivityGridVM()
                {
                    Ratings = await _positivityService.GetAllOrderedAsync(),
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
                var model = new CreateOrEditPositivityVM();
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrEditPositivityVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _positivityService.CreateAsync(_mapper.Map<PositivityDTO>(model));
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
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var dto = await _positivityService.GetByIdAsync(id);
                var model = _mapper.Map<CreateOrEditPositivityVM>(dto);

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateOrEditPositivityVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<PositivityDTO>(model);
                    await _positivityService.UpdateAsync(model.Id, dto);

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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _positivityService.RemoveAsync(id);
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

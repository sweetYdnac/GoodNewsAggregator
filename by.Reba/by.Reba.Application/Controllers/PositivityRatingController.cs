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
using by.Reba.Application.Models.PositivityRating;
using by.Reba.Core.DataTransferObjects.PositivityRating;

namespace by.Reba.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositivityRatingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPositivityRatingService _positivityRatingService;

        public PositivityRatingController(
            IMapper mapper,
            IPositivityRatingService positivityRatingService)
        {
            _mapper = mapper;
            _positivityRatingService = positivityRatingService;
        }

        [HttpGet]
        public async Task<IActionResult> Grid()
        {
            try
            {
                var model = new PositivityRatingsGridVM()
                {
                    Ratings = await _positivityRatingService.GetAllOrderedAsync(),
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
                    var result = await _positivityRatingService.CreateAsync(_mapper.Map<PositivityRatingDTO>(model));

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
                var dto = await _positivityRatingService.GetByIdAsync(id);
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
                    var dto = _mapper.Map<PositivityRatingDTO>(model);
                    var result = await _positivityRatingService.UpdateAsync(model.Id, dto);

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
                var result = await _positivityRatingService.RemoveAsync(id);
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

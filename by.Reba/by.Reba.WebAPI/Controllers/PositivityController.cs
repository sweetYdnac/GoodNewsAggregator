using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.WebAPI.Models.Requests.Positivity;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Models.Responces.Positivity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with positivities
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PositivitiesController : ControllerBase
    {
        private readonly IPositivityService _positivityService;
        private readonly IMapper _mapper;

        public PositivitiesController(IPositivityService positivityService, IMapper mapper) => 
            (_positivityService, _mapper) = (positivityService, mapper);

        /// <summary>
        /// Get Positivity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PositivityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPositivity([FromRoute]Guid id)
        {
            try
            {
                var positivity = await _positivityService.GetByIdAsync(id);
                return Ok(positivity);
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                return NotFound(new ErrorModel() { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Get positivities collection
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetPositivitiesResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPositivities()
        {
            try
            {
                var positivities = await _positivityService.GetAllOrderedAsync();
                return Ok(new GetPositivitiesResponseModel() { Positivities = positivities });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }

        }

        /// <summary>
        /// Create positivity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePositivity([FromBody]CreatePositivityRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<PositivityDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from CreatePositivityRequestModel to PositivityDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });

                }

                await _positivityService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetPositivity), new { id = dto.Id }, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Patch specific positivity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchPositivity([FromBody]PatchPositivityRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<PositivityDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from PatchPositivityRequestModel to PositivityDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                await _positivityService.UpdateAsync(request.Id, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                return NotFound(new ErrorModel() { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }

        /// <summary>
        /// Delete positivity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePositivity([FromRoute]Guid id)
        {
            try
            {
                await _positivityService.RemoveAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                return NotFound(new ErrorModel() { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }
        }
    }
}

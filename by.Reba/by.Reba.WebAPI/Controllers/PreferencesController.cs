using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.WebAPI.Models.Requests.Preference;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Models.Responces.Preference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with user preference resource
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PreferencesController(
            IPreferenceService preferenceService,
            IMapper mapper,
            IConfiguration configuration) =>

            (_preferenceService, _mapper, _configuration) = (preferenceService, mapper, configuration);

        /// <summary>
        /// Get preference by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetPreferenceResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPreference([FromRoute] Guid id)
        {
            try
            {
                var preference = await _preferenceService.GetPreferenceByIdAsync(id);
                return Ok(new GetPreferenceResponseModel() { Preference = preference });
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
        /// Create preference for specific user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var id = HttpContext.User.Claims.Where(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Select(c => c.Value).FirstOrDefault();
                var role = HttpContext.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(c => c.Value).FirstOrDefault();

                if (id is null || role is null)
                {
                    var message = "Token does not have correct credentials";

                    Log.Error(message);
                    return Unauthorized( new ErrorModel() { Message = message });
                }

                if (!role.Equals(_configuration["Roles:Admin"]) && !id.Equals(request.UserId))
                {
                    var message = $"User with id = {id} do not have access right to create preference for user with id = {request.UserId}";

                    Log.Error(message);
                    return StatusCode(403, new ErrorModel() { Message = message });
                }              

                var dto = _mapper.Map<PreferenceDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from CreatePreferenceRequestModel to PreferenceDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                await _preferenceService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetPreference), new { id = dto.Id }, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return StatusCode(500, new ErrorModel() { Message = ex.Message });
            }          
        }

        /// <summary>
        /// Patch preference
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchPreference([FromBody] PatchPreferenceRequestModel request)
        {
            try
            {
                if (request is null)
                {
                    var message = "Request model is null";

                    Log.Error(message);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                var dto = _mapper.Map<PreferenceDTO>(request);

                if (dto is null)
                {
                    var message = "Invalid mapping from PatchPreferenceRequestModel to PreferenceDTO";

                    Log.Error(message);
                    return StatusCode(500, new ErrorModel() { Message = message });
                }

                await _preferenceService.UpdateAsync(request.Id, dto);
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

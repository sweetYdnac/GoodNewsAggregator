using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.WebAPI.Models.Requests.Preference;
using by.Reba.WebAPI.Models.Responces;
using by.Reba.WebAPI.Models.Responces.Preference;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with user preference resource
    /// </summary>
    [Authorize()]
    [ApiController]
    [Route("api/[controller]")]
    public class PreferenceController : ControllerBase
    {
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
            IMapper mapper
            )
        {
            _preferenceService = preferenceService;
            _positivityService = positivityService;
            _categoryService = categoryService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get preference from a specific user by email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(GetPreferenceResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPreference(GetPreferenceRequestModel request)
        {
            if (request is null)
            {
                return BadRequest(new ErrorModel() { Message = "Request model is null" });
            }

            var preference = await _preferenceService.GetPreferenceByEmailAsync(request.Email);

            return preference is null
                ? NotFound(new ErrorModel() { Message = $"User with email = { request.Email } doesn't have preference." })
                : Ok(new GetPreferenceResponseModel() { Preference = preference });
        }


        [HttpPost]
        public async Task<IActionResult> CreatePreference(CreatePreferenceRequestModel request)
        {
            var dto = _mapper.Map<PreferenceDTO>(request);

            if (dto is null)
            {
                return BadRequest(new ErrorModel() { Message = "Recieved invalid request model" });
            }

            dto.UserId = await _userService.GetIdByEmailAsync(request.Email);

            var result = await _preferenceService.CreateAsync(dto);

            return result == 0
                ? StatusCode(500, new ErrorModel() { Message = "Preference doesn't created" })
                : CreatedAtAction(nameof(GetPreference), new GetPreferenceRequestModel() { Email = request.Email}, dto);

            // TODO: Добавить в TokenResponce UserId????? и горя не знать...
        }
    }
}

using AutoMapper;
using by.Reba.Application.Models.Account;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using by.Reba.Core.DataTransferObjects.UserPreference;

namespace by.Reba.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICategoryService _categoryService;
        private readonly IPositivityRatingService _positivityRatingService;
        private readonly IUserPreferenceService _userPreferenceService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AccountController(
            IUserService userService,
            IMapper mapper,
            IRoleService roleService,
            IConfiguration configuration,
            ICategoryService categoryService,
            IPositivityRatingService positivityRatingService,
            IUserPreferenceService userPreferenceService)
        {
            _userService = userService;
            _mapper = mapper;
            _roleService = roleService;
            _configuration = configuration;
            _categoryService = categoryService;
            _positivityRatingService = positivityRatingService;
            _userPreferenceService = userPreferenceService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var isPasswordCorrect = await _userService.CheckUserPasswordAsync(model.Email, model.Password);
                if (isPasswordCorrect)
                {
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Article");
                }
            }

            ModelState.AddModelError(nameof(model.Password), "Логин или пароль не верны.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var userRoleId = await _roleService.GetRoleIdByNameAsync(_configuration["Roles:User"]);
                var userDTO = _mapper.Map<UserDTO>(model);

                if (userDTO != null && userRoleId != null)
                {
                    userDTO.RoleId = userRoleId.Value;
                    var result = await _userService.RegisterUserAsync(userDTO);

                    if (result > 0)
                    {
                        await Authenticate(model.Email);
                        return RedirectToAction("Index", "Article");
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Article");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyNickname(string nickname)
        {
            var isExist = await _userService.VerifyNicknameAsync(nickname);

            return isExist ? Json($"Никнейм {nickname} уже используется.")
                           : Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var isExist = await _userService.VerifyEmailAsync(email);

            return isExist ? Json($"Почта {email} уже используется.")
                           : Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> NavigationUserPreview()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity?.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest();
                }

                var user = _mapper.Map<UserNavigationPreviewVM>(await _userService.GetUserNavigationByEmailAsync(userEmail));
                return View(user);
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var currentUserEmail = HttpContext?.User?.Identity?.Name;
                var currentUserId = await _userService.GetIdByEmailAsync(currentUserEmail);
                var dto = await _userService.GetUserDetailsByEmailAsync(id);

                var model = _mapper.Map<UserDetailsVM>(dto);
                model.IsAdmin = await _roleService.IsAdminAsync(currentUserEmail);
                model.IsSelf = id.Equals(currentUserId);

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
        public async Task<IActionResult> Edit()
        {
            try
            {
                var dto = await _userService.GetEditUserDTOByEmailAsync(HttpContext?.User?.Identity?.Name);
                var model = _mapper.Map<EditUserVM>(dto);

                var categories = await _categoryService.GetAllAsync();
                var ratings = await _positivityRatingService.GetAllOrderedAsync();

                model.Categories = categories.Select(c => new SelectListItem(c.Title, c.Id.ToString(), model.CategoriesId.Contains(c.Id))).AsEnumerable();
                model.PositivityRatings = ratings.Select(r => new SelectListItem(r.Title, r.Id.ToString(), model.RatingId.Equals(r.Id))).AsEnumerable();

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<EditUserDTO>(model);
                    var result = await _userService.UpdateAsync(model.Id, dto);
                    return RedirectToAction(nameof(Details), new {userEmail = model.Email});
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
        [Authorize]
        public async Task<IActionResult> CreatePreference()
        {
            try
            {
                var ratings = await _positivityRatingService.GetAllOrderedAsync();
                var firstRating = ratings.FirstOrDefault();

                var categories = await _categoryService.GetAllAsync();

                var model = new CreateUserPreferenceVM()
                {
                    AllCategories = categories.Select(c => new SelectListItem() { Text = c.Title, Value = c.Id.ToString() }).ToList(),
                    AllRatings = ratings.Select(r => new SelectListItem() { Text = r.Title, Value = r.Id.ToString(), Selected = r.Id.Equals(firstRating?.Id) }).ToList(),
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
        [Authorize]
        public async Task<IActionResult> CreatePreference(CreateUserPreferenceVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<UserPreferenceDTO>(model);
                    dto.UserId = await _userService.GetIdByEmailAsync(HttpContext?.User?.Identity?.Name);
                    var result = await _userPreferenceService.CreateAsync(dto);

                    return RedirectToAction("Index", "Article");
                }

                var ratings = await _positivityRatingService.GetAllOrderedAsync();
                var firstRating = ratings.FirstOrDefault();

                var categories = await _categoryService.GetAllAsync();

                model.AllCategories = categories.Select(c => new SelectListItem() { Text = c.Title, Value = c.Id.ToString() });
                model.AllRatings = ratings.Select(r => new SelectListItem() { Text = r.Title, Value = r.Id.ToString(), Selected = r.Id.Equals(firstRating?.Id) });

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        private async Task Authenticate(string email)
        {
            var userDTO = await _userService.GetUserByEmailAsync(email);

            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userDTO.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userDTO.RoleName)
            };

            var identity = new ClaimsIdentity(claims, 
                "ApplicationCookie", 
                ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(identity));
        }
    }
}

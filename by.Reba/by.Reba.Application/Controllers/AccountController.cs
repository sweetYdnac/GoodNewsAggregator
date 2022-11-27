using AutoMapper;
using by.Reba.Application.Models.Account;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.Core.SortTypes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using Serilog.Events;
using System.Security.Claims;

namespace by.Reba.Application.Controllers
{
    public class AccountController : Controller
    {
        private const int COUNT_PER_PAGE = 15;

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICategoryService _categoryService;
        private readonly IPositivityService _positivityService;
        private readonly IPreferenceService _preferenceService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountController(
            IUserService userService,
            IMapper mapper,
            IRoleService roleService,
            IConfiguration configuration,
            ICategoryService categoryService,
            IPositivityService positivityService,
            IPreferenceService preferenceService) =>

            (_userService, _roleService, _categoryService, _positivityService, _preferenceService, _mapper, _configuration) =
            (userService, roleService, categoryService, positivityService, preferenceService, mapper, configuration);

        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
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
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userRoleId = await _roleService.GetRoleIdByNameAsync(_configuration["Roles:User"]);
                    var userDTO = _mapper.Map<UserDTO>(model);

                    if (userDTO is not null && userRoleId is not null)
                    {
                        userDTO.RoleId = userRoleId.Value;
                        var result = await _userService.RegisterUserAsync(userDTO);

                        if (result > 0)
                        {
                            await Authenticate(model.Email);

                            var userId = await _userService.GetIdByEmailAsync(model.Email);
                            await _preferenceService.CreateDefaultPreferenceAsync(userId);

                            return RedirectToAction(nameof(CreatePreference));
                        }
                    }
                }

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
        public new async Task<IActionResult> SignOut()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Article");
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyNickname(string nickname)
        {
            try
            {
                var isExist = await _userService.IsNicknameExistAsync(nickname);

                return isExist ? Json($"Никнейм {nickname} уже используется.")
                               : Json(true);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            try
            {
                var isExist = await _userService.IsEmailExistAsync(email);

                return isExist ? Json($"Почта {email} уже используется.")
                               : Json(true);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> NavigationUserPreview()
        {
            try
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
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
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

                var categories = await _categoryService.GetAllOrderedAsync();
                var ratings = await _positivityService.GetAllOrderedAsync();

                model.Categories = categories.Select(c => new SelectListItem(c.Title, c.Id.ToString(), model.CategoriesId.Contains(c.Id)));
                model.Ratings = ratings.Select(r => new SelectListItem(r.Title, r.Id.ToString(), model.MinPositivity.Equals(r.Id)));

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
                var userEmail = HttpContext?.User?.Identity?.Name;

                if (await _userService.IsNicknameExistAsync(model.Nickname, userEmail))
                {
                    ModelState.AddModelError(nameof(model.Nickname), "Никнейм уже используется");
                }

                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<EditUserDTO>(model);
                    var result = await _userService.UpdateAsync(model.Id, dto);
                    return RedirectToAction(nameof(Details), new { id = model.Id });
                }

                var categories = await _categoryService.GetAllOrderedAsync();
                var ratings = await _positivityService.GetAllOrderedAsync();

                model.Categories = categories.Select(c => new SelectListItem(c.Title, c.Id.ToString(), model.CategoriesId.Contains(c.Id)));
                model.Ratings = ratings.Select(r => new SelectListItem(r.Title, r.Id.ToString(), model.MinPositivity.Equals(r.Id)));

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
                var ratings = await _positivityService.GetAllOrderedAsync();
                var firstRating = ratings.FirstOrDefault();

                var categories = await _categoryService.GetAllOrderedAsync();

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
                    var dto = _mapper.Map<PreferenceDTO>(model);
                    dto.UserId = await _userService.GetIdByEmailAsync(HttpContext?.User?.Identity?.Name);
                    var result = await _preferenceService.CreateAsync(dto);

                    return RedirectToAction("Index", "Article");
                }

                var ratings = await _positivityService.GetAllOrderedAsync();
                var firstRating = ratings.FirstOrDefault();

                var categories = await _categoryService.GetAllOrderedAsync();

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Grid(UserSort sortOrder, string searchString, int page = 1)
        {
            try
            {
                var model = new UsersGridVM()
                {
                    Users = await _userService.GetUsersGridAsync(page, COUNT_PER_PAGE, sortOrder, searchString),
                    SearchString = searchString,
                    SortOrder = sortOrder,
                    PagingInfo = new PagingInfo()
                    {
                        TotalItems = await _userService.GetTotalCountAsync(searchString),
                        CurrentPage = page,
                        ItemsPerPage = COUNT_PER_PAGE,
                    },
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _userService.RemoveAsync(id);
                return RedirectToAction(nameof(Grid));
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

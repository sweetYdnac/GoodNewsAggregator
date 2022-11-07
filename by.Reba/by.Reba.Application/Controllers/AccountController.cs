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

namespace by.Reba.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AccountController(
            IUserService userService,
            IMapper mapper,
            IRoleService roleService,
            IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _roleService = roleService;
            _configuration = configuration;
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
        public async Task<IActionResult> Details()
        {
            try
            {
                var email = HttpContext?.User?.Identity?.Name;
                var dto = await _userService.GetUserDetailsByEmailAsync(email);
                var model = _mapper.Map<UserDetailsVM>(dto);
                model.IsAdmin = await _roleService.IsAdminAsync(HttpContext?.User?.Identity?.Name);

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

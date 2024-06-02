using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Greatmarch.Beer.API.Auth.Models.ViewModel;
using Greatmarch.Beer.Model.Entities;
using Greatmarch.Beer.DomainService.Users.Interface;

namespace Greatmarch.Beer.API.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IJWTService _JWTService;
        private readonly IUserService _userService;

        public AuthController(ILogger<AuthController> logger, IJWTService JWTService, IUserService userService)
        {
            _logger = logger;
            _JWTService = JWTService;
            _userService = userService;
        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserCredentialsViewModel userCredentials)
        {
            var user = await _userService.GetByEmailAsync(userCredentials.Email);
            var tokenResult = await _JWTService.AuthAsync(userCredentials.Email, userCredentials.Password);
            if (tokenResult != "401")
            {
                HttpContext.Response.Cookies.Append(
                   "Reporting-Endpoints.AspNetCore.Application.API",
                   tokenResult,
                   new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });
                return Ok(new { id = user.Id, first_name = user.FirstName, last_name = user.LastName, email = user.Email });
            }
            else
                return Unauthorized("Авторизируйся нормально блядота!");
        }

        //[HttpPost("/addUser")]
        //[Authorize]
        //public async Task<IActionResult> AddUser(User user)
        //{
        //    await _JWTService.RegistrationAsync(user);
        //    return Ok();
        //}

        [Authorize]
        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}
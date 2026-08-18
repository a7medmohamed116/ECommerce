using AdminDashBoard.Services;
using ECommerce.Application.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdminDashBoard.Controllers
{
    public class AdminController : Controller
    {
        private readonly AuthenticationApiClient _authenticationApiClient;

        public AdminController(AuthenticationApiClient authenticationApiClient)
        {
            _authenticationApiClient = authenticationApiClient;
        }

         
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
            var result = await _authenticationApiClient.LoginAsync(loginDto);

            if (result is null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid Email Or Password");

                return View(loginDto);
            }

            HttpContext.Session.SetString("AccessToken", result.Token);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, result.Email),
                new Claim(ClaimTypes.Name, result.DisplayName)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);


            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("AccessToken");
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
    }
}

using AdminDashBoard.Services;
using ECommerce.Application.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Mvc;

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
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AccessToken");

            return RedirectToAction(nameof(Login));
        }
    }
}

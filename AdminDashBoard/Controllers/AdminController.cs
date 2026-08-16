using ECommerce.Application.DTOs.IdentityDTOs;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashBoard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
        }

         
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid Login attpempt");
                return View(loginDto);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password,false,false);
            if (!result.Succeeded || (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))){
                ModelState.AddModelError("", "You are not authorized");
                return View(loginDto);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}

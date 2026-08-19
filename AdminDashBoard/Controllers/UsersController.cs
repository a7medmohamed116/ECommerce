using AdminDashBoard.Models.Roles;
using AdminDashBoard.Models.Users;
using AdminDashBoard.Services;
using ECommerce.Application.DTOs.RolesDto;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace AdminDashBoard.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserApiClient _userApiClient;

        public UsersController(UserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userApiClient.GetAllUsersAsync();
            var model = result.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles =u.Roles
            }).ToList();

            return View(model);
        }



        public async Task<IActionResult> Edit(string id,CancellationToken ct = default)
        {
            var user = await _userApiClient.GetUserForEditAsync(id, ct);

            if (user is null)
                return NotFound();

            var model = new UserRoleViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Roles = user.Roles.Select(r => new UpdateRoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = r.IsSelected
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleDto model,CancellationToken ct = default)
        {
            await _userApiClient.UpdateUserRolesAsync(
                model.UserId,
                model,
                ct);

            return RedirectToAction(nameof(Index));
        }
    }
}

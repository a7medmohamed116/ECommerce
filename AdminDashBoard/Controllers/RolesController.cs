using AdminDashBoard.Models.Roles;
using AdminDashBoard.Services;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.RolesDto;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDashBoard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleApiClient _roleApiClient;

        public RolesController(RoleApiClient roleApiClient)
        {
            _roleApiClient = roleApiClient;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var roles = await _roleApiClient.GetAllAsync(ct);
            var model = roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Createo( RoleViewModel model, CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleApiClient.CreateAsync(new CreateRoleDto() { Name = model.Name }, ct);


                if (result)
                    return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("Name", "Role ALready Exist");
            var roles = await _roleApiClient.GetAllAsync(ct);
            var roleModels = roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            return View(nameof(Index), roleModels);
        }
        // RedirectToAction("Index") makes a new HTTP request to the Index action. return View(nameof(Index), roles) renders the View directly within the current request.
        //That's why, in the error case, you need to pass the roles yourself to the View.
        public async Task<IActionResult> Deleteo(string id,CancellationToken ct = default)
        {
            await _roleApiClient.DeleteAsync(id, ct);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string id,CancellationToken ct = default)
        {
            var role = await _roleApiClient.GetByIdAsync(id, ct);

            if (role is null)
                return RedirectToAction(nameof(Index));

            var model = new UpdateRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleViewModel model,CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleApiClient.UpdateAsync(
                    model.Id,
                    new RoleDto
                    {
                        Id = model.Id,
                        Name = model.Name
                    },
                    ct);

                if (result)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}

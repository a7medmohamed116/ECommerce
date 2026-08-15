using AdminDashBoard.Models.Roles;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDashBoard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager; 

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles =await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isexit = await _roleManager.RoleExistsAsync(model.Name);
                if (!isexit)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Name", "Role Already Exist");
            }
            return View(nameof(Index), await _roleManager.Roles.ToListAsync());// RedirectToAction("Index") makes a new HTTP request to the Index action. return View(nameof(Index), roles) renders the View directly within the current request.
            //That's why, in the error case, you need to pass the roles yourself to the View.
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role is not null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role is null)
            {
                ModelState.AddModelError("Id", "No Role with this id");
                return RedirectToAction("Index");
            }
            var updateRoleViewModel = new UpdateRoleViewModel() { Id = id, Name = role.Name! };

            return View(updateRoleViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleexist = await _roleManager.RoleExistsAsync(model.Name); 
                if (!roleexist)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
                    if (role is not null)
                    {
                        role.Name = model.Name;
                        await _roleManager.UpdateAsync(role);
                        return RedirectToAction("Index");
                    }
                    
                }

            }
            ModelState.AddModelError("Name", "Role Already Exist");
            return View(model);



        }
    }
}

using AdminDashBoard.Models.Roles;
using AdminDashBoard.Models.Users;
using AdminDashBoard.Services;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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



        //public async Task<IActionResult> Edit(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    var roles = await _roleManager.Roles.ToListAsync();
        //    var userModel = new UserRoleViewModel()
        //    {
        //        UserId = user.Id,
        //        UserName = user.UserName,
        //        Roles = roles.Select(r => new UpdateRoleViewModel
        //        {
        //            Id = r.Id,
        //            Name = r.Name,
        //            IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
        //        }).ToList()
        //    };
        //    return View(userModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(UserRoleViewModel model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.UserId);
        //    var rolesforuser = await _userManager.GetRolesAsync(user);
        //    //role was granted -> i will  unckeck for this role and i will remove it from the user


        //    //role was not granted -> i will check for this role and i will add it to the user

        //    foreach (var role in model.Roles) //all roles in system
        //    {
        //        if(rolesforuser.Any(r=>r==role.Name) && !role.IsSelected)
        //            await _userManager.RemoveFromRoleAsync(user, role.Name);
        //        if (!rolesforuser.Any(r => r == role.Name) && role.IsSelected)
        //            await _userManager.AddToRoleAsync(user, role.Name);
        //    }
        //    return RedirectToAction("Index");
        //}

    }
}

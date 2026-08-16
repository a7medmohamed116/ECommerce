using AdminDashBoard.Models.Roles;
using AdminDashBoard.Models.Users;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly StoreIdentityDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager ,RoleManager<IdentityRole> roleManager,StoreIdentityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users.ToListAsync();
            var uroles = await _context.UserRoles.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            var result = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,

                Roles = uroles
                .Where(ur => ur.UserId == user.Id)
                .Join(
                    roles,
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => role.Name
                ).ToList()
                     }).ToList();



            //var users = await _userManager.Users.Select( u => new UserViewModel
            //{
            //    Id = u.Id,
            //    DisplayName = u.DisplayName,
            //    UserName = u.UserName,
            //    Email = u.Email,
            //    Roles =  _userManager.GetRolesAsync(u).Result.ToList() //Result blocks the current thread until GetRolesAsync(u) completes. // data reader conflict must add multipile Active result sets = true in data base connection
            //}).ToListAsync();
            return View(result);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var userModel = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => new UpdateRoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var rolesforuser = await _userManager.GetRolesAsync(user);
            //role was granted -> i will  unckeck for this role and i will remove it from the user


            //role was not granted -> i will check for this role and i will add it to the user

            foreach (var role in model.Roles) //all roles in system
            {
                if(rolesforuser.Any(r=>r==role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!rolesforuser.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction("Index");
        }

    }
}

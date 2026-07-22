using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.DataSeeding
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync(ct);
                }
                if (!await _roleManager.Roles.AnyAsync(ct))
                {
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!await _userManager.Users.AnyAsync(ct))
                {
                    var SuperAdmin = new ApplicationUser()
                    {
                        DisplayName = "AhmedMohamed",
                        Email = "ahmed@gmail.com",
                        UserName = "A7medMAli",
                        PhoneNumber = "01020617381"
                    };


                    var CreatedAdmin = await _userManager.CreateAsync(SuperAdmin, "P@ssw0rd");
                    if (CreatedAdmin.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");
                    }
                    else
                    {
                        Console.WriteLine("Can't Assign Role To Admin");
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }

        }

    }//after seeding here register in infrastrucure register then in webapp extensions then tied dbstore with identitydbstore in infrastructure by addidentitycore
}

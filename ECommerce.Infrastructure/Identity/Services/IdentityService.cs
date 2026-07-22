using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result<bool>.Fail(Error.NotFound("Not Found Error", $"User With Email {email} Not Found"));
            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            return Result<bool>.OK(checkPassword);
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result<IdentityUserResult>.Fail(Error.NotFound("Not Found Error", $"User With Email {email} Not Found"));
            var returneduser = new IdentityUserResult(user.Id , user.DisplayName,user.Email,user.UserName);
            return Result<IdentityUserResult>.OK(returneduser);
            

            
        }
    }
}

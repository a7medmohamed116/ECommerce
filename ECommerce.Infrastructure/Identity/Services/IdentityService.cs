using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.IdentityDTOs;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
           
            return Result<bool>.OK(user is not null);

        }

        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result<bool>.Fail(Error.NotFound("Not Found Error", $"User With Email {email} Not Found"));
            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            return Result<bool>.OK(checkPassword);
        }

        public async Task<Result<IdentityUserResult>> CreateNewUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var newuser = new ApplicationUser()
            {
                Email = registerDto.Email,
                DisplayName =registerDto.DisplayName,
                PhoneNumber =registerDto.PhoneNumber,
                UserName = registerDto.UserName
               
            };
            var result = await _userManager.CreateAsync(newuser, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(X => new Error(X.Code, X.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }
            return Result<IdentityUserResult>.OK(new IdentityUserResult(newuser.Id,newuser.DisplayName,newuser.Email,newuser.UserName));
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result<IdentityUserResult>.Fail(Error.NotFound("Not Found Error", $"User With Email {email} Not Found"));
            var returneduser = new IdentityUserResult(user.Id , user.DisplayName,user.Email,user.UserName);
            return Result<IdentityUserResult>.OK(returneduser);
            

            
        }

        public async Task<Result<IdentityUserResult>> FindUserByIdAsync(string userId,CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result<IdentityUserResult>.Fail(
                    Error.NotFound("Not Found", "User not found"));

            return Result<IdentityUserResult>.OK(
                new IdentityUserResult(
                    user.Id,
                    user.DisplayName,
                    user.Email,
                    user.UserName));
        }

        public async Task<Result<IReadOnlyList<IdentityRoleResult>>> GetAllRolesAsync(CancellationToken ct = default)
        {
            var roles = await _roleManager.Roles
                .Select(r => new IdentityRoleResult(
                    r.Id,
                    r.Name!))
                .ToListAsync(ct);

            return Result<IReadOnlyList<IdentityRoleResult>>.OK(roles);
        }

        public async Task<Result<IReadOnlyList<IdentityUserResult>>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await _userManager.Users
                .Select(user => new IdentityUserResult(
                    user.Id,
                    user.DisplayName,
                    user.Email,
                    user.UserName))
                .ToListAsync(ct);

            return Result<IReadOnlyList<IdentityUserResult>>.OK(users);
        }

        public async Task<Result<AddressDto>> GetUserAddress(string email, CancellationToken ct = default)
        {
            var user = _userManager.Users.Include(X => X.Address).FirstOrDefault(X => X.Email == email);
            if (user.Address is null)
                return Result<AddressDto>.Fail(Error.NotFound("Address.NotFound", "Address not found"));
            var address = new AddressDto()
            {
                City = user.Address.City,
                Street = user.Address.Street,
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Country = user.Address.Country
            };
            return Result<AddressDto>.OK(address);
            
                
            
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRoles(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound("Not Found", "User Not Found"));
            var roles = await _userManager.GetRolesAsync(user);
            return Result<IReadOnlyList<string>>.OK(roles.ToList());


        }

        public async Task<Result<AddressDto>> UpdateAddress(AddressDto addressDto, string email, CancellationToken ct = default)
        {
            var user = _userManager.Users.Include(X => X.Address).FirstOrDefault(X => X.Email == email);
            if (user.Address == null)
            {

                user.Address = new Address
                {
                    FirstName = addressDto.FirstName,
                    LastName = addressDto.LastName,
                    Street = addressDto.Street,
                    City = addressDto.City,
                    Country = addressDto.Country

                };

            }
            else
            {
                
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                
            }
            var address = user.Address;
           
            var result = await _userManager.UpdateAsync(user);
            
            //await _dbContext.SaveChangesAsync(ct);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(X => new Error(X.Code, X.Description)).ToList();
                return Result<AddressDto>.Fail(errors);
            }
            return Result<AddressDto>.OK(new AddressDto() { City = address.City, FirstName = address.FirstName, LastName = address.LastName, Street = address.Street  ,Country = address.Country});
            

        }

        public async Task<Result<bool>> UpdateUserRolesAsync(string userId,IEnumerable<string> selectedRoles,CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result<bool>.Fail(
                    Error.NotFound("Not Found", "User not found"));

            var currentRoles = await _userManager.GetRolesAsync(user);

            var selectedRolesList = selectedRoles.ToList();

            foreach (var role in currentRoles)
            {
                if (!selectedRolesList.Contains(role))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role);

                    if (!result.Succeeded)
                    {
                        var errors = result.Errors
                            .Select(x => new Error(x.Code, x.Description))
                            .ToList();

                        return Result<bool>.Fail(errors);
                    }
                }
            }

            foreach (var role in selectedRolesList)
            {
                if (!currentRoles.Contains(role))
                {
                    var result = await _userManager.AddToRoleAsync(user, role);

                    if (!result.Succeeded)
                    {
                        var errors = result.Errors
                            .Select(x => new Error(x.Code, x.Description))
                            .ToList();

                        return Result<bool>.Fail(errors);
                    }
                }
            }

            return Result<bool>.OK(true);
        }
    }
}

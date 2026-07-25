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
        public IdentityService(UserManager<ApplicationUser> userManager )
        {
            _userManager = userManager;
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
    }
}

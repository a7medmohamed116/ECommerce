using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.IdentityDTOs;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IIdentityService _identityService;

        public AuthenticationService(IIdentityService identityService )
        {
            _identityService = identityService;
        }

        #region Good code
            
            // to much to Find user by email.Verify the password.Generate a JWT token. Return the user information. in one servic 
            // aslo will need the same validators in the register service so we will divide the serive 
            //need usermanager
            // find user by email and check email check password in new interface [IIdentityService]  return identityuserresult that initialized in common 
        #endregion



        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            //Find user by email
            var user = await _identityService.FindUserByEmailAsync(loginDto.Email, ct);
            if (!user.IsSuccess) return Result<UserDto>.Fail(user.Errors);
            //Verify the password
            var checkPassword = await _identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);
            if(!checkPassword.IsSuccess) return Result<UserDto>.Fail(checkPassword.Errors);//system errors, not "wrong password" errors
            if (!checkPassword.data) return Result<UserDto>.Fail(Error.Unauthorized("Invalid Email Or Password!"));
            //Generate a JWT token.

            //Return Data
            var returnedLoginuser = new UserDto()
            {
                Email = loginDto.Email,
                Token = "JWT",
                DisplayName = user.data.DisplayName
            };
            return Result<UserDto>.OK(returnedLoginuser);
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = await _identityService.CreateNewUserAsync(registerDto, ct);
            if (!user.IsSuccess)
            {
                return Result<UserDto>.Fail(user.Errors);
            }

            var returnedRegisteruser = new UserDto()
            {
                Email = user.data.Email,
                Token = "JWT",
                DisplayName = user.data.DisplayName
            };
            return Result<UserDto>.OK(returnedRegisteruser);


        }
    }
}

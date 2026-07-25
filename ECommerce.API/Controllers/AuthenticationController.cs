using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
             _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct = default)
            => ToActionResult(await _authenticationService.LoginAsync(loginDto, ct));


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct = default)
            => ToActionResult(await _authenticationService.RegisterAsync(registerDto, ct));

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> EmailExist([FromQuery] string email, CancellationToken ct = default)
        {
            var checkRegistredEmail = await _authenticationService.CheckExistAsync(email, ct);
            return ToActionResult(checkRegistredEmail);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct = default)
        {
            var email = GetUserEmail()!;
            var user = await _authenticationService.GetCurrentUser(email, ct);
            return ToActionResult(user);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>>GetAddress(CancellationToken ct = default)
        {
            var result = await _authenticationService.GetUserAddress(GetUserEmail(), ct);
            return ToActionResult(result);
        }

        [Authorize]
        [HttpPost("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress( AddressDto addressDto,CancellationToken ct = default)
        {
            var result = await _authenticationService.UpdateUserAddress(addressDto, GetUserEmail());
            return ToActionResult(result);
        }


    }
}

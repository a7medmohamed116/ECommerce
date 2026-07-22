using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.IdentityDTOs;
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
    }
}

using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.UsersDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController :ApiBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserToManageDto>>> GetAllUsers(
            CancellationToken ct = default)
        {
            var result = await _userService.GetAllUsersAsync(ct);

            return ToActionResult(result);
        }

    }
}

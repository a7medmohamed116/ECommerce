using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.RolesDto;
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


        [HttpGet("{id}")]
        public async Task<ActionResult<UserRoleDto>> GetUserForEdit(string id,CancellationToken ct = default)
        {
            var result = await _userService.GetUserForEditAsync(id, ct);

            return ToActionResult(result);
        }

        [HttpPut("{id}/roles")]
        public async Task<ActionResult<bool>> UpdateUserRoles(string id,UserRoleDto model,CancellationToken ct = default)
        {
            model.UserId = id;

            var result = await _userService.UpdateUserRolesAsync(model, ct);

            return ToActionResult(result);
        }

    }
}

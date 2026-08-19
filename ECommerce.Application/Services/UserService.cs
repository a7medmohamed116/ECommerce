using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.RolesDto;
using ECommerce.Application.DTOs.UsersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityService _identityService;

        public UserService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<IReadOnlyList<UserToManageDto>>> GetAllUsersAsync(
            CancellationToken ct = default)
        {
            var usersResult = await _identityService.GetAllUsersAsync(ct);

            if (!usersResult.IsSuccess)
                return Result<IReadOnlyList<UserToManageDto>>
                    .Fail(usersResult.Errors);

            var result = new List<UserToManageDto>();

            foreach (var user in usersResult.data)
            {
                var rolesResult = await _identityService.GetUserRoles(user.Email, ct);

                if (!rolesResult.IsSuccess)
                    return Result<IReadOnlyList<UserToManageDto>>
                        .Fail(rolesResult.Errors);

                result.Add(new UserToManageDto
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = rolesResult.data
                });
            }

            return Result<IReadOnlyList<UserToManageDto>>.OK(result);
        }

        public async Task<Result<UserRoleDto>> GetUserForEditAsync(
     string userId,
     CancellationToken ct = default)
        {
            var userResult = await _identityService.FindUserByIdAsync(userId, ct);

            if (!userResult.IsSuccess)
                return Result<UserRoleDto>.Fail(userResult.Errors);

            var rolesResult = await _identityService.GetAllRolesAsync(ct);

            if (!rolesResult.IsSuccess)
                return Result<UserRoleDto>.Fail(rolesResult.Errors);

            var userRolesResult = await _identityService.GetUserRoles(
                userResult.data.Email,
                ct);

            if (!userRolesResult.IsSuccess)
                return Result<UserRoleDto>.Fail(userRolesResult.Errors);

            var userRoles = userRolesResult.data;

            var result = new UserRoleDto
            {
                UserId = userResult.data.Id,
                UserName = userResult.data.UserName,
                Roles = rolesResult.data
                    .Select(role => new UpdateRoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        IsSelected = userRoles.Contains(role.Name)
                    })
                    .ToList()
            };

            return Result<UserRoleDto>.OK(result);
        }

        public async Task<Result<bool>> UpdateUserRolesAsync(
    UserRoleDto model,
    CancellationToken ct = default)
        {
            var selectedRoles = model.Roles
                .Where(x => x.IsSelected)
                .Select(x => x.Name)
                .ToList();

            return await _identityService.UpdateUserRolesAsync(
                model.UserId,
                selectedRoles,
                ct);
        }
    }
}

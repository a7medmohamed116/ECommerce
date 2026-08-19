using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
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
    }
}

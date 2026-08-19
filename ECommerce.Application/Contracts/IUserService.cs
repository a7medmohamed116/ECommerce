using ECommerce.Application.Common;
using ECommerce.Application.DTOs.RolesDto;
using ECommerce.Application.DTOs.UsersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IUserService
    {
        Task<Result<IReadOnlyList<UserToManageDto>>> GetAllUsersAsync(CancellationToken ct = default);


        Task<Result<UserRoleDto>> GetUserForEditAsync(string userId,CancellationToken ct = default);

        Task<Result<bool>> UpdateUserRolesAsync(UserRoleDto model,CancellationToken ct = default);
    }
}

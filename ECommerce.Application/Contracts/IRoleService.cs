using ECommerce.Application.Common;
using ECommerce.Application.DTOs.RolesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IRoleService
    {
        Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(
            CancellationToken ct = default);

        Task<Result<bool>> CreateAsync(
            CreateRoleDto model,
            CancellationToken ct = default);

        Task<Result<bool>> DeleteAsync(
            string id,
            CancellationToken ct = default);

        Task<Result<RoleDto>> GetByIdAsync(
            string id,
            CancellationToken ct = default);

        Task<Result<bool>> UpdateAsync(
            RoleDto model,
            CancellationToken ct = default);
    }
}

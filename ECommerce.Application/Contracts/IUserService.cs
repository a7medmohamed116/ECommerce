using ECommerce.Application.Common;
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
    }
}

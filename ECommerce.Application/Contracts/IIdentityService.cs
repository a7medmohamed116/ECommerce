using ECommerce.Application.Common;
using ECommerce.Application.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email ,CancellationToken ct =default);
        Task<Result<bool>> CheckPasswordAsync(string email ,string password,CancellationToken ct =default);
        Task<Result<IdentityUserResult>> CreateNewUserAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetUserRoles(string email, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetUserAddress(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateAddress(AddressDto addressDto ,string email,CancellationToken ct = default);

        Task<Result<IReadOnlyList<IdentityUserResult>>> GetAllUsersAsync(CancellationToken ct = default);
    }
}

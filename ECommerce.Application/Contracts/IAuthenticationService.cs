using ECommerce.Application.Common;
using ECommerce.Application.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto , CancellationToken ct =default);
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto , CancellationToken ct =default);
        Task<Result<bool>> CheckExistAsync(string email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUser(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetUserAddress(string email, CancellationToken ct = default);
    }
}

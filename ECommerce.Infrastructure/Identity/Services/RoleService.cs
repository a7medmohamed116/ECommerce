using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.RolesDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<bool>> CreateAsync(CreateRoleDto model,CancellationToken ct = default)
        {
            var exists = await _roleManager.RoleExistsAsync(model.Name);

            if (exists)
            {
                return Result<bool>.Fail(
                    Error.Validation(
                        "Role.Exists",
                        "Role Already Exist"));
            }

            var result = await _roleManager.CreateAsync(
                new IdentityRole(model.Name));

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(x => new Error(x.Code, x.Description))
                    .ToList();

                return Result<bool>.Fail(errors);
            }

            return Result<bool>.OK(true);
        }

        public async Task<Result<bool>> DeleteAsync(string id,CancellationToken ct = default)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                return Result<bool>.Fail(
                    Error.NotFound(
                        "Role.NotFound",
                        "Role not found"));
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(x => new Error(x.Code, x.Description))
                    .ToList();

                return Result<bool>.Fail(errors);
            }

            return Result<bool>.OK(true);
        }

        public async Task<Result<IReadOnlyList<RoleDto>>> GetAllAsync(CancellationToken ct = default)
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name!
                })
                .ToListAsync(ct);

            return Result<IReadOnlyList<RoleDto>>.OK(roles);
        }

        public async Task<Result<RoleDto>> GetByIdAsync(string id,CancellationToken ct = default)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                return Result<RoleDto>.Fail(
                    Error.NotFound(
                        "Role.NotFound",
                        "Role not found"));
            }

            return Result<RoleDto>.OK(new RoleDto
            {
                Id = role.Id,
                Name = role.Name!
            });
        }

        public async Task<Result<bool>> UpdateAsync(RoleDto model,CancellationToken ct = default)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role is null)
            {
                return Result<bool>.Fail(
                    Error.NotFound(
                        "Role.NotFound",
                        "Role not found"));
            }

            var existingRole = await _roleManager.RoleExistsAsync(model.Name);

            if (existingRole && role.Name != model.Name)
            {
                return Result<bool>.Fail(
                    Error.Validation(
                        "Role.Exists",
                        "Role Already Exist"));
            }

            role.Name = model.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(x => new Error(x.Code, x.Description))
                    .ToList();

                return Result<bool>.Fail(errors);
            }

            return Result<bool>.OK(true);
        }
    }
}


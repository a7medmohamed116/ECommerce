using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.RolesDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : ApiBaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAll(
            CancellationToken ct = default)
        {
            var result = await _roleService.GetAllAsync(ct);

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create(
            CreateRoleDto model,
            CancellationToken ct = default)
        {
            var result = await _roleService.CreateAsync(model, ct);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(
            string id,
            CancellationToken ct = default)
        {
            var result = await _roleService.GetByIdAsync(id, ct);

            return ToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(
            string id,
            RoleDto model,
            CancellationToken ct = default)
        {
            model.Id = id;

            var result = await _roleService.UpdateAsync(model, ct);

            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(
            string id,
            CancellationToken ct = default)
        {
            var result = await _roleService.DeleteAsync(id, ct);

            return ToActionResult(result);
        }
    }
}

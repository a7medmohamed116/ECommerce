using AdminDashBoard.Models.Roles;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.RolesDto;

namespace AdminDashBoard.Services
{
    public class RoleApiClient 
    {
        private readonly HttpClient _httpClient;

        public RoleApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<RoleDto>> GetAllAsync(
            CancellationToken ct = default)
        {
            var roles = await _httpClient.GetFromJsonAsync<
                IReadOnlyList<RoleDto>>(
                    "api/Roles",
                    ct);

            return roles ?? [];
        }

        public async Task<bool> CreateAsync(
            CreateRoleDto model,
            CancellationToken ct = default)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/Roles",
                model,
                ct);

            return response.IsSuccessStatusCode;
        }

        public async Task<IdentityRoleResult?> GetByIdAsync(
        string id,
        CancellationToken ct = default)
        {
            return await _httpClient.GetFromJsonAsync<IdentityRoleResult>(
                $"api/Roles/{id}",
                ct);
        }

        public async Task<bool> UpdateAsync(
            string id,
            RoleDto model,
            CancellationToken ct = default)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/Roles/{id}",
                model,
                ct);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(
            string id,
            CancellationToken ct = default)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/Roles/{id}",
                ct);

            return response.IsSuccessStatusCode;
        }
    }
}

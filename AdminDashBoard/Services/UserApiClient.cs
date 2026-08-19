using ECommerce.Application.DTOs.RolesDto;
using ECommerce.Application.DTOs.UsersDto;

namespace AdminDashBoard.Services
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<UserToManageDto>> GetAllUsersAsync(
            CancellationToken ct = default)
        {
            var users = await _httpClient.GetFromJsonAsync<
                IReadOnlyList<UserToManageDto>>(
                    "api/Users",
                    ct);

            return users ?? [];
        }


        public async Task<UserRoleDto?> GetUserForEditAsync(
        string id,
        CancellationToken ct = default)
        {
            return await _httpClient.GetFromJsonAsync<UserRoleDto>(
                $"api/Users/{id}",
                ct);
        }

        public async Task<bool> UpdateUserRolesAsync(
        string id,
        UserRoleDto model,
        CancellationToken ct = default)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/Users/{id}/roles",
                model,
                ct);

            return response.IsSuccessStatusCode;
        }
    }

}

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
    }
}

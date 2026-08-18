using ECommerce.Application.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AdminDashBoard.Services
{
    public class AuthenticationApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthenticationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto?> LoginAsync(LoginDto model)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/authentication/login",
                model);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<UserDto>();
        }
    }
}

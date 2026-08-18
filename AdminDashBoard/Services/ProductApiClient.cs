using AdminDashBoard.Models.Products;
using ECommerce.Application.DTOs.ProductDTOs;

namespace AdminDashBoard.Services
{
    public class ProductApiClient
    {
        private readonly HttpClient _httpClient;

        public ProductApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto?> CreateAsync(CreateProductViewModel model)
        {
            using var content = new MultipartFormDataContent();

            content.Add(
                new StringContent(model.Name),
                nameof(model.Name));

            content.Add(
                new StringContent(model.Description),
                nameof(model.Description));

            content.Add(
                new StringContent(model.Price.ToString()),
                nameof(model.Price));

            content.Add(
                new StringContent(model.BrandId.ToString()),
                nameof(model.BrandId));

            content.Add(
                new StringContent(model.TypeId.ToString()),
                nameof(model.TypeId));

            if (model.Image != null)
            {
                var stream = model.Image.OpenReadStream();

                var imageContent = new StreamContent(stream);

                content.Add(
                    imageContent,
                    "Image",
                    model.Image.FileName);
            }

            var response = await _httpClient.PostAsync(
                "api/products",
                content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"API Error: {response.StatusCode}\n{error}");
            }

            return await response.Content
                .ReadFromJsonAsync<ProductDto>();
        }

        public async Task<List<BrandDto>> GetBrandsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BrandDto>>(
                "api/products/brands") ?? [];
        }

        public async Task<List<TypeDto>> GetTypesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TypeDto>>(
                "api/products/types") ?? [];
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>(
                $"api/products/{id}");
        }
    }
}

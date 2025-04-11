using System.Text;
using System.Text.Json;
using Domain.DTOs;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var response = await _http.GetAsync("api/productcatalog/products");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(content)!;
    }

    public async Task<ProductDto> GetProductById(long id)
    {
        var response = await _http.GetAsync($"api/productcatalog/products/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductDto>(content)!;
    }

    public async Task<ProductDto> CreateProduct(CreateProductRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync("api/productcatalog/products",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductDto>(content)!;
    }

    public async Task<ProductDto> UpdateProduct(long id, UpdateProductRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PutAsync($"api/productcatalog/products/{id}",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductDto>(content)!;
    }

    public async Task DeleteProduct(long id)
    {
        var response = await _http.DeleteAsync($"api/productcatalog/products/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategories()
    {
        var response = await _http.GetAsync("api/productcatalog/categories");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(content)!;
    }

    public async Task<CategoryDto> GetCategoryById(long id)
    {
        var response = await _http.GetAsync($"api/productcatalog/categories/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CategoryDto>(content)!;
    }

    public async Task<CategoryDto> CreateCategory(CreateCategoryRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync("api/productcatalog/categories",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CategoryDto>(content)!;
    }

    public async Task<CategoryDto> UpdateCategory(long id, UpdateCategoryRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PutAsync($"api/productcatalog/categories/{id}",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CategoryDto>(content)!;
    }

    public async Task DeleteCategory(long id)
    {
        var response = await _http.DeleteAsync($"api/productcatalog/categories/{id}");
        response.EnsureSuccessStatusCode();
    }
}
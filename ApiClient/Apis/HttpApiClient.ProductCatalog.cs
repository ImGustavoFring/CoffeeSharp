using System.Net.Http.Json;
using Domain.DTOs;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.ProductCatalog.Requests;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.WebUtilities;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string ProductCatalogControllerPath = "api/product-catalog";
    
    public async Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetAllProducts(
        string? name = null,
        long? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        if (categoryId.HasValue) queryParams["categoryId"] = categoryId.Value.ToString();
        if (minPrice.HasValue) queryParams["minPrice"] = minPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        if (maxPrice.HasValue) queryParams["maxPrice"] = maxPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{ProductCatalogControllerPath}/products", queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>()!;
        int totalCount = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
        {
            int.TryParse(values.FirstOrDefault(), out totalCount);
        }

        return (items, totalCount)!;
    }

    public async Task<ProductDto> GetProductById(long id)
    {
        var response = await _http.GetAsync($"{ProductCatalogControllerPath}/products/{id}");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProductDto>())!;
    }

    public async Task<ProductDto> CreateProduct(CreateProductRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{ProductCatalogControllerPath}/products", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProductDto>())!;
    }

    public async Task<ProductDto> UpdateProduct(long id, UpdateProductRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{ProductCatalogControllerPath}/products/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProductDto>())!;
    }

    public async Task DeleteProduct(long id)
    {
        var response = await _http.DeleteAsync($"{ProductCatalogControllerPath}/products/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<(IEnumerable<CategoryDto> Items, int TotalCount)> GetAllCategories(
        string? name = null,
        long? parentId = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        if (parentId.HasValue) queryParams["parentId"] = parentId.Value.ToString();
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{ProductCatalogControllerPath}/categories", queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryDto>>()!;
        int totalCount = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
        {
            int.TryParse(values.FirstOrDefault(), out totalCount);
        }

        return (items, totalCount)!;
    }

    public async Task<CategoryDto> GetCategoryById(long id)
    {
        var response = await _http.GetAsync($"{ProductCatalogControllerPath}/categories/{id}");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CategoryDto>())!;
    }

    public async Task<CategoryDto> CreateCategory(CreateCategoryRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{ProductCatalogControllerPath}/categories", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CategoryDto>())!;
    }

    public async Task<CategoryDto> UpdateCategory(long id, UpdateCategoryRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{ProductCatalogControllerPath}/categories/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CategoryDto>())!;
    }

    public async Task DeleteCategory(long id)
    {
        var response = await _http.DeleteAsync($"{ProductCatalogControllerPath}/categories/{id}");
        response.EnsureSuccessStatusCode();
    }
}

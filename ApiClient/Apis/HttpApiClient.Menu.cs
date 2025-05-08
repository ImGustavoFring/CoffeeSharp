using System.Net.Http.Json;
using Domain.DTOs.Menu.Requests;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.WebUtilities;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string MenuPresetsControllerPath = "/api/menus/presets";
    private const string MenuItemsControllerPath = "/api/menus/items";

    public async Task<IEnumerable<MenuPresetDto>> GetAllPresets(
        string? name = null,
        string? description = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        if (!string.IsNullOrEmpty(description)) queryParams["description"] = description;
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString(MenuPresetsControllerPath, queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<MenuPresetDto>>())!;
    }

    public async Task<MenuPresetDto> GetPresetById(long id)
    {
        var response = await _http.GetAsync($"{MenuPresetsControllerPath}/{id}");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MenuPresetDto>())!;
    }

    public async Task<MenuPresetDto> CreatePreset(CreateMenuPresetRequest request)
    {
        var response = await _http.PostAsJsonAsync(MenuPresetsControllerPath, request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MenuPresetDto>())!;
    }

    public async Task<MenuPresetDto> UpdatePreset(long id, UpdateMenuPresetRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{MenuPresetsControllerPath}/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MenuPresetDto>())!;
    }

    public async Task DeletePreset(long id)
    {
        var response = await _http.DeleteAsync($"{MenuPresetsControllerPath}/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<MenuPresetItemDto>> GetPresetItems(
        long? menuPresetId = null,
        long? productId = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (menuPresetId.HasValue) queryParams["menuPresetId"] = menuPresetId.Value.ToString();
        if (productId.HasValue) queryParams["productId"] = productId.Value.ToString();
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString(MenuItemsControllerPath, queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<MenuPresetItemDto>>())!;
    }

    public async Task<MenuPresetItemDto> GetPresetItemById(long id)
    {
        var response = await _http.GetAsync($"{MenuItemsControllerPath}/{id}");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MenuPresetItemDto>())!;
    }

    public async Task<MenuPresetItemDto> CreatePresetItem(CreateMenuPresetItemRequest request)
    {
        var response = await _http.PostAsJsonAsync(MenuItemsControllerPath, request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MenuPresetItemDto>())!;
    }

    public async Task<MenuPresetItemDto> UpdatePresetItem(long id, UpdateMenuPresetItemRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{MenuItemsControllerPath}/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MenuPresetItemDto>())!;
    }

    public async Task DeletePresetItem(long id)
    {
        var response = await _http.DeleteAsync($"{MenuItemsControllerPath}/{id}");
        response.EnsureSuccessStatusCode();
    }
}

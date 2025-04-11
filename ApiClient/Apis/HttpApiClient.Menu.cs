using System.Text.Json;
using Domain.DTOs;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<IEnumerable<MenuPresetDto>> GetAllPresets()
    {
        var response = await _http.GetAsync("api/menu/presets");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<MenuPresetDto>>(content)!;
    }

    public async Task<MenuPresetDto> GetPresetById(long id)
    {
        var response = await _http.GetAsync($"api/menu/presets/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MenuPresetDto>(content)!;
    }

    public async Task<MenuPresetDto> CreatePreset(CreateMenuPresetRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PostAsync("api/menu/presets", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MenuPresetDto>(resultContent)!;
    }

    public async Task<MenuPresetDto> UpdatePreset(long id, UpdateMenuPresetRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PutAsync($"api/menu/presets/{id}", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MenuPresetDto>(resultContent)!;
    }

    public async Task DeletePreset(long id)
    {
        var response = await _http.DeleteAsync($"api/menu/presets/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<MenuPresetItemDto>> GetPresetItems(long? menuPresetId = null)
    {
        var url = menuPresetId.HasValue ? $"api/menu/items?menuPresetId={menuPresetId.Value}" : "api/menu/items";
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<MenuPresetItemDto>>(content)!;
    }

    public async Task<MenuPresetItemDto> GetPresetItemById(long id)
    {
        var response = await _http.GetAsync($"api/menu/items/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MenuPresetItemDto>(content)!;
    }

    public async Task<MenuPresetItemDto> CreatePresetItem(CreateMenuPresetItemRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PostAsync("api/menu/items", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MenuPresetItemDto>(resultContent)!;
    }

    public async Task<MenuPresetItemDto> UpdatePresetItem(long id, UpdateMenuPresetItemRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PutAsync($"api/menu/items/{id}", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MenuPresetItemDto>(resultContent)!;
    }

    public async Task DeletePresetItem(long id)
    {
        var response = await _http.DeleteAsync($"api/menu/items/{id}");
        response.EnsureSuccessStatusCode();
    }
}
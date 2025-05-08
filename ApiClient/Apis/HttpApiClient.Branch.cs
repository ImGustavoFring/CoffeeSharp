using System.Net.Http.Json;
using Domain.DTOs.Branch.Requests;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.WebUtilities;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string BranchesControllerPath = "/api/branches";

    public async Task<(IEnumerable<BranchDto> branches, int totalCount)> GetAllBranches(
        string? name = null,
        string? address = null,
        int page = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        if (!string.IsNullOrEmpty(address)) queryParams["address"] = address;
        queryParams["page"] = page.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString(BranchesControllerPath, queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var branches = await response.Content.ReadFromJsonAsync<IEnumerable<BranchDto>>();
        var totalCountHeader = response.Headers.TryGetValues("X-Total-Count", out var values) ? values.FirstOrDefault() : null;
        var totalCount = int.TryParse(totalCountHeader, out var count) ? count : 0;

        return (branches!, totalCount);
    }

    public async Task<BranchDto?> GetBranchById(long id)
    {
        var response = await _http.GetAsync($"{BranchesControllerPath}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BranchDto>();
    }

    public async Task<BranchDto> CreateBranch(CreateBranchRequest request)
    {
        var response = await _http.PostAsJsonAsync(BranchesControllerPath, request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<BranchDto>())!;
    }

    public async Task<BranchDto> UpdateBranch(long id, UpdateBranchRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{BranchesControllerPath}/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<BranchDto>())!;
    }

    public async Task DeleteBranch(long id)
    {
        var response = await _http.DeleteAsync($"{BranchesControllerPath}/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AssignMenuPresetToBranch(long branchId, AssignMenuPresetRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{BranchesControllerPath}/{branchId}/menupreset", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<BranchMenuDto>> GetBranchMenus(
        long? branchId = null,
        long? menuPresetItemId = null,
        long? menuPresetId = null,
        bool? availability = null,
        int page = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (branchId.HasValue) queryParams["branchId"] = branchId.Value.ToString();
        if (menuPresetItemId.HasValue) queryParams["menuPresetItemId"] = menuPresetItemId.Value.ToString();
        if (menuPresetId.HasValue) queryParams["menuPresetId"] = menuPresetId.Value.ToString();
        if (availability.HasValue) queryParams["availability"] = availability.Value.ToString().ToLower();
        queryParams["page"] = page.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{BranchesControllerPath}/menus", queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<IEnumerable<BranchMenuDto>>())!;
    }

    public async Task<BranchMenuDto> UpdateBranchMenuAvailability(long id, bool availability)
    {
        var url = QueryHelpers.AddQueryString($"{BranchesControllerPath}/menus/{id}/availability",
            new Dictionary<string, string?> { ["availability"] = availability.ToString().ToLower() });

        var response = await _http.PatchAsync(url, null);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<BranchMenuDto>())!;
    }
}

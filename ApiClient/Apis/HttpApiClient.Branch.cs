using System.Net.Http.Json;
using Domain.DTOs;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<IEnumerable<BranchDto>> GetAllBranches()
    {
        var response = await _http.GetAsync("/api/branch");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<BranchDto>>();
    }

    public async Task<BranchDto?> GetBranchById(long id)
    {
        var response = await _http.GetAsync($"/api/branch/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BranchDto>();
    }

    public async Task<BranchDto> CreateBranch(CreateBranchRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/branch", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BranchDto>();
    }

    public async Task<BranchDto> UpdateBranch(long id, UpdateBranchRequest request)
    {
        var response = await _http.PutAsJsonAsync($"/api/branch/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BranchDto>();
    }

    public async Task DeleteBranch(long id)
    {
        var response = await _http.DeleteAsync($"/api/branch/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AssignMenuPresetToBranch(long branchId, AssignMenuPresetRequest request)
    {
        var response = await _http.PostAsJsonAsync($"/api/branch/{branchId}/menupreset", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<BranchMenuDto>> GetBranchMenuByBranchId(long branchId)
    {
        var response = await _http.GetAsync($"/api/branch/{branchId}/menu");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<BranchMenuDto>>();
    }

    public async Task<BranchMenuDto> UpdateBranchMenuAvailability(long id, bool availability)
    {
        var response = await _http.PatchAsync($"/api/branch/menu/{id}/availability?availability={availability}", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BranchMenuDto>();
    }
}
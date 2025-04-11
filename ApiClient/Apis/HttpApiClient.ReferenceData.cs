using System.Text;
using System.Text.Json;
using Domain.DTOs;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<IEnumerable<RatingDto>> GetAllRatings()
    {
        var response = await _http.GetAsync("api/reference/ratings");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<RatingDto>>(content)!;
    }

    public async Task<RatingDto> GetRatingById(long id)
    {
        var response = await _http.GetAsync($"api/reference/ratings/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RatingDto>(content)!;
    }

    public async Task<RatingDto> CreateRating(CreateRatingRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync("api/reference/ratings",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RatingDto>(content)!;
    }

    public async Task<RatingDto> UpdateRating(long id, UpdateRatingRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PutAsync($"api/reference/ratings/{id}",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RatingDto>(content)!;
    }

    public async Task DeleteRating(long id)
    {
        var response = await _http.DeleteAsync($"api/reference/ratings/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<EmployeeRoleDto>> GetAllEmployeeRoles()
    {
        var response = await _http.GetAsync("api/reference/employeeRoles");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<EmployeeRoleDto>>(content)!;
    }

    public async Task<EmployeeRoleDto> GetEmployeeRoleById(long id)
    {
        var response = await _http.GetAsync($"api/reference/employeeRoles/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeRoleDto>(content)!;
    }

    public async Task<EmployeeRoleDto> CreateEmployeeRole(CreateEmployeeRoleRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync("api/reference/employeeRoles",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeRoleDto>(content)!;
    }

    public async Task<EmployeeRoleDto> UpdateEmployeeRole(long id, UpdateEmployeeRoleRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PutAsync($"api/reference/employeeRoles/{id}",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeRoleDto>(content)!;
    }

    public async Task DeleteEmployeeRole(long id)
    {
        var response = await _http.DeleteAsync($"api/reference/employeeRoles/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<BalanceHistoryStatusDto>> GetAllBalanceHistoryStatuses()
    {
        var response = await _http.GetAsync("api/reference/balanceHistoryStatuses");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<BalanceHistoryStatusDto>>(content)!;
    }

    public async Task<BalanceHistoryStatusDto> GetBalanceHistoryStatusById(long id)
    {
        var response = await _http.GetAsync($"api/reference/balanceHistoryStatuses/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BalanceHistoryStatusDto>(content)!;
    }

    public async Task<BalanceHistoryStatusDto> CreateBalanceHistoryStatus(CreateBalanceHistoryStatusRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync("api/reference/balanceHistoryStatuses",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BalanceHistoryStatusDto>(content)!;
    }

    public async Task<BalanceHistoryStatusDto> UpdateBalanceHistoryStatus(long id,
        UpdateBalanceHistoryStatusRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PutAsync($"api/reference/balanceHistoryStatuses/{id}",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BalanceHistoryStatusDto>(content)!;
    }

    public async Task DeleteBalanceHistoryStatus(long id)
    {
        var response = await _http.DeleteAsync($"api/reference/balanceHistoryStatuses/{id}");
        response.EnsureSuccessStatusCode();
    }
}
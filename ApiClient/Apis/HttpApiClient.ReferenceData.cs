using System.Net.Http.Json;
using Domain.DTOs.ReferenceData.Requests;
using Domain.DTOs.Shared;
using Microsoft.AspNetCore.WebUtilities;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string ReferenceDataControllerPath = "/api/references";

    public async Task<(IEnumerable<RatingDto> Items, int TotalCount)> GetAllRatings(string? name = null, long? value = null, int pageIndex = 0, int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        if (value.HasValue) queryParams["value"] = value.Value.ToString();
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{ReferenceDataControllerPath}/ratings", queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        int totalCount = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
            int.TryParse(values.FirstOrDefault(), out totalCount);

        var items = (await response.Content.ReadFromJsonAsync<IEnumerable<RatingDto>>())!;
        return (items, totalCount);
    }

    public async Task<RatingDto?> GetRatingById(long id)
    {
        var response = await _http.GetAsync($"{ReferenceDataControllerPath}/ratings/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<RatingDto>())!;
    }

    public async Task<RatingDto> CreateRating(CreateRatingRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{ReferenceDataControllerPath}/ratings", request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<RatingDto>())!;
    }

    public async Task<RatingDto> UpdateRating(long id, UpdateRatingRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{ReferenceDataControllerPath}/ratings/{id}", request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<RatingDto>())!;
    }

    public async Task DeleteRating(long id)
    {
        var response = await _http.DeleteAsync($"{ReferenceDataControllerPath}/ratings/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<(IEnumerable<EmployeeRoleDto> Items, int TotalCount)> GetAllEmployeeRoles(string? name = null, int pageIndex = 0, int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{ReferenceDataControllerPath}/employee-roles", queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        int totalCount = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
            int.TryParse(values.FirstOrDefault(), out totalCount);

        var items = (await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeRoleDto>>())!;
        return (items, totalCount);
    }

    public async Task<EmployeeRoleDto?> GetEmployeeRoleById(long id)
    {
        var response = await _http.GetAsync($"{ReferenceDataControllerPath}/employee-roles/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<EmployeeRoleDto>())!;
    }

    public async Task<EmployeeRoleDto> CreateEmployeeRole(CreateEmployeeRoleRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{ReferenceDataControllerPath}/employee-roles", request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<EmployeeRoleDto>())!;
    }

    public async Task<EmployeeRoleDto> UpdateEmployeeRole(long id, UpdateEmployeeRoleRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{ReferenceDataControllerPath}/employee-roles/{id}", request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<EmployeeRoleDto>())!;
    }

    public async Task DeleteEmployeeRole(long id)
    {
        var response = await _http.DeleteAsync($"{ReferenceDataControllerPath}/employee-roles/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<(IEnumerable<BalanceHistoryStatusDto> Items, int TotalCount)> GetAllBalanceHistoryStatuses(string? name = null, int pageIndex = 0, int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{ReferenceDataControllerPath}/balance-history-statuses", queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        int totalCount = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
            int.TryParse(values.FirstOrDefault(), out totalCount);

        var items = (await response.Content.ReadFromJsonAsync<IEnumerable<BalanceHistoryStatusDto>>())!;
        return (items, totalCount);
    }

    public async Task<BalanceHistoryStatusDto?> GetBalanceHistoryStatusById(long id)
    {
        var response = await _http.GetAsync($"{ReferenceDataControllerPath}/balance-history-statuses/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<BalanceHistoryStatusDto>())!;
    }

    public async Task<BalanceHistoryStatusDto> CreateBalanceHistoryStatus(CreateBalanceHistoryStatusRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{ReferenceDataControllerPath}/balance-history-statuses", request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<BalanceHistoryStatusDto>())!;
    }

    public async Task<BalanceHistoryStatusDto> UpdateBalanceHistoryStatus(long id, UpdateBalanceHistoryStatusRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{ReferenceDataControllerPath}/balance-history-statuses/{id}", request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<BalanceHistoryStatusDto>())!;
    }

    public async Task DeleteBalanceHistoryStatus(long id)
    {
        var response = await _http.DeleteAsync($"{ReferenceDataControllerPath}/balance-history-statuses/{id}");
        response.EnsureSuccessStatusCode();
    }
}

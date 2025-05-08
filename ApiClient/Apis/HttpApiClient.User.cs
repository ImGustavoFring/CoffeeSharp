using Domain.DTOs.User.Requests;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using Domain.DTOs.Shared;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string UserControllerPath = "api/user";

    public async Task<(IEnumerable<AdminDto> Admins, int Total)> GetAllAdmins(string? userName = null, int pageIndex = 0, int pageSize = 50)
    {
        var query = new Dictionary<string, string?>
        {
            ["userName"] = userName,
            ["pageIndex"] = pageIndex.ToString(),
            ["pageSize"] = pageSize.ToString()
        };

        var url = QueryHelpers.AddQueryString($"{UserControllerPath}/admins", query);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var admins = (await response.Content.ReadFromJsonAsync<IEnumerable<AdminDto>>())!;
        int total = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
        {
            int.TryParse(values.FirstOrDefault(), out total);
        }

        return (admins, total);
    }

    public async Task<AdminDto?> GetAdminById(long id)
    {
        var response = await _http.GetAsync($"{UserControllerPath}/admins/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return (await response.Content.ReadFromJsonAsync<AdminDto>())!;
    }

    public async Task<AdminDto> AddAdmin(CreateAdminRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{UserControllerPath}/admins/add", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AdminDto>())!;
    }

    public async Task<AdminDto> UpdateAdmin(long id, UpdateAdminRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{UserControllerPath}/admins/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AdminDto>())!;
    }

    public async Task DeleteAdmin(long id)
    {
        var response = await _http.DeleteAsync($"{UserControllerPath}/admins/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<(IEnumerable<EmployeeDto> Employees, int Total)> GetAllEmployees(
        string? name = null,
        string? userName = null,
        long? roleId = null,
        long? branchId = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var query = new Dictionary<string, string?>()
        {
            ["name"] = name,
            ["userName"] = userName,
            ["roleId"] = roleId?.ToString(),
            ["branchId"] = branchId?.ToString(),
            ["pageIndex"] = pageIndex.ToString(),
            ["pageSize"] = pageSize.ToString()
        };

        var url = QueryHelpers.AddQueryString($"{UserControllerPath}/employees", query);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var employees = (await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>())!;
        int total = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
        {
            int.TryParse(values.FirstOrDefault(), out total);
        }

        return (employees, total);
    }

    public async Task<EmployeeDto?> GetEmployeeById(long id)
    {
        var response = await _http.GetAsync($"{UserControllerPath}/employees/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return (await response.Content.ReadFromJsonAsync<EmployeeDto>())!;
    }

    public async Task<EmployeeDto> AddEmployee(CreateEmployeeRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{UserControllerPath}/employees", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<EmployeeDto>())!;
    }

    public async Task<EmployeeDto> UpdateEmployee(long id, UpdateEmployeeRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{UserControllerPath}/employees/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<EmployeeDto>())!;
    }

    public async Task DeleteEmployee(long id)
    {
        var response = await _http.DeleteAsync($"{UserControllerPath}/employees/{id}");
        response.EnsureSuccessStatusCode();
    }
}

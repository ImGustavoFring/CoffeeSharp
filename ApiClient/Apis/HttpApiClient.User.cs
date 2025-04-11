using System.Text;
using System.Text.Json;
using Domain.DTOs;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<IEnumerable<AdminDto>> GetAllAdmins()
    {
        var response = await _http.GetAsync("api/user/admins");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<AdminDto>>(content)!;
    }

    public async Task<AdminDto> GetAdminById(long id)
    {
        var response = await _http.GetAsync($"api/user/admin/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AdminDto>(content)!;
    }

    public async Task<AdminDto> AddAdmin(CreateAdminRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync("api/user/admin/add",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AdminDto>(content)!;
    }

    public async Task<AdminDto> UpdateAdmin(long id, UpdateAdminRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PutAsync($"api/user/admin/{id}",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AdminDto>(content)!;
    }

    public async Task DeleteAdmin(long id)
    {
        var response = await _http.DeleteAsync($"api/user/admin/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployees()
    {
        var response = await _http.GetAsync("api/user/employees");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<EmployeeDto>>(content)!;
    }

    public async Task<EmployeeDto> GetEmployeeById(long id)
    {
        var response = await _http.GetAsync($"api/user/employee/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeDto>(content)!;
    }

    public async Task<EmployeeDto> AddEmployee(CreateEmployeeRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PostAsync("api/user/employee",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeDto>(content)!;
    }

    public async Task<EmployeeDto> UpdateEmployee(long id, UpdateEmployeeRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var response = await _http.PutAsync($"api/user/employee/{id}",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeDto>(content)!;
    }

    public async Task DeleteEmployee(long id)
    {
        var response = await _http.DeleteAsync($"api/user/employee/{id}");
        response.EnsureSuccessStatusCode();
    }
}
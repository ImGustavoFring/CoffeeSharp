using System.Net.Http.Json;
using ApiClient.Models;
using Domain.DTOs;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<string?> AdminLogin(AdminLoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/auth/admin/login", request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result!.token;
    }

    public async Task<string?> EmployeeLogin(EmployeeLoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/auth/employee/login", request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result!.token;
    }
}
using System.Net.Http.Json;
using ApiClient.Models;
using Domain.DTOs.Auth.Requests;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string AuthControllerPath = "/api/auth";
    public async Task<string?> AdminLogin(AdminLoginRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{AuthControllerPath}/admin/login", request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        var token = result!.token;
        SetAccessToken(token);
        return token;
    }

    public async Task<string?> EmployeeLogin(EmployeeLoginRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{AuthControllerPath}/employee/login", request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        var token = result!.token;
        SetAccessToken(token);
        return token;
    }
}
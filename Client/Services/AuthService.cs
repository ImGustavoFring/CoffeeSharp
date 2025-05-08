using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiClient.Apis;
using Domain.DTOs;
using Domain.DTOs.Auth.Requests;

namespace Client.Services;

public sealed class AuthService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CoffeeSharpClient", "auth.json");

    private static AuthSettings? _authSettings;

    private static AuthSettings Load()
    {
        if (_authSettings != null) return _authSettings;

        if (File.Exists(SettingsPath))
        {
            var json = File.ReadAllText(SettingsPath);
            _authSettings = JsonSerializer.Deserialize<AuthSettings>(json) ?? new AuthSettings();
        }
        else
        {
            _authSettings = new AuthSettings();
        }

        return _authSettings;
    }

    private static void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
        var json = JsonSerializer.Serialize(_authSettings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsPath, json);
    }

    public static string AccessToken
    {
        get => Load().AccessToken;
        set
        {
            Load().AccessToken = value;
            Save();
        }
    }

    public string? UserType => Load().UserType;

    public static AuthService Instance { get; } = new AuthService();

    private AuthService()
    {
    }

    public async Task<bool> Login(string username, string password, bool loggingAsAdmin = false)
    {
        string? token;
        if (loggingAsAdmin)
        {
            token = await HttpClient.Instance.AdminLogin(new AdminLoginRequest(username, password));
        }
        else
        {
            token = await HttpClient.Instance.EmployeeLogin(new EmployeeLoginRequest(username, password));
        }

        if (token == null)
        {
            return false;
        }
        
        AccessToken = token;
        return true;
    }
}
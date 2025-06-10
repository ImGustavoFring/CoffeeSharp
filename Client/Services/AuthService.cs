using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiClient.Apis;
using Client.ViewModels;
using Domain.DTOs;
using Domain.DTOs.Auth.Requests;

namespace Client.Services;

public sealed class AuthService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CoffeeSharpClient", "auth.json");

    private static AuthSettings? _authSettings;

    public static AuthSettings Load()
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
    
    public bool IsAuthenticated
    {
        get
        {
            var auth = Load();
            return !String.IsNullOrEmpty(auth.AccessToken) && !auth.IsTokenExpired;
        }
    }

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
        _mainWindowViewModel.RefreshAuthData();
        return true;
    }

    public bool StartUpLogin()
    {
        var auth = Load();
        if (String.IsNullOrEmpty(auth.AccessToken) || auth.IsTokenExpired) 
            return false;
        HttpClient.Instance.SetAccessToken(auth.AccessToken);
        return true;
    }

    private MainWindowViewModel _mainWindowViewModel;

    public MainWindowViewModel MainWindowViewModel
    {
        get => _mainWindowViewModel;
        set => _mainWindowViewModel = value;
    }
}
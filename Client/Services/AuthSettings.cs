using System;
using System.Runtime.InteropServices.JavaScript;

namespace Client.Services;

public class AuthSettings
{
    public string AccessToken { get; set; } = String.Empty;
    
    public string Role { get; set; } = String.Empty; // admin | employee
}

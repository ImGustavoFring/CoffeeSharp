using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using Client.Utils;

namespace Client.Services;

public class AuthSettings
{
    private string _accessToken = String.Empty;
    private IReadOnlyCollection<Claim>? _claims { get; set; }

    public string AccessToken
    {
        get => _accessToken;
        set
        {
            _accessToken = value;
            _claims = JwtHelper.ParseClaimsFromToken(_accessToken);
            HttpClient.Instance.SetAccessToken(value);
        }
    }
    
    public string? Id => (_claims != null) ? JwtHelper.GetClaimValue(_claims, "id") : null;

    public string? Name => (_claims != null) ? JwtHelper.GetClaimValue(_claims, ClaimTypes.Name) : null;

    public string? UserType => (_claims != null) ? JwtHelper.GetClaimValue(_claims, "user_type") : null;
}
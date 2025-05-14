using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Client.Utils;

public class JwtHelper
{
    public static IReadOnlyCollection<Claim> ParseClaimsFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
            throw new ArgumentException("Invalid JWT token.");

        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.Claims.ToList();
    }

    public static string? GetClaimValue(IReadOnlyCollection<Claim> claims, string claimType)
    {
        return claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }
    
    public static bool IsTokenExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
            throw new ArgumentException("Invalid JWT token.");

        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.ValidTo < DateTime.UtcNow;
    }
}
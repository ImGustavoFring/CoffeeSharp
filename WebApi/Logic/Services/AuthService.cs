using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Logic.Services.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAdminCrudService _adminService;
        private readonly IConfiguration _configuration;

        public AuthService(IAdminCrudService adminService, IConfiguration configuration)
        {
            _adminService = adminService;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            var admin = await _adminService.GetAdminByUsernameAsync(userName);
            if (admin == null || !VerifyPassword(password, admin.PasswordHash))
            {
                throw new UnauthorizedAccessException("Неверные учетные данные");
            }
            return GenerateJwtToken(admin);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private string GenerateJwtToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.UserName),
                new Claim("user_type", "admin")
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

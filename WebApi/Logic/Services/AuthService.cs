using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Logic.Services.Interfaces;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<string> AdminLoginAsync(string userName, string password)
        {
            var admin = await _unitOfWork.Admins.GetSingleAsync(x => x.UserName == userName);

            if (admin == null || !VerifyPassword(password, admin.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return GenerateJwtToken(admin);
        }

        public async Task<string> EmployeeLoginAsync(string userName, string password)
        {
            var employee = await _unitOfWork.Employees.GetSingleAsync(x => x.UserName == userName);

            if (employee == null || !VerifyPassword(password, employee.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return GenerateJwtToken(employee);
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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateJwtToken(Employee employee)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]); 
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", employee.Id.ToString()),
                    new Claim(ClaimTypes.Name, employee.UserName),
                    new Claim("user_type", "employee"),
                    new Claim(ClaimTypes.Role, employee.RoleId.ToString())

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

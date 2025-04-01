using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Logic.Features.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Features
{
    public class UserService : IUserService
    {
        private readonly IAdminCrudService _adminService;
        private readonly IConfiguration _configuration;

        public UserService(IAdminCrudService adminService, IConfiguration configuration)
        {
            _adminService = adminService;
            _configuration = configuration;
        }

        public async Task<Admin> AddAdminAsync(string userName, string password)
        {
            var existingAdmin = await _adminService.GetAdminByUsernameAsync(userName);
            if (existingAdmin != null)
            {
                throw new InvalidOperationException("Admin with this username already exists");
            }

            var createdAdmin = await _adminService.AddAdminFromRawPasswordAsync(userName, password);

            return createdAdmin;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _adminService.GetAllAdminsAsync();
        }
    }
}

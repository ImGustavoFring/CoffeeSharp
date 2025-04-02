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

        public async Task<Admin?> GetAdminByIdAsync(long id)
        {
            return await _adminService.GetAdminByIdAsync(id);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            var existingAdmin = await _adminService.GetAdminByIdAsync(admin.Id);
            if (existingAdmin == null) throw new KeyNotFoundException("Admin not found");

            existingAdmin.UserName = admin.UserName;

            if (!string.IsNullOrWhiteSpace(admin.PasswordHash))
            {
                existingAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
            }

            return await _adminService.UpdateAdminAsync(existingAdmin);
        }

        public async Task DeleteAdminAsync(long id)
        {
            await _adminService.DeleteAdminAsync(id);
        }
    }
}

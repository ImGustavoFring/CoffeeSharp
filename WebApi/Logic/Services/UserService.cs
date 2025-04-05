using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Admin> AddAdminAsync(string userName, string password)
        {
            var existingAdmin = await _unitOfWork.Admins
                .GetSingleAsync(x => x.UserName == userName);

            if (existingAdmin != null)
            {
                throw new InvalidOperationException("Admin with this username already exists");
            }

            var admin = new Admin
            {
                UserName = userName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _unitOfWork.Admins.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();

            return admin;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _unitOfWork.Admins.GetAllAsync();
        }

        public async Task<Admin?> GetAdminByIdAsync(long id)
        {
            return await _unitOfWork.Admins.GetByIdAsync(id);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            var existingAdmin = await _unitOfWork.Admins.GetByIdAsync(admin.Id);
            if (existingAdmin == null)
                throw new KeyNotFoundException("Admin not found");

            existingAdmin.UserName = admin.UserName;

            if (!string.IsNullOrWhiteSpace(admin.PasswordHash))
            {
                existingAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(admin.PasswordHash);
            }

            await _unitOfWork.Admins.UpdateAsync(existingAdmin);
            await _unitOfWork.SaveChangesAsync();

            return existingAdmin;
        }

        public async Task DeleteAdminAsync(long id)
        {
            await _unitOfWork.Admins.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

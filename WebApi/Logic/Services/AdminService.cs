using Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin> _repository;

        public AdminService(IRepository<Admin> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Admin?> GetAdminByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Admin> AddAdminAsync(Admin admin)
        {
            return await _repository.AddAsync(admin);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            return await _repository.UpdateAsync(admin);
        }

        public async Task DeleteAdminAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Admin?> GetAdminByUsernameAsync(string username)
        {
            var admins = await _repository.GetAllAsync();
            return admins.FirstOrDefault(a => a.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Admin> AddAdminWithHashedPasswordAsync(string username, string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return await AddAdminAsync(new Admin {UserName = username, PasswordHash = passwordHash});
        }
    }
}

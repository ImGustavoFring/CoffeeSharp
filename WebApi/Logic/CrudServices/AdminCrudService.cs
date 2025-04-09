using Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class AdminCrudService : IAdminCrudService
    {
        private readonly IRepository<Admin> _repository;

        public AdminCrudService(IRepository<Admin> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Admin?> GetAdminByIdAsync(long id)
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

        public async Task DeleteAdminAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Admin?> GetAdminByUsernameAsync(string username)
        {
            var admins = await _repository.GetAllAsync();
            return admins.FirstOrDefault(a => a.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Admin> AddAdminFromRawPasswordAsync(string username, string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return await AddAdminAsync(new Admin {UserName = username, PasswordHash = passwordHash});
        }
    }
}

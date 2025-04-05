using Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
        Task<Admin> AddAdminAsync(string userName, string password);
        Task<Admin?> GetAdminByIdAsync(long id);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(long id);
    }
}

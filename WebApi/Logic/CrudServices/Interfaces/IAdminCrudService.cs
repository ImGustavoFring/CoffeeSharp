using Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IAdminCrudService
    {
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
        Task<Admin?> GetAdminByIdAsync(long id);
        Task<Admin> AddAdminAsync(Admin admin);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(long id);
        Task<Admin?> GetAdminByUsernameAsync(string username);
        Task<Admin> AddAdminFromRawPasswordAsync(string username, string password);
    }
}

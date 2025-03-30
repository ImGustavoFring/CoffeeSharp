using Domain.Entities;

namespace WebApi.Logic.Features.Interfaces
{
    public interface IUserService
    {
        Task<Admin> AddAdminAsync(string userName, string password);
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
    }
}

using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IEmployeeRoleCrudService
    {
        Task<IEnumerable<EmployeeRole>> GetAllRolesAsync();
        Task<EmployeeRole?> GetRoleByIdAsync(long id);
        Task<EmployeeRole> AddRoleAsync(EmployeeRole role);
        Task<EmployeeRole> UpdateRoleAsync(EmployeeRole role);
        Task DeleteRoleAsync(long id);
    }
}

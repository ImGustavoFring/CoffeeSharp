using CoffeeSharp.Domain.Entities;
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
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(long id);
        Task<Employee> AddEmployeeAsync(string name, string userName, string password, long roleId, long branchId);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(long id);
    }
}

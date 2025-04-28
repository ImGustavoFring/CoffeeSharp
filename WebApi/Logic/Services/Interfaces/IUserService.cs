using CoffeeSharp.Domain.Entities;
using Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IUserService
    {
        Task<(IEnumerable<Admin> Admins, int TotalCount)> GetAllAdminsAsync(
            string? userNameFilter = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<Admin> AddAdminAsync(string userName, string password);
        Task<Admin?> GetAdminByIdAsync(long id);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(long id);
        Task<(IEnumerable<Employee> Employees, int TotalCount)> GetAllEmployeesAsync(
            string? nameFilter = null,
            string? userNameFilter = null,
            long? roleId = null,
            long? branchId = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<Employee?> GetEmployeeByIdAsync(long id);
        Task<Employee> AddEmployeeAsync(string name, string userName, string password, long roleId, long branchId);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(long id);
    }
}

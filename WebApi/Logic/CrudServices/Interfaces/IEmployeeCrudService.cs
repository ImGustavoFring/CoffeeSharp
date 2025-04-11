using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IEmployeeCrudService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(long id);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(long id);
    }
}

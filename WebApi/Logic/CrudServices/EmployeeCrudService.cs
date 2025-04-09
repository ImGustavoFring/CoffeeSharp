using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class EmployeeCrudService : IEmployeeCrudService
    {
        private readonly IRepository<Employee> _repository;

        public EmployeeCrudService(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            return await _repository.AddAsync(employee);
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            return await _repository.UpdateAsync(employee);
        }

        public async Task DeleteEmployeeAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

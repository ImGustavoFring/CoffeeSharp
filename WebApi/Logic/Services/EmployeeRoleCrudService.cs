using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class EmployeeRoleCrudService : IEmployeeRoleCrudService
    {
        private readonly IRepository<EmployeeRole> _repository;

        public EmployeeRoleCrudService(IRepository<EmployeeRole> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmployeeRole>> GetAllRolesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<EmployeeRole?> GetRoleByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<EmployeeRole> AddRoleAsync(EmployeeRole role)
        {
            return await _repository.AddAsync(role);
        }

        public async Task<EmployeeRole> UpdateRoleAsync(EmployeeRole role)
        {
            return await _repository.UpdateAsync(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

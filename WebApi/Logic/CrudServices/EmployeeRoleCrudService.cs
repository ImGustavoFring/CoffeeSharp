﻿using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
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

        public async Task<EmployeeRole?> GetRoleByIdAsync(long id)
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

        public async Task DeleteRoleAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class BranchCrudService : IBranchCrudService
    {
        private readonly IRepository<Branch> _repository;

        public BranchCrudService(IRepository<Branch> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Branch?> GetBranchByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Branch> AddBranchAsync(Branch branch)
        {
            return await _repository.AddAsync(branch);
        }

        public async Task<Branch> UpdateBranchAsync(Branch branch)
        {
            return await _repository.UpdateAsync(branch);
        }

        public async Task DeleteBranchAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

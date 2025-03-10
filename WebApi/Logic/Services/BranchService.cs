using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BranchService : IBranchService
    {
        private readonly IRepository<Branch> _repository;

        public BranchService(IRepository<Branch> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Branch?> GetBranchByIdAsync(int id)
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

        public async Task DeleteBranchAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BranchMenuCrudService : IBranchMenuCrudService
    {
        private readonly IRepository<BranchMenu> _repository;

        public BranchMenuCrudService(IRepository<BranchMenu> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BranchMenu>> GetAllBranchMenusAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BranchMenu?> GetBranchMenuByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<BranchMenu> AddBranchMenuAsync(BranchMenu menu)
        {
            return await _repository.AddAsync(menu);
        }

        public async Task<BranchMenu> UpdateBranchMenuAsync(BranchMenu menu)
        {
            return await _repository.UpdateAsync(menu);
        }

        public async Task DeleteBranchMenuAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

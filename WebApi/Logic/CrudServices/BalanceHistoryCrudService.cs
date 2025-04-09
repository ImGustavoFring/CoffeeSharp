using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class BalanceHistoryCrudService : IBalanceHistoryCrudService
    {
        private readonly IRepository<BalanceHistory> _repository;

        public BalanceHistoryCrudService(IRepository<BalanceHistory> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BalanceHistory>> GetAllHistoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BalanceHistory?> GetHistoryByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<BalanceHistory> AddHistoryAsync(BalanceHistory history)
        {
            return await _repository.AddAsync(history);
        }

        public async Task<BalanceHistory> UpdateHistoryAsync(BalanceHistory history)
        {
            return await _repository.UpdateAsync(history);
        }

        public async Task DeleteHistoryAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

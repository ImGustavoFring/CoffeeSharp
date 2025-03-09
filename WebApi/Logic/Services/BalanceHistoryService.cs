using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BalanceHistoryService : IBalanceHistoryService
    {
        private readonly IService<BalanceHistory> _repository;

        public BalanceHistoryService(IService<BalanceHistory> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BalanceHistory>> GetAllHistoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BalanceHistory?> GetHistoryByIdAsync(int id)
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

        public async Task DeleteHistoryAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

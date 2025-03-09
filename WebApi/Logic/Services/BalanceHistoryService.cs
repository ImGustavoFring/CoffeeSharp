using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BalanceHistoryService : IBalanceHistoryService
    {
        private readonly IRepository<BalanceHistory> _balanceHistoryRepository;

        public BalanceHistoryService(IRepository<BalanceHistory> balanceHistoryRepository)
        {
            _balanceHistoryRepository = balanceHistoryRepository;
        }

        public async Task<IEnumerable<BalanceHistory>> GetAllHistoriesAsync()
        {
            return await _balanceHistoryRepository.GetAllAsync();
        }

        public async Task<BalanceHistory?> GetHistoryByIdAsync(int id)
        {
            return await _balanceHistoryRepository.GetByIdAsync(id);
        }

        public async Task<BalanceHistory> AddHistoryAsync(BalanceHistory history)
        {
            return await _balanceHistoryRepository.AddAsync(history);
        }

        public async Task<BalanceHistory> UpdateHistoryAsync(BalanceHistory history)
        {
            return await _balanceHistoryRepository.UpdateAsync(history);
        }

        public async Task DeleteHistoryAsync(int id)
        {
            await _balanceHistoryRepository.DeleteAsync(id);
        }
    }
}

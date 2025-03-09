using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BalanceHistoryService : IBalanceHistoryService
    {
        private readonly IService<BalanceHistory> _BalanceHistoryService;

        public BalanceHistoryService(IService<BalanceHistory> repository)
        {
            _BalanceHistoryService = repository;
        }

        public async Task<IEnumerable<BalanceHistory>> GetAllHistoriesAsync()
        {
            return await _BalanceHistoryService.GetAllAsync();
        }

        public async Task<BalanceHistory?> GetHistoryByIdAsync(int id)
        {
            return await _BalanceHistoryService.GetByIdAsync(id);
        }

        public async Task<BalanceHistory> AddHistoryAsync(BalanceHistory history)
        {
            return await _BalanceHistoryService.AddAsync(history);
        }

        public async Task<BalanceHistory> UpdateHistoryAsync(BalanceHistory history)
        {
            return await _BalanceHistoryService.UpdateAsync(history);
        }

        public async Task DeleteHistoryAsync(int id)
        {
            await _BalanceHistoryService.DeleteAsync(id);
        }
    }
}

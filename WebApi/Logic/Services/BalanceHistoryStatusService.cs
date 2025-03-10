using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BalanceHistoryStatusService : IBalanceHistoryStatusService
    {
        private readonly IRepository<BalanceHistoryStatus> _repository;

        public BalanceHistoryStatusService(IRepository<BalanceHistoryStatus> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BalanceHistoryStatus>> GetAllStatusesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BalanceHistoryStatus?> GetStatusByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<BalanceHistoryStatus> AddStatusAsync(BalanceHistoryStatus status)
        {
            return await _repository.AddAsync(status);
        }

        public async Task<BalanceHistoryStatus> UpdateStatusAsync(BalanceHistoryStatus status)
        {
            return await _repository.UpdateAsync(status);
        }

        public async Task DeleteStatusAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

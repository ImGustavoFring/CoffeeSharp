using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BalanceHistoryStatusService : IBalanceHistoryStatusService
    {
        private readonly IRepository<BalanceHistoryStatus> _balanceHistoryStatusRepository;

        public BalanceHistoryStatusService(IRepository<BalanceHistoryStatus> balanceHistoryStatusRepository)
        {
            _balanceHistoryStatusRepository = balanceHistoryStatusRepository;
        }

        public async Task<IEnumerable<BalanceHistoryStatus>> GetAllStatusesAsync()
        {
            return await _balanceHistoryStatusRepository.GetAllAsync();
        }

        public async Task<BalanceHistoryStatus> AddStatusAsync(BalanceHistoryStatus status)
        {
            return await _balanceHistoryStatusRepository.AddAsync(status);
        }
    }
}

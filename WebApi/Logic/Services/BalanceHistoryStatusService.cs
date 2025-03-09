using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BalanceHistoryStatusService : IBalanceHistoryStatusService
    {
        private readonly IService<BalanceHistoryStatus> _BalanceHistoryStatusService;

        public BalanceHistoryStatusService(IService<BalanceHistoryStatus> repository)
        {
            _BalanceHistoryStatusService = repository;
        }

        public async Task<IEnumerable<BalanceHistoryStatus>> GetAllStatusesAsync()
        {
            return await _BalanceHistoryStatusService.GetAllAsync();
        }

        public async Task<BalanceHistoryStatus> AddStatusAsync(BalanceHistoryStatus status)
        {
            return await _BalanceHistoryStatusService.AddAsync(status);
        }
    }
}

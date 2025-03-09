using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class BalanceHistoryStatusService : IBalanceHistoryStatusService
    {
        private readonly IService<BalanceHistoryStatus> _repository;

        public BalanceHistoryStatusService(IService<BalanceHistoryStatus> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BalanceHistoryStatus>> GetAllStatusesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BalanceHistoryStatus> AddStatusAsync(BalanceHistoryStatus status)
        {
            return await _repository.AddAsync(status);
        }
    }
}

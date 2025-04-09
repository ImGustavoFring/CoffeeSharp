using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class BalanceHistoryStatusCrudService : IBalanceHistoryStatusCrudService
    {
        private readonly IRepository<BalanceHistoryStatus> _repository;

        public BalanceHistoryStatusCrudService(IRepository<BalanceHistoryStatus> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BalanceHistoryStatus>> GetAllStatusesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BalanceHistoryStatus?> GetStatusByIdAsync(long id)
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

        public async Task DeleteStatusAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

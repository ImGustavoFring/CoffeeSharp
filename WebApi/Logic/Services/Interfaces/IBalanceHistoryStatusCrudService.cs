using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IBalanceHistoryStatusCrudService
    {
        Task<IEnumerable<BalanceHistoryStatus>> GetAllStatusesAsync();
        Task<BalanceHistoryStatus?> GetStatusByIdAsync(int id);
        Task<BalanceHistoryStatus> AddStatusAsync(BalanceHistoryStatus status);
        Task<BalanceHistoryStatus> UpdateStatusAsync(BalanceHistoryStatus status);
        Task DeleteStatusAsync(int id);
    }
}

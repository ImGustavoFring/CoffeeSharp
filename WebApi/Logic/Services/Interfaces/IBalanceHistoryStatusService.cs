using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IBalanceHistoryStatusService
    {
        Task<IEnumerable<BalanceHistoryStatus>> GetAllStatusesAsync();
        Task<BalanceHistoryStatus> AddStatusAsync(BalanceHistoryStatus status);
    }
}

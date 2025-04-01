using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IBalanceHistoryCrudService
    {
        Task<IEnumerable<BalanceHistory>> GetAllHistoriesAsync();
        Task<BalanceHistory?> GetHistoryByIdAsync(int id);
        Task<BalanceHistory> AddHistoryAsync(BalanceHistory history);
        Task<BalanceHistory> UpdateHistoryAsync(BalanceHistory history);
        Task DeleteHistoryAsync(int id);
    }
}

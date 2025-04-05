using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IReferenceDataService
    {
        Task<IEnumerable<Rating>> GetAllRatingsAsync();
        Task<Rating?> GetRatingByIdAsync(long id);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<Rating> UpdateRatingAsync(Rating rating);
        Task DeleteRatingAsync(long id);
        Task<IEnumerable<EmployeeRole>> GetAllEmployeeRolesAsync();
        Task<EmployeeRole?> GetEmployeeRoleByIdAsync(long id);
        Task<EmployeeRole> AddEmployeeRoleAsync(EmployeeRole role);
        Task<EmployeeRole> UpdateEmployeeRoleAsync(EmployeeRole role);
        Task DeleteEmployeeRoleAsync(long id);
        Task<IEnumerable<BalanceHistoryStatus>> GetAllBalanceHistoryStatusesAsync();
        Task<BalanceHistoryStatus?> GetBalanceHistoryStatusByIdAsync(long id);
        Task<BalanceHistoryStatus> AddBalanceHistoryStatusAsync(BalanceHistoryStatus status);
        Task<BalanceHistoryStatus> UpdateBalanceHistoryStatusAsync(BalanceHistoryStatus status);
        Task DeleteBalanceHistoryStatusAsync(long id);
    }
}

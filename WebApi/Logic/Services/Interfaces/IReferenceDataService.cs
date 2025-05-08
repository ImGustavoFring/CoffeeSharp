using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IReferenceDataService
    {
        Task<(IEnumerable<Rating> Items, int TotalCount)> GetAllRatingsAsync(
            string? name = null,
            long? value = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<Rating?> GetRatingByIdAsync(long id);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<Rating> UpdateRatingAsync(Rating rating);
        Task DeleteRatingAsync(long id);
        Task<(IEnumerable<EmployeeRole> Items, int TotalCount)> GetAllEmployeeRolesAsync(
             string? nameFilter = null,
             int pageIndex = 0,
             int pageSize = 50);
        Task<EmployeeRole?> GetEmployeeRoleByIdAsync(long id);
        Task<EmployeeRole> AddEmployeeRoleAsync(EmployeeRole role);
        Task<EmployeeRole> UpdateEmployeeRoleAsync(EmployeeRole role);
        Task DeleteEmployeeRoleAsync(long id);
        Task<(IEnumerable<BalanceHistoryStatus> Items, int TotalCount)> GetAllBalanceHistoryStatusesAsync(
            string? nameFilter = null,
            int pageIndex = 0,
            int pageSize = 50);
        Task<BalanceHistoryStatus?> GetBalanceHistoryStatusByIdAsync(long id);
        Task<BalanceHistoryStatus> AddBalanceHistoryStatusAsync(BalanceHistoryStatus status);
        Task<BalanceHistoryStatus> UpdateBalanceHistoryStatusAsync(BalanceHistoryStatus status);
        Task DeleteBalanceHistoryStatusAsync(long id);
    }
}

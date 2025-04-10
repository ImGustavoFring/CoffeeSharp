﻿using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IBalanceHistoryCrudService
    {
        Task<IEnumerable<BalanceHistory>> GetAllHistoriesAsync();
        Task<BalanceHistory?> GetHistoryByIdAsync(long id);
        Task<BalanceHistory> AddHistoryAsync(BalanceHistory history);
        Task<BalanceHistory> UpdateHistoryAsync(BalanceHistory history);
        Task DeleteHistoryAsync(long id);
    }
}

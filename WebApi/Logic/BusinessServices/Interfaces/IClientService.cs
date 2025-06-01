using CoffeeSharp.Domain.Entities;
using Domain.Enums;

namespace WebApi.Logic.BusinessServices.Interfaces
{
    public interface IClientService
    {
        Task<BalanceHistory> AddBalanceAsync(long clientId, decimal amount);
        Task<Client> CancelTransactionAsync(long transactionId);
        Task<Client> CompletePendingBalanceTransactionAsync(long transactionId);
        Task<Client> CreateClientAsync(Client client);
        Task<BalanceHistory> DeductBalanceAsync(long clientId, decimal amount);
        Task DeleteClientAsync(long id);
        Task<Client> GetClientByIdAsync(long id);
        Task<(IEnumerable<Client> Clients, int TotalCount)> GetClientsAsync(string? telegramId, string? name, int pageIndex, int pageSize);
        Task<(IEnumerable<BalanceHistory> Transactions, int TotalCount)> GetClientTransactionsAsync(long? clientId, TransactionType transactionType, TransactionStatus? transactionStatus, DateTime? createdFrom, DateTime? createdTo, DateTime? finishedFrom, DateTime? finishedTo, bool orderByNewestFirst, int pageIndex, int pageSize);
        Task<Client> UpdateClientAsync(Client client);
    }
}
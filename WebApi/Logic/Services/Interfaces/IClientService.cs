using CoffeeSharp.Domain.Entities;
using Domain.Enums;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IClientService
    {
        Task<Client> AddBalanceAsync(long clientId, decimal amount);
        Task<Client> CancelTransactionAsync(long transactionId);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> DeductBalanceAsync(long clientId, decimal amount);
        Task DeleteClientAsync(long id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(long id);
        Task<IEnumerable<BalanceHistory>> GetClientTransactionsAsync(long clientId, bool orderByNewestFirst, TransactionType transactionType);
        Task<Client> UpdateClientAsync(Client client);
        Task<Client> CompletePendingBalanceTransactionAsync(long transactionId);
    }
}
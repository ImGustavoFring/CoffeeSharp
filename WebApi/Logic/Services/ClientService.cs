using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSharp.Domain.Entities;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebApi.Configurations;
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TransactionSettings _transactionSettings;

        public ClientService(IUnitOfWork unitOfWork, IOptions<TransactionSettings> transactionOptions)
        {
            _unitOfWork = unitOfWork;
            _transactionSettings = transactionOptions.Value;
        }

        public async Task<(IEnumerable<Client> Clients, int TotalCount)> GetClientsAsync(
            string? telegramId,
            string? name,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<Client, bool>> filter = client => true;
            if (!string.IsNullOrWhiteSpace(telegramId))
                filter = filter.AndAlso(client => client.TelegramId == telegramId);
            if (!string.IsNullOrWhiteSpace(name))
                filter = filter.AndAlso(client => client.Name.Contains(name));

            int totalCount = await _unitOfWork.Clients.CountAsync(filter);
            IEnumerable<Client> clients = await _unitOfWork.Clients.GetManyAsync(
                filter: filter,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (clients, totalCount);
        }

        public async Task<Client> GetClientByIdAsync(long id)
            => await _unitOfWork.Clients.GetByIdAsync(id)
               ?? throw new ArgumentException("Client not found.");

        public async Task<Client> CreateClientAsync(Client client)
        {
            client.Balance = 0;
            await _unitOfWork.Clients.AddOneAsync(client);
            await _unitOfWork.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            Client existingClient = await _unitOfWork.Clients.GetByIdAsync(client.Id)
                ?? throw new ArgumentException("Client not found.");

            existingClient.TelegramId = client.TelegramId;
            existingClient.Name = client.Name;

            _unitOfWork.Clients.Update(existingClient);
            await _unitOfWork.SaveChangesAsync();
            return existingClient;
        }

        public async Task DeleteClientAsync(long id)
        {
            await _unitOfWork.Clients.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<BalanceHistory> AddBalanceAsync(long clientId, decimal amount)
        {
            Client client = await _unitOfWork.Clients.GetByIdAsync(clientId)
                ?? throw new ArgumentException("Client not found.");

            long pendingStatusKey = _transactionSettings.PendingStatus;

            BalanceHistoryStatus pendingStatus = await _unitOfWork.BalanceHistoryStatuses
                .GetByIdAsync(pendingStatusKey)
                ?? throw new ArgumentException("Balance history status not found.");

            var balanceHistory = new BalanceHistory
            {
                ClientId = client.Id,
                Sum = amount,
                CreatedAt = DateTime.UtcNow,
                BalanceHistoryStatusId = pendingStatus.Id
            };

            await _unitOfWork.BalanceHistories.AddOneAsync(balanceHistory);
            await _unitOfWork.SaveChangesAsync();
            return balanceHistory;
        }

        public async Task<(IEnumerable<BalanceHistory> Transactions, int TotalCount)> GetClientTransactionsAsync(
            long? clientId,
            TransactionType transactionType,
            TransactionStatus? transactionStatus,
            DateTime? createdFrom,
            DateTime? createdTo,
            DateTime? finishedFrom,
            DateTime? finishedTo,
            bool orderByNewestFirst,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<BalanceHistory, bool>> f = h => true;

            if (clientId.HasValue)
                f = f.AndAlso(h => h.ClientId == clientId.Value);

            if (transactionType == TransactionType.TopUp)
                f = f.AndAlso(h => h.Sum > 0);
            else if (transactionType == TransactionType.Deduction)
                f = f.AndAlso(h => h.Sum < 0);

            if (transactionStatus.HasValue)
            {
                long statusId = transactionStatus.Value switch
                {
                    TransactionStatus.Pending => _transactionSettings.PendingStatus,
                    TransactionStatus.Completed => _transactionSettings.CompletedStatus,
                    TransactionStatus.Cancelled => _transactionSettings.CancelledStatus,
                    _ => throw new ArgumentOutOfRangeException(nameof(transactionStatus))
                };
                f = f.AndAlso(h => h.BalanceHistoryStatusId == statusId);
            }

            if (createdFrom.HasValue)
                f = f.AndAlso(h => h.CreatedAt >= createdFrom.Value);
            if (createdTo.HasValue)
                f = f.AndAlso(h => h.CreatedAt <= createdTo.Value);

            if (finishedFrom.HasValue)
                f = f.AndAlso(h => h.FinishedAt.HasValue && h.FinishedAt.Value >= finishedFrom.Value);
            if (finishedTo.HasValue)
                f = f.AndAlso(h => h.FinishedAt.HasValue && h.FinishedAt.Value <= finishedTo.Value);

            Func<IQueryable<BalanceHistory>, IOrderedQueryable<BalanceHistory>> orderBy =
                orderByNewestFirst
                    ? q => q.OrderByDescending(h => h.CreatedAt)
                    : q => q.OrderBy(h => h.CreatedAt);

            int totalCount = await _unitOfWork.BalanceHistories.CountAsync(f);
            var txs = await _unitOfWork.BalanceHistories.GetManyAsync(
                filter: f,
                orderBy: orderBy,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (txs, totalCount);
        }

        public async Task<Client> CompletePendingBalanceTransactionAsync(long txId)
        {
            var tx = await _unitOfWork.BalanceHistories.GetByIdAsync(txId)
                ?? throw new ArgumentException("Transaction not found.");

            if (tx.BalanceHistoryStatusId != _transactionSettings.PendingStatus)
                throw new InvalidOperationException("Only pending transactions can be completed.");

            var client = await _unitOfWork.Clients.GetByIdAsync(tx.ClientId)
                ?? throw new ArgumentException("Client not found.");

            client.Balance += tx.Sum;
            _unitOfWork.Clients.Update(client);

            var completedStatus = await _unitOfWork.BalanceHistoryStatuses
                .GetByIdAsync(_transactionSettings.CompletedStatus)
                ?? throw new ArgumentException("Balance history status not found.");

            tx.BalanceHistoryStatusId = completedStatus.Id;
            tx.FinishedAt = DateTime.UtcNow;
            _unitOfWork.BalanceHistories.Update(tx);

            await _unitOfWork.SaveChangesAsync();
            return client;
        }

        public async Task<BalanceHistory> DeductBalanceAsync(long clientId, decimal amount)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId)
                ?? throw new ArgumentException("Client not found.");

            if (client.Balance < amount)
                throw new ArgumentException("Insufficient balance.");

            client.Balance -= amount;
            _unitOfWork.Clients.Update(client);

            var completedStatus = await _unitOfWork.BalanceHistoryStatuses
                .GetByIdAsync(_transactionSettings.CompletedStatus)
                ?? throw new ArgumentException("Balance history status not found.");

            var history = new BalanceHistory
            {
                ClientId = client.Id,
                Sum = -amount,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow,
                BalanceHistoryStatusId = completedStatus.Id
            };

            await _unitOfWork.BalanceHistories.AddOneAsync(history);
            await _unitOfWork.SaveChangesAsync();
            return history;
        }

        public async Task<Client> CancelTransactionAsync(long txId)
        {
            var tx = await _unitOfWork.BalanceHistories.GetByIdAsync(txId)
                ?? throw new ArgumentException("Transaction not found.");

            var client = await _unitOfWork.Clients.GetByIdAsync(tx.ClientId)
                ?? throw new ArgumentException("Client not found.");

            if (tx.BalanceHistoryStatusId == _transactionSettings.CompletedStatus)
            {
                client.Balance -= tx.Sum;
                _unitOfWork.Clients.Update(client);
            }

            var cancelledStatus = await _unitOfWork.BalanceHistoryStatuses
                .GetByIdAsync(_transactionSettings.CancelledStatus)
                ?? throw new ArgumentException("Balance history status not found.");

            tx.BalanceHistoryStatusId = cancelledStatus.Id;
            tx.FinishedAt = DateTime.UtcNow;
            _unitOfWork.BalanceHistories.Update(tx);

            await _unitOfWork.SaveChangesAsync();
            return client;
        }
    }
}

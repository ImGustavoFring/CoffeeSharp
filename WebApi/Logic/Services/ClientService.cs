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
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ClientService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
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

            long pendingStatusKey = long.Parse(_configuration["Transaction:PendingStatus"] ?? "0");
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
            Expression<Func<BalanceHistory, bool>> filter = history => true;

            if (clientId.HasValue)
                filter = filter.AndAlso(history => history.ClientId == clientId.Value);

            if (transactionType == TransactionType.TopUp)
                filter = filter.AndAlso(history => history.Sum > 0);
            else if (transactionType == TransactionType.Deduction)
                filter = filter.AndAlso(history => history.Sum < 0);

            if (transactionStatus.HasValue)
            {
                string configKey = transactionStatus.Value switch
                {
                    TransactionStatus.Pending => "PendingStatus",
                    TransactionStatus.Completed => "CompletedStatus",
                    TransactionStatus.Cancelled => "CancelledStatus",
                    _ => throw new ArgumentOutOfRangeException(nameof(transactionStatus))
                };
                long statusId = long.Parse(_configuration[$"Transaction:{configKey}"] ?? "0");
                filter = filter.AndAlso(history => history.BalanceHistoryStatusId == statusId);
            }

            if (createdFrom.HasValue)
                filter = filter.AndAlso(history => history.CreatedAt >= createdFrom.Value);
            if (createdTo.HasValue)
                filter = filter.AndAlso(history => history.CreatedAt <= createdTo.Value);

            if (finishedFrom.HasValue)
                filter = filter.AndAlso(history => history.FinishedAt.HasValue && history.FinishedAt.Value >= finishedFrom.Value);
            if (finishedTo.HasValue)
                filter = filter.AndAlso(history => history.FinishedAt.HasValue && history.FinishedAt.Value <= finishedTo.Value);

            Func<IQueryable<BalanceHistory>, IOrderedQueryable<BalanceHistory>> orderBy = orderByNewestFirst
                ? query => query.OrderByDescending(history => history.CreatedAt)
                : query => query.OrderBy(history => history.CreatedAt);

            int totalCount = await _unitOfWork.BalanceHistories.CountAsync(filter);
            IEnumerable<BalanceHistory> transactions = await _unitOfWork.BalanceHistories.GetManyAsync(
                filter: filter,
                orderBy: orderBy,
                pageIndex: pageIndex,
                pageSize: pageSize);

            return (transactions, totalCount);
        }

        public async Task<Client> CompletePendingBalanceTransactionAsync(long transactionId)
        {
            BalanceHistory transaction = await _unitOfWork.BalanceHistories.GetByIdAsync(transactionId)
                ?? throw new ArgumentException("Transaction not found.");

            long pendingStatusKey = long.Parse(_configuration["Transaction:PendingStatus"] ?? "0");
            if (transaction.BalanceHistoryStatusId != pendingStatusKey)
                throw new InvalidOperationException("Only pending transactions can be completed.");

            Client client = await _unitOfWork.Clients.GetByIdAsync(transaction.ClientId)
                ?? throw new ArgumentException("Client not found.");

            client.Balance += transaction.Sum;
            _unitOfWork.Clients.Update(client);

            long completedStatusKey = long.Parse(_configuration["Transaction:CompletedStatus"] ?? "1");
            BalanceHistoryStatus completedStatus = await _unitOfWork.BalanceHistoryStatuses
                .GetByIdAsync(completedStatusKey)
                ?? throw new ArgumentException("Balance history status not found.");

            transaction.BalanceHistoryStatusId = completedStatus.Id;
            transaction.FinishedAt = DateTime.UtcNow;
            _unitOfWork.BalanceHistories.Update(transaction);
            await _unitOfWork.SaveChangesAsync();
            return client;
        }

        public async Task<BalanceHistory> DeductBalanceAsync(long clientId, decimal amount)
        {
            Client client = await _unitOfWork.Clients.GetByIdAsync(clientId)
                ?? throw new ArgumentException("Client not found.");

            if (client.Balance < amount)
                throw new ArgumentException("Insufficient balance.");

            client.Balance -= amount;
            _unitOfWork.Clients.Update(client);

            long completedStatusKey = long.Parse(_configuration["Transaction:CompletedStatus"] ?? "0");
            BalanceHistoryStatus completedStatus = await _unitOfWork.BalanceHistoryStatuses
                .GetByIdAsync(completedStatusKey)
                ?? throw new ArgumentException("Balance history status not found.");

            var balanceHistory = new BalanceHistory
            {
                ClientId = client.Id,
                Sum = -amount,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow,
                BalanceHistoryStatusId = completedStatus.Id
            };

            await _unitOfWork.BalanceHistories.AddOneAsync(balanceHistory);
            await _unitOfWork.SaveChangesAsync();
            return balanceHistory;
        }

        public async Task<Client> CancelTransactionAsync(long transactionId)
        {
            BalanceHistory transaction = await _unitOfWork.BalanceHistories.GetByIdAsync(transactionId)
                ?? throw new ArgumentException("Transaction not found.");

            Client client = await _unitOfWork.Clients.GetByIdAsync(transaction.ClientId)
                ?? throw new ArgumentException("Client not found.");

            long completedStatusKey = long.Parse(_configuration["Transaction:CompletedStatus"] ?? "1");
            if (transaction.BalanceHistoryStatusId == completedStatusKey)
            {
                client.Balance -= transaction.Sum;
                _unitOfWork.Clients.Update(client);
            }

            long cancelledStatusKey = long.Parse(_configuration["Transaction:CancelledStatus"] ?? "2");
            BalanceHistoryStatus cancelledStatus = await _unitOfWork.BalanceHistoryStatuses
                .GetByIdAsync(cancelledStatusKey)
                ?? throw new ArgumentException("Balance history status not found.");

            transaction.BalanceHistoryStatusId = cancelledStatus.Id;
            transaction.FinishedAt = DateTime.UtcNow;
            _unitOfWork.BalanceHistories.Update(transaction);
            await _unitOfWork.SaveChangesAsync();
            return client;
        }
    }
}

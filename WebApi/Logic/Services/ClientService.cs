﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSharp.Domain.Entities;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
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

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _unitOfWork.Clients.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(long id)
        {
            return await _unitOfWork.Clients.GetByIdAsync(id);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            client.Balance = 0;
            return await _unitOfWork.Clients.AddAsync(client);
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            var existing = await _unitOfWork.Clients.GetByIdAsync(client.Id);
            if (existing == null)
            {
                throw new ArgumentException("Client not found.");
            }
            existing.TelegramId = client.TelegramId;
            existing.Name = client.Name;

            return await _unitOfWork.Clients.UpdateAsync(existing);
        }

        public async Task DeleteClientAsync(long id)
        {
            await _unitOfWork.Clients.DeleteAsync(id);
        }

        public async Task<Client> AddBalanceAsync(long clientId, decimal amount)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new ArgumentException("Client not found.");
            }

            client.Balance += amount;
            await _unitOfWork.Clients.UpdateAsync(client);

            var balanceHistory = new BalanceHistory
            {
                ClientId = clientId,
                Sum = amount,
                CreatedAt = DateTime.UtcNow,
                BalanceHistoryStatusId = long.Parse(_configuration["Transaction:DefaultStatus"])
            };

            await _unitOfWork.BalanceHistories.AddAsync(balanceHistory);

            return client;
        }

        public async Task<Client> DeductBalanceAsync(long clientId, decimal amount)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new ArgumentException("Client not found.");
            }

            if (client.Balance < amount)
            {
                throw new ArgumentException("Insufficient balance.");
            }

            client.Balance -= amount;
            await _unitOfWork.Clients.UpdateAsync(client);

            var balanceHistory = new BalanceHistory
            {
                ClientId = clientId,
                Sum = -amount,
                CreatedAt = DateTime.UtcNow,
                BalanceHistoryStatusId = long.Parse(_configuration["Transaction:DefaultStatus"])
            };
            await _unitOfWork.BalanceHistories.AddAsync(balanceHistory);

            return client;
        }

        public async Task<IEnumerable<BalanceHistory>> GetClientTransactionsAsync(long clientId, bool orderByNewestFirst, TransactionType transactionType)
        {
            Expression<Func<BalanceHistory, bool>> filter = bh => bh.ClientId == clientId;
            if (transactionType == TransactionType.TopUp)
            {
                filter = bh => bh.ClientId == clientId && bh.Sum > 0;
            }
            else if (transactionType == TransactionType.Deduction)
            {
                filter = bh => bh.ClientId == clientId && bh.Sum < 0;
            }

            Func<IQueryable<BalanceHistory>, IOrderedQueryable<BalanceHistory>> orderBy = q =>
                orderByNewestFirst ? q.OrderByDescending(bh => bh.CreatedAt) : q.OrderBy(bh => bh.CreatedAt);

            return await _unitOfWork.BalanceHistories.GetAllAsync(filter: filter, orderBy: orderBy);
        }

        public async Task<decimal> CancelTransactionAsync(long transactionId)
        {
            var transaction = await _unitOfWork.BalanceHistories.GetByIdAsync(transactionId);
            if (transaction == null)
                throw new ArgumentException("Transaction not found.");

            var client = await _unitOfWork.Clients.GetByIdAsync(transaction.ClientId);
            if (client == null)
                throw new ArgumentException("Client not found.");

            decimal previousBalance = client.Balance - transaction.Sum;

            client.Balance = previousBalance;
            await _unitOfWork.Clients.UpdateAsync(client);

            return previousBalance;
        }
    }
}

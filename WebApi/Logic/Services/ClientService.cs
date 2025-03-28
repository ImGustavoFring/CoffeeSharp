﻿using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _repository;

        public ClientService(IRepository<Client> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Client> AddClientAsync(Client client)
        {
            return await _repository.AddAsync(client);
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            return await _repository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}

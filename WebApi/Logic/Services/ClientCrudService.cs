using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class ClientCrudService : IClientCrudService
    {
        private readonly IRepository<Client> _repository;

        public ClientCrudService(IRepository<Client> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(long id)
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

        public async Task DeleteClientAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}

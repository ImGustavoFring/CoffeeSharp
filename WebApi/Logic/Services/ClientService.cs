using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class ClientService : IClientService
    {
        private readonly IService<Client> _clientService;

        public ClientService(IService<Client> clientRepository)
        {
            _clientService = clientRepository;
        }

        // Получение всех клиентов
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _clientService.GetAllAsync();
        }

        // Получение клиента по id
        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _clientService.GetByIdAsync(id);
        }

        // Добавление нового клиента
        public async Task<Client> AddClientAsync(Client client)
        {
            return await _clientService.AddAsync(client);
        }

        // Обновление информации о клиенте
        public async Task<Client> UpdateClientAsync(Client client)
        {
            return await _clientService.UpdateAsync(client);
        }

        // Удаление клиента по id
        public async Task DeleteClientAsync(int id)
        {
            await _clientService.DeleteAsync(id);
        }
    }

}

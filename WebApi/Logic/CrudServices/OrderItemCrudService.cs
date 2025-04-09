using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
{
    public class OrderItemCrudService : IOrderItemCrudService
    {
        private readonly IRepository<OrderItem> _repository;

        public OrderItemCrudService(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteOrderItemAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

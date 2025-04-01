using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class OrderCrudService : IOrderCrudService
    {
        private readonly IRepository<Order> _repository;

        public OrderCrudService(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            return await _repository.AddAsync(order);
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            return await _repository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

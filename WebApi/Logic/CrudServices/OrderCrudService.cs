using CoffeeSharp.Domain.Entities;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Logic.CrudServices.Interfaces;

namespace WebApi.Logic.CrudServices
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

        public async Task<Order?> GetOrderByIdAsync(long id)
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

        public async Task DeleteOrderAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

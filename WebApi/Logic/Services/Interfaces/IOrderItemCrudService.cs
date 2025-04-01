using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IOrderItemCrudService
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        Task<OrderItem?> GetOrderItemByIdAsync(int id);
        Task<OrderItem> AddOrderItemAsync(OrderItem item);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem item);
        Task DeleteOrderItemAsync(int id);
    }
}

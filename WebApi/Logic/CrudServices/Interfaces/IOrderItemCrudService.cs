using CoffeeSharp.Domain.Entities;

namespace WebApi.Logic.CrudServices.Interfaces
{
    public interface IOrderItemCrudService
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        Task<OrderItem?> GetOrderItemByIdAsync(long id);
        Task<OrderItem> AddOrderItemAsync(OrderItem item);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem item);
        Task DeleteOrderItemAsync(long id);
    }
}

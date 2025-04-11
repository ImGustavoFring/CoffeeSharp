using CoffeeSharp.Domain.Entities;
using Domain.DTOs;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<Order?> GetOrderByIdAsync(long id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByClientAsync(long clientId);
        Task<Order> MarkOrderAsPickedUpAsync(long orderId);

        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(long? orderId = null, long? employeeId = null);
        Task<OrderItem> CreateOrderItemAsync(long orderId, CreateOrderItemRequest request);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(long orderItemId);
        Task<OrderItem> AssignOrderItemToCookAsync(long orderItemId, long employeeId);
        Task<OrderItem> MarkOrderItemCompletedAsync(long orderItemId, long employeeId);
        Task<OrderItem> ReassignOrderItemAsync(long orderItemId, long newEmployeeId);

        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request);
        Task<Feedback> UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(long feedbackId);
    }
}

using CoffeeSharp.Domain.Entities;
using Domain.DTOs.Order.Requests;
using Domain.Enums;

namespace WebApi.Logic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderItem> AssignOrderItemAsync(long id, long employeeId);
        Task<OrderItem> CompleteOrderItemAsync(long id, long employeeId);
        Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request);
        Task<Order> CreateOrderAsync(Order order);
        Task<OrderItem> CreateOrderItemAsync(long orderId, CreateOrderItemRequest request);
        Task DeleteFeedbackAsync(long id);
        Task DeleteOrderItemAsync(long id);
        Task<(IEnumerable<Feedback>, int)> GetFeedbacksAsync(long? orderId, int? ratingId, int pageIndex, int pageSize);
        Task<Order?> GetOrderByIdAsync(long id);
        Task<(IEnumerable<OrderItem>, int)> GetOrderItemsAsync(long? orderId, long? employeeId, OrderItemStatus? status, long? branchId, int pageIndex, int pageSize);
        Task<(IEnumerable<Order>, int)> GetOrdersAsync(long? clientId, long? branchId, DateTime? createdFrom, DateTime? createdTo, OrderStatus? status, int pageIndex, int pageSize);
        Task<Order> PickupOrderAsync(long orderId);
        Task<OrderItem> ReassignOrderItemAsync(long id, long newEmployeeId);
        Task<Feedback> UpdateFeedbackAsync(Feedback feedback);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
    }
}
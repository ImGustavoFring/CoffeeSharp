using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using System.Linq.Expressions;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public OrderService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one order item.");

            decimal totalCost = 0;
            foreach (var itemRequest in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemRequest.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with id {itemRequest.ProductId} not found.");
                totalCost += itemRequest.Count * product.Price;
            }

            var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId);
            if (client == null)
                throw new ArgumentException("Client not found.");

            if (client.Balance < totalCost)
                throw new ArgumentException("Insufficient balance.");

            client.Balance -= totalCost;
            await _unitOfWork.Clients.UpdateAsync(client);

            var balanceHistory = new BalanceHistory
            {
                ClientId = client.Id,
                Sum = -totalCost,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow,
                BalanceHistoryStatusId = long.Parse(_configuration["Transaction:InternalDeductionStatus"] ?? "0")
            };
            await _unitOfWork.BalanceHistories.AddAsync(balanceHistory);

            var order = new Order
            {
                ClientId = request.ClientId,
                ClientNote = request.ClientNote,
                BranchId = request.BranchId,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = request.ExpectedIn
            };
            await _unitOfWork.Orders.AddAsync(order);

            foreach (var itemRequest in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemRequest.ProductId);
                if (product == null)
                    throw new ArgumentException("Product not found.");

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = itemRequest.ProductId,
                    Count = itemRequest.Count,
                    Price = product.Price
                };
                await _unitOfWork.OrderItems.AddAsync(orderItem);
            }
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(long id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _unitOfWork.Orders.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByClientAsync(long clientId)
        {
            return await _unitOfWork.Orders.GetAllAsync(filter: o => o.ClientId == clientId);
        }

        public async Task<Order> MarkOrderAsPickedUpAsync(long orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found.");

            var orderItems = await _unitOfWork.OrderItems.GetAllAsync(filter: oi => oi.OrderId == orderId);

            if (!orderItems.Any() || orderItems.Any(oi => oi.DoneAt == null))
                throw new InvalidOperationException("Order is not ready for pickup.");

            order.FinishedAt = DateTime.UtcNow;
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _unitOfWork.Feedbacks.GetAllAsync();
        }

        public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null || order.FinishedAt == null)
                throw new ArgumentException("Order not found or not completed. Feedback is allowed only for completed orders.");

            var feedback = new Feedback
            {
                Content = request.Content,
                RatingId = request.RatingId,
                OrderId = request.OrderId
            };
            return await _unitOfWork.Feedbacks.AddAsync(feedback);
        }

        public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback)
        {
            var existing = await _unitOfWork.Feedbacks.GetByIdAsync(feedback.Id);
            if (existing == null)
                throw new ArgumentException("Feedback not found.");
            existing.Content = feedback.Content;
            existing.RatingId = feedback.RatingId;
            return await _unitOfWork.Feedbacks.UpdateAsync(existing);
        }

        public async Task DeleteFeedbackAsync(long feedbackId)
        {
            await _unitOfWork.Feedbacks.DeleteAsync(feedbackId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(long? orderId = null, long? employeeId = null)
        {
            Expression<Func<OrderItem, bool>>? filter = null;
            if (orderId.HasValue && employeeId.HasValue)
                filter = oi => oi.OrderId == orderId.Value && oi.EmployeeId == employeeId.Value;
            else if (orderId.HasValue)
                filter = oi => oi.OrderId == orderId.Value;
            else if (employeeId.HasValue)
                filter = oi => oi.EmployeeId == employeeId.Value;
            return await _unitOfWork.OrderItems.GetAllAsync(filter: filter);
        }

        public async Task<OrderItem> CreateOrderItemAsync(long orderId, CreateOrderItemRequest request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = request.ProductId,
                Count = request.Count,
                Price = product.Price
            };
            await _unitOfWork.OrderItems.AddAsync(orderItem);
            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            var existing = await _unitOfWork.OrderItems.GetByIdAsync(orderItem.Id);
            if (existing == null)
                throw new ArgumentException("OrderItem not found.");
            existing.Count = orderItem.Count;
            await _unitOfWork.OrderItems.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteOrderItemAsync(long orderItemId)
        {
            await _unitOfWork.OrderItems.DeleteAsync(orderItemId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<OrderItem> AssignOrderItemToCookAsync(long orderItemId, long employeeId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId);
            if (orderItem == null)
                throw new ArgumentException("OrderItem not found.");
            if (orderItem.EmployeeId.HasValue)
                throw new InvalidOperationException("OrderItem is already assigned.");

            orderItem.EmployeeId = employeeId;
            orderItem.StartedAt = DateTime.UtcNow;
            await _unitOfWork.OrderItems.UpdateAsync(orderItem);

            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> MarkOrderItemCompletedAsync(long orderItemId, long employeeId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId);
            if (orderItem == null)
                throw new ArgumentException("OrderItem not found.");
            if (orderItem.EmployeeId != employeeId)
                throw new UnauthorizedAccessException("You are not assigned to this OrderItem.");

            orderItem.DoneAt = DateTime.UtcNow;
            await _unitOfWork.OrderItems.UpdateAsync(orderItem);

            var orderItems = await _unitOfWork.OrderItems.GetAllAsync(filter: oi => oi.OrderId == orderItem.OrderId);

            if (orderItems.All(oi => oi.DoneAt != null))
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(orderItem.OrderId);
                if (order != null)
                {
                    order.DoneAt = DateTime.UtcNow;
                    await _unitOfWork.Orders.UpdateAsync(order);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> ReassignOrderItemAsync(long orderItemId, long newEmployeeId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId);
            if (orderItem == null)
                throw new ArgumentException("OrderItem not found.");

            orderItem.EmployeeId = newEmployeeId;
            orderItem.StartedAt = DateTime.UtcNow;
            await _unitOfWork.OrderItems.UpdateAsync(orderItem);
            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }
    }
}

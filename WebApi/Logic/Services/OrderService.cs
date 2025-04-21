using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using Domain.Enums;
using System.Linq.Expressions;
using WebApi.Infrastructure.Extensions;
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

            var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _unitOfWork.Products.GetManyAsync(p => productIds.Contains(p.Id));

            decimal totalCost = 0m;

            foreach (var items in request.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == items.ProductId)
                             ?? throw new ArgumentException($"Product with id {items.ProductId} not found.");

                var available = await _unitOfWork.BranchMenus.GetManyAsync(
                    filter: bm => bm.BranchId == request.BranchId
                                 && bm.Availability
                                 && bm.MenuPresetItems.ProductId == items.ProductId,
                    includes: new List<Expression<Func<BranchMenu, object>>>
                    {
                bm => bm.MenuPresetItems
                    },
                    disableTracking: true);

                if (!available.Any())
                    throw new ArgumentException($"Product with id {items.ProductId} is not available in branch {request.BranchId}.");

                totalCost += items.Count * product.Price;
            }

            var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId)
                         ?? throw new ArgumentException("Client not found.");

            if (client.Balance < totalCost)
                throw new ArgumentException("Insufficient balance.");

            client.Balance -= totalCost;

            var statusId = long.Parse(_configuration["Transaction:CompletedStatus"] ?? "0");
            var historyStatus = await _unitOfWork.BalanceHistoryStatuses.GetByIdAsync(statusId)
                                 ?? throw new InvalidOperationException("BalanceHistoryStatus not found.");

            client.BalanceHistories.Add(new BalanceHistory
            {
                Sum = -totalCost,
                CreatedAt = DateTime.UtcNow,
                FinishedAt = DateTime.UtcNow,
                BalanceHistoryStatus = historyStatus
            });

            var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId)
                         ?? throw new ArgumentException("Branch not found.");

            var order = new Order
            {
                Client = client,
                Branch = branch,
                ClientNote = request.ClientNote,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = request.ExpectedIn
            };

            client.Orders.Add(order);

            foreach (var items in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(items.ProductId)!;
                order.OrderItems.Add(new OrderItem
                {
                    Product = product,
                    Count = items.Count,
                    Price = product.Price
                });
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
            return await _unitOfWork.Orders.GetManyAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByClientAsync(long clientId)
        {
            return await _unitOfWork.Orders.GetManyAsync(filter: o => o.ClientId == clientId);
        }

        public async Task<Order> MarkOrderAsPickedUpAsync(long orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId)
                         ?? throw new ArgumentException("Order not found.");

            var items = await _unitOfWork.OrderItems.GetManyAsync(
                filter: oi => oi.OrderId == orderId,
                disableTracking: false);

            if (!items.Any() || items.Any(oi => oi.DoneAt == null))
                throw new InvalidOperationException("Order is not ready for pickup.");

            order.FinishedAt = DateTime.UtcNow;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _unitOfWork.Feedbacks.GetManyAsync();
        }

        public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId)
                         ?? throw new ArgumentException("Order not found or not completed.");

            if (order.FinishedAt == null)
                throw new ArgumentException("Order not completed.");

            var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId)
                          ?? throw new ArgumentException("Rating not found.");

            var feedback = new Feedback
            {
                Content = request.Content,
                RatingId = rating.Id,
                OrderId = order.Id
            };

            return await _unitOfWork.Feedbacks.AddOneAsync(feedback);
        }

        public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback)
        {
            var existing = await _unitOfWork.Feedbacks.GetByIdAsync(feedback.Id)
                             ?? throw new ArgumentException("Feedback not found.");

            var rating = await _unitOfWork.Ratings.GetByIdAsync(feedback.RatingId)
                          ?? throw new ArgumentException("Rating not found.");

            existing.Content = feedback.Content;
            existing.RatingId = rating.Id;

            _unitOfWork.Feedbacks.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteFeedbackAsync(long feedbackId)
        {
            await _unitOfWork.Feedbacks.DeleteAsync(feedbackId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(
            long? orderId = null,
            long? employeeId = null,
            OrderItemStatus? status = null,
            long? branchId = null)
        {
            Expression<Func<OrderItem, bool>>? filter = null;

            if (orderId.HasValue && employeeId.HasValue)
            {
                filter = oi => oi.OrderId == orderId.Value && oi.EmployeeId == employeeId.Value;
            }
            else if (orderId.HasValue)
            {
                filter = oi => oi.OrderId == orderId.Value;
            }
            else if (employeeId.HasValue)
            {
                filter = oi => oi.EmployeeId == employeeId.Value;
            }

            if (status.HasValue)
            {
                Expression<Func<OrderItem, bool>> statusFilter = status.Value switch
                {
                    OrderItemStatus.Pending => oi => oi.StartedAt == null && oi.DoneAt == null,
                    OrderItemStatus.InProgress => oi => oi.StartedAt != null && oi.DoneAt == null,
                    OrderItemStatus.Completed => oi => oi.DoneAt != null,
                    _ => throw new ArgumentOutOfRangeException(nameof(status))
                };

                filter = filter == null
                    ? statusFilter
                    : filter.AndAlso(statusFilter);
            }

            if (branchId.HasValue)
            {
                Expression<Func<OrderItem, bool>> branchFilter = oi => oi.Order.BranchId == branchId.Value;
                filter = filter == null
                    ? branchFilter
                    : filter.AndAlso(branchFilter);
            }

            return await _unitOfWork.OrderItems.GetManyAsync(
                filter: filter,
                includes: new List<Expression<Func<OrderItem, object>>>
                {
                    oi => oi.Order
                });
        }



        public async Task<OrderItem> CreateOrderItemAsync(long orderId, CreateOrderItemRequest request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new ArgumentException("Product not found.");
            }

            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Count = request.Count,
                Price = product.Price
            };

            await _unitOfWork.OrderItems.AddOneAsync(orderItem);
            await _unitOfWork.SaveChangesAsync();

            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            var existing = await _unitOfWork.OrderItems.GetByIdAsync(orderItem.Id);

            if (existing == null)
            {
                throw new ArgumentException("OrderItem not found.");
            }

            existing.Count = orderItem.Count;

            _unitOfWork.OrderItems.Update(existing);
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
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId)
                             ?? throw new ArgumentException("OrderItem not found.");

            if (orderItem.EmployeeId.HasValue)
                throw new InvalidOperationException("OrderItem is already assigned.");

            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId)
                              ?? throw new ArgumentException("Employee not found.");

            var order = await _unitOfWork.Orders.GetByIdAsync(orderItem.OrderId)
                           ?? throw new ArgumentException("Order not found.");

            if (employee.BranchId != order.BranchId)
                throw new UnauthorizedAccessException("Employee does not belong to the order's branch.");

            orderItem.EmployeeId = employee.Id;
            orderItem.StartedAt = DateTime.UtcNow;

            _unitOfWork.OrderItems.Update(orderItem);
            await _unitOfWork.SaveChangesAsync();

            return orderItem;
        }

        public async Task<OrderItem> MarkOrderItemCompletedAsync(long orderItemId, long employeeId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId)
                             ?? throw new ArgumentException("OrderItem not found.");

            if (orderItem.EmployeeId != employeeId)
                throw new UnauthorizedAccessException("You are not assigned to this OrderItem.");

            orderItem.DoneAt = DateTime.UtcNow;
            _unitOfWork.OrderItems.Update(orderItem);

            var allItems = await _unitOfWork.OrderItems.GetManyAsync(
                filter: oi => oi.OrderId == orderItem.OrderId);

            if (allItems.All(oi => oi.DoneAt != null))
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(orderItem.OrderId)
                             ?? throw new ArgumentException("Order not found.");

                order.DoneAt = DateTime.UtcNow;
                _unitOfWork.Orders.Update(order);
            }

            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> ReassignOrderItemAsync(long orderItemId, long newEmployeeId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId)
                             ?? throw new ArgumentException("OrderItem not found.");

            var employee = await _unitOfWork.Employees.GetByIdAsync(newEmployeeId)
                              ?? throw new ArgumentException("Employee not found.");

            orderItem.EmployeeId = employee.Id;
            orderItem.StartedAt = DateTime.UtcNow;

            _unitOfWork.OrderItems.Update(orderItem);
            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }
    }
}

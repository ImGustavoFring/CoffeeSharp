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
        private readonly IConfiguration _config;

        public OrderService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<(IEnumerable<Order>, int)> GetOrdersAsync(
            long? clientId,
            long? branchId,
            DateTime? createdFrom,
            DateTime? createdTo,
            OrderStatus? status,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<Order, bool>> filter = order => true;
            if (clientId.HasValue) filter = filter.AndAlso(order => order.ClientId == clientId.Value);
            if (branchId.HasValue) filter = filter.AndAlso(order => order.BranchId == branchId.Value);
            if (createdFrom.HasValue) filter = filter.AndAlso(order => order.CreatedAt >= createdFrom.Value);
            if (createdTo.HasValue) filter = filter.AndAlso(order => order.CreatedAt <= createdTo.Value);
            if (status.HasValue)
            {
                Expression<Func<Order, bool>> statusFilter = status.Value switch
                {
                    OrderStatus.InProgress => order => order.DoneAt == null && order.FinishedAt == null,
                    OrderStatus.ReadyForPickup => order => order.DoneAt != null && order.FinishedAt == null,
                    OrderStatus.Completed => order => order.DoneAt != null && order.FinishedAt != null,
                    _ => throw new ArgumentOutOfRangeException(nameof(status))
                };
                filter = filter.AndAlso(statusFilter);
            }
            var total = await _unitOfWork.Orders.CountAsync(filter);
            var list = await _unitOfWork.Orders.GetManyAsync(filter: filter, pageIndex: pageIndex, pageSize: pageSize);
            return (list, total);
        }

        public async Task<Order?> GetOrderByIdAsync(long id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            var productIds = request.Items.Select(item => item.ProductId).Distinct().ToList();
            var products = (await _unitOfWork.Products.GetManyAsync(product => productIds.Contains(product.Id)))
                .ToDictionary(product => product.Id);

            decimal totalCost = 0;
            foreach (var item in request.Items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    throw new ArgumentException($"Product {item.ProductId} not found.");
                var available = await _unitOfWork.BranchMenus.GetManyAsync(
                    branchMenu => branchMenu.BranchId == request.BranchId && branchMenu.Availability && branchMenu.MenuPresetItems.ProductId == item.ProductId,
                    includes: new List<Expression<Func<BranchMenu, object>>> { branchMenu => branchMenu.MenuPresetItems },
                    disableTracking: true);
                if (!available.Any())
                    throw new ArgumentException($"Product {item.ProductId} not available in branch {request.BranchId}.");
                totalCost += item.Count * product.Price;
            }

            var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId)
                         ?? throw new ArgumentException("Client not found.");
            if (client.Balance < totalCost)
                throw new ArgumentException("Insufficient balance.");

            client.Balance -= totalCost;
            var statusId = long.Parse(_config["Transaction:CompletedStatus"] ?? "0");
            var historyStatus = await _unitOfWork.BalanceHistoryStatuses.GetByIdAsync(statusId)
                             ?? throw new ArgumentException("Balance history status not found.");
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
                ClientId = client.Id,
                BranchId = branch.Id,
                ClientNote = request.ClientNote,
                CreatedAt = DateTime.UtcNow,
                ExpectedIn = request.ExpectedIn
            };
            foreach (var item in request.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Count = item.Count,
                    Price = products[item.ProductId].Price
                });
            }

            await _unitOfWork.Orders.AddOneAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<(IEnumerable<OrderItem>, int)> GetOrderItemsAsync(
            long? orderId,
            long? employeeId,
            OrderItemStatus? status,
            long? branchId,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<OrderItem, bool>> filter = orderItem => true;
            if (orderId.HasValue) filter = filter.AndAlso(orderItem => orderItem.OrderId == orderId.Value);
            if (employeeId.HasValue) filter = filter.AndAlso(orderItem => orderItem.EmployeeId == employeeId.Value);
            if (branchId.HasValue) filter = filter.AndAlso(orderItem => orderItem.Order.BranchId == branchId.Value);
            if (status.HasValue)
            {
                Expression<Func<OrderItem, bool>> statusFilter = status.Value switch
                {
                    OrderItemStatus.Pending => orderItem => orderItem.StartedAt == null && orderItem.DoneAt == null,
                    OrderItemStatus.InProgress => orderItem => orderItem.StartedAt != null && orderItem.DoneAt == null,
                    OrderItemStatus.Completed => orderItem => orderItem.DoneAt != null,
                    _ => throw new ArgumentOutOfRangeException(nameof(status))
                };
                filter = filter.AndAlso(statusFilter);
            }

            var total = await _unitOfWork.OrderItems.CountAsync(filter);
            var list = await _unitOfWork.OrderItems.GetManyAsync(
                filter: filter,
                includes: new List<Expression<Func<OrderItem, object>>> { orderItem => orderItem.Order },
                pageIndex: pageIndex,
                pageSize: pageSize);
            return (list, total);
        }

        public async Task<OrderItem> CreateOrderItemAsync(long orderId, CreateOrderItemRequest request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId)
                       ?? throw new ArgumentException("Product not found.");
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId)
                      ?? throw new ArgumentException("Order not found.");
            var item = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Count = request.Count,
                Price = product.Price
            };
            await _unitOfWork.OrderItems.AddOneAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return item;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            var existing = await _unitOfWork.OrderItems.GetByIdAsync(orderItem.Id)
                           ?? throw new ArgumentException("OrderItem not found.");
            existing.Count = orderItem.Count;
            
            _unitOfWork.OrderItems.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteOrderItemAsync(long id)
        {
            await _unitOfWork.OrderItems.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<OrderItem> AssignOrderItemAsync(long id, long employeeId)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id)
                       ?? throw new ArgumentException("OrderItem not found.");
            if (item.EmployeeId.HasValue)
                throw new InvalidOperationException("OrderItem already assigned.");
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId)
                      ?? throw new ArgumentException("Employee not found.");
            var order = await _unitOfWork.Orders.GetByIdAsync(item.OrderId.Value)
                      ?? throw new ArgumentException("Order not found.");
            if (employee.BranchId != order.BranchId)
                throw new UnauthorizedAccessException("Employee from different branch.");
            item.EmployeeId = employeeId;
            item.StartedAt = DateTime.UtcNow;
            _unitOfWork.OrderItems.Update(item);
            await _unitOfWork.SaveChangesAsync();
            return item;
        }

        public async Task<OrderItem> CompleteOrderItemAsync(long id, long employeeId)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id)
                       ?? throw new ArgumentException("OrderItem not found.");
            if (item.EmployeeId != employeeId)
                throw new UnauthorizedAccessException("Not assigned to this item.");
            item.DoneAt = DateTime.UtcNow;
            _unitOfWork.OrderItems.Update(item);

            var all = await _unitOfWork.OrderItems.GetManyAsync(orderItem => orderItem.OrderId == item.OrderId);
            if (all.All(orderItem => orderItem.DoneAt != null))
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(item.OrderId.Value)
                          ?? throw new ArgumentException("Order not found.");
                order.DoneAt = DateTime.UtcNow;
                _unitOfWork.Orders.Update(order);
            }

            await _unitOfWork.SaveChangesAsync();
            return item;
        }

        public async Task<OrderItem> ReassignOrderItemAsync(long id, long newEmployeeId)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id)
                       ?? throw new ArgumentException("OrderItem not found.");
            var employee = await _unitOfWork.Employees.GetByIdAsync(newEmployeeId)
                      ?? throw new ArgumentException("Employee not found.");
            item.EmployeeId = newEmployeeId;
            item.StartedAt = DateTime.UtcNow;
            _unitOfWork.OrderItems.Update(item);
            await _unitOfWork.SaveChangesAsync();
            return item;
        }

        public async Task<Order> PickupOrderAsync(long orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId)
                      ?? throw new ArgumentException("Order not found.");
            var items = await _unitOfWork.OrderItems.GetManyAsync(orderItem => orderItem.OrderId == orderId);
            if (items.Any(orderItem => orderItem.DoneAt == null))
                throw new InvalidOperationException("Order not ready for pickup.");
            order.FinishedAt = DateTime.UtcNow;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<(IEnumerable<Feedback>, int)> GetFeedbacksAsync(
            long? orderId,
            int? ratingId,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<Feedback, bool>> filter = feedback => true;
            if (orderId.HasValue) filter = filter.AndAlso(feedback => feedback.OrderId == orderId.Value);
            if (ratingId.HasValue) filter = filter.AndAlso(feedback => feedback.RatingId == ratingId.Value);
            var total = await _unitOfWork.Feedbacks.CountAsync(filter);
            var list = await _unitOfWork.Feedbacks.GetManyAsync(filter: filter, pageIndex: pageIndex, pageSize: pageSize);
            return (list, total);
        }

        public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId)
                      ?? throw new ArgumentException("Order not found.");
            if (order.FinishedAt == null)
                throw new ArgumentException("Order not completed yet.");
            var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId)
                         ?? throw new ArgumentException("Rating not found.");
            var feedback = new Feedback
            {
                OrderId = order.Id,
                RatingId = rating.Id,
                Content = request.Content
            };
            await _unitOfWork.Feedbacks.AddOneAsync(feedback);
            await _unitOfWork.SaveChangesAsync();
            return feedback;
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

        public async Task DeleteFeedbackAsync(long id)
        {
            var existing = await _unitOfWork.Feedbacks.GetByIdAsync(id)
                           ?? throw new ArgumentException("Feedback not found.");

            await _unitOfWork.Feedbacks.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

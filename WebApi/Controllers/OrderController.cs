using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using Domain.DTOs.Order.Requests;
using Domain.DTOs.Shared;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] long? clientId,
            [FromQuery] long? branchId,
            [FromQuery] DateTime? createdFrom,
            [FromQuery] DateTime? createdTo,
            [FromQuery] OrderStatus? status,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (orders, total) = await _orderService.GetOrdersAsync(
                clientId, branchId, createdFrom, createdTo, status, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", total.ToString());
            var dtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                ClientId = order.ClientId,
                ClientNote = order.ClientNote,
                CreatedAt = order.CreatedAt,
                DoneAt = order.DoneAt,
                FinishedAt = order.FinishedAt,
                ExpectedIn = order.ExpectedIn,
                BranchId = order.BranchId
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            var dto = new OrderDto
            {
                Id = order.Id,
                ClientId = order.ClientId,
                ClientNote = order.ClientNote,
                CreatedAt = order.CreatedAt,
                DoneAt = order.DoneAt,
                FinishedAt = order.FinishedAt,
                ExpectedIn = order.ExpectedIn,
                BranchId = order.BranchId
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            try
            {
                var order = new Order
                {
                    ClientId = request.ClientId,
                    BranchId = request.BranchId,
                    ClientNote = request.ClientNote,
                    ExpectedIn = request.ExpectedIn,
                    OrderItems = request.Items
                        .Select(i => new OrderItem
                        {
                            ProductId = i.ProductId,
                            Count = i.Count
                        })
                        .ToList()
                };
                var created = await _orderService.CreateOrderAsync(order);
                var dto = new OrderDto
                {
                    Id = created.Id,
                    ClientId = created.ClientId,
                    ClientNote = created.ClientNote,
                    CreatedAt = created.CreatedAt,
                    ExpectedIn = created.ExpectedIn,
                    BranchId = created.BranchId
                };
                return CreatedAtAction(nameof(GetOrderById), new { id = dto.Id }, dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("items")]
        [Authorize(Policy = "AllStaff")]
        public async Task<IActionResult> GetOrderItems(
            [FromQuery] long? orderId,
            [FromQuery] long? employeeId,
            [FromQuery] OrderItemStatus? status,
            [FromQuery] long? branchId,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _orderService.GetOrderItemsAsync(
                orderId, employeeId, status, branchId, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", total.ToString());
            var dtos = items.Select(orderItem => new OrderItemDto
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                EmployeeId = orderItem.EmployeeId,
                Price = orderItem.Price,
                Count = orderItem.Count,
                StartedAt = orderItem.StartedAt,
                DoneAt = orderItem.DoneAt
            });
            return Ok(dtos);
        }

        [HttpPost("items")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateOrderItem([FromBody] CreateOrderItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var orderItem = new OrderItem
                {
                    OrderId = request.OrderId,
                    ProductId = request.ProductId,
                    Count = request.Count
                };

                var createdItem = await _orderService.CreateOrderItemAsync(orderItem);

                var dto = new OrderItemDto
                {
                    Id = orderItem.Id,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    EmployeeId = orderItem.EmployeeId,
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    StartedAt = orderItem.StartedAt,
                    DoneAt = orderItem.DoneAt
                };
                return CreatedAtAction(nameof(GetOrderById), new { id = createdItem.OrderId }, dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("items/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateOrderItem(long id, [FromBody] UpdateOrderItemRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            try
            {
                var orderItem = await _orderService.UpdateOrderItemAsync(new OrderItem
                {
                    Id = request.Id,
                    Count = request.Count
                });
                var dto = new OrderItemDto
                {
                    Id = orderItem.Id,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    EmployeeId = orderItem.EmployeeId,
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    StartedAt = orderItem.StartedAt,
                    DoneAt = orderItem.DoneAt
                };
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("items/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteOrderItem(long id)
        {
            try
            {
                await _orderService.DeleteOrderItemAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("items/{id}/assign")]
        [Authorize(Policy = "AllStaff")]
        public async Task<IActionResult> AssignOrderItem(long id)
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                var orderItem = await _orderService.AssignOrderItemAsync(id, employeeId);
                var dto = new OrderItemDto
                {
                    Id = orderItem.Id,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    EmployeeId = orderItem.EmployeeId,
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    StartedAt = orderItem.StartedAt,
                    DoneAt = orderItem.DoneAt
                };
                return Ok(dto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPatch("items/{id}/complete")]
        [Authorize(Policy = "AllStaff")]
        public async Task<IActionResult> CompleteOrderItem(long id)
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                var orderItem = await _orderService.CompleteOrderItemAsync(id, employeeId);
                var dto = new OrderItemDto
                {
                    Id = orderItem.Id,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    EmployeeId = orderItem.EmployeeId,
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    StartedAt = orderItem.StartedAt,
                    DoneAt = orderItem.DoneAt
                };
                return Ok(dto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPatch("items/{id}/reassign")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> ReassignOrderItem(long id, [FromQuery] long newEmployeeId)
        {
            try
            {
                var orderItem = await _orderService.ReassignOrderItemAsync(id, newEmployeeId);
                var dto = new OrderItemDto
                {
                    Id = orderItem.Id,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    EmployeeId = orderItem.EmployeeId,
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    StartedAt = orderItem.StartedAt,
                    DoneAt = orderItem.DoneAt
                };
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/pickup")]
        [Authorize(Policy = "AllStaff")]
        public async Task<IActionResult> PickupOrder(long id)
        {
            try
            {
                var order = await _orderService.PickupOrderAsync(id);
                var dto = new OrderDto
                {
                    Id = order.Id,
                    ClientId = order.ClientId,
                    ClientNote = order.ClientNote,
                    CreatedAt = order.CreatedAt,
                    DoneAt = order.DoneAt,
                    FinishedAt = order.FinishedAt,
                    ExpectedIn = order.ExpectedIn,
                    BranchId = order.BranchId
                };
                return Ok(dto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("feedbacks")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetFeedbacks(
            [FromQuery] long? orderId,
            [FromQuery] int? ratingId,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _orderService.GetFeedbacksAsync(orderId, ratingId, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", total.ToString());
            var dtos = items.Select(feedback => new FeedbackDto
            {
                Id = feedback.Id,
                Content = feedback.Content,
                RatingId = feedback.RatingId,
                OrderId = feedback.OrderId
            });
            return Ok(dtos);
        }

        [HttpPost("feedbacks")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var feedbackEntity = new Feedback
                {
                    OrderId = request.OrderId,
                    RatingId = request.RatingId,
                    Content = request.Content
                };

                var feedback = await _orderService.CreateFeedbackAsync(feedbackEntity);

                var dto = new FeedbackDto
                {
                    Id = feedback.Id,
                    Content = feedback.Content,
                    RatingId = feedback.RatingId,
                    OrderId = feedback.OrderId
                };

                return CreatedAtAction(nameof(GetOrderById), new { id = dto.OrderId }, dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("feedbacks/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateFeedback(long id, [FromBody] UpdateFeedbackRequest request)
        {
            if (id != request.Id) return BadRequest("ID mismatch");
            try
            {
                var feedback = await _orderService.UpdateFeedbackAsync(new Feedback
                {
                    Id = request.Id,
                    Content = request.Content,
                    RatingId = request.RatingId
                });
                var dto = new FeedbackDto
                {
                    Id = feedback.Id,
                    Content = feedback.Content,
                    RatingId = feedback.RatingId,
                    OrderId = feedback.OrderId
                };
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("feedbacks/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteFeedback(long id)
        {
            try
            {
                await _orderService.DeleteFeedbackAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private long GetCurrentEmployeeId()
        {
            var claim = User.FindFirst("id");
            if (claim == null || !long.TryParse(claim.Value, out var id))
                throw new UnauthorizedAccessException();
            return id;
        }
    }
}


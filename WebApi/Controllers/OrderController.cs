using CoffeeSharp.Domain.Entities;
using Domain.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                ClientId = o.ClientId,
                ClientNote = o.ClientNote,
                CreatedAt = o.CreatedAt,
                DoneAt = o.DoneAt,
                FinishedAt = o.FinishedAt,
                ExpectedIn = o.ExpectedIn,
                BranchId = o.BranchId
            });
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            var orderDto = new OrderDto
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
            return Ok(orderDto);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetOrdersByClient(long clientId)
        {
            var orders = await _orderService.GetOrdersByClientAsync(clientId);
            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                ClientId = o.ClientId,
                ClientNote = o.ClientNote,
                CreatedAt = o.CreatedAt,
                DoneAt = o.DoneAt,
                FinishedAt = o.FinishedAt,
                ExpectedIn = o.ExpectedIn,
                BranchId = o.BranchId
            });
            return Ok(orderDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var order = await _orderService.CreateOrderAsync(request);
                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    ClientId = order.ClientId,
                    ClientNote = order.ClientNote,
                    CreatedAt = order.CreatedAt,
                    ExpectedIn = order.ExpectedIn,
                    BranchId = order.BranchId
                };
                return CreatedAtAction(nameof(GetOrderById), new { id = orderDto.Id }, orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetOrderItems(
            [FromQuery] long? orderId,
            [FromQuery] long? employeeId,
            [FromQuery] OrderItemStatus? status,
            [FromQuery] long? branchId)
        {
            var items = await _orderService.GetOrderItemsAsync(orderId, employeeId, status, branchId);

            var dtos = items.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                OrderId = oi.OrderId,
                ProductId = oi.ProductId,
                EmployeeId = oi.EmployeeId,
                Price = oi.Price,
                Count = oi.Count,
                StartedAt = oi.StartedAt,
                DoneAt = oi.DoneAt
            });

            return Ok(dtos);
        }

        [HttpPost("{orderId}/item")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateOrderItem(long orderId, [FromBody] CreateOrderItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var orderItem = await _orderService.CreateOrderItemAsync(orderId, request);
                var itemDto = new OrderItemDto
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
                return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, itemDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("item/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateOrderItem(long id, [FromBody] OrderItemDto request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch.");
            try
            {
                var orderItem = new OrderItem
                {
                    Id = request.Id,
                    Count = request.Count
                };
                var updated = await _orderService.UpdateOrderItemAsync(orderItem);
                var dto = new OrderItemDto
                {
                    Id = updated.Id,
                    OrderId = updated.OrderId,
                    ProductId = updated.ProductId,
                    EmployeeId = updated.EmployeeId,
                    Price = updated.Price,
                    Count = updated.Count,
                    StartedAt = updated.StartedAt,
                    DoneAt = updated.DoneAt
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("item/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteOrderItem(long id)
        {
            try
            {
                await _orderService.DeleteOrderItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("item/{id}/reassign")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> ReassignOrderItem(long id, [FromQuery] long newEmployeeId)
        {
            try
            {
                var item = await _orderService.ReassignOrderItemAsync(id, newEmployeeId);
                var dto = new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    EmployeeId = item.EmployeeId,
                    Price = item.Price,
                    Count = item.Count,
                    StartedAt = item.StartedAt,
                    DoneAt = item.DoneAt
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("item/{id}/assign")]
        [Authorize(Policy = "AllStaff")]
        public async Task<IActionResult> AssignOrderItem(long id)
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                var item = await _orderService.AssignOrderItemToCookAsync(id, employeeId);
                var dto = new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    EmployeeId = item.EmployeeId,
                    Price = item.Price,
                    Count = item.Count,
                    StartedAt = item.StartedAt,
                    DoneAt = item.DoneAt
                };
                return Ok(dto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("item/{id}/complete")]
        [Authorize(Policy = "AllStaff")]
        public async Task<IActionResult> CompleteOrderItem(long id)
        {
            try
            {
                var employeeId = GetCurrentEmployeeId();
                var item = await _orderService.MarkOrderItemCompletedAsync(id, employeeId);
                var dto = new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    EmployeeId = item.EmployeeId,
                    Price = item.Price,
                    Count = item.Count,
                    StartedAt = item.StartedAt,
                    DoneAt = item.DoneAt
                };
                return Ok(dto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private long GetCurrentEmployeeId()
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long employeeId))
            {
                throw new UnauthorizedAccessException("Invalid user identifier");
            }
            return employeeId;
        }

        [HttpPatch("{id}/pickup")]
        [Authorize(Policy = "AllStaff")]
        public async Task<IActionResult> MarkOrderAsPickedUp(long id)
        {
            try
            {
                var order = await _orderService.MarkOrderAsPickedUpAsync(id);
                var orderDto = new OrderDto
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
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("feedback")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbacks = await _orderService.GetAllFeedbacksAsync();
            var feedbackDtos = feedbacks.Select(f => new FeedbackDto
            {
                Id = f.Id,
                Content = f.Content,
                RatingId = f.RatingId,
                OrderId = f.OrderId
            });
            return Ok(feedbackDtos);
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var feedback = await _orderService.CreateFeedbackAsync(request);
                var dto = new FeedbackDto
                {
                    Id = feedback.Id,
                    Content = feedback.Content,
                    RatingId = feedback.RatingId,
                    OrderId = feedback.OrderId
                };
                return CreatedAtAction(nameof(GetOrderById), new { id = feedback.OrderId }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("feedback/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateFeedback(long id, [FromBody] UpdateFeedbackRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch.");
            try
            {
                var feedback = new Feedback
                {
                    Id = request.Id,
                    Content = request.Content,
                    RatingId = request.RatingId
                };
                var updated = await _orderService.UpdateFeedbackAsync(feedback);
                var dto = new FeedbackDto
                {
                    Id = updated.Id,
                    Content = updated.Content,
                    RatingId = updated.RatingId,
                    OrderId = updated.OrderId
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("feedback/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteFeedback(long id)
        {
            try
            {
                await _orderService.DeleteFeedbackAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}


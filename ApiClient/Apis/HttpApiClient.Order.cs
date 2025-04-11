using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<IEnumerable<OrderDto>> GetAllOrders()
    {
        var response = await _http.GetAsync("api/order");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<OrderDto>>(content)!;
    }

    public async Task<OrderDto> GetOrderById(long id)
    {
        var response = await _http.GetAsync($"api/order/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderDto>(content)!;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByClient(long clientId)
    {
        var response = await _http.GetAsync($"api/order/client/{clientId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<OrderDto>>(content)!;
    }

    public async Task<OrderDto> CreateOrder(CreateOrderRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/order", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderDto>(content)!;
    }

    public async Task<IEnumerable<OrderItemDto>> GetOrderItems(long orderId)
    {
        var response = await _http.GetAsync($"api/order/{orderId}/items");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<OrderItemDto>>(content)!;
    }

    public async Task<OrderItemDto> CreateOrderItem(long orderId, CreateOrderItemRequest request)
    {
        var response = await _http.PostAsJsonAsync($"api/order/{orderId}/item", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderItemDto>(content)!;
    }

    public async Task<OrderItemDto> UpdateOrderItem(OrderItemDto request)
    {
        var response = await _http.PutAsJsonAsync($"api/order/item/{request.Id}", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderItemDto>(content)!;
    }

    public async Task DeleteOrderItem(long id)
    {
        var response = await _http.DeleteAsync($"api/order/item/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<OrderItemDto> ReassignOrderItem(long id, long newEmployeeId)
    {
        var response = await _http.PatchAsync($"api/order/item/{id}/reassign?newEmployeeId={newEmployeeId}", null);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderItemDto>(content)!;
    }

    public async Task<OrderItemDto> AssignOrderItem(long id)
    {
        var response = await _http.PatchAsync($"api/order/item/{id}/assign", null);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderItemDto>(content)!;
    }

    public async Task<OrderItemDto> CompleteOrderItem(long id)
    {
        var response = await _http.PatchAsync($"api/order/item/{id}/complete", null);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderItemDto>(content)!;
    }

    public async Task<OrderDto> MarkOrderAsPickedUp(long id)
    {
        var response = await _http.PatchAsync($"api/order/{id}/pickup", null);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderDto>(content)!;
    }

    public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacks()
    {
        var response = await _http.GetAsync("api/order/feedback");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<FeedbackDto>>(content)!;
    }

    public async Task<FeedbackDto> CreateFeedback(CreateFeedbackRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/order/feedback", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FeedbackDto>(content)!;
    }

    public async Task<FeedbackDto> UpdateFeedback(UpdateFeedbackRequest request)
    {
        var response = await _http.PutAsJsonAsync($"api/order/feedback/{request.Id}", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FeedbackDto>(content)!;
    }

    public async Task DeleteFeedback(long id)
    {
        var response = await _http.DeleteAsync($"api/order/feedback/{id}");
        response.EnsureSuccessStatusCode();
    }
}
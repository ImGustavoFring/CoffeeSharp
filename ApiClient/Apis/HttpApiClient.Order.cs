using System.Net.Http.Json;
using Domain.DTOs;
using Domain.DTOs.Order.Requests;
using Domain.DTOs.Shared;
using Domain.Enums;
using Microsoft.AspNetCore.WebUtilities;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string OrdersControllerPath = "/api/orders";

    public async Task<(IEnumerable<OrderDto>, int)> GetOrders(
        long? clientId = null,
        long? branchId = null,
        DateTime? createdFrom = null,
        DateTime? createdTo = null,
        OrderStatus? status = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string>();

        if (clientId.HasValue) queryParams["clientId"] = clientId.Value.ToString();
        if (branchId.HasValue) queryParams["branchId"] = branchId.Value.ToString();
        if (createdFrom.HasValue) queryParams["createdFrom"] = createdFrom.Value.ToString("O");
        if (createdTo.HasValue) queryParams["createdTo"] = createdTo.Value.ToString("O");
        if (status.HasValue) queryParams["status"] = status.Value.ToString();
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString(OrdersControllerPath, queryParams);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var totalCountHeader = response.Headers.TryGetValues("X-Total-Count", out var values) ? values.FirstOrDefault() : null;
        int total = totalCountHeader != null && int.TryParse(totalCountHeader, out var t) ? t : 0;

        var orders = await response.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>();
        return (orders!, total);
    }

    public async Task<OrderDto?> GetOrderById(long id)
    {
        var response = await _http.GetAsync($"{OrdersControllerPath}/{id}");
        if (!response.IsSuccessStatusCode) return null;

        response.EnsureSuccessStatusCode();
        var order = await response.Content.ReadFromJsonAsync<OrderDto>();
        return order!;
    }

    public async Task<OrderDto> CreateOrder(CreateOrderRequest request)
    {
        var response = await _http.PostAsJsonAsync(OrdersControllerPath, request);
        response.EnsureSuccessStatusCode();
        var order = await response.Content.ReadFromJsonAsync<OrderDto>();
        return order!;
    }

    public async Task<(IEnumerable<OrderItemDto>, int)> GetOrderItems(
        long? orderId = null,
        long? employeeId = null,
        OrderItemStatus? status = null,
        long? branchId = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string>();
        if (orderId.HasValue) queryParams["orderId"] = orderId.Value.ToString();
        if (employeeId.HasValue) queryParams["employeeId"] = employeeId.Value.ToString();
        if (status.HasValue) queryParams["status"] = status.Value.ToString();
        if (branchId.HasValue) queryParams["branchId"] = branchId.Value.ToString();
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{OrdersControllerPath}/items", queryParams!);
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var totalCountHeader = response.Headers.TryGetValues("X-Total-Count", out var values) ? values.FirstOrDefault() : null;
        int total = totalCountHeader != null && int.TryParse(totalCountHeader, out var t) ? t : 0;

        var items = await response.Content.ReadFromJsonAsync<IEnumerable<OrderItemDto>>();
        return (items!, total);
    }

    public async Task<OrderItemDto> CreateOrderItem(CreateOrderItemRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{OrdersControllerPath}/items", request);
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadFromJsonAsync<OrderItemDto>();
        return item!;
    }

    public async Task<OrderItemDto> UpdateOrderItem(long id, UpdateOrderItemRequest request)
    {
        if (id != request.Id) throw new ArgumentException("ID mismatch");

        var response = await _http.PutAsJsonAsync($"{OrdersControllerPath}/items/{id}", request);
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadFromJsonAsync<OrderItemDto>();
        return item!;
    }

    public async Task DeleteOrderItem(long id)
    {
        var response = await _http.DeleteAsync($"{OrdersControllerPath}/items/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<OrderItemDto> AssignOrderItem(long id)
    {
        var response = await _http.PatchAsync($"{OrdersControllerPath}/items/{id}/assign", null);
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadFromJsonAsync<OrderItemDto>();
        return item!;
    }

    public async Task<OrderItemDto> CompleteOrderItem(long id)
    {
        var response = await _http.PatchAsync($"{OrdersControllerPath}/items/{id}/complete", null);
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadFromJsonAsync<OrderItemDto>();
        return item!;
    }

    public async Task<OrderItemDto> ReassignOrderItem(long id, long newEmployeeId)
    {
        var url = QueryHelpers.AddQueryString($"{OrdersControllerPath}/items/{id}/reassign",
            new Dictionary<string, string> { { "newEmployeeId", newEmployeeId.ToString() } }!);

        var response = await _http.PatchAsync(url, null);
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadFromJsonAsync<OrderItemDto>();
        return item!;
    }

    public async Task<OrderDto> PickupOrder(long id)
    {
        var response = await _http.PatchAsync($"{OrdersControllerPath}/{id}/pickup", null);
        response.EnsureSuccessStatusCode();
        var order = await response.Content.ReadFromJsonAsync<OrderDto>();
        return order!;
    }

    public async Task<(IEnumerable<FeedbackDto>, int)> GetFeedbacks(
        long? orderId = null,
        int? ratingId = null,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string>();
        if (orderId.HasValue) queryParams["orderId"] = orderId.Value.ToString();
        if (ratingId.HasValue) queryParams["ratingId"] = ratingId.Value.ToString();
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{OrdersControllerPath}/feedbacks", queryParams!);
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var totalCountHeader = response.Headers.TryGetValues("X-Total-Count", out var values) ? values.FirstOrDefault() : null;
        int total = totalCountHeader != null && int.TryParse(totalCountHeader, out var t) ? t : 0;

        var items = await response.Content.ReadFromJsonAsync<IEnumerable<FeedbackDto>>();
        return (items!, total);
    }

    public async Task<FeedbackDto> CreateFeedback(CreateFeedbackRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{OrdersControllerPath}/feedbacks", request);
        response.EnsureSuccessStatusCode();
        var feedback = await response.Content.ReadFromJsonAsync<FeedbackDto>();
        return feedback!;
    }

    public async Task<FeedbackDto> UpdateFeedback(long id, UpdateFeedbackRequest request)
    {
        if (id != request.Id) throw new ArgumentException("ID mismatch");

        var response = await _http.PutAsJsonAsync($"{OrdersControllerPath}/feedbacks/{id}", request);
        response.EnsureSuccessStatusCode();
        var feedback = await response.Content.ReadFromJsonAsync<FeedbackDto>();
        return feedback!;
    }

    public async Task DeleteFeedback(long id)
    {
        var response = await _http.DeleteAsync($"{OrdersControllerPath}/feedbacks/{id}");
        response.EnsureSuccessStatusCode();
    }
}

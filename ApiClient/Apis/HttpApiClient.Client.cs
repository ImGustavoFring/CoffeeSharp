using System.Net.Http.Json;
using Domain.DTOs;
using Domain.DTOs.Client.Requests;
using Domain.DTOs.Shared;
using Domain.Enums;
using Microsoft.AspNetCore.WebUtilities;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private const string ClientControllerPath = "/api/clients";

    public async Task<IEnumerable<ClientDto>> GetClients(string? telegramId = null, string? name = null,
        int pageIndex = 0, int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(telegramId)) queryParams["telegramId"] = telegramId;
        if (!string.IsNullOrEmpty(name)) queryParams["name"] = name;
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString(ClientControllerPath, queryParams);
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<ClientDto>>())!;
    }

    public async Task<ClientDto> GetClientById(long id)
    {
        var response = await _http.GetAsync($"{ClientControllerPath}/{id}");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ClientDto>())!;
    }

    public async Task<ClientDto> CreateClient(CreateClientRequest request)
    {
        var response = await _http.PostAsJsonAsync(ClientControllerPath, request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ClientDto>())!;
    }

    public async Task<ClientDto> UpdateClient(long id, UpdateClientRequest request)
    {
        var response = await _http.PutAsJsonAsync($"{ClientControllerPath}/{id}", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ClientDto>())!;
    }

    public async Task DeleteClient(long id)
    {
        var response = await _http.DeleteAsync($"{ClientControllerPath}/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<BalanceHistoryDto> AddBalance(long clientId, AddBalanceRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{ClientControllerPath}/{clientId}/balance/add", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<BalanceHistoryDto>())!;
    }

    public async Task<BalanceHistoryDto> DeductBalance(long clientId, DeductBalanceRequest request)
    {
        var response = await _http.PostAsJsonAsync($"{ClientControllerPath}/{clientId}/balance/deduct", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<BalanceHistoryDto>())!;
    }

    public async Task<IEnumerable<BalanceHistoryDto>> GetClientTransactions(long? clientId = null,
        TransactionType transactionType = TransactionType.All,
        TransactionStatus? transactionStatus = null,
        DateTime? createdFrom = null,
        DateTime? createdTo = null,
        DateTime? finishedFrom = null,
        DateTime? finishedTo = null,
        bool orderByNewestFirst = true,
        int pageIndex = 0,
        int pageSize = 50)
    {
        var queryParams = new Dictionary<string, string?>();
        if (clientId.HasValue) queryParams["clientId"] = clientId.Value.ToString();
        queryParams["transactionType"] = transactionType.ToString();
        if (transactionStatus.HasValue) queryParams["transactionStatus"] = transactionStatus.Value.ToString();
        if (createdFrom.HasValue) queryParams["createdFrom"] = createdFrom.Value.ToString("O");
        if (createdTo.HasValue) queryParams["createdTo"] = createdTo.Value.ToString("O");
        if (finishedFrom.HasValue) queryParams["finishedFrom"] = finishedFrom.Value.ToString("O");
        if (finishedTo.HasValue) queryParams["finishedTo"] = finishedTo.Value.ToString("O");
        queryParams["orderByNewestFirst"] = orderByNewestFirst.ToString();
        queryParams["pageIndex"] = pageIndex.ToString();
        queryParams["pageSize"] = pageSize.ToString();

        var url = QueryHelpers.AddQueryString($"{ClientControllerPath}/transactions", queryParams);
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<BalanceHistoryDto>>())!;
    }

    public async Task<ClientDto> CompleteBalanceTransaction(long transactionId)
    {
        var response = await _http.PatchAsync($"{ClientControllerPath}/transactions/{transactionId}/complete", null);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ClientDto>())!;
    }

    public async Task<ClientDto> CancelTransaction(long transactionId)
    {
        var response = await _http.PatchAsync($"{ClientControllerPath}/transactions/{transactionId}/cancel", null);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ClientDto>())!;
    }
}

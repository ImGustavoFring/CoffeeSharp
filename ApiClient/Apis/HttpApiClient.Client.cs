using System.Text.Json;
using Domain.DTOs;
using Domain.Enums;

namespace ApiClient.Apis;

public partial class HttpApiClient
{
    public async Task<IEnumerable<ClientDto>> GetAllClients()
    {
        var response = await _http.GetAsync("api/client");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ClientDto>>(content)!;
    }

    public async Task<ClientDto> GetClientById(long id)
    {
        var response = await _http.GetAsync($"api/client/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClientDto>(content)!;
    }

    public async Task<ClientDto> CreateClient(CreateClientRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PostAsync("api/client", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClientDto>(resultContent)!;
    }

    public async Task<ClientDto> UpdateClient(long id, UpdateClientRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PutAsync($"api/client/{id}", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClientDto>(resultContent)!;
    }

    public async Task DeleteClient(long id)
    {
        var response = await _http.DeleteAsync($"api/client/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<ClientDto> AddBalance(long clientId, AddBalanceRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PostAsync($"api/client/{clientId}/balance/add", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClientDto>(resultContent)!;
    }

    public async Task<ClientDto> DeductBalance(long clientId, DeductBalanceRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PostAsync($"api/client/{clientId}/balance/deduct", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClientDto>(resultContent)!;
    }

    public async Task<IEnumerable<BalanceHistoryDto>> GetClientTransactions(long clientId,
        bool orderByNewestFirst = true, TransactionType transactionType = TransactionType.All)
    {
        var response =
            await _http.GetAsync(
                $"api/client/{clientId}/transactions?orderByNewestFirst={orderByNewestFirst}&transactionType={transactionType}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<BalanceHistoryDto>>(content)!;
    }

    public async Task<CancelTransactionResponse> CancelTransaction(CancelTransactionRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8,
            "application/json");
        var response = await _http.PostAsync("api/client/transaction/cancel", content);
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CancelTransactionResponse>(resultContent)!;
    }
}
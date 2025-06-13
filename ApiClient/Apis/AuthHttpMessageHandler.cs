namespace ApiClient.Apis;

public class AuthHttpMessageHandler: DelegatingHandler
{
    private readonly Func<string?> _getToken;

    public AuthHttpMessageHandler(Func<string?> getToken)
    {
        _getToken = getToken;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _getToken();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        Console.WriteLine($"REQUEST: {request}");
        if (request.Content != null)
        {
            Console.WriteLine(request.Content.ReadAsStringAsync().Result);
        }
        var resp = await base.SendAsync(request, cancellationToken);
        Console.WriteLine($"RESPONSE: {resp}");
        if (resp.Content != null)
        {
            Console.WriteLine(resp.Content.ReadAsStringAsync().Result);
        }
        return resp;
    }
}
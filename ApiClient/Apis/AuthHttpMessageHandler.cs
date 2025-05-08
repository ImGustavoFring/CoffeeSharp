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

        return await base.SendAsync(request, cancellationToken);
    }
}
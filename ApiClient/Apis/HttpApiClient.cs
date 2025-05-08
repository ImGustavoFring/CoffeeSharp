namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private string? _accessToken;
    private readonly HttpClient _http;

    public HttpApiClient(string baseUrl)
    {
        var handler = new AuthHttpMessageHandler(() => _accessToken)
        {
            InnerHandler = new HttpClientHandler()
        };
        _http = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public void SetAccessToken(string token)
    {
        _accessToken = token;
    }
}
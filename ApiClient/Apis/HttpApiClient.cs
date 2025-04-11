namespace ApiClient.Apis;

public partial class HttpApiClient
{
    private readonly HttpClient _http;

    public HttpApiClient(HttpClient httpClient)
    {
        _http = httpClient;
    }
}
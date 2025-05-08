using System;
using ApiClient.Apis;

namespace Client.Services;

public sealed class HttpClient : HttpApiClient
{
    private static readonly Lazy<HttpClient> _instance =
        new Lazy<HttpClient>(() => new HttpClient("http://localhost:5000/"));

    private HttpClient(string baseUrl) : base(baseUrl)
    {
    }

    public static HttpClient Instance => _instance.Value;
}

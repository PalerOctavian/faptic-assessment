using System.Net;
using Newtonsoft.Json;

namespace FapticService.Common.Extensions;

public static class HttpExtensions
{
    public static async Task<TResponse> GetAndParseAsync<TResponse>(
        this HttpClient client,
        string? requestUri,
        IDictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Path.Combine(client.BaseAddress.ToString(), requestUri)),
            
        };

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        TResponse? parsed;
        using (var httpResponse = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken))
        {
            await HandleHttpStatusCodeBadRequestAsync(httpResponse);
            parsed = await DeserializeResponseAsync<TResponse>(httpResponse);
        }

        return parsed;
    }
    
    private static async Task<TResponse> DeserializeResponseAsync<TResponse>(
        HttpResponseMessage httpResponseMessage)
    {
        httpResponseMessage.EnsureSuccessStatusCode();
        var s = await httpResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(s);
    }
    
    private static async Task HandleHttpStatusCodeBadRequestAsync(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            throw new ArgumentException(await httpResponseMessage.Content.ReadAsStringAsync());
    }
}
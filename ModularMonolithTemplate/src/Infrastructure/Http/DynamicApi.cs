using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.Http;

public sealed class DynamicApi(HttpClient http)
    : IDynamicApi
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T> GetAsync<T>(string url, CancellationToken ct = default)
    {
        // url can be relative or absolute; HttpClient supports both
        var result = await http.GetFromJsonAsync<T>(url, JsonOptions, ct);
        if (result is null)
        {
            throw new InvalidOperationException($"GET {url} returned no content.");
        }
        return result;
    }

    public async Task<TOut> PostAsync<TIn, TOut>(string url, TIn body, CancellationToken ct = default)
    {
        using var resp = await http.PostAsJsonAsync(url, body, JsonOptions, ct);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<TOut>(JsonOptions, ct);
        if (result is null)
        {
            throw new InvalidOperationException($"POST {url} returned no content.");
        }
        return result;
    }
}

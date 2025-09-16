using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.Http;

public interface IDynamicApi
{
    Task<T> GetAsync<T>(string url, CancellationToken ct = default);
    Task<TOut> PostAsync<TIn, TOut>(string url, TIn body, CancellationToken ct = default);
}

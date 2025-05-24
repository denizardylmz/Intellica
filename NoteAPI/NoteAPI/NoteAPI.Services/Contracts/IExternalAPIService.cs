using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NoteAPI.Services.Contracts;

public interface IExternalApiService
{
    Task<TResponse> GetAsync<TResponse>(
        string url,
        List<(string key, string value)> queryParams,
        string keyName = "api-key",
        string key = null,
        CancellationToken cancellationToken = default);
    Task<TResponse> PostAsync<TRequest,TResponse>(string url, TRequest payload,  string keyName = "api-key", string key = null, CancellationToken cancellationToken = default);
}
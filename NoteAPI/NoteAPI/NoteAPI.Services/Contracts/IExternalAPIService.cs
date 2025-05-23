using System.Threading;
using System.Threading.Tasks;

namespace NoteAPI.Services.Contracts;

public interface IExternalApiService
{
    Task<TResponse> GetAsync<TResponse>(string url, string key = null, CancellationToken cancellationToken = default);
    Task<TResponse> PostAsync<TRequest,TResponse>(string url, TRequest payload, string key = null, CancellationToken cancellationToken = default);
}
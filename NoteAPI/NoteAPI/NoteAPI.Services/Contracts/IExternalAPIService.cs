using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NoteAPI.Services.Models.OllamaModels;

namespace NoteAPI.Services.Contracts;

public interface IExternalApiService
{
    void SetClient(HttpClient _httpClient);
    Task<TResponse> GetAsync<TResponse>(
        string url,
        List<(string key, string value)> queryParams,
        string keyName = "api-key",
        string key = null,
        CancellationToken cancellationToken = default);
    Task<TResponse> PostAsync<TRequest,TResponse>(string url, TRequest payload,  string keyName = "api-key", string key = null, CancellationToken cancellationToken = default);
    Task<OllamaFullResponse> TalkWithAI(OllamaRequest request, CancellationToken cancellationToken = default);
    
}
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NoteAPI.Services.Contracts;

namespace NoteAPI.Services.Services;


public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    
    public ExternalApiService(HttpClient httpClient)
    {
            _httpClient = httpClient;
    }
    
    public async Task<TResponse> GetAsync<TResponse>(string url, string key = null, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (key != null)    
            request.Headers.TryAddWithoutValidation("api-key", key);

        var result = await _httpClient.SendAsync(request, cancellationToken);

        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException(await result.Content.ReadAsStringAsync(cancellationToken));
            
        var json = await result.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponse>(json);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest payload, string key = null,
        CancellationToken cancellationToken = default)
    {
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var stringContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = stringContent;
        
        if (key != null)    
            request.Headers.TryAddWithoutValidation("api-key", key);
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        
        if (!response.IsSuccessStatusCode)  
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(cancellationToken));
        
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponse>(json);
    }
}
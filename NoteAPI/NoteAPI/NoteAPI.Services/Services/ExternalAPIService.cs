using System;
using System.Collections.Generic;
using System.Linq;
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
    
    public async Task<TResponse> GetAsync<TResponse>(
        string url, 
        List<(string key, string value)> queryParams,
        string keyName = "api-key",
        string key = null,
        CancellationToken cancellationToken = default)
    {
        if (queryParams != null && queryParams.Count != 0)
        {
            var queryString = string.Join("&",
                queryParams.Select(param => $"{Uri.EscapeDataString(param.key)}={Uri.EscapeDataString(param.value)}"));
        
            url += url.Contains('?') ? "&" : "?";
            url += queryString;
        }
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (key != null)
            request.Headers.TryAddWithoutValidation(keyName, key);

        var result = await _httpClient.SendAsync(request, cancellationToken);

        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException(await result.Content.ReadAsStringAsync(cancellationToken));
            
        var json = await result.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponse>(json);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest payload, string keyName = "api-key", string key = null,
        CancellationToken cancellationToken = default)
    {
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var stringContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = stringContent;
        
        if (key != null)    
            request.Headers.TryAddWithoutValidation(keyName, key);
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        
        if (!response.IsSuccessStatusCode)  
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(cancellationToken));
        
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResponse>(json);
    }
}
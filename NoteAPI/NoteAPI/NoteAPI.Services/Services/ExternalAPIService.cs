using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NoteAPI.API.Common.Settings;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Models.OllamaModels;

namespace NoteAPI.Services.Services;


public class ExternalApiService : IExternalApiService
{
    private HttpClient _httpClient;
    private readonly HttpClient _aiClient;
    private readonly IHttpClientFactory _httpClientFactory;


    public ExternalApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        var config = configuration.GetSection("ExternalServices").Get<ExternalServices>();
        _httpClientFactory = httpClientFactory;
        _aiClient = _httpClientFactory.CreateClient(config.OllamaModel.Name);
    }

    public void SetClient(HttpClient _httpClient)
    {
        this._httpClient = _httpClient;
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


    public async Task<OllamaFullResponse> TalkWithAI(OllamaRequest request, CancellationToken cancellationToken = default)
    {
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var fullText = new StringBuilder();
        OllamaResponse lastResponse = null;

        try
        {
            var response = await _aiClient.PostAsync("api/generate", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var respObj = JsonConvert.DeserializeObject<OllamaResponse>(line);
                        if (respObj != null)
                        {
                            if (!string.IsNullOrEmpty(respObj.Response))
                                fullText.Append(respObj.Response);

                            if (respObj.Done)
                                lastResponse = respObj;
                        }
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Request cancelled by user. Partial response will be returned.");
            return new OllamaFullResponse
            {
                Model = lastResponse?.Model ?? request.Model,
                CreatedAt = lastResponse?.CreatedAt ?? DateTime.UtcNow,
                FullText = fullText.ToString(),
                DoneReason = "cancelled",
                Context = lastResponse?.Context,
                TotalDuration = lastResponse?.TotalDuration ?? 0,
                LoadDuration = lastResponse?.LoadDuration ?? 0,
                PromptEvalCount = lastResponse?.PromptEvalCount ?? 0,
                PromptEvalDuration = lastResponse?.PromptEvalDuration ?? 0,
                EvalCount = lastResponse?.EvalCount ?? 0,
                EvalDuration = lastResponse?.EvalDuration ?? 0
            };
        }

        return new OllamaFullResponse
        {
            Model = lastResponse?.Model,
            CreatedAt = lastResponse?.CreatedAt ?? DateTime.UtcNow,
            FullText = fullText.ToString(),
            DoneReason = lastResponse?.DoneReason,
            Context = lastResponse?.Context,
            TotalDuration = lastResponse?.TotalDuration ?? 0,
            LoadDuration = lastResponse?.LoadDuration ?? 0,
            PromptEvalCount = lastResponse?.PromptEvalCount ?? 0,
            PromptEvalDuration = lastResponse?.PromptEvalDuration ?? 0,
            EvalCount = lastResponse?.EvalCount ?? 0,
            EvalDuration = lastResponse?.EvalDuration ?? 0
        };
    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NoteAPI.API.Common.Settings;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Models.OllamaModels;

namespace NoteAPI.Services.Services
{
    public class OllamaChatBotService : IOllamaChatBotService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient? _aiClient;

        public OllamaChatBotService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            var config = configuration.GetSection("ExternalServices").Get<ExternalServices>();
            _httpClientFactory = httpClientFactory;
            _aiClient = httpClientFactory.CreateClient(config.OllamaModel.Name);
        }

        public async Task<OllamaFullResponse> TalkWithAIAsync(OllamaRequest request, CancellationToken cancellationToken = default)
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
}

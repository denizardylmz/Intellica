using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoteAPI.Services.Models.OllamaModels;

namespace NoteAPI.Services.Contracts
{
    public interface IOllamaChatBotService
    {
        Task<OllamaFullResponse> TalkWithAIAsync(OllamaRequest request, CancellationToken cancellationToken = default);
    }
}

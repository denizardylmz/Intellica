using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoteAPI.Services.Models.Binance;

namespace NoteAPI.Services.Contracts
{
    public interface ITelegramMessageService
    {
        Task SendBotMessageAsync(string message, CancellationToken cancellationToken);
        Task SendProfitMessageAsync(string symbol, DateTime startFrom, decimal FIFOValue);
        Task SendAnalysisMessageAsync(string symbol, AnalysisResultDto data);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;
using NoteAPI.Services.Core;
using NoteAPI.Services.Models.Binance;
using NoteAPI.Services.Models.OllamaModels;

namespace NoteAPI.Services.Contracts
{
    public interface IBinanceService
    {
        Task<BinancePrice> GetCurrentPriceAsync(string symbol);
        Task<decimal?> GetRsiAsync(string symbol, KlineInterval interval = KlineInterval.OneMinute, int limit = 30);
        Task<(List<decimal?>, List<decimal?>)> GetEMAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes);
        Task<MacdResult> GetMACDAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes);
        Task<List<decimal?>> GetStochasticRsiAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes);
        Task<BollingerResult> GetBollingerAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes);
        Task<AnalysisResultDto> GetAnalysisAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes);
        Task<OllamaFullResponse> AnalyzeMarketWithAIAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes);
        Task FetchKlineDataToDBAsync();
    }
}

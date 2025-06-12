using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;

namespace NoteAPI.Services.Contracts
{
    public interface IBinanceService
    {
        Task<BinancePrice> GetCurrentPriceAsync(string symbol);
        Task<decimal?> GetRsiAsync(string symbol, KlineInterval interval = KlineInterval.OneMinute, int limit = 30);
        Task FetchKlineDataToDBAsync();
    }
}

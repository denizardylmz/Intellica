using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Services.Core
{
    static class StockMathCore
    {
        static public decimal? CalculateRSI(List<decimal> closes, int day)
        {
            if (closes == null || closes.Count < day + 1)
                return null;

            decimal gainSum = 0;
            decimal lossSum = 0;

            for (int i = 1; i < closes.Count; i++)
            {
                var diff = closes[i] - closes[i - 1];
                if (diff > 0)
                    gainSum += diff;
                else
                    lossSum += Math.Abs(diff);
            }

            var avgGain = gainSum / 14;
            var avgLoss = lossSum / 14;

            if (avgLoss == 0)
                return 100; // Hiç düşüş olmadıysa RSI 100

            var rs = avgGain / avgLoss;
            var rsi = 100 - (100 / (1 + rs));
            return Math.Round(rsi, 2);
        }
    }
}

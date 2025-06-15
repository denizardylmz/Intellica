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
            if (closes == null || closes.Count < day)
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

            var avgGain = gainSum / day;
            var avgLoss = lossSum / day;

            if (avgLoss == 0)
                return avgGain == 0 ? 50 : 100;

            var rs = avgGain / avgLoss;
            var rsi = 100 - (100 / (1 + rs));
            return Math.Round(rsi, 2);
        }


        public static List<decimal?> CalculateEMA(List<decimal> closes, int period)
        {
            var emaList = new List<decimal?>();

            if (closes == null || closes.Count < period)
                return Enumerable.Repeat<decimal?>(null, closes.Count).ToList(); // null'larla doldur ki indeks uyumu bozulmasın

            decimal multiplier = 2m / (period + 1);
            decimal? prevEma = null;

            for (int i = 0; i < closes.Count; i++)
            {
                if (i + 1 < period)
                {
                    emaList.Add(null); // Henüz yeterli veri yok
                }
                else if (i + 1 == period)
                {
                    var sma = closes.Take(period).Average(); // İlk EMA = SMA
                    emaList.Add(sma);
                    prevEma = sma;
                }
                else
                {
                    var ema = ((closes[i] - prevEma.Value) * multiplier) + prevEma.Value;
                    emaList.Add(Math.Round(ema, 8)); // Dilersen 2 yerine 8 basamak
                    prevEma = ema;
                }
            }

            return emaList;
        }

        public static (List<decimal?> EMA50, List<decimal?> EMA100) CalculateEMA50and100(List<decimal> closes)
        {
            var ema50 = CalculateEMA(closes, 50);
            var ema100 = CalculateEMA(closes, 100);
            return (ema50, ema100);
        }

        public static MacdResult CalculateMACD(List<decimal> closes, int fastPeriod = 12, int slowPeriod = 26, int signalPeriod = 9)
        {
            var fastEma = CalculateEMA(closes, fastPeriod);
            var slowEma = CalculateEMA(closes, slowPeriod);

            var macdLine = new List<decimal?>();

            for (int i = 0; i < closes.Count; i++)
            {
                if (fastEma[i].HasValue && slowEma[i].HasValue)
                    macdLine.Add(Math.Round(fastEma[i].Value - slowEma[i].Value, 8));
                else
                    macdLine.Add(null);
            }

            var signalLine = CalculateEMA(macdLine.Where(x => x.HasValue).Select(x => x.Value).ToList(), signalPeriod);

            // signalLine eksik başlar, eşitle
            var paddedSignal = Enumerable.Repeat<decimal?>(null, macdLine.Count - signalLine.Count).Concat(signalLine).ToList();

            var histogram = new List<decimal?>();
            for (int i = 0; i < macdLine.Count; i++)
            {
                if (macdLine[i].HasValue && paddedSignal[i].HasValue)
                    histogram.Add(Math.Round(macdLine[i].Value - paddedSignal[i].Value, 8));
                else
                    histogram.Add(null);
            }

            return new MacdResult
            {
                MacdLine = macdLine,
                SignalLine = paddedSignal,
                Histogram = histogram
            };
        }


        public static BollingerResult CalculateBollingerBands(List<decimal> closes, int period = 20, decimal multiplier = 2)
        {
            var upper = new List<decimal?>();
            var lower = new List<decimal?>();
            var middle = new List<decimal?>();

            for (int i = 0; i < closes.Count; i++)
            {
                if (i + 1 < period)
                {
                    upper.Add(null);
                    lower.Add(null);
                    middle.Add(null);
                    continue;
                }

                var window = closes.Skip(i + 1 - period).Take(period).ToList();
                var sma = window.Average();
                var stdDev = (decimal)Math.Sqrt((double)window.Average(x => (x - sma) * (x - sma)));

                middle.Add(Math.Round(sma, 8));
                upper.Add(Math.Round(sma + multiplier * stdDev, 8));
                lower.Add(Math.Round(sma - multiplier * stdDev, 8));
            }

            return new BollingerResult
            {
                UpperBand = upper,
                LowerBand = lower,
                MiddleBand = middle
            };
        }

        public static List<decimal?> CalculateStochasticRsi(List<decimal> closes, int period = 14)
        {
            if (closes == null || closes.Count < period * 3)
                return closes.Select(x => (decimal?)null).ToList();

            var rsiValues = closes.Select((_, i) =>
                i >= period
                ? CalculateRSI(closes.Skip(i + 1 - period).Take(period).ToList(), period)
                : (decimal?)null
            ).ToList();

            var stochRsi = new List<decimal?>();
            for (int i = 0; i < rsiValues.Count; i++)
            {
                if (i < period || !rsiValues[i].HasValue)
                {
                    stochRsi.Add(null);
                    continue;
                }

                var window = rsiValues.Skip(i + 1 - period).Take(period).Where(x => x.HasValue).Select(x => x.Value).ToList();

                if (!window.Any())
                {
                    stochRsi.Add(null);
                    continue;
                }

                var min = window.Min();
                var max = window.Max();
                var current = rsiValues[i]!.Value;

                if (max - min == 0)
                    stochRsi.Add(0);
                else
                    stochRsi.Add(Math.Round((current - min) / (max - min) * 100, 2));
            }

            return stochRsi;
        }





    }

    public class BollingerResult
    {
        public List<decimal?> UpperBand { get; set; }
        public List<decimal?> LowerBand { get; set; }
        public List<decimal?> MiddleBand { get; set; }
    }
    public class MacdResult
    {
        public List<decimal?> MacdLine { get; set; }
        public List<decimal?> SignalLine { get; set; }
        public List<decimal?> Histogram { get; set; }
    }


}

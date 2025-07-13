using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Objects.Models.Spot;

namespace NoteAPI.Services.Core
{
    public class TradeAnalyzer
    {
        private Queue<BuyLot> _fifo = new();

        public decimal Analyze(List<BinanceTrade> trades)
        {
            var results = new List<decimal>();

            foreach (var trade in trades.OrderBy(t => t.Timestamp))
            {
                if (trade.IsBuyer)
                {
                    // BUY işlemi
                    _fifo.Enqueue(new BuyLot
                    {
                        RemainingAmount = trade.Quantity,
                        Price = trade.Price,
                        BuyDate = trade.Timestamp
                    });
                }
                else
                {
                    // SELL işlemi
                    decimal remainingToSell = trade.Quantity;
                    decimal totalProfit = 0;

                    while (remainingToSell > 0 && _fifo.Any())
                    {
                        var buy = _fifo.Peek();
                        var sellQty = Math.Min(buy.RemainingAmount, remainingToSell);
                        var profit = (trade.Price - buy.Price) * sellQty;

                        totalProfit += profit;

                        buy.RemainingAmount -= sellQty;
                        remainingToSell -= sellQty;

                        if (buy.RemainingAmount == 0)
                            _fifo.Dequeue();
                    }

                    // Komisyon varsa ve USDT ile ödenmişse düş
                    if (trade.FeeAsset == "USDT")
                        totalProfit -= trade.Fee;

                    results.Add(totalProfit);
                }
            }

            return results.Sum();
        }
    }


    public class Trade
    {
        public DateTime Date { get; set; }
        public string Coin { get; set; }
        public string Type { get; set; } 
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string FeeCoin { get; set; }
    }


    public class BuyLot
    {
        public decimal RemainingAmount { get; set; }
        public decimal Price { get; set; }
        public DateTime BuyDate { get; set; }
    }

}

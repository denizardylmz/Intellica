using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Models.Binance;
using Telegram.Bot;

namespace NoteAPI.Services.Services
{
    public class TelegramMessageService : ITelegramMessageService
    {
        public ITelegramBotClient BotClient { get; set; }
        private readonly string _chatId;
        private readonly string _BotChatContent;
        private readonly IServiceScopeFactory _scopeFactory;
        public TelegramMessageService(ITelegramBotClient bot, IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            BotClient = bot;
            _chatId = config["Telegram:ChatId"];
            _BotChatContent = config["Telegram:MessageTemplate"];
            _scopeFactory = scopeFactory;
        }

        public async Task SendAnalysisMessageAsync(string symbol, AnalysisResultDto data)
        {
            var message = _BotChatContent.Replace("{{symbol}}", symbol)
                                        .Replace("{{interval}}", KlineInterval.FifteenMinutes.ToString())
                                        .Replace("{{rsi}}", data.Rsi?.ToString("0.##") ?? "—")
                                        .Replace("{{ema50}}", data.Ema50?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{ema100}}", data.Ema100?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{macdLine}}", data.MacdLine?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{signalLine}}", data.SignalLine?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{histogram}}", data.Histogram?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{upperBand}}", data.UpperBand?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{middleBand}}", data.MiddleBand?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{lowerBand}}", data.LowerBand?.LastOrDefault()?.ToString("0.##") ?? "—")
                                        .Replace("{{stochasticRsi}}", string.Join(", ", data.StochasticRsi?.TakeLast(3).Select(x => x?.ToString("0.##")) ?? new[] { "—" }));

            await BotClient.SendMessage(chatId: _chatId, text: message);
        }

        public async Task SendProfitMessageAsync(string symbol, DateTime startFrom, decimal FIFOValue)
        {
            var message =   $"📈 *Realized PnL Report*\n\n" +
                            $"🔹 *Coin:* `{symbol}`\n" +
                            $"📅 *Start Date:* `{startFrom:dd/MM/yyyy}`\n" +
                            $"💰 *Net Profit:* `{FIFOValue:N2}` USDT";

            await BotClient.SendMessage(
                chatId: _chatId,
                text: message,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                cancellationToken: default
            );
        }

        public Task SendBotMessageAsync(string message, CancellationToken cancellationToken) => BotClient.SendMessage(chatId: _chatId, text: message, cancellationToken: cancellationToken);
    }
}

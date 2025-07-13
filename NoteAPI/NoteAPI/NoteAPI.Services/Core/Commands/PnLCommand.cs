using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoteAPI.Services.Contracts;
using Telegram.Bot;

namespace NoteAPI.Services.Core.Commands
{
    [TelegramCommand("pnl")]
    internal class PnLCommand : ICommandHandler
    {
        public string Command { get; set; } = "pnl";

        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public PnLCommand(
            ITelegramBotClient telegramBotClient,
            IServiceScopeFactory scopeFactory
            )
        {
            _botClient = telegramBotClient;
            _scopeFactory = scopeFactory;
        }


        public async Task HandleAsync(List<string> parameters, long chatId, CancellationToken cancellationToken)
        {
            if (parameters.Count < 2 || parameters.Contains("help", StringComparer.OrdinalIgnoreCase))
            {
                await Help(chatId, cancellationToken);
                return;
            }

            var coin = parameters[0];
            if (string.IsNullOrWhiteSpace(coin))
            {
                await _botClient.SendMessage(chatId, "Lütfen geçerli bir coin girin. Örn: /total PEPEUSDT 14", cancellationToken: cancellationToken);
                return;
            }

            if (!double.TryParse(parameters[1], out double offsetDays))
            {
                await _botClient.SendMessage(chatId, "Lütfen geçerli bir gün sayısı girin. Örn: /total PEPEUSDT 14", cancellationToken: cancellationToken);
                return;
            }

            var startDate = DateTime.Now.Date.AddDays(-offsetDays);

            using var scope = _scopeFactory.CreateScope();
            try
            {
                var binanceService = scope.ServiceProvider.GetRequiredService<IBinanceService>();
                var profit = await binanceService.GetTotalRealizedPnL(symbol: coin.ToUpperInvariant(), startDate: startDate, sendChatbotMessage: true);

#if DEBUG
                await _botClient.SendMessage(chatId: chatId, text: $"[DEBUG] Coin: {coin}\nOffset Gün: {offsetDays}\nBaşlangıç: {startDate:d} \n\n Profit : {profit.ToString("N2")}", cancellationToken: cancellationToken);
#endif
            }
            catch (Exception ex)
            {
                await _botClient.SendMessage(chatId: chatId, text: "❌ Hata oluştu, lütfen tekrar deneyin.", cancellationToken: cancellationToken);
#if DEBUG
                await _botClient.SendMessage(chatId: chatId, text: $"[DEBUG] {ex.Message}", cancellationToken: cancellationToken);
#endif
            }
        }

        public Task Help(long chatId, CancellationToken cancellationToken)
        {
            var message =
                "📊 *TotalPnL Komutu Yardımı*\n\n" +
                "Bu komut, belirttiğiniz coinde son X gün içindeki *gerçekleşmiş* (realized) kar/zararınızı getirir.\n\n" +
                "🛠️ *Kullanım:* \n" +
                "`/total <COIN> <GÜN>`\n\n" +
                "📌 *Parametreler:*\n" +
                "• `<COIN>`: Coin sembolü (örn: `PEPEUSDT`)\n" +
                "• `<GÜN>`: Kaç gün geriye gidileceği (örn: `14`)\n\n" +
                "🧪 *Örnek:* \n" +
                "`/total PEPEUSDT 14` → PEPEUSDT çiftinde son 14 günün realized PnL bilgisi döner.";

            return _botClient.SendMessage(
                chatId: chatId,
                text: message,
                cancellationToken: cancellationToken,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            );
        }
    }
}

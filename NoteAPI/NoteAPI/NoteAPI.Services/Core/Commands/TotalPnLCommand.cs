using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoteAPI.Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NoteAPI.Services.Core.Commands
{
    [TelegramCommand(commandName: "TotalPnL")]
    public class TotalPnLCommand : ICommandHandler
    {
        public string Command { get; set; } = "TotalPnL";

        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public TotalPnLCommand(
            ITelegramBotClient telegramBotClient,
            IServiceScopeFactory scopeFactory
            )
        {
            _botClient = telegramBotClient;
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(List<string> parameters, long chatId, CancellationToken cancellationToken)
        {
            // Yardım istiyorsa veya parametre eksikse yardım göster
            if (parameters.Count == 0 || parameters.Contains("help", StringComparer.OrdinalIgnoreCase))
            {
                await Help(chatId, cancellationToken);
                return;
            }

            // 1. parametre = kaç gün önce
            if (!double.TryParse(parameters[0], out double offsetDays))
            {
                await _botClient.SendMessage(chatId, "⚠️ Geçerli bir gün sayısı girilmedi. Örn: `/total 14`", cancellationToken: cancellationToken);
                return;
            }

            var startDate = DateTime.Now.Date.AddDays(-offsetDays);

            using var scope = _scopeFactory.CreateScope();
            try
            {
                var binanceService = scope.ServiceProvider.GetRequiredService<IBinanceService>();
                var profit = await binanceService.GetPnLAllSpots(startDate, sendChatbotMessage: true);

#if DEBUG
                var debugText = $"[DEBUG] Params: {string.Join(", ", parameters)}\nStart Date: {startDate:dd.MM.yyyy}";
                await _botClient.SendMessage(chatId: chatId, text: debugText, cancellationToken: cancellationToken);
#endif
            }
            catch (Exception ex)
            {
                await _botClient.SendMessage(chatId: chatId, text: "❌ Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.", cancellationToken: cancellationToken);
#if DEBUG
                await _botClient.SendMessage(chatId: chatId, text: $"[EXCEPTION] {ex.Message}", cancellationToken: cancellationToken);
#endif
            }
        }


        public Task Help(long chatId, CancellationToken cancellationToken)
        {
            var message =
                "📊 *TotalPnL Komutu Yardımı*\n\n" +
                "Bu komut, belirttiğiniz coin için son X gün içerisindeki *gerçekleşmiş kar/zararınızı (realized PnL)* hesaplar.\n\n" +
                "🛠️ *Kullanım:* \n" +
                "`/total <COIN> <GÜN>`\n\n" +
                "📌 *Parametreler:*\n" +
                "• `<COIN>` – Coin/işlem çifti (örneğin: `BTCUSDT`, `PEPEUSDT`)\n" +
                "• `<GÜN>` – Kaç gün geriye dönük hesaplama yapılacağı (örneğin: `14`)\n\n" +
                "🧪 *Örnek:*\n" +
                "`/total PEPEUSDT 14`\n" +
                "Son 14 günde PEPEUSDT işlem çiftinde oluşan gerçekleşmiş PnL bilgisini döner.";

            return _botClient.SendMessage(
                chatId: chatId,
                text: message,
                cancellationToken: cancellationToken,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            );
        }

    }
}

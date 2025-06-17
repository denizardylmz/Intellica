using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using NoteAPI.Services.Contracts;
using Telegram.Bot;

namespace NoteAPI.Services.Services
{
    public class TelegramMessageService : ITelegramMessageService
    {
        public ITelegramBotClient BotClient { get; set; }
        private readonly string _chatId;

        public TelegramMessageService(ITelegramBotClient bot, IConfiguration config)
        {
            BotClient = bot;
            _chatId = config["Telegram:ChatId"];
        }

        public async Task SendMessageAsync(string message)
        {
            await BotClient.SendMessage(chatId: _chatId, text: message);
        }
    }
}

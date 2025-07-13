using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using NoteAPI.API.Common.Settings;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NoteAPI.Services.BackgroundServices
{
    public class TelegramBotBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ITelegramBotClient _botClient { get; set; }
        private readonly string _chatId;
        private readonly TelegramSettings settings;
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        private CancellationToken _cancelToken;


        public TelegramBotBackgroundService(
            ITelegramBotClient botClient,
            IConfiguration config,
            IServiceScopeFactory scopeFactory,
            IEnumerable<ICommandHandler> commandHandlers
            )
        {
            _botClient = botClient;
            _chatId = config["Telegram:ChatId"];
            _scopeFactory = scopeFactory;
            _commandHandlers = commandHandlers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.RecieveMessages(stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public Task RecieveMessages(CancellationToken cancellationToken)
        {
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandlePollingErrorAsync,
                cancellationToken: cancellationToken
            );

            return Task.CompletedTask;
        }
        public Task SendBotMessageAsync(string message, CancellationToken cancellationToken) => _botClient.SendMessage(chatId: _chatId, text: message, cancellationToken: cancellationToken);
        private Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message)
                return Task.CompletedTask;

            var messageText = update.Message.Text;
            var chatId = update.Message.Chat.Id;

            var parsed = CommandParser.Parse(messageText);
            if (parsed == null)
            {
                return Task.CompletedTask;
            }

            // Parser komut bulamazsa, Bot Eko verir.
            if (string.IsNullOrWhiteSpace(parsed.Command))
            {
                _= this.SendBotMessageAsync("Ekko :  " + parsed.Content, cancellationToken: _cancelToken);
                return Task.CompletedTask;
            }

            //Finds related function for user's command.
            var handler = _commandHandlers.FirstOrDefault(h => h.Command.Equals(parsed.Command, StringComparison.OrdinalIgnoreCase));


            if (handler != null)
            {
                _= handler.HandleAsync(parsed.Parameters, chatId, cancellationToken);
            }
            else
            {
                var commandList = _commandHandlers.Select(x => x.Command);

                var strActiveComments =  string.Join(", " , commandList) ;

                _ = this.SendBotMessageAsync("Bilinmeyen komut! " + parsed.Command + "\n Active Command List: " + strActiveComments, cancellationToken: _cancelToken);
            }

            return Task.CompletedTask;
        }
        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Polling hatası: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}

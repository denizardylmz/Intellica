using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Services.Core
{
    public static class CommandParser
    {
        public static CommandMessage Parse(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText))
                return null;

            var parts = messageText.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return null;

            var command = parts[0].TrimStart('/');
            var parameters = parts.Skip(1).ToList();

            return new CommandMessage
            {
                Content = messageText,
                Command = command,
                Parameters = parameters,
                IsHelp = false
            };
        }
    }

    public class CommandMessage
    {
        public string Content { get; set; }
        public string Command { get; set; }
        public List<string> Parameters { get; set; } = new List<string>();
        public bool IsHelp { get; set; }
    }



}

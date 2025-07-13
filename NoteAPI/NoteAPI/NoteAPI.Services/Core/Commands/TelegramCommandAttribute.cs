using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Services.Core.Commands
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TelegramCommandAttribute : Attribute
    {
        public string CommandName { get; }

        public TelegramCommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }

}

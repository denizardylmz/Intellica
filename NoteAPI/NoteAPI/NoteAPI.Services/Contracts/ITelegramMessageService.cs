using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Services.Contracts
{
    public interface ITelegramMessageService
    {
        Task SendMessageAsync(string message);
    }




}

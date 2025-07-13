using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoteAPI.Services.Contracts
{
    public interface ICommandHandler
    {
        string Command { get; set; }
        Task HandleAsync(List<string> parameters, long chatId, CancellationToken cancellationToken);
        Task Help(long chatId, CancellationToken cancellationToken);
    }
}

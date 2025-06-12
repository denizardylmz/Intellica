using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteAPI.Services.Contracts;
using Quartz;

namespace NoteAPI.Services.Jobs
{
    public class KlineFetchJob : IJob
    {
        private readonly IBinanceService _fetcher;

        public KlineFetchJob(IBinanceService fetcher)
        {
            _fetcher = fetcher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _fetcher.FetchKlineDataToDBAsync();
        }
    }

}

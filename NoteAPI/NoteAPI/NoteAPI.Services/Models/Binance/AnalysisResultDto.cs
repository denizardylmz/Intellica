using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Services.Models.Binance
{
    public class AnalysisResultDto
    {
        public decimal? Rsi { get; set; }
        public List<decimal?> Ema50 { get; set; }
        public List<decimal?> Ema100 { get; set; }
        public List<decimal?> MacdLine { get; set; }
        public List<decimal?> SignalLine { get; set; }
        public List<decimal?> Histogram { get; set; }
        public List<decimal?> UpperBand { get; set; }
        public List<decimal?> MiddleBand { get; set; }
        public List<decimal?> LowerBand { get; set; }
        public List<decimal?> StochasticRsi { get; set; }
    }




}

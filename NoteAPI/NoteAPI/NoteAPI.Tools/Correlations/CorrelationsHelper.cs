using System;

namespace NoteAPI.Tools.Correlations
{
    public class CorrelationsHelper
    {
        public static string BuildCorrelationId()
        {
            return $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF")}";
        }
    }
}

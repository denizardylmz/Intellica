using System;

namespace NoteAPI.Tools.Logs
{
    public class LogsHelper
    {
        public static string BuildLogPrefix(string className, string methodName)
        {

            if (string.IsNullOrEmpty(className))
                throw new ArgumentNullException(nameof(className));

            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            string result = $"{DateTime.UtcNow.ToString("yyyy/MM/dd-HH:mm:ss")}::{className}::{methodName}";

            return result;
        }

        public static string BuildCorrelatedLogPrefix(string correlationId, string className, string methodName)
        {
            return BuildCorrelatedLogPrefix(correlationId, className, methodName, string.Empty);
        }

        public static string BuildCorrelatedLogPrefix(string correlationId, string className, string methodName, string payLoad)
        {
            if (string.IsNullOrEmpty(correlationId))
                throw new ArgumentNullException(nameof(correlationId));

            if (string.IsNullOrEmpty(className))
                throw new ArgumentNullException(nameof(className));

            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            string result = $"{DateTime.UtcNow.ToString("yyyy/MM/dd-HH:mm:ss")}::{correlationId}::{className}::{methodName}";

            if (!string.IsNullOrEmpty(payLoad))
                result = string.Concat(result, "::", payLoad);

            return result;
        }
    }
}

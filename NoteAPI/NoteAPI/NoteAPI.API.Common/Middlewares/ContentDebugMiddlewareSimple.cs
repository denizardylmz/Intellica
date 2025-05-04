using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NoteAPI.API.Common.Middlewares
{
    public class ContentDebugMiddlewareSimple
    {
        private readonly RequestDelegate _next;
        const string API_KEY_HEADER_NAME = "x-api-key";
        const string USER_ID_HEADER_NAME = "x-user-id";
        const string PROJECT_ID_HEADER_NAME = "x-project-id";
        const string CORRELATION_ID_HEADER_NAME = "x-correlation-id";

        private readonly ILogger<ContentDebugMiddlewareSimple> _logger;

        public ContentDebugMiddlewareSimple(
            RequestDelegate next,
            ILogger<ContentDebugMiddlewareSimple> logger)
        {
            _next = next;
            _logger = logger;
        }

        //https://alexbierhaus.medium.com/api-request-and-response-logging-middleware-using-net-5-c-a0af639920da
        public async Task Invoke(HttpContext context)
        {
            string request = string.Empty;

            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                request = await reader.ReadToEndAsync();

            _logger.LogDebug($"ContentDebugMiddleware::Invoke::{Environment.NewLine}HEADERS{Environment.NewLine}{context.Request.Headers}{Environment.NewLine}CONTENT{Environment.NewLine}{request}");
            
            await _next(context);            
        }        
    }
}

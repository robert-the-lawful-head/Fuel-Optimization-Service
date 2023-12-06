using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FBOLinx.Web.Middleware.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggingService _LoggingService;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggingService loggingService)
        {
            _next = next;
            _LoggingService = loggingService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex, _LoggingService).ConfigureAwait(false);
            }
        }
        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception, ILoggingService logger)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;

            logger.LogError(exception.Message,exception.ToString(), LogLevel.Error, LogColorCode.Red);
            logger.LogError(exception.Message, exception.StackTrace.ToString(), LogLevel.Error, LogColorCode.Red);
            if (exception.InnerException != null)
                logger.LogError(exception.Message,exception.InnerException.StackTrace.ToString(), LogLevel.Error, LogColorCode.Red);

            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}

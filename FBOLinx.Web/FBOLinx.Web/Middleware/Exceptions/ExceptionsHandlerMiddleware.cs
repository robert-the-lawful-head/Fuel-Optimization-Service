using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FBOLinx.Web.Middleware.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ExceptionHandlerMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider.GetRequiredService<ILoggingService>();

                    await HandleExceptionMessageAsync(context, ex, scopedServices).ConfigureAwait(false);
                }

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

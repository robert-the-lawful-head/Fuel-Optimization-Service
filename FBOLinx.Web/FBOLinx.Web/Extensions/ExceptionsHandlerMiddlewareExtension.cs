using FBOLinx.Web.Middleware.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace FBOLinx.Web.Extensions
{
    public static class ExceptionsHandlerMiddlewareExtension
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}

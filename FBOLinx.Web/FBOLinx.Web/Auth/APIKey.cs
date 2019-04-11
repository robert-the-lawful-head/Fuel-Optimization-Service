using System.Threading.Tasks;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FBOLinx.Web.Auth
{
    public class APIKeyAttribute : TypeFilterAttribute
    {
        public APIKeyAttribute() : base(typeof(APIKeyActionFilter)) { }
    }

    public class APIKeyActionFilter : IAsyncActionFilter
    {
        const string API_KEY = "x-api-key";
        private readonly UserService _userService;

        public APIKeyActionFilter(UserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;
            if (headers.ContainsKey(API_KEY))
            {
                //bool isAuthorized = await _userService.CheckAPIKey(headers[API_KEY]);
                bool isAuthorized = false;
                if (!isAuthorized)
                    context.Result = new UnauthorizedResult();
                else
                    await next();
            }
            else
                context.Result = new UnauthorizedResult();
        }
    }
}
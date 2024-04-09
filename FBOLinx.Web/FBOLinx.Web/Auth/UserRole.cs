using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FBOLinx.Web.Auth
{
    public class UserRoleAttribute : TypeFilterAttribute
    {
        public UserRoleAttribute(params UserRoles[] roles) : base(typeof(UserRoleActionFilter))
        {
            Arguments = new object[] {roles};
        }
    }

    public class UserRoleActionFilter : IAsyncActionFilter
    {
        private readonly UserRoles[] _Roles;

        public UserRoleActionFilter(UserRoles[] roles)
        {
            _Roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentRole = context.HttpContext.User.Claims.FirstOrDefault((c => c.Type == ClaimTypes.Role));
            if (currentRole == null || !_Roles.Contains((UserRoles) System.Convert.ToInt16(currentRole.Value)))
                context.Result = new UnauthorizedResult();
            else
                await next();
        }
    }
}

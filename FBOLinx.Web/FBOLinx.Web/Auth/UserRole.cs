using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FBOLinx.Web.Auth
{
    public class UserRoleAttribute : TypeFilterAttribute
    {
        public UserRoleAttribute(params Models.User.UserRoles[] roles) : base(typeof(UserRoleActionFilter))
        {
            Arguments = new object[] {roles};
        }
    }

    public class UserRoleActionFilter : IAsyncActionFilter
    {
        private readonly Models.User.UserRoles[] _Roles;

        public UserRoleActionFilter(Models.User.UserRoles[] roles)
        {
            _Roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentRole = context.HttpContext.User.Claims.FirstOrDefault((c => c.Type == ClaimTypes.Role));
            if (currentRole == null || !_Roles.Contains((User.UserRoles) System.Convert.ToInt16(currentRole.Value)))
                context.Result = new UnauthorizedResult();
            else
                await next();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute(string claimType, List<string> supportedClaimValues) : base(typeof(ClaimRequirementFilter))
    {
        Arguments = supportedClaimValues.Select(value => new Claim(claimType, value)).ToArray();
    }
}

public class ClaimRequirementFilter : IAuthorizationFilter
{
    readonly Claim _claim;

    public ClaimRequirementFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
        if (!hasClaim)
        {
            context.Result = new ForbidResult();
        }
    }
}
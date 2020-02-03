using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FBOLinx.Web.Auth
{
    public class APIKeyActionFilter : IAsyncActionFilter
    {
        private readonly List<IntegrationPartners.IntegrationPartnerTypes> _PartnerTypes;
        private IAPIKeyManager _APIKeyManager;

        public APIKeyActionFilter(IntegrationPartners.IntegrationPartnerTypes[] partnerTypes, IAPIKeyManager apiKeyManager)
        {
            _APIKeyManager = apiKeyManager;
            if (partnerTypes != null)
                _PartnerTypes = new List<IntegrationPartners.IntegrationPartnerTypes>(partnerTypes);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var record = await _APIKeyManager.GetIntegrationPartner();
            if (record == null)
                context.Result = new UnauthorizedResult();
            else
            {
                if (_PartnerTypes != null && _PartnerTypes.Count > 0 && !_PartnerTypes.Contains(record.PartnerType))
                {
                    context.Result = new ForbidResult();
                }
                else
                    await next();
            }
        }
    }
}
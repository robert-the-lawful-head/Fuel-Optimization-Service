using FBOLinx.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.Web.Auth
{
    public class APIKeyAttribute : TypeFilterAttribute
    {
        public APIKeyAttribute(params IntegrationPartnerTypes[] partnerTypes) : base(
            typeof(APIKeyActionFilter))
        {
            Arguments = new object[] { partnerTypes };
        }
    }
}
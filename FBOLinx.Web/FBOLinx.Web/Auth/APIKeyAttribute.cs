using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.Web.Auth
{
    public class APIKeyAttribute : TypeFilterAttribute
    {
        public APIKeyAttribute(params IntegrationPartners.IntegrationPartnerTypes[] partnerTypes) : base(
            typeof(APIKeyActionFilter))
        {
            Arguments = new object[] { partnerTypes };
        }
    }
}
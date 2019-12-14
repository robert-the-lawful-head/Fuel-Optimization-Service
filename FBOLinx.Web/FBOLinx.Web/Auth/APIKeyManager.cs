using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Auth
{
    public class APIKeyManager : IAPIKeyManager
    {
        private const string _APIHeaderKey = "x-api-key";
        private IHttpContextAccessor _HttpContextAccessor;
        private FboLinxContext _fboLinxContext;

        public APIKeyManager(IHttpContextAccessor httpContextAccessor,
            FboLinxContext fboLinxContext)
        {
            _fboLinxContext = fboLinxContext;
            _HttpContextAccessor = httpContextAccessor;
        }

        public async Task<IntegrationPartners> GetIntegrationPartner()
        {
            var record = await GetIntegrationPartnerFromKey();
            return record;
        }

        #region Private Methods
        private async Task<IntegrationPartners> GetIntegrationPartnerFromKey()
        {
            string key = GetAPIKeyFromHeader();
            if (string.IsNullOrEmpty(key))
                return null;
            var record = await _fboLinxContext.IntegrationPartners.Where(x => x.Apikey.ToString() == key).FirstOrDefaultAsync();
            return record;
        }

        private string GetAPIKeyFromHeader()
        {
            try
            {
                var headers = _HttpContextAccessor.HttpContext.Request.Headers;
                if (headers.ContainsKey(_APIHeaderKey))
                {
                    return headers[_APIHeaderKey];
                }
            }
            catch (System.Exception)
            {

            }
            return "";
        }
        #endregion
    }
}

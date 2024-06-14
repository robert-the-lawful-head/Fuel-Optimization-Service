using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet.Api;
using FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet.Client;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.JetNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations.AppPartnerSDKSettings;

namespace FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet
{
    public interface IJetNetService
    {
        Task<JetNetDto> GetJetNetInformation(string tailNumber);
    }

    public class JetNetService : IJetNetService
    {
        private AppPartnerSDKSettings.JetNetAPISettings _jetNetApiSettings;
        private IMemoryCache _MemoryCache;

        public JetNetService(IOptions<AppPartnerSDKSettings> appPartnerSDKSettings, IMemoryCache memoryCache)
        {
            _jetNetApiSettings = appPartnerSDKSettings?.Value?.JetNet;
            _MemoryCache = memoryCache;
        }

        public async Task<JetNetDto> GetJetNetInformation(string tailNumber)
        {
            var client = GetJetNetApiClient();
            var api = new JetNetApi(client);

            //var token = GetAuthorizationTokenFromCache(); // Doesn't look like the token expiration lasts 120 minutes
            //if (token == null || string.IsNullOrEmpty(token.bearerToken))
            //{
                var jetNetUser = new JetNetUser() { emailaddress = _jetNetApiSettings.Username, password = _jetNetApiSettings.Password };
                var token = await api.GetJetNetToken(jetNetUser);
            //var cacheEntryOptions =
            //    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(100));
            //if (token != null && !string.IsNullOrEmpty(token.bearerToken))
            //    _MemoryCache.Set("JetNet", token, cacheEntryOptions);
            //}

            try
            {
                return await api.GetJetNetData(tailNumber, token);
            }
            catch(Exception ex)
            {
                return new JetNetDto();
            }
        }

        private JetNetApiClient GetJetNetApiClient()
        {
            var client =
                new JetNetApiClient();
            return client;
        }

        private JetNetApiToken GetAuthorizationTokenFromCache()
        {
            try
            {
                JetNetApiToken result = null;
                if (_MemoryCache.TryGetValue("JetNet", out result) && result != null)
                    return result;

                return null;
            }
            catch (System.Exception exception)
            {
                return null;
            }
        }

    }
}

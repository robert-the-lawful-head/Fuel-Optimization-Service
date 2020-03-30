using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using IO.Swagger.Model;
using Microsoft.Extensions.Options;

namespace FBOLinx.Web.Services
{
    public class FuelerLinxService
    {
        #region Private Members
        private string _ProductionUsername = "fbolinx";
        private string _ProductionPassword = "HjAQamk^Md!L9V-_";
        private string _APIKey = "";
        private AppPartnerSDKSettings.FuelerlinxSDKSettings _fuelerlinxSdkSettings;
        private IOptions<AppSettings> _appSettings;

        #endregion

        public FuelerLinxService(IOptions<AppSettings> appSettings, IOptions<AppPartnerSDKSettings> appPartnerSDKSettings)
        {
            _appSettings = appSettings;
            _fuelerlinxSdkSettings = appPartnerSDKSettings.Value.FuelerLinx;
            _APIKey = _fuelerlinxSdkSettings.APIKey;
            PrepareAPIClientConfiguration();
        }
        
        #region Public Methods
        public async Task<Models.Responses.FuelerLinxUpliftsByLocationResponseContent> GetOrderCountByLocation(Models.Requests.FuelerLinxUpliftsByLocationRequestContent request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            string upliftsByLocationURL = _appSettings.Value.FuelerLinxUrl + "/integratedservices/vendors/fbolinx.asmx/GetOrderCountByLocation";
            if (string.IsNullOrEmpty(authToken))
                return null;

            request.UserServiceKey = authToken;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("APIKey", _APIKey);
                using (HttpResponseMessage response = await client.PostAsync(upliftsByLocationURL,
                    new Utilities.JsonContent(new FuelerLinxUpliftsByLocationRequest()
                    {
                        request = request
                    })))
                {
                    Task<FuelerLinxUpliftsByLocationResponse> upliftsResult = response.Content.ReadAsAsync<FuelerLinxUpliftsByLocationResponse>();
                    if (upliftsResult == null || upliftsResult.Result == null || upliftsResult.Result.d == null)
                        return new FuelerLinxUpliftsByLocationResponseContent() { ICAO = request.ICAO };
                    upliftsResult.Result.d.ICAO = request.ICAO;
                    return upliftsResult.Result.d;
                }
            }
        }

        public Task<object> GetTransactionsForNearbyAirports(int distanceInMiles, DateTime startDate, DateTime endDate)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            api.GetNearByAirports(new FBOLinxNearByAirportsRequest()
            {
                DistanceMile = distanceInMiles,
                StartDateTime = startDate,
                EndDateTime = endDate
            });
            return null;
        }
        #endregion

        #region Private Methods
        private async Task<string> GetAuthenticationTokenFromService()
        {
            try
            {
                string authURL = _appSettings.Value.FuelerLinxUrl + "/integratedservices/vendors/fbolinx.asmx/GetAuthenticationToken";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("APIKey", _APIKey);
                    using (HttpResponseMessage response = await client.PostAsync(authURL,
                        new FBOLinx.Web.Utilities.JsonContent(new Models.Requests.FuelerLinxAuthenticationRequest()
                        {
                            request = new FuelerLinxAuthenticationRequestContent()
                            {
                                Username = _ProductionUsername,
                                Password = _ProductionPassword
                            }

                        })))
                    {
                        var authenticationResult =
                            response.Content.ReadAsAsync<Models.Responses.FuelerLinxAuthenticationResponse>();
                        if (authenticationResult == null || authenticationResult.Result == null || authenticationResult.Result.d == null)
                            return "";
                        return authenticationResult.Result.d.Token;
                    }
                }
            }
            catch (System.Exception exception)
            {
                return "";
            }
        }

        private void PrepareAPIClientConfiguration()
        {
            if (!IO.Swagger.Client.Configuration.ApiKey.ContainsKey("x-api-key"))
                IO.Swagger.Client.Configuration.ApiKey.Add("x-api-key", _APIKey);
        }
        #endregion
    }
}

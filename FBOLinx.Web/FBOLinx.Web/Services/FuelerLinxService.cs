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
using Microsoft.Extensions.Options;

namespace FBOLinx.Web.Services
{
    public class FuelerLinxService
    {
        #region Private Members
        private string _ProductionUsername = "fbolinx";
        private string _ProductionPassword = "HjAQamk^Md!L9V-_";
        private string _ProductionAPIKey = "F83D59B7-AD0D-4B3C-A67B-934CD6AA6F2B";
        private string _APIKey = "F83D59B7-AD0D-4B3C-A67B-934CD6AA6F2B";
        private IOptions<AppSettings> _appSettings;

        #endregion

        public FuelerLinxService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        #region Public Methods
        public async Task<FuelerLinxUpliftsByLocationResponseContent> GetOrderCountByLocation(FuelerLinxUpliftsByLocationRequestContent request)
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
        public async Task<List<FuelerLinxVolumesNearByAirportResponseContent>> GetCountOfOrderVolumesNearByAirport(FuelerLinxVolumesNearByAirportRequestContent request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            string volumesNearbyURL = _appSettings.Value.FuelerLinxUrl + "/integratedservices/vendors/fbolinx.asmx/GetOrderCountByLocation";
            if (string.IsNullOrEmpty(authToken))
                return null;

            request.UserServiceKey = authToken;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("APIKey", _APIKey);
                using (HttpResponseMessage response = await client.PostAsync(volumesNearbyURL,
                    new Utilities.JsonContent(new FuelerLinxVolumesNearByAirportRequest()
                    {
                        request = request
                    })))
                {
                    Task<FuelerLinxVolumesNearByAirportResponse> upliftsResult = response.Content.ReadAsAsync<FuelerLinxVolumesNearByAirportResponse>();
                    if (upliftsResult == null || upliftsResult.Result == null || upliftsResult.Result.d == null)
                        return new List<FuelerLinxVolumesNearByAirportResponseContent>() {  };
                    return upliftsResult.Result.d;
                }
            }
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
                        new Utilities.JsonContent(new FuelerLinxAuthenticationRequest()
                        {
                            request = new FuelerLinxAuthenticationRequestContent()
                            {
                                Username = _ProductionUsername,
                                Password = _ProductionPassword
                            }

                        })))
                    {
                        var authenticationResult =
                            response.Content.ReadAsAsync<FuelerLinxAuthenticationResponse>();
                        if (authenticationResult == null || authenticationResult.Result == null || authenticationResult.Result.d == null)
                            return "";
                        return authenticationResult.Result.d.Token;
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion
    }
}

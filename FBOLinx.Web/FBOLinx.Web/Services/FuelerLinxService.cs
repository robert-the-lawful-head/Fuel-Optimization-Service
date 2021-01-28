using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using FBOLinx.Core.Utilities.Http;
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
        public async Task<FuelerLinxUpliftsByLocationResponseContent> GetOrderCountByLocation(FuelerLinxUpliftsByLocationRequestContent request)
        {
            try
            {
                var authToken = await GetAuthenticationTokenFromService();
                string upliftsByLocationURL = _appSettings.Value.FuelerLinxUrl +
                                              "/integratedservices/vendors/fbolinx.asmx/GetOrderCountByLocation";
                if (string.IsNullOrEmpty(authToken))
                    return null;

                request.UserServiceKey = authToken;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("APIKey", _APIKey);
                    using (HttpResponseMessage response = await client.PostAsync(upliftsByLocationURL,
                        new FBOLinx.Core.Utilities.Http.JsonContent(new FuelerLinxUpliftsByLocationRequest()
                        {
                            request = request
                        })))
                    {
                        FuelerLinxUpliftsByLocationResponse upliftsResult =
                            Newtonsoft.Json.JsonConvert.DeserializeObject<FuelerLinxUpliftsByLocationResponse>(
                                await response.Content.ReadAsStringAsync());
                        if (upliftsResult == null || upliftsResult.d == null)
                            return new FuelerLinxUpliftsByLocationResponseContent() {ICAO = request.ICAO};
                        upliftsResult.d.ICAO = request.ICAO;
                        return upliftsResult.d;
                    }
                }
            }
            catch (System.Exception)
            {
                return new FuelerLinxUpliftsByLocationResponseContent() {ICAO = request.ICAO};
            }

        }

        public FBOLinxNearbyAirportsResponse GetTransactionsForNearbyAirports(FBOLinxNearbyAirportsRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FBOLinxNearbyAirportsResponse results = api.GetTransactionsCountForNearbyAirports(request);
            return results;
        }

        public FBOLinxOrdersResponse GetTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FBOLinxOrdersResponse results = api.GetTransactionsCount(request);
            return results;
        }

        public FboLinxContractFuelVendorsCountResponse GetContractFuelVendorsTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FboLinxContractFuelVendorsCountResponse results = api.GetContractFuelVendorsTransactionsCount(request);
            return results;
        }

        public FboLinxFbosTransactionsCountResponse GetFBOsTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FboLinxFbosTransactionsCountResponse results = api.GetFBOsTransactionsCount(request);
            return results;
        }

        public FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FboLinxCustomerTransactionsCountAtAirportResponse results = api.GetCustomerTransactionsCount(request);
            return results;
        }

        public FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerFBOTransactionsCount(FBOLinxOrdersRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FboLinxCustomerTransactionsCountAtAirportResponse results = api.GetCustomerFBOTransactionsCount(request);
            return results;
        }

        public FBOLinxOrdersResponse GetTransactionsDirectOrdersCount(FBOLinxOrdersRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FBOLinxOrdersResponse results = api.GetTransactionsDirectOrdersCount(request);
            return results;
        }

        public FBOLinxContractFuelOrdersResponse GetContractFuelRequests(FBOLinxOrdersRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FBOLinxContractFuelOrdersResponse results = api.GetContractFuelOrders(request);
            return results;
        }

        public FboLinxAircraftsResponse GetAircraftsFromFuelerinx()
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FboLinxAircraftsResponse results = api.GetAircraftTailsGroupedByCompany();
            return results;
        }

        public FuelVendorDTO UpdateFuelVendorEmails(FBOLinxFuelVendorUpdateRequest request)
        {
            var api = new IO.Swagger.Api.FBOLinxApi(_fuelerlinxSdkSettings.APIEndpoint);
            FBOLinxFuelVendorUpdateResponse result = api.UpdateFuelVendor(request);
            return result.Result;
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
                    using (HttpResponseMessage response = await client.PostAsync(authURL, new JsonContent(new FuelerLinxAuthenticationRequest()
                        {
                            request = new FuelerLinxAuthenticationRequestContent()
                            {
                                Username = _ProductionUsername,
                                Password = _ProductionPassword
                            }

                        })))
                    {
                        var authenticationResult =
                            Newtonsoft.Json.JsonConvert.DeserializeObject<FuelerLinxAuthenticationResponse>(
                                await response.Content.ReadAsStringAsync());
                        if (authenticationResult == null || authenticationResult.d == null)
                            return "";
                        return authenticationResult.d.Token;
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void PrepareAPIClientConfiguration()
        {
            if (!IO.Swagger.Client.Configuration.ApiKey.ContainsKey("x-api-key"))
                IO.Swagger.Client.Configuration.ApiKey.Add("x-api-key", _APIKey);

            if (!IO.Swagger.Client.Configuration.ApiKey.ContainsKey("Authorization"))
                IO.Swagger.Client.Configuration.ApiKey.Add("Authorization", "");
        }
        #endregion
    }
}

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
using Microsoft.Extensions.Options;
using Fuelerlinx.SDK;

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
        private HttpClient _httpClient;

        #endregion

        public FuelerLinxService(IOptions<AppSettings> appSettings, IOptions<AppPartnerSDKSettings> appPartnerSDKSettings)
        {
            _appSettings = appSettings;
            _fuelerlinxSdkSettings = appPartnerSDKSettings.Value.FuelerLinx;
            _APIKey = _fuelerlinxSdkSettings.APIKey;
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

        public async Task<FBOLinxNearbyAirportsResponse> GetTransactionsForNearbyAirports(FBOLinxNearbyAirportsRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FBOLinxNearbyAirportsResponse result = await apiClient.GetTransactionsCountForNearbyAirportsAsync(request);
            return result;
        }

        public async Task<FBOLinxOrdersResponse> GetTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FBOLinxOrdersResponse result = await apiClient.GetTransactionsCountAsync(request);
            return result;
        }

        public async Task<FboLinxContractFuelVendorsCountResponse> GetContractFuelVendorsTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxContractFuelVendorsCountResponse result = await apiClient.GetContractFuelVendorsTransactionsCountAsync(request);
            return result;
        }

        public async Task<FboLinxContractFuelVendorsCountsByAirportsResponse> GetContractFuelVendorsTransactionsCountByAirports(FBOLinxOrdersForMultipleAirportsRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxContractFuelVendorsCountsByAirportsResponse result = await apiClient.GetContractFuelVendorsTransactionsCountByAirportsAsync(request);
            return result;
        }

        public async Task<FboLinxFbosTransactionsCountResponse> GetFBOsTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxFbosTransactionsCountResponse result = await apiClient.GetFboTransactionsCountAsync(request);
            return result;
        }

        public async Task<FBOLinxGroupOrdersResponse> GetTransactionsCountForFbosAndAirports(FBOLinxGroupOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FBOLinxGroupOrdersResponse result = await apiClient.GetGroupFbosTransactionsCountAsync(request);
            return result;
        }

        public async Task<FboLinxCustomerTransactionsCountAtAirportResponse> GetCustomerTransactionsCountForAirport(FBOLinxOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxCustomerTransactionsCountAtAirportResponse result = await apiClient.GetCustomerTransactionsCountAsync(request);
            return result;
        }

        public async Task<FboLinxCustomerTransactionsCountAtAirportResponse> GetCustomerTransactionsCountForMultipleAirports(FBOLinxOrdersForMultipleAirportsRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxCustomerTransactionsCountAtAirportResponse result = await apiClient.GetCustomerTransactionsCountForMultipleAirportsAsync(request);
            return result;
        }

        public async Task<FboLinxCustomerTransactionsCountAtAirportResponse> GetCustomerFBOTransactionsCount(FBOLinxOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxCustomerTransactionsCountAtAirportResponse result = await apiClient.GetCustomerFBOTransactionsCountAsync(request);
            return result;
        }

        public async Task<FBOLinxOrdersResponse> GetTransactionsDirectOrdersCount(FBOLinxOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FBOLinxOrdersResponse result = await apiClient.GetTransactionsDirectOrdersCountAsync(request);
            return result;
        }

        public async Task<FBOLinxContractFuelOrdersResponse> GetContractFuelRequests(FBOLinxOrdersRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FBOLinxContractFuelOrdersResponse result = await apiClient.GetContractFuelOrdersAsync(request);
            return result;
        }

        public async Task<FboLinxAircraftsResponse> GetAircraftsFromFuelerinx()
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxAircraftsResponse result = await apiClient.GetAircraftTailsGroupedByCompanyAsync();
            return result;
        }

        public async Task<FuelVendorDTO> UpdateFuelVendorEmails(FBOLinxFuelVendorUpdateRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FBOLinxFuelVendorUpdateResponse result = await apiClient.UpdateFuelVendorAsync(request);
            return result.Result;
        }

        public async Task<int> GetLatestFlightDeptPullHistoryForIcao(FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest request)
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            int result = await apiClient.GetLatestPullHistoryFlightDepartmentForICAOAsync(request);
            return result;
        }

        public async Task<ICollection<FbolinxCustomerFuelVendors>> GetCustomerFuelVendors()
        {
            var authToken = await GetAuthenticationTokenFromService();
            var apiClient = GetApiClient(authToken);
            FboLinxCustomerFuelVendorsResponse result = await apiClient.GetCustomerFuelVendorsAsync();
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

        public IClient GetApiClient(string authToken)
        {
            _httpClient = new HttpClient();

            AdjustHeader("x-api-key", _APIKey);

            if (!string.IsNullOrEmpty(authToken))
            {
                AdjustHeader("Authorization", "Bearer " + authToken.Replace("JWT ", ""));
            }

            Client client = new Client(_fuelerlinxSdkSettings.APIEndpoint, _httpClient);
            return client;
        }

        private void AdjustHeader(string headerName, string headerValue)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(headerName))
                _httpClient.DefaultRequestHeaders.Remove(headerName);
            _httpClient.DefaultRequestHeaders.Add(headerName, headerValue);
        }
        #endregion
    }
}

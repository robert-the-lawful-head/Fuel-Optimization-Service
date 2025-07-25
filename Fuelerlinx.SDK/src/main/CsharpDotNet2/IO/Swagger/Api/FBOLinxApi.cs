using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IFBOLinxApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <returns>FboLinxAircraftsResponse</returns>
        FboLinxAircraftsResponse GetAircraftTailsGroupedByCompany ();
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FBOLinxContractFuelOrdersResponse</returns>
        FBOLinxContractFuelOrdersResponse GetContractFuelOrders (FBOLinxOrdersRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FboLinxContractFuelVendorsCountResponse</returns>
        FboLinxContractFuelVendorsCountResponse GetContractFuelVendorsTransactionsCount (FBOLinxOrdersRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FboLinxContractFuelVendorsCountsByAirportsResponse</returns>
        FboLinxContractFuelVendorsCountsByAirportsResponse GetContractFuelVendorsTransactionsCountByAirports (FBOLinxOrdersForMultipleAirportsRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FboLinxCustomerTransactionsCountAtAirportResponse</returns>
        FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerFBOTransactionsCount (FBOLinxOrdersRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <returns>FboLinxCustomerFuelVendorsResponse</returns>
        FboLinxCustomerFuelVendorsResponse GetCustomerFuelVendors ();
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FboLinxCustomerTransactionsCountAtAirportResponse</returns>
        FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerTransactionsCount (FBOLinxOrdersRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FboLinxCustomerTransactionsCountAtAirportResponse</returns>
        FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerTransactionsCountForMultipleAirports (FBOLinxOrdersForMultipleAirportsRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FboLinxFbosTransactionsCountResponse</returns>
        FboLinxFbosTransactionsCountResponse GetFboTransactionsCount (FBOLinxOrdersRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FBOLinxGroupOrdersResponse</returns>
        FBOLinxGroupOrdersResponse GetGroupFbosTransactionsCount (FBOLinxGroupOrdersRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>int?</returns>
        int? GetLatestPullHistoryFlightDepartmentForICAO (FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FBOLinxOrdersResponse</returns>
        FBOLinxOrdersResponse GetTransactionsCount (FBOLinxOrdersRequest body);
        /// <summary>
        /// FBOLinx only - Fetch transactions associated with a particular airport and airports within X range of that airport. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FBOLinxNearbyAirportsResponse</returns>
        FBOLinxNearbyAirportsResponse GetTransactionsCountForNearbyAirports (FBOLinxNearbyAirportsRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FBOLinxOrdersResponse</returns>
        FBOLinxOrdersResponse GetTransactionsDirectOrdersCount (FBOLinxOrdersRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>FBOLinxFuelVendorUpdateResponse</returns>
        FBOLinxFuelVendorUpdateResponse UpdateFuelVendor (FBOLinxFuelVendorUpdateRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FBOLinxApi : IFBOLinxApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FBOLinxApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FBOLinxApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FBOLinxApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FBOLinxApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        ///  
        /// </summary>
        /// <returns>FboLinxAircraftsResponse</returns>            
        public FboLinxAircraftsResponse GetAircraftTailsGroupedByCompany ()
        {
            
    
            var path = "/api/FBOLinx/get-aircraft-tails-grouped-by-company";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftTailsGroupedByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftTailsGroupedByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxAircraftsResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxAircraftsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FBOLinxContractFuelOrdersResponse</returns>            
        public FBOLinxContractFuelOrdersResponse GetContractFuelOrders (FBOLinxOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-contract-orders";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetContractFuelOrders: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetContractFuelOrders: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FBOLinxContractFuelOrdersResponse) ApiClient.Deserialize(response.Content, typeof(FBOLinxContractFuelOrdersResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FboLinxContractFuelVendorsCountResponse</returns>            
        public FboLinxContractFuelVendorsCountResponse GetContractFuelVendorsTransactionsCount (FBOLinxOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-contract-fuel-vendors-orders-count-at-airport";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetContractFuelVendorsTransactionsCount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetContractFuelVendorsTransactionsCount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxContractFuelVendorsCountResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxContractFuelVendorsCountResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FboLinxContractFuelVendorsCountsByAirportsResponse</returns>            
        public FboLinxContractFuelVendorsCountsByAirportsResponse GetContractFuelVendorsTransactionsCountByAirports (FBOLinxOrdersForMultipleAirportsRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-contract-fuel-vendors-orders-counts-by-airports";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetContractFuelVendorsTransactionsCountByAirports: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetContractFuelVendorsTransactionsCountByAirports: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxContractFuelVendorsCountsByAirportsResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxContractFuelVendorsCountsByAirportsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FboLinxCustomerTransactionsCountAtAirportResponse</returns>            
        public FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerFBOTransactionsCount (FBOLinxOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-customer-fbo-orders-count-at-airport";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerFBOTransactionsCount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerFBOTransactionsCount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxCustomerTransactionsCountAtAirportResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxCustomerTransactionsCountAtAirportResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <returns>FboLinxCustomerFuelVendorsResponse</returns>            
        public FboLinxCustomerFuelVendorsResponse GetCustomerFuelVendors ()
        {
            
    
            var path = "/api/FBOLinx/get-customer-fuel-vendors";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerFuelVendors: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerFuelVendors: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxCustomerFuelVendorsResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxCustomerFuelVendorsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FboLinxCustomerTransactionsCountAtAirportResponse</returns>            
        public FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerTransactionsCount (FBOLinxOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-customer-orders-count-at-airport";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerTransactionsCount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerTransactionsCount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxCustomerTransactionsCountAtAirportResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxCustomerTransactionsCountAtAirportResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FboLinxCustomerTransactionsCountAtAirportResponse</returns>            
        public FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerTransactionsCountForMultipleAirports (FBOLinxOrdersForMultipleAirportsRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-customer-orders-count-at-multiple-airports";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerTransactionsCountForMultipleAirports: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomerTransactionsCountForMultipleAirports: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxCustomerTransactionsCountAtAirportResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxCustomerTransactionsCountAtAirportResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FboLinxFbosTransactionsCountResponse</returns>            
        public FboLinxFbosTransactionsCountResponse GetFboTransactionsCount (FBOLinxOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-fbo-orders-count-at-airport";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboTransactionsCount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboTransactionsCount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboLinxFbosTransactionsCountResponse) ApiClient.Deserialize(response.Content, typeof(FboLinxFbosTransactionsCountResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FBOLinxGroupOrdersResponse</returns>            
        public FBOLinxGroupOrdersResponse GetGroupFbosTransactionsCount (FBOLinxGroupOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-fbos-and-airports-orders-count";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetGroupFbosTransactionsCount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetGroupFbosTransactionsCount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FBOLinxGroupOrdersResponse) ApiClient.Deserialize(response.Content, typeof(FBOLinxGroupOrdersResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>int?</returns>            
        public int? GetLatestPullHistoryFlightDepartmentForICAO (FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-latest-pullhistory-flight-dept-by-icao";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetLatestPullHistoryFlightDepartmentForICAO: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetLatestPullHistoryFlightDepartmentForICAO: " + response.ErrorMessage, response.ErrorMessage);
    
            return (int?) ApiClient.Deserialize(response.Content, typeof(int?), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FBOLinxOrdersResponse</returns>            
        public FBOLinxOrdersResponse GetTransactionsCount (FBOLinxOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-orders-count-at-airport";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsCount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsCount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FBOLinxOrdersResponse) ApiClient.Deserialize(response.Content, typeof(FBOLinxOrdersResponse), response.Headers);
        }
    
        /// <summary>
        /// FBOLinx only - Fetch transactions associated with a particular airport and airports within X range of that airport. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FBOLinxNearbyAirportsResponse</returns>            
        public FBOLinxNearbyAirportsResponse GetTransactionsCountForNearbyAirports (FBOLinxNearbyAirportsRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-nearby-airports";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsCountForNearbyAirports: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsCountForNearbyAirports: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FBOLinxNearbyAirportsResponse) ApiClient.Deserialize(response.Content, typeof(FBOLinxNearbyAirportsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FBOLinxOrdersResponse</returns>            
        public FBOLinxOrdersResponse GetTransactionsDirectOrdersCount (FBOLinxOrdersRequest body)
        {
            
    
            var path = "/api/FBOLinx/get-direct-orders-count";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsDirectOrdersCount: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsDirectOrdersCount: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FBOLinxOrdersResponse) ApiClient.Deserialize(response.Content, typeof(FBOLinxOrdersResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>FBOLinxFuelVendorUpdateResponse</returns>            
        public FBOLinxFuelVendorUpdateResponse UpdateFuelVendor (FBOLinxFuelVendorUpdateRequest body)
        {
            
    
            var path = "/api/FBOLinx/update-fuelvendor-emails";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelVendor: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelVendor: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FBOLinxFuelVendorUpdateResponse) ApiClient.Deserialize(response.Content, typeof(FBOLinxFuelVendorUpdateResponse), response.Headers);
        }
    
    }
}

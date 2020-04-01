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
        /// <param name="body"></param>
        /// <returns>FBOLinxOrdersResponse</returns>
        FBOLinxOrdersResponse GetTransactionsCount (FBOLinxOrdersRequest body);
        /// <summary>
        ///  
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
        ///  
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
    
    }
}

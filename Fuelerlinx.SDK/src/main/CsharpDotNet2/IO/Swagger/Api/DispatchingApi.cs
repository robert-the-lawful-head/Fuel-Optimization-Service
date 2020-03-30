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
    public interface IDispatchingApi
    {
        /// <summary>
        /// Cancel a previously dispatched fuel order by it&#39;s transaction id. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>CancelFuelOrdersResponse</returns>
        CancelFuelOrdersResponse CancelFuelOrder (CancelFuelOrderRequest body);
        /// <summary>
        /// Places a fuel order based on the requested details.  This will notify the fuel vendor and any additional integrations the customer has enabled. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>DispatchFuelResponse</returns>
        DispatchFuelResponse OrderFuel (DispatchFuelRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class DispatchingApi : IDispatchingApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchingApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public DispatchingApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchingApi"/> class.
        /// </summary>
        /// <returns></returns>
        public DispatchingApi(String basePath)
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
        /// Cancel a previously dispatched fuel order by it&#39;s transaction id. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>CancelFuelOrdersResponse</returns>            
        public CancelFuelOrdersResponse CancelFuelOrder (CancelFuelOrderRequest body)
        {
            
    
            var path = "/api/Dispatching/cancel-fuel-order";
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
                throw new ApiException ((int)response.StatusCode, "Error calling CancelFuelOrder: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CancelFuelOrder: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CancelFuelOrdersResponse) ApiClient.Deserialize(response.Content, typeof(CancelFuelOrdersResponse), response.Headers);
        }
    
        /// <summary>
        /// Places a fuel order based on the requested details.  This will notify the fuel vendor and any additional integrations the customer has enabled. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>DispatchFuelResponse</returns>            
        public DispatchFuelResponse OrderFuel (DispatchFuelRequest body)
        {
            
    
            var path = "/api/Dispatching/orderfuel";
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderFuel: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderFuel: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DispatchFuelResponse) ApiClient.Deserialize(response.Content, typeof(DispatchFuelResponse), response.Headers);
        }
    
    }
}

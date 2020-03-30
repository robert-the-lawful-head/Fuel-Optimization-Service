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
    public interface IGroundServicesApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="userAuthenticationToken"></param>
        /// <returns>FlightBridgeAuthorizationCheckResponse</returns>
        FlightBridgeAuthorizationCheckResponse GetUserInfoForFlightBridge (string userAuthenticationToken);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class GroundServicesApi : IGroundServicesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroundServicesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public GroundServicesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="GroundServicesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public GroundServicesApi(String basePath)
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
        /// <param name="userAuthenticationToken"></param> 
        /// <returns>FlightBridgeAuthorizationCheckResponse</returns>            
        public FlightBridgeAuthorizationCheckResponse GetUserInfoForFlightBridge (string userAuthenticationToken)
        {
            
            // verify the required parameter 'userAuthenticationToken' is set
            if (userAuthenticationToken == null) throw new ApiException(400, "Missing required parameter 'userAuthenticationToken' when calling GetUserInfoForFlightBridge");
            
    
            var path = "/api/GroundServices/flightbridge/user-info/from-token/{userAuthenticationToken}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userAuthenticationToken" + "}", ApiClient.ParameterToString(userAuthenticationToken));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetUserInfoForFlightBridge: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetUserInfoForFlightBridge: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FlightBridgeAuthorizationCheckResponse) ApiClient.Deserialize(response.Content, typeof(FlightBridgeAuthorizationCheckResponse), response.Headers);
        }
    
    }
}

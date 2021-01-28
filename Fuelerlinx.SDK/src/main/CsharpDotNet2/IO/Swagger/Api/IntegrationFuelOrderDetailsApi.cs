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
    public interface IIntegrationFuelOrderDetailsApi
    {
        /// <summary>
        /// Internal use only - associates a fuel order transaction with an integration record. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>IntegrationFuelOrderDetailsResponse</returns>
        IntegrationFuelOrderDetailsResponse AddIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body);
        /// <summary>
        /// Internal use only - cancel an integration record from being tied to a transaction and notify the partner. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>DeleteIntegrationFuelOrderDetailsResponse</returns>
        DeleteIntegrationFuelOrderDetailsResponse CancelIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body);
        /// <summary>
        /// Internal use only - update an integration record associated with a fuel order transaction. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>IntegrationFuelOrderDetailsResponse</returns>
        IntegrationFuelOrderDetailsResponse UpdateIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class IntegrationFuelOrderDetailsApi : IIntegrationFuelOrderDetailsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationFuelOrderDetailsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public IntegrationFuelOrderDetailsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationFuelOrderDetailsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public IntegrationFuelOrderDetailsApi(String basePath)
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
        /// Internal use only - associates a fuel order transaction with an integration record. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>IntegrationFuelOrderDetailsResponse</returns>            
        public IntegrationFuelOrderDetailsResponse AddIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body)
        {
            
    
            var path = "/api/IntegrationFuelOrderDetails";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AddIntegrationFuelOrderDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddIntegrationFuelOrderDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationFuelOrderDetailsResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationFuelOrderDetailsResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - cancel an integration record from being tied to a transaction and notify the partner. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>DeleteIntegrationFuelOrderDetailsResponse</returns>            
        public DeleteIntegrationFuelOrderDetailsResponse CancelIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body)
        {
            
    
            var path = "/api/IntegrationFuelOrderDetails/cancel";
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
                throw new ApiException ((int)response.StatusCode, "Error calling CancelIntegrationFuelOrderDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CancelIntegrationFuelOrderDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteIntegrationFuelOrderDetailsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteIntegrationFuelOrderDetailsResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - update an integration record associated with a fuel order transaction. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>IntegrationFuelOrderDetailsResponse</returns>            
        public IntegrationFuelOrderDetailsResponse UpdateIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body)
        {
            
    
            var path = "/api/IntegrationFuelOrderDetails";
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIntegrationFuelOrderDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIntegrationFuelOrderDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationFuelOrderDetailsResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationFuelOrderDetailsResponse), response.Headers);
        }
    
    }
}

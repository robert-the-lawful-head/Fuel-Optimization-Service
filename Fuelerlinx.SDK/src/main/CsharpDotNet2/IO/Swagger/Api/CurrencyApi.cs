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
    public interface ICurrencyApi
    {
        /// <summary>
        /// Fetch a list of currencies with their current rate-to-dollar conversion value. 
        /// </summary>
        /// <returns>CurrencyListResponse</returns>
        CurrencyListResponse GetCurrencyList ();
        /// <summary>
        /// Internal use only - Sync the latest exchange rates with the European Central Bank 
        /// </summary>
        /// <returns>SyncLatestExchangeRatesResponse</returns>
        SyncLatestExchangeRatesResponse SyncLatestExchangeRates ();
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CurrencyApi : ICurrencyApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CurrencyApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CurrencyApi(String basePath)
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
        /// Fetch a list of currencies with their current rate-to-dollar conversion value. 
        /// </summary>
        /// <returns>CurrencyListResponse</returns>            
        public CurrencyListResponse GetCurrencyList ()
        {
            
    
            var path = "/api/Currency/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrencyList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrencyList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrencyListResponse) ApiClient.Deserialize(response.Content, typeof(CurrencyListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Sync the latest exchange rates with the European Central Bank 
        /// </summary>
        /// <returns>SyncLatestExchangeRatesResponse</returns>            
        public SyncLatestExchangeRatesResponse SyncLatestExchangeRates ()
        {
            
    
            var path = "/api/Currency/exchange-rates/sync-latest";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SyncLatestExchangeRates: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SyncLatestExchangeRates: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SyncLatestExchangeRatesResponse) ApiClient.Deserialize(response.Content, typeof(SyncLatestExchangeRatesResponse), response.Headers);
        }
    
    }
}

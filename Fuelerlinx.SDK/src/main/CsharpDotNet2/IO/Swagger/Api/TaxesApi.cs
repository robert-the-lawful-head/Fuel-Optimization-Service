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
    public interface ITaxesApi
    {
        /// <summary>
        /// Internal use only - Delete a tax-by-country record. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteTaxesByCountryResponse</returns>
        DeleteTaxesByCountryResponse DeleteTaxByCountry (int? id);
        /// <summary>
        /// Fetch taxes by country by it&#39;s [id].  These taxes include MOT/VAT and under what circumstances they are applicable. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TaxesByCountryResponse</returns>
        TaxesByCountryResponse GetTaxByCountryById (int? id);
        /// <summary>
        /// Fetch all tax-by-country records that are available.  These taxes include MOT/VAT and under what circumstances they are applicable. 
        /// </summary>
        /// <returns>TaxesByCountryListResponse</returns>
        TaxesByCountryListResponse GetTaxesByCountryList ();
        /// <summary>
        /// Fetch taxes for a specified [countryName].  These taxes include MOT/VAT and under what circumstances they are applicable. 
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>TaxesByCountryResponse</returns>
        TaxesByCountryResponse GetTaxesByCountryName (string countryName);
        /// <summary>
        /// Internal use only - Add a tax-by-country record to store MOT/VAT and control which circumstances they apply to. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTaxesByCountryResponse</returns>
        PostTaxesByCountryResponse PostTaxByCountry (PostTaxesByCountryRequest body);
        /// <summary>
        /// Internal use only - Update a tax-by-country record to store MOT/VAT and control which circumstances they apply to. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateTaxesByCountryResponse</returns>
        UpdateTaxesByCountryResponse UpdateTaxByCountry (UpdateTaxesByCountryRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class TaxesApi : ITaxesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public TaxesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public TaxesApi(String basePath)
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
        /// Internal use only - Delete a tax-by-country record. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteTaxesByCountryResponse</returns>            
        public DeleteTaxesByCountryResponse DeleteTaxByCountry (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteTaxByCountry");
            
    
            var path = "/api/Taxes/by-country/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTaxByCountry: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTaxByCountry: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteTaxesByCountryResponse) ApiClient.Deserialize(response.Content, typeof(DeleteTaxesByCountryResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch taxes by country by it&#39;s [id].  These taxes include MOT/VAT and under what circumstances they are applicable. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>TaxesByCountryResponse</returns>            
        public TaxesByCountryResponse GetTaxByCountryById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetTaxByCountryById");
            
    
            var path = "/api/Taxes/by-country/id/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTaxByCountryById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTaxByCountryById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TaxesByCountryResponse) ApiClient.Deserialize(response.Content, typeof(TaxesByCountryResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all tax-by-country records that are available.  These taxes include MOT/VAT and under what circumstances they are applicable. 
        /// </summary>
        /// <returns>TaxesByCountryListResponse</returns>            
        public TaxesByCountryListResponse GetTaxesByCountryList ()
        {
            
    
            var path = "/api/Taxes/by-country/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTaxesByCountryList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTaxesByCountryList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TaxesByCountryListResponse) ApiClient.Deserialize(response.Content, typeof(TaxesByCountryListResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch taxes for a specified [countryName].  These taxes include MOT/VAT and under what circumstances they are applicable. 
        /// </summary>
        /// <param name="countryName"></param> 
        /// <returns>TaxesByCountryResponse</returns>            
        public TaxesByCountryResponse GetTaxesByCountryName (string countryName)
        {
            
            // verify the required parameter 'countryName' is set
            if (countryName == null) throw new ApiException(400, "Missing required parameter 'countryName' when calling GetTaxesByCountryName");
            
    
            var path = "/api/Taxes/by-country/country-name/{countryName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "countryName" + "}", ApiClient.ParameterToString(countryName));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTaxesByCountryName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTaxesByCountryName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TaxesByCountryResponse) ApiClient.Deserialize(response.Content, typeof(TaxesByCountryResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a tax-by-country record to store MOT/VAT and control which circumstances they apply to. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTaxesByCountryResponse</returns>            
        public PostTaxesByCountryResponse PostTaxByCountry (PostTaxesByCountryRequest body)
        {
            
    
            var path = "/api/Taxes/by-country";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTaxByCountry: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTaxByCountry: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTaxesByCountryResponse) ApiClient.Deserialize(response.Content, typeof(PostTaxesByCountryResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update a tax-by-country record to store MOT/VAT and control which circumstances they apply to. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateTaxesByCountryResponse</returns>            
        public UpdateTaxesByCountryResponse UpdateTaxByCountry (UpdateTaxesByCountryRequest body)
        {
            
    
            var path = "/api/Taxes/by-country";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTaxByCountry: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTaxByCountry: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateTaxesByCountryResponse) ApiClient.Deserialize(response.Content, typeof(UpdateTaxesByCountryResponse), response.Headers);
        }
    
    }
}

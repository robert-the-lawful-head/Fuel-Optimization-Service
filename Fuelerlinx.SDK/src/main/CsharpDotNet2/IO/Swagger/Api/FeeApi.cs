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
    public interface IFeeApi
    {
        /// <summary>
        /// Delete a company-specific service/fee. 
        /// </summary>
        /// <param name="feeId"></param>
        /// <returns>DeleteServicesAndFeesByCompanyResponse</returns>
        DeleteServicesAndFeesByCompanyResponse DeleteServicesAndFeesByCompany (int? feeId);
        /// <summary>
        /// Fetch a company-specific service/fee by it&#39;s Id. 
        /// </summary>
        /// <param name="feeId"></param>
        /// <returns>ServicesAndFeesByCompanyResponse</returns>
        ServicesAndFeesByCompanyResponse GetServicesAndFeesByCompany (int? feeId);
        /// <summary>
        /// Fetch a company-specific service/fee by the ICAO and FBO. 
        /// </summary>
        /// <param name="icao"></param>
        /// <param name="fboName"></param>
        /// <returns>ServicesAndFeesByCompanyListResponse</returns>
        ServicesAndFeesByCompanyListResponse GetServicesAndFeesByCompanyByLocation (string icao, string fboName);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>ServicesAndFeesByCompanyListResponse</returns>
        ServicesAndFeesByCompanyListResponse GetServicesAndFeesByCompanyId (int? companyId);
        /// <summary>
        /// Add a new company-specific service/fee. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostServicesAndFeesByCompanyResponse</returns>
        PostServicesAndFeesByCompanyResponse PostServicesAndFeesByCompany (PostServicesAndFeesByCompanyRequest body);
        /// <summary>
        /// Update a company-specific service/fee. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateServicesAndFeesByCompanyResponse</returns>
        UpdateServicesAndFeesByCompanyResponse UpdateServicesAndFeesByCompany (UpdateServicesAndFeesByCompanyRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FeeApi : IFeeApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeeApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FeeApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FeeApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FeeApi(String basePath)
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
        /// Delete a company-specific service/fee. 
        /// </summary>
        /// <param name="feeId"></param> 
        /// <returns>DeleteServicesAndFeesByCompanyResponse</returns>            
        public DeleteServicesAndFeesByCompanyResponse DeleteServicesAndFeesByCompany (int? feeId)
        {
            
            // verify the required parameter 'feeId' is set
            if (feeId == null) throw new ApiException(400, "Missing required parameter 'feeId' when calling DeleteServicesAndFeesByCompany");
            
    
            var path = "/api/Fee/company-specific/{feeId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "feeId" + "}", ApiClient.ParameterToString(feeId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteServicesAndFeesByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteServicesAndFeesByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteServicesAndFeesByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(DeleteServicesAndFeesByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a company-specific service/fee by it&#39;s Id. 
        /// </summary>
        /// <param name="feeId"></param> 
        /// <returns>ServicesAndFeesByCompanyResponse</returns>            
        public ServicesAndFeesByCompanyResponse GetServicesAndFeesByCompany (int? feeId)
        {
            
            // verify the required parameter 'feeId' is set
            if (feeId == null) throw new ApiException(400, "Missing required parameter 'feeId' when calling GetServicesAndFeesByCompany");
            
    
            var path = "/api/Fee/company-specific/{feeId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "feeId" + "}", ApiClient.ParameterToString(feeId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetServicesAndFeesByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetServicesAndFeesByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ServicesAndFeesByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(ServicesAndFeesByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a company-specific service/fee by the ICAO and FBO. 
        /// </summary>
        /// <param name="icao"></param> 
        /// <param name="fboName"></param> 
        /// <returns>ServicesAndFeesByCompanyListResponse</returns>            
        public ServicesAndFeesByCompanyListResponse GetServicesAndFeesByCompanyByLocation (string icao, string fboName)
        {
            
            // verify the required parameter 'icao' is set
            if (icao == null) throw new ApiException(400, "Missing required parameter 'icao' when calling GetServicesAndFeesByCompanyByLocation");
            
            // verify the required parameter 'fboName' is set
            if (fboName == null) throw new ApiException(400, "Missing required parameter 'fboName' when calling GetServicesAndFeesByCompanyByLocation");
            
    
            var path = "/api/Fee/company-specific/by-location/{icao}/{fboName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "icao" + "}", ApiClient.ParameterToString(icao));
path = path.Replace("{" + "fboName" + "}", ApiClient.ParameterToString(fboName));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetServicesAndFeesByCompanyByLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetServicesAndFeesByCompanyByLocation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ServicesAndFeesByCompanyListResponse) ApiClient.Deserialize(response.Content, typeof(ServicesAndFeesByCompanyListResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>ServicesAndFeesByCompanyListResponse</returns>            
        public ServicesAndFeesByCompanyListResponse GetServicesAndFeesByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetServicesAndFeesByCompanyId");
            
    
            var path = "/api/Fee/company-specific/by-company-id/{companyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetServicesAndFeesByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetServicesAndFeesByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ServicesAndFeesByCompanyListResponse) ApiClient.Deserialize(response.Content, typeof(ServicesAndFeesByCompanyListResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new company-specific service/fee. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostServicesAndFeesByCompanyResponse</returns>            
        public PostServicesAndFeesByCompanyResponse PostServicesAndFeesByCompany (PostServicesAndFeesByCompanyRequest body)
        {
            
    
            var path = "/api/Fee/company-specific";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostServicesAndFeesByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostServicesAndFeesByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostServicesAndFeesByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(PostServicesAndFeesByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a company-specific service/fee. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateServicesAndFeesByCompanyResponse</returns>            
        public UpdateServicesAndFeesByCompanyResponse UpdateServicesAndFeesByCompany (UpdateServicesAndFeesByCompanyRequest body)
        {
            
    
            var path = "/api/Fee/company-specific";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateServicesAndFeesByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateServicesAndFeesByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateServicesAndFeesByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(UpdateServicesAndFeesByCompanyResponse), response.Headers);
        }
    
    }
}

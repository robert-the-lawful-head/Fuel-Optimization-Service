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
    public interface ICompanyApi
    {
        /// <summary>
        /// Internal use only - Delete an existing company. 
        /// </summary>
        /// <param name="id2"></param>
        /// <param name="id"></param>
        /// <returns>DeleteCompanyResponse</returns>
        DeleteCompanyResponse DeleteCompany (string id2, int? id);
        /// <summary>
        /// Internal use only - Fetch all companies (active only). 
        /// </summary>
        /// <returns>CompanyListResponse</returns>
        CompanyListResponse GetActiveCompanyList ();
        /// <summary>
        ///  
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>CompanyAccountSettingsResponse</returns>
        CompanyAccountSettingsResponse GetCompanyAccountSettingsByCompanyId (int? companyId);
        /// <summary>
        /// Fetch a company&#39;s information by the company {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CompanyResponse</returns>
        CompanyResponse GetCompanyById (int? id);
        /// <summary>
        /// Internal use only - Fetch all companies (both active and inactive). 
        /// </summary>
        /// <returns>CompanyListResponse</returns>
        CompanyListResponse GetCompanyList ();
        /// <summary>
        /// Internal use only - Add a new company. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyResponse</returns>
        PostCompanyResponse PostCompany (PostCompanyRequest body);
        /// <summary>
        /// Internal use only - Update an existing company with the provided information. 
        /// </summary>
        /// <param name="id2"></param>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyResponse</returns>
        UpdateCompanyResponse UpdateCompany (string id2, int? id, UpdateCompanyRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyAccountSettingsResponse</returns>
        UpdateCompanyAccountSettingsResponse UpdateCompanyAccountSettings (UpdateCompanyAccountSettingsRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CompanyApi : ICompanyApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CompanyApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CompanyApi(String basePath)
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
        /// Internal use only - Delete an existing company. 
        /// </summary>
        /// <param name="id2"></param> 
        /// <param name="id"></param> 
        /// <returns>DeleteCompanyResponse</returns>            
        public DeleteCompanyResponse DeleteCompany (string id2, int? id)
        {
            
            // verify the required parameter 'id2' is set
            if (id2 == null) throw new ApiException(400, "Missing required parameter 'id2' when calling DeleteCompany");
            
    
            var path = "/api/Company/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id2));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (id != null) headerParams.Add("id", ApiClient.ParameterToString(id)); // header parameter
                            
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all companies (active only). 
        /// </summary>
        /// <returns>CompanyListResponse</returns>            
        public CompanyListResponse GetActiveCompanyList ()
        {
            
    
            var path = "/api/Company/list/active";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetActiveCompanyList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetActiveCompanyList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyListResponse) ApiClient.Deserialize(response.Content, typeof(CompanyListResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>CompanyAccountSettingsResponse</returns>            
        public CompanyAccountSettingsResponse GetCompanyAccountSettingsByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetCompanyAccountSettingsByCompanyId");
            
    
            var path = "/api/Company/account-settings/{companyId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyAccountSettingsByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyAccountSettingsByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyAccountSettingsResponse) ApiClient.Deserialize(response.Content, typeof(CompanyAccountSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a company&#39;s information by the company {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>CompanyResponse</returns>            
        public CompanyResponse GetCompanyById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetCompanyById");
            
    
            var path = "/api/Company/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyResponse) ApiClient.Deserialize(response.Content, typeof(CompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all companies (both active and inactive). 
        /// </summary>
        /// <returns>CompanyListResponse</returns>            
        public CompanyListResponse GetCompanyList ()
        {
            
    
            var path = "/api/Company/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyListResponse) ApiClient.Deserialize(response.Content, typeof(CompanyListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a new company. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyResponse</returns>            
        public PostCompanyResponse PostCompany (PostCompanyRequest body)
        {
            
    
            var path = "/api/Company";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update an existing company with the provided information. 
        /// </summary>
        /// <param name="id2"></param> 
        /// <param name="id"></param> 
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyResponse</returns>            
        public UpdateCompanyResponse UpdateCompany (string id2, int? id, UpdateCompanyRequest body)
        {
            
            // verify the required parameter 'id2' is set
            if (id2 == null) throw new ApiException(400, "Missing required parameter 'id2' when calling UpdateCompany");
            
    
            var path = "/api/Company/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id2));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                         if (id != null) headerParams.Add("id", ApiClient.ParameterToString(id)); // header parameter
                        postBody = ApiClient.Serialize(body); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyAccountSettingsResponse</returns>            
        public UpdateCompanyAccountSettingsResponse UpdateCompanyAccountSettings (UpdateCompanyAccountSettingsRequest body)
        {
            
    
            var path = "/api/Company/account-settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyAccountSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyAccountSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyAccountSettingsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyAccountSettingsResponse), response.Headers);
        }
    
    }
}

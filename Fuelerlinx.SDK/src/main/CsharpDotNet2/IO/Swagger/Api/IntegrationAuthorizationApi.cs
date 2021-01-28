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
    public interface IIntegrationAuthorizationApi
    {
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>IntegrationAuthorizationResponse</returns>
        IntegrationAuthorizationResponse AddIntegrationAuthorization (IntegrationAuthorizationDTO body);
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteIntegrationAuthorizationResponse</returns>
        DeleteIntegrationAuthorizationResponse DeleteIntegrationAuthorization (int? id);
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="integrationPartnerType"></param>
        /// <param name="affiliation"></param>
        /// <returns>IntegrationAuthorizationListResponse</returns>
        IntegrationAuthorizationListResponse GetIntegrationAuthorizationByAffiliation (int? companyId, int? integrationPartnerType, int? affiliation);
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>IntegrationAuthorizationListResponse</returns>
        IntegrationAuthorizationListResponse GetIntegrationAuthorizationByCompanyId (int? companyId);
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IntegrationAuthorizationResponse</returns>
        IntegrationAuthorizationResponse GetIntegrationAuthorizationById (int? id);
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="integrationPartnerType"></param>
        /// <returns>IntegrationAuthorizationListResponse</returns>
        IntegrationAuthorizationListResponse GetIntegrationAuthorizationByPartnerType (int? companyId, int? integrationPartnerType);
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>IntegrationAuthorizationResponse</returns>
        IntegrationAuthorizationResponse UpdateIntegrationAuthorization (int? id, IntegrationAuthorizationDTO body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class IntegrationAuthorizationApi : IIntegrationAuthorizationApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationAuthorizationApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public IntegrationAuthorizationApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationAuthorizationApi"/> class.
        /// </summary>
        /// <returns></returns>
        public IntegrationAuthorizationApi(String basePath)
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
        /// Internal use only 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>IntegrationAuthorizationResponse</returns>            
        public IntegrationAuthorizationResponse AddIntegrationAuthorization (IntegrationAuthorizationDTO body)
        {
            
    
            var path = "/api/IntegrationAuthorization";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AddIntegrationAuthorization: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddIntegrationAuthorization: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationAuthorizationResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationAuthorizationResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteIntegrationAuthorizationResponse</returns>            
        public DeleteIntegrationAuthorizationResponse DeleteIntegrationAuthorization (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteIntegrationAuthorization");
            
    
            var path = "/api/IntegrationAuthorization/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteIntegrationAuthorization: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteIntegrationAuthorization: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteIntegrationAuthorizationResponse) ApiClient.Deserialize(response.Content, typeof(DeleteIntegrationAuthorizationResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="integrationPartnerType"></param> 
        /// <param name="affiliation"></param> 
        /// <returns>IntegrationAuthorizationListResponse</returns>            
        public IntegrationAuthorizationListResponse GetIntegrationAuthorizationByAffiliation (int? companyId, int? integrationPartnerType, int? affiliation)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetIntegrationAuthorizationByAffiliation");
            
            // verify the required parameter 'integrationPartnerType' is set
            if (integrationPartnerType == null) throw new ApiException(400, "Missing required parameter 'integrationPartnerType' when calling GetIntegrationAuthorizationByAffiliation");
            
            // verify the required parameter 'affiliation' is set
            if (affiliation == null) throw new ApiException(400, "Missing required parameter 'affiliation' when calling GetIntegrationAuthorizationByAffiliation");
            
    
            var path = "/api/IntegrationAuthorization/company/{companyId}/integrationtype/{integrationPartnerType}/affiliation/{affiliation}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
path = path.Replace("{" + "integrationPartnerType" + "}", ApiClient.ParameterToString(integrationPartnerType));
path = path.Replace("{" + "affiliation" + "}", ApiClient.ParameterToString(affiliation));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationByAffiliation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationByAffiliation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationAuthorizationListResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationAuthorizationListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>IntegrationAuthorizationListResponse</returns>            
        public IntegrationAuthorizationListResponse GetIntegrationAuthorizationByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetIntegrationAuthorizationByCompanyId");
            
    
            var path = "/api/IntegrationAuthorization/company/{companyId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationAuthorizationListResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationAuthorizationListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>IntegrationAuthorizationResponse</returns>            
        public IntegrationAuthorizationResponse GetIntegrationAuthorizationById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetIntegrationAuthorizationById");
            
    
            var path = "/api/IntegrationAuthorization/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationAuthorizationResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationAuthorizationResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="integrationPartnerType"></param> 
        /// <returns>IntegrationAuthorizationListResponse</returns>            
        public IntegrationAuthorizationListResponse GetIntegrationAuthorizationByPartnerType (int? companyId, int? integrationPartnerType)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetIntegrationAuthorizationByPartnerType");
            
            // verify the required parameter 'integrationPartnerType' is set
            if (integrationPartnerType == null) throw new ApiException(400, "Missing required parameter 'integrationPartnerType' when calling GetIntegrationAuthorizationByPartnerType");
            
    
            var path = "/api/IntegrationAuthorization/company/{companyId}/integrationtype/{integrationPartnerType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
path = path.Replace("{" + "integrationPartnerType" + "}", ApiClient.ParameterToString(integrationPartnerType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationByPartnerType: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIntegrationAuthorizationByPartnerType: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationAuthorizationListResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationAuthorizationListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only 
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="body"></param> 
        /// <returns>IntegrationAuthorizationResponse</returns>            
        public IntegrationAuthorizationResponse UpdateIntegrationAuthorization (int? id, IntegrationAuthorizationDTO body)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UpdateIntegrationAuthorization");
            
    
            var path = "/api/IntegrationAuthorization/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIntegrationAuthorization: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIntegrationAuthorization: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationAuthorizationResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationAuthorizationResponse), response.Headers);
        }
    
    }
}

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
    public interface IPartnerApi
    {
        /// <summary>
        /// Apply credential changes for a certain type/afiiliation of integration partner. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostIntegrationPartnerCredentialsResponse</returns>
        PostIntegrationPartnerCredentialsResponse ApplyPartnerCredentialsByTypeAndAffiliation (PostIntegrationPartnerCredentialsRequest body);
        /// <summary>
        /// Fetch all integration partners of a certain type. 
        /// </summary>
        /// <param name="partnerType"></param>
        /// <returns>IntegrationPartnerListResponse</returns>
        IntegrationPartnerListResponse GetAvailablePartners (int? partnerType);
        /// <summary>
        /// Fetch all integration partners. 
        /// </summary>
        /// <returns>IntegrationPartnerListResponse</returns>
        IntegrationPartnerListResponse GetAvailablePartnersByType ();
        /// <summary>
        /// Fetch the credentials model for a certain type/affiliation of integration partner.  If the authenticated user has anything setup for that partner then the model will contain the user&#39;s data. 
        /// </summary>
        /// <param name="partnerType"></param>
        /// <param name="affiliation"></param>
        /// <returns>IntegrationPartnerCredentialsResponse</returns>
        IntegrationPartnerCredentialsResponse GetPartnerCredentialsByTypeAndAffiliation (int? partnerType, int? affiliation);
        /// <summary>
        /// Fetch the integration partner by the provided API key. 
        /// </summary>
        /// <returns></returns>
        void GetPartnerInfo ();
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class PartnerApi : IPartnerApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public PartnerApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerApi"/> class.
        /// </summary>
        /// <returns></returns>
        public PartnerApi(String basePath)
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
        /// Apply credential changes for a certain type/afiiliation of integration partner. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostIntegrationPartnerCredentialsResponse</returns>            
        public PostIntegrationPartnerCredentialsResponse ApplyPartnerCredentialsByTypeAndAffiliation (PostIntegrationPartnerCredentialsRequest body)
        {
            
    
            var path = "/api/Partner/credentials";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ApplyPartnerCredentialsByTypeAndAffiliation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ApplyPartnerCredentialsByTypeAndAffiliation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostIntegrationPartnerCredentialsResponse) ApiClient.Deserialize(response.Content, typeof(PostIntegrationPartnerCredentialsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all integration partners of a certain type. 
        /// </summary>
        /// <param name="partnerType"></param> 
        /// <returns>IntegrationPartnerListResponse</returns>            
        public IntegrationPartnerListResponse GetAvailablePartners (int? partnerType)
        {
            
            // verify the required parameter 'partnerType' is set
            if (partnerType == null) throw new ApiException(400, "Missing required parameter 'partnerType' when calling GetAvailablePartners");
            
    
            var path = "/api/Partner/list/type/{partnerType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "partnerType" + "}", ApiClient.ParameterToString(partnerType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAvailablePartners: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAvailablePartners: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationPartnerListResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationPartnerListResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all integration partners. 
        /// </summary>
        /// <returns>IntegrationPartnerListResponse</returns>            
        public IntegrationPartnerListResponse GetAvailablePartnersByType ()
        {
            
    
            var path = "/api/Partner/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAvailablePartnersByType: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAvailablePartnersByType: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationPartnerListResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationPartnerListResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch the credentials model for a certain type/affiliation of integration partner.  If the authenticated user has anything setup for that partner then the model will contain the user&#39;s data. 
        /// </summary>
        /// <param name="partnerType"></param> 
        /// <param name="affiliation"></param> 
        /// <returns>IntegrationPartnerCredentialsResponse</returns>            
        public IntegrationPartnerCredentialsResponse GetPartnerCredentialsByTypeAndAffiliation (int? partnerType, int? affiliation)
        {
            
            // verify the required parameter 'partnerType' is set
            if (partnerType == null) throw new ApiException(400, "Missing required parameter 'partnerType' when calling GetPartnerCredentialsByTypeAndAffiliation");
            
            // verify the required parameter 'affiliation' is set
            if (affiliation == null) throw new ApiException(400, "Missing required parameter 'affiliation' when calling GetPartnerCredentialsByTypeAndAffiliation");
            
    
            var path = "/api/Partner/credentials/type/{partnerType}/affiliation/{affiliation}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "partnerType" + "}", ApiClient.ParameterToString(partnerType));
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPartnerCredentialsByTypeAndAffiliation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPartnerCredentialsByTypeAndAffiliation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IntegrationPartnerCredentialsResponse) ApiClient.Deserialize(response.Content, typeof(IntegrationPartnerCredentialsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch the integration partner by the provided API key. 
        /// </summary>
        /// <returns></returns>            
        public void GetPartnerInfo ()
        {
            
    
            var path = "/api/Partner";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPartnerInfo: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPartnerInfo: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

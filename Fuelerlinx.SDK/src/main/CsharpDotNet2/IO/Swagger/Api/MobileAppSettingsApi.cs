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
    public interface IMobileAppSettingsApi
    {
        /// <summary>
        /// Internal use only - Delete a mobile app setting by id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteMobileAppSettingsResponse</returns>
        DeleteMobileAppSettingsResponse DeleteSetting (int? id);
        /// <summary>
        /// Internal use only - Fetch a mobile app setting by a key. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>MobileAppSettingsResponse</returns>
        MobileAppSettingsResponse GetSettingByKey (string key);
        /// <summary>
        /// Internal use only - Add a mobile app setting. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostMobileAppSettingsResponse</returns>
        PostMobileAppSettingsResponse PostSetting (PostMobileAppSettingsRequest body);
        /// <summary>
        /// Internal use only - Update a mobile app setting. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateMobileAppSettingsResponse</returns>
        UpdateMobileAppSettingsResponse UpdateSetting (UpdateMobileAppSettingsRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MobileAppSettingsApi : IMobileAppSettingsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileAppSettingsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public MobileAppSettingsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileAppSettingsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MobileAppSettingsApi(String basePath)
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
        /// Internal use only - Delete a mobile app setting by id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteMobileAppSettingsResponse</returns>            
        public DeleteMobileAppSettingsResponse DeleteSetting (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSetting");
            
    
            var path = "/api/MobileAppSettings/setting/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSetting: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSetting: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteMobileAppSettingsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteMobileAppSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch a mobile app setting by a key. 
        /// </summary>
        /// <param name="key"></param> 
        /// <returns>MobileAppSettingsResponse</returns>            
        public MobileAppSettingsResponse GetSettingByKey (string key)
        {
            
            // verify the required parameter 'key' is set
            if (key == null) throw new ApiException(400, "Missing required parameter 'key' when calling GetSettingByKey");
            
    
            var path = "/api/MobileAppSettings/setting/{key}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "key" + "}", ApiClient.ParameterToString(key));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSettingByKey: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSettingByKey: " + response.ErrorMessage, response.ErrorMessage);
    
            return (MobileAppSettingsResponse) ApiClient.Deserialize(response.Content, typeof(MobileAppSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a mobile app setting. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostMobileAppSettingsResponse</returns>            
        public PostMobileAppSettingsResponse PostSetting (PostMobileAppSettingsRequest body)
        {
            
    
            var path = "/api/MobileAppSettings/setting";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSetting: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSetting: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostMobileAppSettingsResponse) ApiClient.Deserialize(response.Content, typeof(PostMobileAppSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update a mobile app setting. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateMobileAppSettingsResponse</returns>            
        public UpdateMobileAppSettingsResponse UpdateSetting (UpdateMobileAppSettingsRequest body)
        {
            
    
            var path = "/api/MobileAppSettings/setting";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSetting: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSetting: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateMobileAppSettingsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateMobileAppSettingsResponse), response.Headers);
        }
    
    }
}

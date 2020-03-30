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
    public interface IUserApi
    {
        /// <summary>
        /// Internal use only - Delete a user from iFlightPlanner to stop the flight planning integration. 
        /// </summary>
        /// <returns>DeleteUserFromIFlightPlannerResponse</returns>
        DeleteUserFromIFlightPlannerResponse DeleteUserFromIFlightPlanner ();
        /// <summary>
        /// Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken]. Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken].
        /// </summary>
        /// <param name="body"></param>
        /// <returns>ExchangeRefreshTokenResponse</returns>
        ExchangeRefreshTokenResponse ExchangeRefreshToken (ExchangeRefreshTokenRequest body);
        /// <summary>
        /// Fetch a user by their [id]. The authenticated user must have access to view this user&#39;s record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserDTO</returns>
        UserDTO GetUser (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>UserResponse</returns>
        UserResponse GetUserByCredentials (string username, string password);
        /// <summary>
        /// Internal use only - Save a company to IFlightPlanner to use the flight planning integration. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>SaveCompanyToIFlightPlannerResponse</returns>
        SaveCompanyToIFlightPlannerResponse SaveCompanyToIFlightPlanner (SaveCompanyToIFlightPlannerRequest body);
        /// <summary>
        /// Internal use only - Save a user to IFlightPlanner to use the flight planning integration. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>SaveUserToIFlightPlannerResponse</returns>
        SaveUserToIFlightPlannerResponse SaveUserToIFlightPlanner (SaveUserToIFlightPlannerRequest body);
        /// <summary>
        /// Authenticates a user by username and password. The returned object contains a [AccessToken] (JWT) that can be used to identify the user for other API requests using the [Bearer] authorization header.  The response also contains a [RefreshToken] that can be used in the /api/user/refreshtoken api to receive a new [Token].
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UserAuthTokenResponse</returns>
        UserAuthTokenResponse UserAuthToken (UserAuthTokenFromCredentialsRequest body);
        /// <summary>
        /// Authenticates a user from an existing [AccessToken]. The returned object contains a [AccessToken] (JWT) that can be used to identify the user for other API requests using the [Bearer] authorization header.  The response also contains a [RefreshToken] that can be used in the /api/user/refreshtoken api to receive a new [Token].
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UserAuthTokenResponse</returns>
        UserAuthTokenResponse UserAuthTokenFromAccessToken (UserAuthTokenFromAccessTokenRequest body);
        /// <summary>
        /// Authenticate a user via the iFlightPlanner integration 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>UserAuthTokenResponse</returns>
        UserAuthTokenResponse UserAuthTokenFromIFlightPlannerGUID (string guid);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class UserApi : IUserApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public UserApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="UserApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UserApi(String basePath)
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
        /// Internal use only - Delete a user from iFlightPlanner to stop the flight planning integration. 
        /// </summary>
        /// <returns>DeleteUserFromIFlightPlannerResponse</returns>            
        public DeleteUserFromIFlightPlannerResponse DeleteUserFromIFlightPlanner ()
        {
            
    
            var path = "/api/User/iflightplanner/user";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteUserFromIFlightPlanner: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteUserFromIFlightPlanner: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteUserFromIFlightPlannerResponse) ApiClient.Deserialize(response.Content, typeof(DeleteUserFromIFlightPlannerResponse), response.Headers);
        }
    
        /// <summary>
        /// Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken]. Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken].
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>ExchangeRefreshTokenResponse</returns>            
        public ExchangeRefreshTokenResponse ExchangeRefreshToken (ExchangeRefreshTokenRequest body)
        {
            
    
            var path = "/api/User/refreshtoken";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ExchangeRefreshToken: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ExchangeRefreshToken: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ExchangeRefreshTokenResponse) ApiClient.Deserialize(response.Content, typeof(ExchangeRefreshTokenResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a user by their [id]. The authenticated user must have access to view this user&#39;s record.
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>UserDTO</returns>            
        public UserDTO GetUser (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetUser");
            
    
            var path = "/api/User/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserDTO) ApiClient.Deserialize(response.Content, typeof(UserDTO), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="username"></param> 
        /// <param name="password"></param> 
        /// <returns>UserResponse</returns>            
        public UserResponse GetUserByCredentials (string username, string password)
        {
            
            // verify the required parameter 'username' is set
            if (username == null) throw new ApiException(400, "Missing required parameter 'username' when calling GetUserByCredentials");
            
            // verify the required parameter 'password' is set
            if (password == null) throw new ApiException(400, "Missing required parameter 'password' when calling GetUserByCredentials");
            
    
            var path = "/api/User/by-credentials/{username}/{password}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "username" + "}", ApiClient.ParameterToString(username));
path = path.Replace("{" + "password" + "}", ApiClient.ParameterToString(password));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetUserByCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetUserByCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserResponse) ApiClient.Deserialize(response.Content, typeof(UserResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Save a company to IFlightPlanner to use the flight planning integration. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>SaveCompanyToIFlightPlannerResponse</returns>            
        public SaveCompanyToIFlightPlannerResponse SaveCompanyToIFlightPlanner (SaveCompanyToIFlightPlannerRequest body)
        {
            
    
            var path = "/api/User/iflightplanner/company";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SaveCompanyToIFlightPlanner: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SaveCompanyToIFlightPlanner: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SaveCompanyToIFlightPlannerResponse) ApiClient.Deserialize(response.Content, typeof(SaveCompanyToIFlightPlannerResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Save a user to IFlightPlanner to use the flight planning integration. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>SaveUserToIFlightPlannerResponse</returns>            
        public SaveUserToIFlightPlannerResponse SaveUserToIFlightPlanner (SaveUserToIFlightPlannerRequest body)
        {
            
    
            var path = "/api/User/iflightplanner/user";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SaveUserToIFlightPlanner: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SaveUserToIFlightPlanner: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SaveUserToIFlightPlannerResponse) ApiClient.Deserialize(response.Content, typeof(SaveUserToIFlightPlannerResponse), response.Headers);
        }
    
        /// <summary>
        /// Authenticates a user by username and password. The returned object contains a [AccessToken] (JWT) that can be used to identify the user for other API requests using the [Bearer] authorization header.  The response also contains a [RefreshToken] that can be used in the /api/user/refreshtoken api to receive a new [Token].
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UserAuthTokenResponse</returns>            
        public UserAuthTokenResponse UserAuthToken (UserAuthTokenFromCredentialsRequest body)
        {
            
    
            var path = "/api/User/token";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserAuthToken: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserAuthToken: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAuthTokenResponse) ApiClient.Deserialize(response.Content, typeof(UserAuthTokenResponse), response.Headers);
        }
    
        /// <summary>
        /// Authenticates a user from an existing [AccessToken]. The returned object contains a [AccessToken] (JWT) that can be used to identify the user for other API requests using the [Bearer] authorization header.  The response also contains a [RefreshToken] that can be used in the /api/user/refreshtoken api to receive a new [Token].
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UserAuthTokenResponse</returns>            
        public UserAuthTokenResponse UserAuthTokenFromAccessToken (UserAuthTokenFromAccessTokenRequest body)
        {
            
    
            var path = "/api/User/accesstoken";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserAuthTokenFromAccessToken: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserAuthTokenFromAccessToken: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAuthTokenResponse) ApiClient.Deserialize(response.Content, typeof(UserAuthTokenResponse), response.Headers);
        }
    
        /// <summary>
        /// Authenticate a user via the iFlightPlanner integration 
        /// </summary>
        /// <param name="guid"></param> 
        /// <returns>UserAuthTokenResponse</returns>            
        public UserAuthTokenResponse UserAuthTokenFromIFlightPlannerGUID (string guid)
        {
            
            // verify the required parameter 'guid' is set
            if (guid == null) throw new ApiException(400, "Missing required parameter 'guid' when calling UserAuthTokenFromIFlightPlannerGUID");
            
    
            var path = "/api/User/iflightplanner/user/{guid}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guid" + "}", ApiClient.ParameterToString(guid));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserAuthTokenFromIFlightPlannerGUID: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserAuthTokenFromIFlightPlannerGUID: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAuthTokenResponse) ApiClient.Deserialize(response.Content, typeof(UserAuthTokenResponse), response.Headers);
        }
    
    }
}

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
        /// Internal use only - Change the username/password for an account if it meets security requirements. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostChangeCredentialsResponse</returns>
        PostChangeCredentialsResponse ChangeCredentials (PostChangeCredentialsRequest body);
        /// <summary>
        /// Deletes company user profile based on Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteCompanyUserProfilesResponse</returns>
        DeleteCompanyUserProfilesResponse DeleteCompanyUserProfiles (int? id);
        /// <summary>
        /// Deletes user credentials based on Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteCredentialsResponse</returns>
        DeleteCredentialsResponse DeleteCredentials (int? id);
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
        /// Fetch the currently authenticated user. 
        /// </summary>
        /// <returns>CustomerDataDTO</returns>
        CustomerDataDTO GetAuthenticatedUser ();
        /// <summary>
        /// Fetches all user profiles by companyId 
        /// </summary>
        /// <returns>CompanyUserProfileListResponse</returns>
        CompanyUserProfileListResponse GetCompanyUserProfiles ();
        /// <summary>
        /// Fetches user credentials by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CredentialsResponse</returns>
        CredentialsResponse GetCredentials (int? id);
        /// <summary>
        /// Fetches all user credentials 
        /// </summary>
        /// <returns>CredentialsListResponse</returns>
        CredentialsListResponse GetCredentialsList ();
        /// <summary>
        /// Internal/Conductor use only - Fetch an auth token to impersonate a user for conductor user management. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserAuthTokenResponse</returns>
        UserAuthTokenResponse GetImpersonatedAuthTokenForUser (int? id);
        /// <summary>
        /// Fetch a user by their [id]. The authenticated user must have access to view this user&#39;s record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CustomerDataDTO</returns>
        CustomerDataDTO GetUser (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>UserResponse</returns>
        UserResponse GetUserByCredentials (string username, string password);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyUserProfilesResponse</returns>
        PostCompanyUserProfilesResponse PostCompanyUserProfiles (PostCompanyUserProfilesRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCredentialsResponse</returns>
        PostCredentialsResponse PostCredentials (PostCredentialsRequest body);
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
        /// Updates company user profile based on companyId 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyUserProfilesResponse</returns>
        UpdateCompanyUserProfilesResponse UpdateCompanyUserProfiles (UpdateCompanyUserProfilesRequest body);
        /// <summary>
        /// Updates user credentials based on Id 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCredentialsResponse</returns>
        UpdateCredentialsResponse UpdateCredentials (UpdateCredentialsRequest body);
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
        /// Internal use only - Change the username/password for an account if it meets security requirements. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostChangeCredentialsResponse</returns>            
        public PostChangeCredentialsResponse ChangeCredentials (PostChangeCredentialsRequest body)
        {
            
    
            var path = "/api/User/change-credentials";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ChangeCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ChangeCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostChangeCredentialsResponse) ApiClient.Deserialize(response.Content, typeof(PostChangeCredentialsResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes company user profile based on Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteCompanyUserProfilesResponse</returns>            
        public DeleteCompanyUserProfilesResponse DeleteCompanyUserProfiles (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteCompanyUserProfiles");
            
    
            var path = "/api/User/company-user-profiles/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyUserProfiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyUserProfiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyUserProfilesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyUserProfilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes user credentials based on Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteCredentialsResponse</returns>            
        public DeleteCredentialsResponse DeleteCredentials (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteCredentials");
            
    
            var path = "/api/User/credentials/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCredentialsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCredentialsResponse), response.Headers);
        }
    
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
        /// Fetch the currently authenticated user. 
        /// </summary>
        /// <returns>CustomerDataDTO</returns>            
        public CustomerDataDTO GetAuthenticatedUser ()
        {
            
    
            var path = "/api/User";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAuthenticatedUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAuthenticatedUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CustomerDataDTO) ApiClient.Deserialize(response.Content, typeof(CustomerDataDTO), response.Headers);
        }
    
        /// <summary>
        /// Fetches all user profiles by companyId 
        /// </summary>
        /// <returns>CompanyUserProfileListResponse</returns>            
        public CompanyUserProfileListResponse GetCompanyUserProfiles ()
        {
            
    
            var path = "/api/User/company-user-profiles/by-company/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyUserProfiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyUserProfiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyUserProfileListResponse) ApiClient.Deserialize(response.Content, typeof(CompanyUserProfileListResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetches user credentials by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>CredentialsResponse</returns>            
        public CredentialsResponse GetCredentials (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetCredentials");
            
    
            var path = "/api/User/credentials/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CredentialsResponse) ApiClient.Deserialize(response.Content, typeof(CredentialsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetches all user credentials 
        /// </summary>
        /// <returns>CredentialsListResponse</returns>            
        public CredentialsListResponse GetCredentialsList ()
        {
            
    
            var path = "/api/User/credentials/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCredentialsList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCredentialsList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CredentialsListResponse) ApiClient.Deserialize(response.Content, typeof(CredentialsListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal/Conductor use only - Fetch an auth token to impersonate a user for conductor user management. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>UserAuthTokenResponse</returns>            
        public UserAuthTokenResponse GetImpersonatedAuthTokenForUser (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetImpersonatedAuthTokenForUser");
            
    
            var path = "/api/User/impersonation/token-for-user/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetImpersonatedAuthTokenForUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetImpersonatedAuthTokenForUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAuthTokenResponse) ApiClient.Deserialize(response.Content, typeof(UserAuthTokenResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a user by their [id]. The authenticated user must have access to view this user&#39;s record.
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>CustomerDataDTO</returns>            
        public CustomerDataDTO GetUser (int? id)
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
    
            return (CustomerDataDTO) ApiClient.Deserialize(response.Content, typeof(CustomerDataDTO), response.Headers);
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
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyUserProfilesResponse</returns>            
        public PostCompanyUserProfilesResponse PostCompanyUserProfiles (PostCompanyUserProfilesRequest body)
        {
            
    
            var path = "/api/User/company-user-profiles";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyUserProfiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyUserProfiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyUserProfilesResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyUserProfilesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCredentialsResponse</returns>            
        public PostCredentialsResponse PostCredentials (PostCredentialsRequest body)
        {
            
    
            var path = "/api/User/credentials";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCredentialsResponse) ApiClient.Deserialize(response.Content, typeof(PostCredentialsResponse), response.Headers);
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
        /// Updates company user profile based on companyId 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyUserProfilesResponse</returns>            
        public UpdateCompanyUserProfilesResponse UpdateCompanyUserProfiles (UpdateCompanyUserProfilesRequest body)
        {
            
    
            var path = "/api/User/company-user-profiles";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyUserProfiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyUserProfiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyUserProfilesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyUserProfilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates user credentials based on Id 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCredentialsResponse</returns>            
        public UpdateCredentialsResponse UpdateCredentials (UpdateCredentialsRequest body)
        {
            
    
            var path = "/api/User/credentials";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCredentialsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCredentialsResponse), response.Headers);
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

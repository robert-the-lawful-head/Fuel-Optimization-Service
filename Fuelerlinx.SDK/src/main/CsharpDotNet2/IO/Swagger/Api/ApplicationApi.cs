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
    public interface IApplicationApi
    {
        /// <summary>
        /// Internal use only - Delete the specific deployment notes. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteDeploymentNotesResponse</returns>
        DeleteDeploymentNotesResponse DeleteDeploymentNotes (int? id);
        /// <summary>
        /// Returns the current build version of the API. 
        /// </summary>
        /// <returns>BuildVersionResponse</returns>
        BuildVersionResponse GetBuildVersion ();
        /// <summary>
        /// Internal use only - Returns a list of deployment notes to be used with a range of build versions on each application. 
        /// </summary>
        /// <returns>DeploymentNotesListResponse</returns>
        DeploymentNotesListResponse GetDeploymentNotesAll ();
        /// <summary>
        /// Internal use only - Fetch deployment notes for a provided {buildVersionNumber}. 
        /// </summary>
        /// <param name="buildVersionNumber"></param>
        /// <param name="applicationType"></param>
        /// <returns>DeploymentNotesResponse</returns>
        DeploymentNotesResponse GetDeploymentNotesByVersionNumber (int? buildVersionNumber, int? applicationType);
        /// <summary>
        /// Internal use only - Add a new deployment note for the provided build version range. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostDeploymentNotesResponse</returns>
        PostDeploymentNotesResponse PostDeploymentNotes (PostDeploymentNotesRequest body);
        /// <summary>
        /// Internal use only - Update an existing deployment note for a build version. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateDeploymentNotesResponse</returns>
        UpdateDeploymentNotesResponse UpdateDeploymentNotes (UpdateDeploymentNotesRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ApplicationApi : IApplicationApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ApplicationApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ApplicationApi(String basePath)
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
        /// Internal use only - Delete the specific deployment notes. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteDeploymentNotesResponse</returns>            
        public DeleteDeploymentNotesResponse DeleteDeploymentNotes (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteDeploymentNotes");
            
    
            var path = "/api/Application/deployment-notes/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteDeploymentNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteDeploymentNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteDeploymentNotesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteDeploymentNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Returns the current build version of the API. 
        /// </summary>
        /// <returns>BuildVersionResponse</returns>            
        public BuildVersionResponse GetBuildVersion ()
        {
            
    
            var path = "/api/Application/build-version";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetBuildVersion: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetBuildVersion: " + response.ErrorMessage, response.ErrorMessage);
    
            return (BuildVersionResponse) ApiClient.Deserialize(response.Content, typeof(BuildVersionResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Returns a list of deployment notes to be used with a range of build versions on each application. 
        /// </summary>
        /// <returns>DeploymentNotesListResponse</returns>            
        public DeploymentNotesListResponse GetDeploymentNotesAll ()
        {
            
    
            var path = "/api/Application/deployment-notes/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetDeploymentNotesAll: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetDeploymentNotesAll: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeploymentNotesListResponse) ApiClient.Deserialize(response.Content, typeof(DeploymentNotesListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch deployment notes for a provided {buildVersionNumber}. 
        /// </summary>
        /// <param name="buildVersionNumber"></param> 
        /// <param name="applicationType"></param> 
        /// <returns>DeploymentNotesResponse</returns>            
        public DeploymentNotesResponse GetDeploymentNotesByVersionNumber (int? buildVersionNumber, int? applicationType)
        {
            
            // verify the required parameter 'buildVersionNumber' is set
            if (buildVersionNumber == null) throw new ApiException(400, "Missing required parameter 'buildVersionNumber' when calling GetDeploymentNotesByVersionNumber");
            
            // verify the required parameter 'applicationType' is set
            if (applicationType == null) throw new ApiException(400, "Missing required parameter 'applicationType' when calling GetDeploymentNotesByVersionNumber");
            
    
            var path = "/api/Application/deployment-notes/by-version-number/{buildVersionNumber}/application-type/{applicationType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "buildVersionNumber" + "}", ApiClient.ParameterToString(buildVersionNumber));
path = path.Replace("{" + "applicationType" + "}", ApiClient.ParameterToString(applicationType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetDeploymentNotesByVersionNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetDeploymentNotesByVersionNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeploymentNotesResponse) ApiClient.Deserialize(response.Content, typeof(DeploymentNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a new deployment note for the provided build version range. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostDeploymentNotesResponse</returns>            
        public PostDeploymentNotesResponse PostDeploymentNotes (PostDeploymentNotesRequest body)
        {
            
    
            var path = "/api/Application/deployment-notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostDeploymentNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostDeploymentNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostDeploymentNotesResponse) ApiClient.Deserialize(response.Content, typeof(PostDeploymentNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update an existing deployment note for a build version. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateDeploymentNotesResponse</returns>            
        public UpdateDeploymentNotesResponse UpdateDeploymentNotes (UpdateDeploymentNotesRequest body)
        {
            
    
            var path = "/api/Application/deployment-notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateDeploymentNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateDeploymentNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateDeploymentNotesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateDeploymentNotesResponse), response.Headers);
        }
    
    }
}

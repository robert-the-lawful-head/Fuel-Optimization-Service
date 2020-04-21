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
    public interface IFBOApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteFboAliasResponse</returns>
        DeleteFboAliasResponse DeleteFboAlias (int? id);
        /// <summary>
        /// Delete company-specific details for a particular FBO. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param>
        /// <returns>DeleteFboDetailsByCompanyResponse</returns>
        DeleteFboDetailsByCompanyResponse DeleteFboDetailsByCompany (int? fboDetailsByCompanyId);
        /// <summary>
        /// Delete a company-specific note for a particular FBO.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param>
        /// <param name="noteId"></param>
        /// <returns>DeleteFboDetailsByCompanyNotesResponse</returns>
        DeleteFboDetailsByCompanyNotesResponse DeleteFboDetailsByCompanyNotes (int? fboDetailsByCompanyId, int? noteId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="acukwikId"></param>
        /// <returns>FBOLinxFBOResponse</returns>
        FBOLinxFBOResponse GetFBOLinxFboByAcukwikId (int? acukwikId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="icao"></param>
        /// <returns>FboAliasResponse</returns>
        FboAliasResponse GetFboAlias (string icao);
        /// <summary>
        /// Fetch company-specific details for a particular FBO based on the provided [fboDetailsByCompanyId]. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param>
        /// <returns>FboDetailsByCompanyResponse</returns>
        FboDetailsByCompanyResponse GetFboDetailsByCompany (int? fboDetailsByCompanyId);
        /// <summary>
        /// Fetch company-specific details for a particular FBO based on the provided [icao] and [fboName]. 
        /// </summary>
        /// <param name="icao"></param>
        /// <param name="fboName"></param>
        /// <returns>FboDetailsByCompanyResponse</returns>
        FboDetailsByCompanyResponse GetFboDetailsByCompanyByLocation (string icao, string fboName);
        /// <summary>
        /// Fetch company-specific notes for a particular FBO based on the provided [fboDetailsByCompanyId]. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param>
        /// <returns>FboDetailsByCompanyNotesResponse</returns>
        FboDetailsByCompanyNotesResponse GetFboDetailsByCompanyNotes (int? fboDetailsByCompanyId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostFboAliasResponse</returns>
        PostFboAliasResponse PostFboAliasAsync (PostFboAliasRequest body);
        /// <summary>
        /// Add company-specific details for a particular FBO.  If a record already exists for this FBO, the previous record will be replaced. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostFboDetailsByCompanyResponse</returns>
        PostFboDetailsByCompanyResponse PostFboDetailsByCompany (PostFboDetailsByCompanyRequest body);
        /// <summary>
        /// Add company-specific notes for a particular FBO.  The note must be associated with a company-specific FBO record. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostFboDetailsByCompanyNotesResponse</returns>
        PostFboDetailsByCompanyNotesResponse PostFboDetailsByCompanyNotes (PostFboDetailsByCompanyNotesRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateFboAliasResponse</returns>
        UpdateFboAliasResponse UpdateFboAlias (UpdateFboAliasRequest body);
        /// <summary>
        /// Update company-specific details for a particular FBO. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateFboDetailsByCompanyResponse</returns>
        UpdateFboDetailsByCompanyResponse UpdateFboDetailsByCompany (UpdateFboDetailsByCompanyRequest body);
        /// <summary>
        /// Update a company-specific note for a particular FBO. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateFboDetailsByCompanyNotesResponse</returns>
        UpdateFboDetailsByCompanyNotesResponse UpdateFboDetailsByCompanyNotes (UpdateFboDetailsByCompanyNotesRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FBOApi : IFBOApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FBOApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FBOApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FBOApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FBOApi(String basePath)
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
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteFboAliasResponse</returns>            
        public DeleteFboAliasResponse DeleteFboAlias (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteFboAlias");
            
    
            var path = "/api/FBO/fboAlias/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFboAlias: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFboAlias: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteFboAliasResponse) ApiClient.Deserialize(response.Content, typeof(DeleteFboAliasResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete company-specific details for a particular FBO. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param> 
        /// <returns>DeleteFboDetailsByCompanyResponse</returns>            
        public DeleteFboDetailsByCompanyResponse DeleteFboDetailsByCompany (int? fboDetailsByCompanyId)
        {
            
            // verify the required parameter 'fboDetailsByCompanyId' is set
            if (fboDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'fboDetailsByCompanyId' when calling DeleteFboDetailsByCompany");
            
    
            var path = "/api/FBO/company-specific-details/{fboDetailsByCompanyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fboDetailsByCompanyId" + "}", ApiClient.ParameterToString(fboDetailsByCompanyId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFboDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFboDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteFboDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(DeleteFboDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a company-specific note for a particular FBO.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param> 
        /// <param name="noteId"></param> 
        /// <returns>DeleteFboDetailsByCompanyNotesResponse</returns>            
        public DeleteFboDetailsByCompanyNotesResponse DeleteFboDetailsByCompanyNotes (int? fboDetailsByCompanyId, int? noteId)
        {
            
            // verify the required parameter 'fboDetailsByCompanyId' is set
            if (fboDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'fboDetailsByCompanyId' when calling DeleteFboDetailsByCompanyNotes");
            
            // verify the required parameter 'noteId' is set
            if (noteId == null) throw new ApiException(400, "Missing required parameter 'noteId' when calling DeleteFboDetailsByCompanyNotes");
            
    
            var path = "/api/FBO/company-specific-details/{fboDetailsByCompanyId}/notes/{noteId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fboDetailsByCompanyId" + "}", ApiClient.ParameterToString(fboDetailsByCompanyId));
path = path.Replace("{" + "noteId" + "}", ApiClient.ParameterToString(noteId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFboDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFboDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteFboDetailsByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteFboDetailsByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="acukwikId"></param> 
        /// <returns>FBOLinxFBOResponse</returns>            
        public FBOLinxFBOResponse GetFBOLinxFboByAcukwikId (int? acukwikId)
        {
            
            // verify the required parameter 'acukwikId' is set
            if (acukwikId == null) throw new ApiException(400, "Missing required parameter 'acukwikId' when calling GetFBOLinxFboByAcukwikId");
            
    
            var path = "/api/FBO/fbolinx/fbo-by-acukwik-id/{acukwikId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "acukwikId" + "}", ApiClient.ParameterToString(acukwikId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFBOLinxFboByAcukwikId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFBOLinxFboByAcukwikId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FBOLinxFBOResponse) ApiClient.Deserialize(response.Content, typeof(FBOLinxFBOResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="icao"></param> 
        /// <returns>FboAliasResponse</returns>            
        public FboAliasResponse GetFboAlias (string icao)
        {
            
            // verify the required parameter 'icao' is set
            if (icao == null) throw new ApiException(400, "Missing required parameter 'icao' when calling GetFboAlias");
            
    
            var path = "/api/FBO/fboAlias/by-airport/{icao}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "icao" + "}", ApiClient.ParameterToString(icao));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboAlias: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboAlias: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboAliasResponse) ApiClient.Deserialize(response.Content, typeof(FboAliasResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company-specific details for a particular FBO based on the provided [fboDetailsByCompanyId]. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param> 
        /// <returns>FboDetailsByCompanyResponse</returns>            
        public FboDetailsByCompanyResponse GetFboDetailsByCompany (int? fboDetailsByCompanyId)
        {
            
            // verify the required parameter 'fboDetailsByCompanyId' is set
            if (fboDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'fboDetailsByCompanyId' when calling GetFboDetailsByCompany");
            
    
            var path = "/api/FBO/company-specific-details/{fboDetailsByCompanyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fboDetailsByCompanyId" + "}", ApiClient.ParameterToString(fboDetailsByCompanyId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(FboDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company-specific details for a particular FBO based on the provided [icao] and [fboName]. 
        /// </summary>
        /// <param name="icao"></param> 
        /// <param name="fboName"></param> 
        /// <returns>FboDetailsByCompanyResponse</returns>            
        public FboDetailsByCompanyResponse GetFboDetailsByCompanyByLocation (string icao, string fboName)
        {
            
            // verify the required parameter 'icao' is set
            if (icao == null) throw new ApiException(400, "Missing required parameter 'icao' when calling GetFboDetailsByCompanyByLocation");
            
            // verify the required parameter 'fboName' is set
            if (fboName == null) throw new ApiException(400, "Missing required parameter 'fboName' when calling GetFboDetailsByCompanyByLocation");
            
    
            var path = "/api/FBO/company-specific-details/by-location/{icao}/{fboName}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboDetailsByCompanyByLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboDetailsByCompanyByLocation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(FboDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company-specific notes for a particular FBO based on the provided [fboDetailsByCompanyId]. 
        /// </summary>
        /// <param name="fboDetailsByCompanyId"></param> 
        /// <returns>FboDetailsByCompanyNotesResponse</returns>            
        public FboDetailsByCompanyNotesResponse GetFboDetailsByCompanyNotes (int? fboDetailsByCompanyId)
        {
            
            // verify the required parameter 'fboDetailsByCompanyId' is set
            if (fboDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'fboDetailsByCompanyId' when calling GetFboDetailsByCompanyNotes");
            
    
            var path = "/api/FBO/company-specific-details/{fboDetailsByCompanyId}/notes";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fboDetailsByCompanyId" + "}", ApiClient.ParameterToString(fboDetailsByCompanyId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFboDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FboDetailsByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(FboDetailsByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostFboAliasResponse</returns>            
        public PostFboAliasResponse PostFboAliasAsync (PostFboAliasRequest body)
        {
            
    
            var path = "/api/FBO/fboAlias";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostFboAliasAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostFboAliasAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostFboAliasResponse) ApiClient.Deserialize(response.Content, typeof(PostFboAliasResponse), response.Headers);
        }
    
        /// <summary>
        /// Add company-specific details for a particular FBO.  If a record already exists for this FBO, the previous record will be replaced. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostFboDetailsByCompanyResponse</returns>            
        public PostFboDetailsByCompanyResponse PostFboDetailsByCompany (PostFboDetailsByCompanyRequest body)
        {
            
    
            var path = "/api/FBO/company-specific-details";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostFboDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostFboDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostFboDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(PostFboDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Add company-specific notes for a particular FBO.  The note must be associated with a company-specific FBO record. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostFboDetailsByCompanyNotesResponse</returns>            
        public PostFboDetailsByCompanyNotesResponse PostFboDetailsByCompanyNotes (PostFboDetailsByCompanyNotesRequest body)
        {
            
    
            var path = "/api/FBO/company-specific-details/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostFboDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostFboDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostFboDetailsByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(PostFboDetailsByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateFboAliasResponse</returns>            
        public UpdateFboAliasResponse UpdateFboAlias (UpdateFboAliasRequest body)
        {
            
    
            var path = "/api/FBO/fboAlias";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFboAlias: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFboAlias: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateFboAliasResponse) ApiClient.Deserialize(response.Content, typeof(UpdateFboAliasResponse), response.Headers);
        }
    
        /// <summary>
        /// Update company-specific details for a particular FBO. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateFboDetailsByCompanyResponse</returns>            
        public UpdateFboDetailsByCompanyResponse UpdateFboDetailsByCompany (UpdateFboDetailsByCompanyRequest body)
        {
            
    
            var path = "/api/FBO/company-specific-details";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFboDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFboDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateFboDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(UpdateFboDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a company-specific note for a particular FBO. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateFboDetailsByCompanyNotesResponse</returns>            
        public UpdateFboDetailsByCompanyNotesResponse UpdateFboDetailsByCompanyNotes (UpdateFboDetailsByCompanyNotesRequest body)
        {
            
    
            var path = "/api/FBO/company-specific-details/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFboDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFboDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateFboDetailsByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateFboDetailsByCompanyNotesResponse), response.Headers);
        }
    
    }
}

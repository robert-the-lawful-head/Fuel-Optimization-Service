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
    public interface IRampFeeApi
    {
        /// <summary>
        /// Add a company-specific ramp fee. If the ramp fee already exists, please use the [PUT] http method.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>RampFeeByCompanyResponse</returns>
        RampFeeByCompanyResponse AddRampFeeByCompany (PostRampFeeByCompanyRequest body);
        /// <summary>
        /// Delete a company-specific ramp fee. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteRampFeeByCompanyResponse</returns>
        DeleteRampFeeByCompanyResponse DeleteRampFeeByCompany (int? id);
        /// <summary>
        /// Delete a company-specific note for a ramp fee.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="noteId"></param>
        /// <returns>DeleteRampFeeByCompanyNotesResponse</returns>
        DeleteRampFeeByCompanyNotesResponse DeleteRampFeeByCompanyNote (int? id, int? noteId);
        /// <summary>
        /// Fetch a crowd-sourced ramp fee pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName]. 
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <param name="icao"></param>
        /// <param name="fboName"></param>
        /// <returns>CrowdSourcedRampFeeResponse</returns>
        CrowdSourcedRampFeeResponse GetCrowdSourcedRampFeeByScenario (string tailNumber, string icao, string fboName);
        /// <summary>
        /// Get a company-specific ramp fee by it&#39;s [id]. The request will fail if the authorized user is not part of the company that the record is attached to.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RampFeeByCompanyResponse</returns>
        RampFeeByCompanyResponse GetRampFeeByCompany (int? id);
        /// <summary>
        /// Fetch a company-specific ramp fee based on the provided {tailNumber}, {airportIdentifier}, and {fbo}. 
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <param name="icao"></param>
        /// <param name="fboName"></param>
        /// <returns>RampFeeByCompanyResponse</returns>
        RampFeeByCompanyResponse GetRampFeeByCompanyByScenario (string tailNumber, string icao, string fboName);
        /// <summary>
        /// Fetch all company-specific notes for the specified [rampFeeByCompanyId]. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RampFeeByCompanyNotesResponse</returns>
        RampFeeByCompanyNotesResponse GetRampFeeByCompanyNotes (int? id);
        /// <summary>
        /// Get a list of company-specific ramp fees at the the provided [icao]. The returned ramp fees will be for all FBOs at that [icao].  Ramp fees will be categorized by size, weight range, tail, etc.
        /// </summary>
        /// <param name="icao"></param>
        /// <returns>RampFeeByCompanyListResponse</returns>
        RampFeeByCompanyListResponse GetRampFeesByCompanyForAirport (string icao);
        /// <summary>
        /// Get a list of company-specific ramp fees at the the provided [icao] and [fboName]. The returned ramp fees will be for the specific [fboName] at that [icao].  Ramp fees will be categorized by size, weight range, tail, etc.
        /// </summary>
        /// <param name="icao"></param>
        /// <param name="fboName"></param>
        /// <returns>RampFeeByCompanyListResponse</returns>
        RampFeeByCompanyListResponse GetRampFeesByCompanyForLocation (string icao, string fboName);
        /// <summary>
        /// Add a company-specific note to a ramp fee. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostRampFeeByCompanyNotesResponse</returns>
        PostRampFeeByCompanyNotesResponse PostRampFeeByCompanyNotes (PostRampFeeByCompanyNotesRequest body);
        /// <summary>
        /// Update a company-specific ramp fee. If this is a new ramp fee, please use the [POST] http method.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateRampFeeByCompanyResponse</returns>
        UpdateRampFeeByCompanyResponse UpdateRampFeeByCompany (UpdateRampFeeByCompanyRequest body);
        /// <summary>
        /// Update a company-specific note for a ramp fee. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateRampFeeByCompanyNotesResponse</returns>
        UpdateRampFeeByCompanyNotesResponse UpdateRampFeeByCompanyNotes (UpdateRampFeeByCompanyNotesRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class RampFeeApi : IRampFeeApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RampFeeApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public RampFeeApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="RampFeeApi"/> class.
        /// </summary>
        /// <returns></returns>
        public RampFeeApi(String basePath)
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
        /// Add a company-specific ramp fee. If the ramp fee already exists, please use the [PUT] http method.
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>RampFeeByCompanyResponse</returns>            
        public RampFeeByCompanyResponse AddRampFeeByCompany (PostRampFeeByCompanyRequest body)
        {
            
    
            var path = "/api/RampFee/company-specific";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AddRampFeeByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddRampFeeByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RampFeeByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(RampFeeByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a company-specific ramp fee. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteRampFeeByCompanyResponse</returns>            
        public DeleteRampFeeByCompanyResponse DeleteRampFeeByCompany (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteRampFeeByCompany");
            
    
            var path = "/api/RampFee/company-specific/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteRampFeeByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteRampFeeByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteRampFeeByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(DeleteRampFeeByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a company-specific note for a ramp fee.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking. 
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="noteId"></param> 
        /// <returns>DeleteRampFeeByCompanyNotesResponse</returns>            
        public DeleteRampFeeByCompanyNotesResponse DeleteRampFeeByCompanyNote (int? id, int? noteId)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteRampFeeByCompanyNote");
            
            // verify the required parameter 'noteId' is set
            if (noteId == null) throw new ApiException(400, "Missing required parameter 'noteId' when calling DeleteRampFeeByCompanyNote");
            
    
            var path = "/api/RampFee/company-specific/{id}/notes/{noteId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteRampFeeByCompanyNote: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteRampFeeByCompanyNote: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteRampFeeByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteRampFeeByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a crowd-sourced ramp fee pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName]. 
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <param name="icao"></param> 
        /// <param name="fboName"></param> 
        /// <returns>CrowdSourcedRampFeeResponse</returns>            
        public CrowdSourcedRampFeeResponse GetCrowdSourcedRampFeeByScenario (string tailNumber, string icao, string fboName)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetCrowdSourcedRampFeeByScenario");
            
            // verify the required parameter 'icao' is set
            if (icao == null) throw new ApiException(400, "Missing required parameter 'icao' when calling GetCrowdSourcedRampFeeByScenario");
            
            // verify the required parameter 'fboName' is set
            if (fboName == null) throw new ApiException(400, "Missing required parameter 'fboName' when calling GetCrowdSourcedRampFeeByScenario");
            
    
            var path = "/api/RampFee/crowd-sourced/tail/{tailNumber}/airport/{icao}/fbo/{fboName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCrowdSourcedRampFeeByScenario: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCrowdSourcedRampFeeByScenario: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CrowdSourcedRampFeeResponse) ApiClient.Deserialize(response.Content, typeof(CrowdSourcedRampFeeResponse), response.Headers);
        }
    
        /// <summary>
        /// Get a company-specific ramp fee by it&#39;s [id]. The request will fail if the authorized user is not part of the company that the record is attached to.
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>RampFeeByCompanyResponse</returns>            
        public RampFeeByCompanyResponse GetRampFeeByCompany (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetRampFeeByCompany");
            
    
            var path = "/api/RampFee/company-specific/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeeByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeeByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RampFeeByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(RampFeeByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a company-specific ramp fee based on the provided {tailNumber}, {airportIdentifier}, and {fbo}. 
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <param name="icao"></param> 
        /// <param name="fboName"></param> 
        /// <returns>RampFeeByCompanyResponse</returns>            
        public RampFeeByCompanyResponse GetRampFeeByCompanyByScenario (string tailNumber, string icao, string fboName)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetRampFeeByCompanyByScenario");
            
            // verify the required parameter 'icao' is set
            if (icao == null) throw new ApiException(400, "Missing required parameter 'icao' when calling GetRampFeeByCompanyByScenario");
            
            // verify the required parameter 'fboName' is set
            if (fboName == null) throw new ApiException(400, "Missing required parameter 'fboName' when calling GetRampFeeByCompanyByScenario");
            
    
            var path = "/api/RampFee/company-specific/tail/{tailNumber}/airport/{icao}/fbo/{fboName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeeByCompanyByScenario: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeeByCompanyByScenario: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RampFeeByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(RampFeeByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all company-specific notes for the specified [rampFeeByCompanyId]. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>RampFeeByCompanyNotesResponse</returns>            
        public RampFeeByCompanyNotesResponse GetRampFeeByCompanyNotes (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetRampFeeByCompanyNotes");
            
    
            var path = "/api/RampFee/company-specific/{id}/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeeByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeeByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RampFeeByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(RampFeeByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Get a list of company-specific ramp fees at the the provided [icao]. The returned ramp fees will be for all FBOs at that [icao].  Ramp fees will be categorized by size, weight range, tail, etc.
        /// </summary>
        /// <param name="icao"></param> 
        /// <returns>RampFeeByCompanyListResponse</returns>            
        public RampFeeByCompanyListResponse GetRampFeesByCompanyForAirport (string icao)
        {
            
            // verify the required parameter 'icao' is set
            if (icao == null) throw new ApiException(400, "Missing required parameter 'icao' when calling GetRampFeesByCompanyForAirport");
            
    
            var path = "/api/RampFee/company-specific/by-airport/{icao}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeesByCompanyForAirport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeesByCompanyForAirport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RampFeeByCompanyListResponse) ApiClient.Deserialize(response.Content, typeof(RampFeeByCompanyListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get a list of company-specific ramp fees at the the provided [icao] and [fboName]. The returned ramp fees will be for the specific [fboName] at that [icao].  Ramp fees will be categorized by size, weight range, tail, etc.
        /// </summary>
        /// <param name="icao"></param> 
        /// <param name="fboName"></param> 
        /// <returns>RampFeeByCompanyListResponse</returns>            
        public RampFeeByCompanyListResponse GetRampFeesByCompanyForLocation (string icao, string fboName)
        {
            
            // verify the required parameter 'icao' is set
            if (icao == null) throw new ApiException(400, "Missing required parameter 'icao' when calling GetRampFeesByCompanyForLocation");
            
            // verify the required parameter 'fboName' is set
            if (fboName == null) throw new ApiException(400, "Missing required parameter 'fboName' when calling GetRampFeesByCompanyForLocation");
            
    
            var path = "/api/RampFee/company-specific/by-location/{icao}/{fboName}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeesByCompanyForLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetRampFeesByCompanyForLocation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RampFeeByCompanyListResponse) ApiClient.Deserialize(response.Content, typeof(RampFeeByCompanyListResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a company-specific note to a ramp fee. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostRampFeeByCompanyNotesResponse</returns>            
        public PostRampFeeByCompanyNotesResponse PostRampFeeByCompanyNotes (PostRampFeeByCompanyNotesRequest body)
        {
            
    
            var path = "/api/RampFee/company-specific/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostRampFeeByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostRampFeeByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostRampFeeByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(PostRampFeeByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a company-specific ramp fee. If this is a new ramp fee, please use the [POST] http method.
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateRampFeeByCompanyResponse</returns>            
        public UpdateRampFeeByCompanyResponse UpdateRampFeeByCompany (UpdateRampFeeByCompanyRequest body)
        {
            
    
            var path = "/api/RampFee/company-specific";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateRampFeeByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateRampFeeByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateRampFeeByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(UpdateRampFeeByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a company-specific note for a ramp fee. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateRampFeeByCompanyNotesResponse</returns>            
        public UpdateRampFeeByCompanyNotesResponse UpdateRampFeeByCompanyNotes (UpdateRampFeeByCompanyNotesRequest body)
        {
            
    
            var path = "/api/RampFee/company-specific/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateRampFeeByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateRampFeeByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateRampFeeByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateRampFeeByCompanyNotesResponse), response.Headers);
        }
    
    }
}

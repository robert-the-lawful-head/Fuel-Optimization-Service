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
    public interface IAircraftApi
    {
        /// <summary>
        /// Add an aircraft to the authorized company. An AircraftData record will be inserted and then assigned to the authorized company.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UserAircraftResponse</returns>
        UserAircraftResponse AddAircraftForCompany (AircraftDataDTO body);
        /// <summary>
        /// Add an existing aircraft to the user&#39;s account. The [tailNumberId] is required and should be created by posting to the /tail API beforehand.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UserAircraftResponse</returns>
        UserAircraftResponse AddAircraftForUser (UserAircraftDTO body);
        /// <summary>
        /// Add a new aircraft with a corresponding tail number to be used by a specific company/user. A tail number is required.  The tail number does not have to be unique.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>AircraftDataResponse</returns>
        AircraftDataResponse AddTail (AircraftDataDTO body);
        /// <summary>
        /// Delete tankering settings for an aircraft. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteUserAircraftTankeringSettingsResponse</returns>
        DeleteUserAircraftTankeringSettingsResponse DeleteUsereAircraftTankeringSettings (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="aircraftId"></param>
        /// <returns>UserAircraftListResponse</returns>
        UserAircraftListResponse GetAircraftByAircraftId (int? aircraftId);
        /// <summary>
        /// Get an aircraft by [companyId] and [tailNumber]. The request will fail if the authorized user is not part of the company that the record is attached to.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="tailNumber"></param>
        /// <returns>AircraftDataResponse</returns>
        AircraftDataResponse GetAircraftByTailNumber (int? companyId, string tailNumber);
        /// <summary>
        /// Get an aircraft by the provided [tailNumberId]. The request will fail if the authorized user is not part of the company that the record is attached to.
        /// </summary>
        /// <param name="tailNumberId"></param>
        /// <returns>AircraftDataResponse</returns>
        AircraftDataResponse GetAircraftByTailNumberId (int? tailNumberId);
        /// <summary>
        /// Get all aircraft assigned to the authorized company. 
        /// </summary>
        /// <returns>AircraftDataListResponse</returns>
        AircraftDataListResponse GetAircraftDataForCompany ();
        /// <summary>
        /// Internal use only - Get all aircraft assigned to the specified {companyId}. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>AircraftDataListResponse</returns>
        AircraftDataListResponse GetAircraftDataForCompanyByCompanyId (int? companyId);
        /// <summary>
        /// Get all aircraft assigned to the authorized user. Each record contains an [AircraftData] property where the actual aircraft information can be found.
        /// </summary>
        /// <returns>UserAircraftListResponse</returns>
        UserAircraftListResponse GetAircraftForUser ();
        /// <summary>
        /// Internal use only - Fetch details for an aircraft from the AMSTAT database. 
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <returns>AircraftResponse</returns>
        AircraftResponse GetAmstatDataByTailNumber (string tailNumber);
        /// <summary>
        /// Internal use only - Get the iFlightPlanner aircraft Id for a specific tail number. 
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <returns>IFlightPlannerAircraftIdResponse</returns>
        IFlightPlannerAircraftIdResponse GetIFlightPlannerAircraftId (string tailNumber);
        /// <summary>
        /// Internal use only - Get a specific iFlightPlanner model by it&#39;s Id. 
        /// </summary>
        /// <param name="aircraftModelId"></param>
        /// <returns>IFLightPlannerModelResponse</returns>
        IFLightPlannerModelResponse GetIFlightPlannerModel (int? aircraftModelId);
        /// <summary>
        /// Internal use only - Get a list of iFlightPlanner aircraft models by the aircraft type (ICAO) code. 
        /// </summary>
        /// <param name="aircraftType"></param>
        /// <returns>IFlightPlannerModelsByTypeResponse</returns>
        IFlightPlannerModelsByTypeResponse GetIFlightPlannerModelsByType (string aircraftType);
        /// <summary>
        /// Internal use only - Fetch an aircraft profile from iFlightPlanner. 
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <returns>IFlightPlannerAircraftProfileResponse</returns>
        IFlightPlannerAircraftProfileResponse GetIFlightPlannerProfileByTailNumber (string tailNumber);
        /// <summary>
        /// Add tankering settings for an aircraft. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostUserAircraftTankeringSettingsResponse</returns>
        PostUserAircraftTankeringSettingsResponse PostUserAircraftTankeringSettings (PostUserAircraftTankeringSettingsRequest body);
        /// <summary>
        /// Internal use only - Register an aircraft with iFlightPlanner. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>IFlightPlannerAircraftRegistrationResponse</returns>
        IFlightPlannerAircraftRegistrationResponse RegisterAircraftWithIFlightPlanner (IFlightPlannerAircraftRegistrationRequest body);
        /// <summary>
        /// Update an existing aircraft with the corresponding [tailNumberId]. A tail number is required.  The tail number does not have to be unique.
        /// </summary>
        /// <param name="tailNumberId"></param>
        /// <param name="body"></param>
        /// <returns>AircraftDataResponse</returns>
        AircraftDataResponse UpdateTail (int? tailNumberId, AircraftDataDTO body);
        /// <summary>
        /// Update tankering settings for an aircraft. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateUserAircraftTankeringSettingsResponse</returns>
        UpdateUserAircraftTankeringSettingsResponse UpdateUserAircraftTankeringSettings (UpdateUserAircraftTankeringSettingsRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class AircraftApi : IAircraftApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public AircraftApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftApi"/> class.
        /// </summary>
        /// <returns></returns>
        public AircraftApi(String basePath)
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
        /// Add an aircraft to the authorized company. An AircraftData record will be inserted and then assigned to the authorized company.
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UserAircraftResponse</returns>            
        public UserAircraftResponse AddAircraftForCompany (AircraftDataDTO body)
        {
            
    
            var path = "/api/Aircraft/company";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AddAircraftForCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddAircraftForCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAircraftResponse) ApiClient.Deserialize(response.Content, typeof(UserAircraftResponse), response.Headers);
        }
    
        /// <summary>
        /// Add an existing aircraft to the user&#39;s account. The [tailNumberId] is required and should be created by posting to the /tail API beforehand.
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UserAircraftResponse</returns>            
        public UserAircraftResponse AddAircraftForUser (UserAircraftDTO body)
        {
            
    
            var path = "/api/Aircraft/user";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AddAircraftForUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddAircraftForUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAircraftResponse) ApiClient.Deserialize(response.Content, typeof(UserAircraftResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new aircraft with a corresponding tail number to be used by a specific company/user. A tail number is required.  The tail number does not have to be unique.
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>AircraftDataResponse</returns>            
        public AircraftDataResponse AddTail (AircraftDataDTO body)
        {
            
    
            var path = "/api/Aircraft/tail";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AddTail: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddTail: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AircraftDataResponse) ApiClient.Deserialize(response.Content, typeof(AircraftDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete tankering settings for an aircraft. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteUserAircraftTankeringSettingsResponse</returns>            
        public DeleteUserAircraftTankeringSettingsResponse DeleteUsereAircraftTankeringSettings (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteUsereAircraftTankeringSettings");
            
    
            var path = "/api/Aircraft/tankering-settings/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteUsereAircraftTankeringSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteUsereAircraftTankeringSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteUserAircraftTankeringSettingsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteUserAircraftTankeringSettingsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="aircraftId"></param> 
        /// <returns>UserAircraftListResponse</returns>            
        public UserAircraftListResponse GetAircraftByAircraftId (int? aircraftId)
        {
            
            // verify the required parameter 'aircraftId' is set
            if (aircraftId == null) throw new ApiException(400, "Missing required parameter 'aircraftId' when calling GetAircraftByAircraftId");
            
    
            var path = "/api/Aircraft/aircraftId/{aircraftId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "aircraftId" + "}", ApiClient.ParameterToString(aircraftId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftByAircraftId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftByAircraftId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAircraftListResponse) ApiClient.Deserialize(response.Content, typeof(UserAircraftListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get an aircraft by [companyId] and [tailNumber]. The request will fail if the authorized user is not part of the company that the record is attached to.
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="tailNumber"></param> 
        /// <returns>AircraftDataResponse</returns>            
        public AircraftDataResponse GetAircraftByTailNumber (int? companyId, string tailNumber)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetAircraftByTailNumber");
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetAircraftByTailNumber");
            
    
            var path = "/api/Aircraft/company/{companyId}/tail/{tailNumber}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftByTailNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftByTailNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AircraftDataResponse) ApiClient.Deserialize(response.Content, typeof(AircraftDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Get an aircraft by the provided [tailNumberId]. The request will fail if the authorized user is not part of the company that the record is attached to.
        /// </summary>
        /// <param name="tailNumberId"></param> 
        /// <returns>AircraftDataResponse</returns>            
        public AircraftDataResponse GetAircraftByTailNumberId (int? tailNumberId)
        {
            
            // verify the required parameter 'tailNumberId' is set
            if (tailNumberId == null) throw new ApiException(400, "Missing required parameter 'tailNumberId' when calling GetAircraftByTailNumberId");
            
    
            var path = "/api/Aircraft/tail/{tailNumberId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tailNumberId" + "}", ApiClient.ParameterToString(tailNumberId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftByTailNumberId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftByTailNumberId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AircraftDataResponse) ApiClient.Deserialize(response.Content, typeof(AircraftDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Get all aircraft assigned to the authorized company. 
        /// </summary>
        /// <returns>AircraftDataListResponse</returns>            
        public AircraftDataListResponse GetAircraftDataForCompany ()
        {
            
    
            var path = "/api/Aircraft/company";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftDataForCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftDataForCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AircraftDataListResponse) ApiClient.Deserialize(response.Content, typeof(AircraftDataListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Get all aircraft assigned to the specified {companyId}. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>AircraftDataListResponse</returns>            
        public AircraftDataListResponse GetAircraftDataForCompanyByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetAircraftDataForCompanyByCompanyId");
            
    
            var path = "/api/Aircraft/company/{companyId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftDataForCompanyByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftDataForCompanyByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AircraftDataListResponse) ApiClient.Deserialize(response.Content, typeof(AircraftDataListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get all aircraft assigned to the authorized user. Each record contains an [AircraftData] property where the actual aircraft information can be found.
        /// </summary>
        /// <returns>UserAircraftListResponse</returns>            
        public UserAircraftListResponse GetAircraftForUser ()
        {
            
    
            var path = "/api/Aircraft/user";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftForUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAircraftForUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserAircraftListResponse) ApiClient.Deserialize(response.Content, typeof(UserAircraftListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch details for an aircraft from the AMSTAT database. 
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <returns>AircraftResponse</returns>            
        public AircraftResponse GetAmstatDataByTailNumber (string tailNumber)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetAmstatDataByTailNumber");
            
    
            var path = "/api/Aircraft/amstat/tail/{tailNumber}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAmstatDataByTailNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAmstatDataByTailNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AircraftResponse) ApiClient.Deserialize(response.Content, typeof(AircraftResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Get the iFlightPlanner aircraft Id for a specific tail number. 
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <returns>IFlightPlannerAircraftIdResponse</returns>            
        public IFlightPlannerAircraftIdResponse GetIFlightPlannerAircraftId (string tailNumber)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetIFlightPlannerAircraftId");
            
    
            var path = "/api/Aircraft/iflightplanner/aircraftid/{tailNumber}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerAircraftId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerAircraftId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFlightPlannerAircraftIdResponse) ApiClient.Deserialize(response.Content, typeof(IFlightPlannerAircraftIdResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Get a specific iFlightPlanner model by it&#39;s Id. 
        /// </summary>
        /// <param name="aircraftModelId"></param> 
        /// <returns>IFLightPlannerModelResponse</returns>            
        public IFLightPlannerModelResponse GetIFlightPlannerModel (int? aircraftModelId)
        {
            
            // verify the required parameter 'aircraftModelId' is set
            if (aircraftModelId == null) throw new ApiException(400, "Missing required parameter 'aircraftModelId' when calling GetIFlightPlannerModel");
            
    
            var path = "/api/Aircraft/iflightplanner/model/{aircraftModelId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "aircraftModelId" + "}", ApiClient.ParameterToString(aircraftModelId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerModel: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerModel: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFLightPlannerModelResponse) ApiClient.Deserialize(response.Content, typeof(IFLightPlannerModelResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Get a list of iFlightPlanner aircraft models by the aircraft type (ICAO) code. 
        /// </summary>
        /// <param name="aircraftType"></param> 
        /// <returns>IFlightPlannerModelsByTypeResponse</returns>            
        public IFlightPlannerModelsByTypeResponse GetIFlightPlannerModelsByType (string aircraftType)
        {
            
            // verify the required parameter 'aircraftType' is set
            if (aircraftType == null) throw new ApiException(400, "Missing required parameter 'aircraftType' when calling GetIFlightPlannerModelsByType");
            
    
            var path = "/api/Aircraft/iflightplanner/modelsbytype/{aircraftType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "aircraftType" + "}", ApiClient.ParameterToString(aircraftType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerModelsByType: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerModelsByType: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFlightPlannerModelsByTypeResponse) ApiClient.Deserialize(response.Content, typeof(IFlightPlannerModelsByTypeResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch an aircraft profile from iFlightPlanner. 
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <returns>IFlightPlannerAircraftProfileResponse</returns>            
        public IFlightPlannerAircraftProfileResponse GetIFlightPlannerProfileByTailNumber (string tailNumber)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetIFlightPlannerProfileByTailNumber");
            
    
            var path = "/api/Aircraft/iflightplanner/aircraftprofile/{tailNumber}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerProfileByTailNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerProfileByTailNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFlightPlannerAircraftProfileResponse) ApiClient.Deserialize(response.Content, typeof(IFlightPlannerAircraftProfileResponse), response.Headers);
        }
    
        /// <summary>
        /// Add tankering settings for an aircraft. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostUserAircraftTankeringSettingsResponse</returns>            
        public PostUserAircraftTankeringSettingsResponse PostUserAircraftTankeringSettings (PostUserAircraftTankeringSettingsRequest body)
        {
            
    
            var path = "/api/Aircraft/tankering-settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostUserAircraftTankeringSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostUserAircraftTankeringSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostUserAircraftTankeringSettingsResponse) ApiClient.Deserialize(response.Content, typeof(PostUserAircraftTankeringSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Register an aircraft with iFlightPlanner. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>IFlightPlannerAircraftRegistrationResponse</returns>            
        public IFlightPlannerAircraftRegistrationResponse RegisterAircraftWithIFlightPlanner (IFlightPlannerAircraftRegistrationRequest body)
        {
            
    
            var path = "/api/Aircraft/iflightplanner/register";
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
                throw new ApiException ((int)response.StatusCode, "Error calling RegisterAircraftWithIFlightPlanner: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling RegisterAircraftWithIFlightPlanner: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFlightPlannerAircraftRegistrationResponse) ApiClient.Deserialize(response.Content, typeof(IFlightPlannerAircraftRegistrationResponse), response.Headers);
        }
    
        /// <summary>
        /// Update an existing aircraft with the corresponding [tailNumberId]. A tail number is required.  The tail number does not have to be unique.
        /// </summary>
        /// <param name="tailNumberId"></param> 
        /// <param name="body"></param> 
        /// <returns>AircraftDataResponse</returns>            
        public AircraftDataResponse UpdateTail (int? tailNumberId, AircraftDataDTO body)
        {
            
            // verify the required parameter 'tailNumberId' is set
            if (tailNumberId == null) throw new ApiException(400, "Missing required parameter 'tailNumberId' when calling UpdateTail");
            
    
            var path = "/api/Aircraft/tail/{tailNumberId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tailNumberId" + "}", ApiClient.ParameterToString(tailNumberId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTail: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTail: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AircraftDataResponse) ApiClient.Deserialize(response.Content, typeof(AircraftDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Update tankering settings for an aircraft. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateUserAircraftTankeringSettingsResponse</returns>            
        public UpdateUserAircraftTankeringSettingsResponse UpdateUserAircraftTankeringSettings (UpdateUserAircraftTankeringSettingsRequest body)
        {
            
    
            var path = "/api/Aircraft/tankering-settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateUserAircraftTankeringSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateUserAircraftTankeringSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateUserAircraftTankeringSettingsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateUserAircraftTankeringSettingsResponse), response.Headers);
        }
    
    }
}

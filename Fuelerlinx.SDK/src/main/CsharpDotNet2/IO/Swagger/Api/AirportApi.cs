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
    public interface IAirportApi
    {
        /// <summary>
        /// Delete the company-specific details of an airport based on the provided {airportDetailsByCompanyId}. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param>
        /// <returns>DeleteAirportDetailsByCompanyResponse</returns>
        DeleteAirportDetailsByCompanyResponse DeleteAirportDetailsByCompany (int? airportDetailsByCompanyId);
        /// <summary>
        /// Delete a company-specific note for a particular airport.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param>
        /// <param name="noteId"></param>
        /// <returns>DeleteAirportDetailsByCompanyNoteResponse</returns>
        DeleteAirportDetailsByCompanyNoteResponse DeleteAirportDetailsByCompanyNotes (int? airportDetailsByCompanyId, int? noteId);
        /// <summary>
        /// Fetch information specifically for the airport with the designated {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport. 
        /// </summary>
        /// <param name="airportIdentifier"></param>
        /// <returns>AcukwikAirportResponse</returns>
        AcukwikAirportResponse GetAcukwikAirport (string airportIdentifier);
        /// <summary>
        /// Fetch company-specific details for an airport based on the provided {airportDetailsByCompanyId} of the record.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param>
        /// <returns>AirportDetailsByCompanyResponse</returns>
        AirportDetailsByCompanyResponse GetAirportDetailsByCompany (int? airportDetailsByCompanyId);
        /// <summary>
        /// Fetch company-specific details for an airport based on the provided {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="airportIdentifier"></param>
        /// <returns>AirportDetailsByCompanyResponse</returns>
        AirportDetailsByCompanyResponse GetAirportDetailsByCompanyByIcao (string airportIdentifier);
        /// <summary>
        /// Fetch the company-specific notes for a particular airport based on the provided {airportDetailsByCompanyId}. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param>
        /// <returns>AirportDetailsByCompanyNotesResponse</returns>
        AirportDetailsByCompanyNotesResponse GetAirportDetailsByCompanyNotes (int? airportDetailsByCompanyId);
        /// <summary>
        /// Internal use only - Fetch all airports in the Acukwik database.  This will included FBO/Handler information as well. 
        /// </summary>
        /// <returns>List&lt;AcukwikAirportDTO&gt;</returns>
        List<AcukwikAirportDTO> GetAllAirports ();
        /// <summary>
        ///  
        /// </summary>
        /// <returns>DistinctCountryListResponse</returns>
        DistinctCountryListResponse GetDistinctAirportCountries ();
        /// <summary>
        ///  
        /// </summary>
        /// <returns>List&lt;GeneralAirportInformation&gt;</returns>
        List<GeneralAirportInformation> GetGeneralAirportInfoList ();
        /// <summary>
        /// Add a new record for company-specific details of an airport.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostAirportDetailsByCompanyResponse</returns>
        PostAirportDetailsByCompanyResponse PostAirportDetailsByCompany (PostAirportDetailsByCompanyRequest body);
        /// <summary>
        /// Add a new company-specific note for an airport. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostAirportDetailsByCompanyNotesResponse</returns>
        PostAirportDetailsByCompanyNotesResponse PostAirportDetailsByCompanyNotes (PostAirportDetailsByCompanyNotesRequest body);
        /// <summary>
        /// Update the company-specific details of an airport.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateAirportDetailsByCompanyResponse</returns>
        UpdateAirportDetailsByCompanyResponse UpdateAirportDetailsByCompany (UpdateAirportDetailsByCompanyRequest body);
        /// <summary>
        /// Update an existing company-specific note for an airport. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateAirportDetailsByCompanyNotesResponse</returns>
        UpdateAirportDetailsByCompanyNotesResponse UpdateAirportDetailsByCompanyNotes (UpdateAirportDetailsByCompanyNotesRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class AirportApi : IAirportApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public AirportApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportApi"/> class.
        /// </summary>
        /// <returns></returns>
        public AirportApi(String basePath)
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
        /// Delete the company-specific details of an airport based on the provided {airportDetailsByCompanyId}. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param> 
        /// <returns>DeleteAirportDetailsByCompanyResponse</returns>            
        public DeleteAirportDetailsByCompanyResponse DeleteAirportDetailsByCompany (int? airportDetailsByCompanyId)
        {
            
            // verify the required parameter 'airportDetailsByCompanyId' is set
            if (airportDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'airportDetailsByCompanyId' when calling DeleteAirportDetailsByCompany");
            
    
            var path = "/api/Airport/company-specific-details/{airportDetailsByCompanyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "airportDetailsByCompanyId" + "}", ApiClient.ParameterToString(airportDetailsByCompanyId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAirportDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAirportDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteAirportDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(DeleteAirportDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a company-specific note for a particular airport.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param> 
        /// <param name="noteId"></param> 
        /// <returns>DeleteAirportDetailsByCompanyNoteResponse</returns>            
        public DeleteAirportDetailsByCompanyNoteResponse DeleteAirportDetailsByCompanyNotes (int? airportDetailsByCompanyId, int? noteId)
        {
            
            // verify the required parameter 'airportDetailsByCompanyId' is set
            if (airportDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'airportDetailsByCompanyId' when calling DeleteAirportDetailsByCompanyNotes");
            
            // verify the required parameter 'noteId' is set
            if (noteId == null) throw new ApiException(400, "Missing required parameter 'noteId' when calling DeleteAirportDetailsByCompanyNotes");
            
    
            var path = "/api/Airport/company-specific-details/{airportDetailsByCompanyId}/notes/{noteId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "airportDetailsByCompanyId" + "}", ApiClient.ParameterToString(airportDetailsByCompanyId));
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAirportDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAirportDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteAirportDetailsByCompanyNoteResponse) ApiClient.Deserialize(response.Content, typeof(DeleteAirportDetailsByCompanyNoteResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch information specifically for the airport with the designated {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport. 
        /// </summary>
        /// <param name="airportIdentifier"></param> 
        /// <returns>AcukwikAirportResponse</returns>            
        public AcukwikAirportResponse GetAcukwikAirport (string airportIdentifier)
        {
            
            // verify the required parameter 'airportIdentifier' is set
            if (airportIdentifier == null) throw new ApiException(400, "Missing required parameter 'airportIdentifier' when calling GetAcukwikAirport");
            
    
            var path = "/api/Airport/airport/{airportIdentifier}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "airportIdentifier" + "}", ApiClient.ParameterToString(airportIdentifier));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAcukwikAirport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAcukwikAirport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AcukwikAirportResponse) ApiClient.Deserialize(response.Content, typeof(AcukwikAirportResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company-specific details for an airport based on the provided {airportDetailsByCompanyId} of the record.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param> 
        /// <returns>AirportDetailsByCompanyResponse</returns>            
        public AirportDetailsByCompanyResponse GetAirportDetailsByCompany (int? airportDetailsByCompanyId)
        {
            
            // verify the required parameter 'airportDetailsByCompanyId' is set
            if (airportDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'airportDetailsByCompanyId' when calling GetAirportDetailsByCompany");
            
    
            var path = "/api/Airport/company-specific-details/{airportDetailsByCompanyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "airportDetailsByCompanyId" + "}", ApiClient.ParameterToString(airportDetailsByCompanyId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAirportDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAirportDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AirportDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(AirportDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company-specific details for an airport based on the provided {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="airportIdentifier"></param> 
        /// <returns>AirportDetailsByCompanyResponse</returns>            
        public AirportDetailsByCompanyResponse GetAirportDetailsByCompanyByIcao (string airportIdentifier)
        {
            
            // verify the required parameter 'airportIdentifier' is set
            if (airportIdentifier == null) throw new ApiException(400, "Missing required parameter 'airportIdentifier' when calling GetAirportDetailsByCompanyByIcao");
            
    
            var path = "/api/Airport/company-specific-details/icao/{airportIdentifier}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "airportIdentifier" + "}", ApiClient.ParameterToString(airportIdentifier));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAirportDetailsByCompanyByIcao: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAirportDetailsByCompanyByIcao: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AirportDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(AirportDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch the company-specific notes for a particular airport based on the provided {airportDetailsByCompanyId}. 
        /// </summary>
        /// <param name="airportDetailsByCompanyId"></param> 
        /// <returns>AirportDetailsByCompanyNotesResponse</returns>            
        public AirportDetailsByCompanyNotesResponse GetAirportDetailsByCompanyNotes (int? airportDetailsByCompanyId)
        {
            
            // verify the required parameter 'airportDetailsByCompanyId' is set
            if (airportDetailsByCompanyId == null) throw new ApiException(400, "Missing required parameter 'airportDetailsByCompanyId' when calling GetAirportDetailsByCompanyNotes");
            
    
            var path = "/api/Airport/company-specific-details/{airportDetailsByCompanyId}/notes";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "airportDetailsByCompanyId" + "}", ApiClient.ParameterToString(airportDetailsByCompanyId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAirportDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAirportDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AirportDetailsByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(AirportDetailsByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all airports in the Acukwik database.  This will included FBO/Handler information as well. 
        /// </summary>
        /// <returns>List&lt;AcukwikAirportDTO&gt;</returns>            
        public List<AcukwikAirportDTO> GetAllAirports ()
        {
            
    
            var path = "/api/Airport/airports";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAllAirports: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAllAirports: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<AcukwikAirportDTO>) ApiClient.Deserialize(response.Content, typeof(List<AcukwikAirportDTO>), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <returns>DistinctCountryListResponse</returns>            
        public DistinctCountryListResponse GetDistinctAirportCountries ()
        {
            
    
            var path = "/api/Airport/countries/distinct";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetDistinctAirportCountries: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetDistinctAirportCountries: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DistinctCountryListResponse) ApiClient.Deserialize(response.Content, typeof(DistinctCountryListResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <returns>List&lt;GeneralAirportInformation&gt;</returns>            
        public List<GeneralAirportInformation> GetGeneralAirportInfoList ()
        {
            
    
            var path = "/api/Airport/general-info/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetGeneralAirportInfoList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetGeneralAirportInfoList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<GeneralAirportInformation>) ApiClient.Deserialize(response.Content, typeof(List<GeneralAirportInformation>), response.Headers);
        }
    
        /// <summary>
        /// Add a new record for company-specific details of an airport.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostAirportDetailsByCompanyResponse</returns>            
        public PostAirportDetailsByCompanyResponse PostAirportDetailsByCompany (PostAirportDetailsByCompanyRequest body)
        {
            
    
            var path = "/api/Airport/company-specific-details";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostAirportDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostAirportDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostAirportDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(PostAirportDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new company-specific note for an airport. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostAirportDetailsByCompanyNotesResponse</returns>            
        public PostAirportDetailsByCompanyNotesResponse PostAirportDetailsByCompanyNotes (PostAirportDetailsByCompanyNotesRequest body)
        {
            
    
            var path = "/api/Airport/company-specific-details/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostAirportDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostAirportDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostAirportDetailsByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(PostAirportDetailsByCompanyNotesResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the company-specific details of an airport.  These details are unique for each flight department. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateAirportDetailsByCompanyResponse</returns>            
        public UpdateAirportDetailsByCompanyResponse UpdateAirportDetailsByCompany (UpdateAirportDetailsByCompanyRequest body)
        {
            
    
            var path = "/api/Airport/company-specific-details";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAirportDetailsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAirportDetailsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateAirportDetailsByCompanyResponse) ApiClient.Deserialize(response.Content, typeof(UpdateAirportDetailsByCompanyResponse), response.Headers);
        }
    
        /// <summary>
        /// Update an existing company-specific note for an airport. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateAirportDetailsByCompanyNotesResponse</returns>            
        public UpdateAirportDetailsByCompanyNotesResponse UpdateAirportDetailsByCompanyNotes (UpdateAirportDetailsByCompanyNotesRequest body)
        {
            
    
            var path = "/api/Airport/company-specific-details/notes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAirportDetailsByCompanyNotes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAirportDetailsByCompanyNotes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateAirportDetailsByCompanyNotesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateAirportDetailsByCompanyNotesResponse), response.Headers);
        }
    
    }
}

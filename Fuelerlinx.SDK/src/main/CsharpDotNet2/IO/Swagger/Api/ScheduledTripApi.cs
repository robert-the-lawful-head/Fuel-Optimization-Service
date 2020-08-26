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
    public interface IScheduledTripApi
    {
        /// <summary>
        /// Delete a previously POSTed leg with a matching [legIdentifier]. 
        /// </summary>
        /// <param name="legIdentifier"></param>
        /// <returns>ScheduledTripDeleteResponse</returns>
        ScheduledTripDeleteResponse DeleteScheduledLegData (string legIdentifier);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteScheduledTripSettingsResponse</returns>
        DeleteScheduledTripSettingsResponse DeleteScheduledTripSettings (int? id);
        /// <summary>
        /// Fetch upcoming scheduled trip info pulled from the user&#39;s scheduling system. Only records that are scheduled to depart after the current time will be returned.
        /// </summary>
        /// <returns>CurrentScheduledTripsResponse</returns>
        CurrentScheduledTripsResponse GetCurrentScheduledTrips ();
        /// <summary>
        /// Fetch the [transaction] and [generatedFuelComment] associated with the fuel order that was placed for the specified leg tied to the [legIdentifier]. The [legIdentifier] should be the unique identifier used by the scheduling integration partner to identify the leg within their system.
        /// </summary>
        /// <param name="legIdentifier"></param>
        /// <returns>FuelOrderDetailsForScheduledLegResponse</returns>
        FuelOrderDetailsForScheduledLegResponse GetFuelOrderDetailsForScheduledLeg (string legIdentifier);
        /// <summary>
        ///  
        /// </summary>
        /// <returns>ScheduledTripSettingsResponse</returns>
        ScheduledTripSettingsResponse GetScheduledTripSettings ();
        /// <summary>
        /// Post a leg from the user&#39;s scheduling system as an object [ScheduledLegData] and it&#39;s corresponding [LegIdentifier].  The scheduling integration partner controls the format of the [ScheduledLegData] and the [LegIdentifier] should be a unique identifier used on the partner&#39;s side. It is recommended to include the tail number, departure airport, arrival airport, and date/time of the departure/arrival as a minimum when sending information.  Additional information (i.e. pax count, cargo, altitude, fuel on board, etc.) is recommended to help enhance the integration.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostScheduledLegFromIntegrationResponse</returns>
        PostScheduledLegFromIntegrationResponse PostScheduledLegData (PostScheduledLegFromIntegrationRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostScheduledTripSettingsResponse</returns>
        PostScheduledTripSettingsResponse PostScheduledTripSettings (PostScheduledTripSettingsRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateScheduledTripSettingsResponse</returns>
        UpdateScheduledTripSettingsResponse UpdateScheduledTripSettings (UpdateScheduledTripSettingsRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ScheduledTripApi : IScheduledTripApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTripApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ScheduledTripApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTripApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ScheduledTripApi(String basePath)
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
        /// Delete a previously POSTed leg with a matching [legIdentifier]. 
        /// </summary>
        /// <param name="legIdentifier"></param> 
        /// <returns>ScheduledTripDeleteResponse</returns>            
        public ScheduledTripDeleteResponse DeleteScheduledLegData (string legIdentifier)
        {
            
            // verify the required parameter 'legIdentifier' is set
            if (legIdentifier == null) throw new ApiException(400, "Missing required parameter 'legIdentifier' when calling DeleteScheduledLegData");
            
    
            var path = "/api/ScheduledTrip/integration/leg/{legIdentifier}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "legIdentifier" + "}", ApiClient.ParameterToString(legIdentifier));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteScheduledLegData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteScheduledLegData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ScheduledTripDeleteResponse) ApiClient.Deserialize(response.Content, typeof(ScheduledTripDeleteResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteScheduledTripSettingsResponse</returns>            
        public DeleteScheduledTripSettingsResponse DeleteScheduledTripSettings (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteScheduledTripSettings");
            
    
            var path = "/api/ScheduledTrip/settings/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteScheduledTripSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteScheduledTripSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteScheduledTripSettingsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteScheduledTripSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch upcoming scheduled trip info pulled from the user&#39;s scheduling system. Only records that are scheduled to depart after the current time will be returned.
        /// </summary>
        /// <returns>CurrentScheduledTripsResponse</returns>            
        public CurrentScheduledTripsResponse GetCurrentScheduledTrips ()
        {
            
    
            var path = "/api/ScheduledTrip/current";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentScheduledTrips: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentScheduledTrips: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentScheduledTripsResponse) ApiClient.Deserialize(response.Content, typeof(CurrentScheduledTripsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch the [transaction] and [generatedFuelComment] associated with the fuel order that was placed for the specified leg tied to the [legIdentifier]. The [legIdentifier] should be the unique identifier used by the scheduling integration partner to identify the leg within their system.
        /// </summary>
        /// <param name="legIdentifier"></param> 
        /// <returns>FuelOrderDetailsForScheduledLegResponse</returns>            
        public FuelOrderDetailsForScheduledLegResponse GetFuelOrderDetailsForScheduledLeg (string legIdentifier)
        {
            
            // verify the required parameter 'legIdentifier' is set
            if (legIdentifier == null) throw new ApiException(400, "Missing required parameter 'legIdentifier' when calling GetFuelOrderDetailsForScheduledLeg");
            
    
            var path = "/api/ScheduledTrip/integration/fuelorderdetails/{legIdentifier}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "legIdentifier" + "}", ApiClient.ParameterToString(legIdentifier));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelOrderDetailsForScheduledLeg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelOrderDetailsForScheduledLeg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FuelOrderDetailsForScheduledLegResponse) ApiClient.Deserialize(response.Content, typeof(FuelOrderDetailsForScheduledLegResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <returns>ScheduledTripSettingsResponse</returns>            
        public ScheduledTripSettingsResponse GetScheduledTripSettings ()
        {
            
    
            var path = "/api/ScheduledTrip/settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetScheduledTripSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetScheduledTripSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ScheduledTripSettingsResponse) ApiClient.Deserialize(response.Content, typeof(ScheduledTripSettingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Post a leg from the user&#39;s scheduling system as an object [ScheduledLegData] and it&#39;s corresponding [LegIdentifier].  The scheduling integration partner controls the format of the [ScheduledLegData] and the [LegIdentifier] should be a unique identifier used on the partner&#39;s side. It is recommended to include the tail number, departure airport, arrival airport, and date/time of the departure/arrival as a minimum when sending information.  Additional information (i.e. pax count, cargo, altitude, fuel on board, etc.) is recommended to help enhance the integration.
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostScheduledLegFromIntegrationResponse</returns>            
        public PostScheduledLegFromIntegrationResponse PostScheduledLegData (PostScheduledLegFromIntegrationRequest body)
        {
            
    
            var path = "/api/ScheduledTrip/integration/leg";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostScheduledLegData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostScheduledLegData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostScheduledLegFromIntegrationResponse) ApiClient.Deserialize(response.Content, typeof(PostScheduledLegFromIntegrationResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostScheduledTripSettingsResponse</returns>            
        public PostScheduledTripSettingsResponse PostScheduledTripSettings (PostScheduledTripSettingsRequest body)
        {
            
    
            var path = "/api/ScheduledTrip/settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostScheduledTripSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostScheduledTripSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostScheduledTripSettingsResponse) ApiClient.Deserialize(response.Content, typeof(PostScheduledTripSettingsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateScheduledTripSettingsResponse</returns>            
        public UpdateScheduledTripSettingsResponse UpdateScheduledTripSettings (UpdateScheduledTripSettingsRequest body)
        {
            
    
            var path = "/api/ScheduledTrip/settings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateScheduledTripSettings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateScheduledTripSettings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateScheduledTripSettingsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateScheduledTripSettingsResponse), response.Headers);
        }
    
    }
}

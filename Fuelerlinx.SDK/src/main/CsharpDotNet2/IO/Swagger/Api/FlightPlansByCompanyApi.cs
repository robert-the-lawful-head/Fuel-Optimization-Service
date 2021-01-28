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
    public interface IFlightPlansByCompanyApi
    {
        /// <summary>
        /// Internal use only - Delete an existing service log record for a iFlightPlanner route request. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteIFlightPlannerRouteRequestServiceLogResponse</returns>
        DeleteIFlightPlannerRouteRequestServiceLogResponse DeleteIFlightPlannerRouteRequestServiceLog (int? id);
        /// <summary>
        /// Fetch upcoming trip info pulled from the user&#39;s flight planning provider. Only records that are scheduled to depart after the current time will be returned.
        /// </summary>
        /// <returns>CurrentPlannedFlightsResponse</returns>
        CurrentPlannedFlightsResponse GetCurrentPlannedFlights ();
        /// <summary>
        /// Fetch upcoming trip info pulled from the user&#39;s flight planning provider by tail number Only records that are scheduled to depart after the current time will be returned.
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <returns>CurrentPlannedFlightsResponse</returns>
        CurrentPlannedFlightsResponse GetCurrentPlannedFlightsByTail (string tailNumber);
        /// <summary>
        /// Fetching stored aviation profiles from the user&#39;s iFlightPlanner account 
        /// </summary>
        /// <returns>IFlightPlannerAviationProfilesResponse</returns>
        IFlightPlannerAviationProfilesResponse GetIFlightPlannerAviationProfiles ();
        /// <summary>
        /// Internal use only - Fetch the service logs for iFlightPlanner route requests based on the provided search parameters. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="tailNumber"></param>
        /// <param name="departureAirport"></param>
        /// <param name="arrivalAirport"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>IFLightPlannerRouteRequestServiceLogListResponse</returns>
        IFLightPlannerRouteRequestServiceLogListResponse GetIFlightPlannerRouteRequestServiceLog (int? companyId, string tailNumber, string departureAirport, string arrivalAirport, DateTime? startDate, DateTime? endDate);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="departureAirportIdentifier"></param>
        /// <param name="arrivalAirportIdentifier"></param>
        /// <returns>RecentATCResponse</returns>
        RecentATCResponse GetRecentATCRoutes (string departureAirportIdentifier, string arrivalAirportIdentifier);
        /// <summary>
        /// Internal use only - Post a new service log record for a iFlightPlanner route request. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostIFlightPlannerRouteRequestServiceLogResponse</returns>
        PostIFlightPlannerRouteRequestServiceLogResponse PostIFlightPlannerRouteRequestServiceLog (PostIFlightPlannerRouteRequestServiceLogRequest body);
        /// <summary>
        /// Accepts a serialized set of trip/leg information from a user&#39;s flight planning provider. If the serialized data is not in the expected \&quot;TripInfo\&quot; structure defined by FlightPlan.com, an error will occur.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTripInfoResponse</returns>
        PostTripInfoResponse PostTripInfo (PostTripInfoRequest body);
        /// <summary>
        /// Internal use only - Update fuel purchase information for a flight planning 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateFuelPurchaseInfoResponse</returns>
        UpdateFuelPurchaseInfoResponse UpdateFuelPurchaseInfoForFlight (UpdateFuelPurchaseInfoRequest body);
        /// <summary>
        /// Internal use only - Update an existing service log record for a iFlightPlanner route request. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateIFlightPlannerRouteRequestServiceLogResponse</returns>
        UpdateIFlightPlannerRouteRequestServiceLogResponse UpdateIFlightPlannerRouteRequestServiceLog (UpdateIFlightPlannerRouteRequestServiceLogRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FlightPlansByCompanyApi : IFlightPlansByCompanyApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightPlansByCompanyApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FlightPlansByCompanyApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightPlansByCompanyApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FlightPlansByCompanyApi(String basePath)
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
        /// Internal use only - Delete an existing service log record for a iFlightPlanner route request. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteIFlightPlannerRouteRequestServiceLogResponse</returns>            
        public DeleteIFlightPlannerRouteRequestServiceLogResponse DeleteIFlightPlannerRouteRequestServiceLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteIFlightPlannerRouteRequestServiceLog");
            
    
            var path = "/api/FlightPlansByCompany/service-logs/iflightplanner-route-request/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteIFlightPlannerRouteRequestServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteIFlightPlannerRouteRequestServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteIFlightPlannerRouteRequestServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteIFlightPlannerRouteRequestServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch upcoming trip info pulled from the user&#39;s flight planning provider. Only records that are scheduled to depart after the current time will be returned.
        /// </summary>
        /// <returns>CurrentPlannedFlightsResponse</returns>            
        public CurrentPlannedFlightsResponse GetCurrentPlannedFlights ()
        {
            
    
            var path = "/api/FlightPlansByCompany/current";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPlannedFlights: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPlannedFlights: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentPlannedFlightsResponse) ApiClient.Deserialize(response.Content, typeof(CurrentPlannedFlightsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch upcoming trip info pulled from the user&#39;s flight planning provider by tail number Only records that are scheduled to depart after the current time will be returned.
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <returns>CurrentPlannedFlightsResponse</returns>            
        public CurrentPlannedFlightsResponse GetCurrentPlannedFlightsByTail (string tailNumber)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetCurrentPlannedFlightsByTail");
            
    
            var path = "/api/FlightPlansByCompany/current/tail/{tailNumber}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPlannedFlightsByTail: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCurrentPlannedFlightsByTail: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CurrentPlannedFlightsResponse) ApiClient.Deserialize(response.Content, typeof(CurrentPlannedFlightsResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetching stored aviation profiles from the user&#39;s iFlightPlanner account 
        /// </summary>
        /// <returns>IFlightPlannerAviationProfilesResponse</returns>            
        public IFlightPlannerAviationProfilesResponse GetIFlightPlannerAviationProfiles ()
        {
            
    
            var path = "/api/FlightPlansByCompany/iflightplanner/aviationprofiles";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerAviationProfiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerAviationProfiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFlightPlannerAviationProfilesResponse) ApiClient.Deserialize(response.Content, typeof(IFlightPlannerAviationProfilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch the service logs for iFlightPlanner route requests based on the provided search parameters. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="tailNumber"></param> 
        /// <param name="departureAirport"></param> 
        /// <param name="arrivalAirport"></param> 
        /// <param name="startDate"></param> 
        /// <param name="endDate"></param> 
        /// <returns>IFLightPlannerRouteRequestServiceLogListResponse</returns>            
        public IFLightPlannerRouteRequestServiceLogListResponse GetIFlightPlannerRouteRequestServiceLog (int? companyId, string tailNumber, string departureAirport, string arrivalAirport, DateTime? startDate, DateTime? endDate)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetIFlightPlannerRouteRequestServiceLog");
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetIFlightPlannerRouteRequestServiceLog");
            
            // verify the required parameter 'departureAirport' is set
            if (departureAirport == null) throw new ApiException(400, "Missing required parameter 'departureAirport' when calling GetIFlightPlannerRouteRequestServiceLog");
            
            // verify the required parameter 'arrivalAirport' is set
            if (arrivalAirport == null) throw new ApiException(400, "Missing required parameter 'arrivalAirport' when calling GetIFlightPlannerRouteRequestServiceLog");
            
    
            var path = "/api/FlightPlansByCompany/service-logs/iflightplanner-route-request/company/{companyId}/tail/{tailNumber}/departure/{departureAirport}/arrival/{arrivalAirport}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
path = path.Replace("{" + "departureAirport" + "}", ApiClient.ParameterToString(departureAirport));
path = path.Replace("{" + "arrivalAirport" + "}", ApiClient.ParameterToString(arrivalAirport));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerRouteRequestServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerRouteRequestServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFLightPlannerRouteRequestServiceLogListResponse) ApiClient.Deserialize(response.Content, typeof(IFLightPlannerRouteRequestServiceLogListResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="departureAirportIdentifier"></param> 
        /// <param name="arrivalAirportIdentifier"></param> 
        /// <returns>RecentATCResponse</returns>            
        public RecentATCResponse GetRecentATCRoutes (string departureAirportIdentifier, string arrivalAirportIdentifier)
        {
            
            // verify the required parameter 'departureAirportIdentifier' is set
            if (departureAirportIdentifier == null) throw new ApiException(400, "Missing required parameter 'departureAirportIdentifier' when calling GetRecentATCRoutes");
            
            // verify the required parameter 'arrivalAirportIdentifier' is set
            if (arrivalAirportIdentifier == null) throw new ApiException(400, "Missing required parameter 'arrivalAirportIdentifier' when calling GetRecentATCRoutes");
            
    
            var path = "/api/FlightPlansByCompany/recent-atc/{departureAirportIdentifier}/{arrivalAirportIdentifier}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "departureAirportIdentifier" + "}", ApiClient.ParameterToString(departureAirportIdentifier));
path = path.Replace("{" + "arrivalAirportIdentifier" + "}", ApiClient.ParameterToString(arrivalAirportIdentifier));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetRecentATCRoutes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetRecentATCRoutes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RecentATCResponse) ApiClient.Deserialize(response.Content, typeof(RecentATCResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Post a new service log record for a iFlightPlanner route request. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostIFlightPlannerRouteRequestServiceLogResponse</returns>            
        public PostIFlightPlannerRouteRequestServiceLogResponse PostIFlightPlannerRouteRequestServiceLog (PostIFlightPlannerRouteRequestServiceLogRequest body)
        {
            
    
            var path = "/api/FlightPlansByCompany/service-logs/iflightplanner-route-request";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostIFlightPlannerRouteRequestServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostIFlightPlannerRouteRequestServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostIFlightPlannerRouteRequestServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(PostIFlightPlannerRouteRequestServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Accepts a serialized set of trip/leg information from a user&#39;s flight planning provider. If the serialized data is not in the expected \&quot;TripInfo\&quot; structure defined by FlightPlan.com, an error will occur.
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTripInfoResponse</returns>            
        public PostTripInfoResponse PostTripInfo (PostTripInfoRequest body)
        {
            
    
            var path = "/api/FlightPlansByCompany/tripinfo";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTripInfo: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTripInfo: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTripInfoResponse) ApiClient.Deserialize(response.Content, typeof(PostTripInfoResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update fuel purchase information for a flight planning 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateFuelPurchaseInfoResponse</returns>            
        public UpdateFuelPurchaseInfoResponse UpdateFuelPurchaseInfoForFlight (UpdateFuelPurchaseInfoRequest body)
        {
            
    
            var path = "/api/FlightPlansByCompany/fuelpurchaseinfo/update";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelPurchaseInfoForFlight: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelPurchaseInfoForFlight: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateFuelPurchaseInfoResponse) ApiClient.Deserialize(response.Content, typeof(UpdateFuelPurchaseInfoResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update an existing service log record for a iFlightPlanner route request. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateIFlightPlannerRouteRequestServiceLogResponse</returns>            
        public UpdateIFlightPlannerRouteRequestServiceLogResponse UpdateIFlightPlannerRouteRequestServiceLog (UpdateIFlightPlannerRouteRequestServiceLogRequest body)
        {
            
    
            var path = "/api/FlightPlansByCompany/service-logs/iflightplanner-route-request";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIFlightPlannerRouteRequestServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIFlightPlannerRouteRequestServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateIFlightPlannerRouteRequestServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateIFlightPlannerRouteRequestServiceLogResponse), response.Headers);
        }
    
    }
}

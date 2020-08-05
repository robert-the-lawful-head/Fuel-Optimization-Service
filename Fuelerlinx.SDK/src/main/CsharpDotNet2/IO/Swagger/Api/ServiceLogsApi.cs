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
    public interface IServiceLogsApi
    {
        /// <summary>
        /// Delete a company aircraft change log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteCompanyAircraftChangeLogResponse</returns>
        DeleteCompanyAircraftChangeLogResponse DeleteCompanyAircraftChangeLog (int? id);
        /// <summary>
        /// Delete a company fueler change log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteCompanyFuelerChangeLogResponse</returns>
        DeleteCompanyFuelerChangeLogResponse DeleteCompanyFuelerChangeLog (int? id);
        /// <summary>
        /// Delete a dispatch email log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteDispatchEmailLogResponse</returns>
        DeleteDispatchEmailLogResponse DeleteDispatchEmailLog (int? id);
        /// <summary>
        /// Delete a fuel order service log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteFuelOrderServiceLogResponse</returns>
        DeleteFuelOrderServiceLogResponse DeleteFuelOrderServiceLog (int? id);
        /// <summary>
        /// Delete a fuel price service log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteFuelPriceServiceLogResponse</returns>
        DeleteFuelPriceServiceLogResponse DeleteFuelPriceServiceLog (int? id);
        /// <summary>
        /// Delete a iFlightPlanner route request service log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteIFlightPlannerRouteRequestServiceLogResponse</returns>
        DeleteIFlightPlannerRouteRequestServiceLogResponse DeleteIFlightPlannerRouteServiceLog (int? id);
        /// <summary>
        /// Delete a scheduling integration dispatch service log record by record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSchedulingIntegrationDispatchServiceLogResponse</returns>
        DeleteSchedulingIntegrationDispatchServiceLogResponse DeleteSchedulingIntegrationDispatchServiceLog (int? id);
        /// <summary>
        /// Delete a scheduling integration service log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSchedulingIntegrationServiceLogResponse</returns>
        DeleteSchedulingIntegrationServiceLogResponse DeleteSchedulingIntegrationServiceLog (int? id);
        /// <summary>
        /// Delete a tankering api calculation log record by the record id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteTankeringApiCalculationLogResponse</returns>
        DeleteTankeringApiCalculationLogResponse DeleteTankeringApiCalculationLog (int? id);
        /// <summary>
        /// Fetch company aircraft change log by tailNumber. 
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <returns>CompanyAircraftChangeLogResponse</returns>
        CompanyAircraftChangeLogResponse GetCompanyAircraftChangeLogByTailNumber (string tailNumber);
        /// <summary>
        /// Fetch company aircraft change log by userId. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>CompanyAircraftChangeLogResponse</returns>
        CompanyAircraftChangeLogResponse GetCompanyAircraftChangeLogByUserId (int? userId);
        /// <summary>
        /// Fetch company fueler change log by companyId. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>CompanyFuelerChangeLogResponse</returns>
        CompanyFuelerChangeLogResponse GetCompanyFuelerChangeLogByCompanyId (int? companyId);
        /// <summary>
        /// Fetch company fueler change log by fuelerId. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="fuelerId"></param>
        /// <returns>CompanyFuelerChangeLogResponse</returns>
        CompanyFuelerChangeLogResponse GetCompanyFuelerChangeLogByFuelerId (int? companyId, int? fuelerId);
        /// <summary>
        /// Fetch dispatch email log by tailNumber. 
        /// </summary>
        /// <param name="tailNumber"></param>
        /// <returns>DispatchEmailLogResponse</returns>
        DispatchEmailLogResponse GetDispatchEmailLogByTailNumber (string tailNumber);
        /// <summary>
        /// Fetch dispatch email log by transactionId. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="transactionId"></param>
        /// <returns>DispatchEmailLogResponse</returns>
        DispatchEmailLogResponse GetDispatchEmailLogByTransactionId (int? userId, int? transactionId);
        /// <summary>
        /// Fetch dispatch email log by userId. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>DispatchEmailLogResponse</returns>
        DispatchEmailLogResponse GetDispatchEmailLogByUserId (int? userId);
        /// <summary>
        /// Fetch fuel order service log by transactionId. 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>FuelOrderServiceLogResponse</returns>
        FuelOrderServiceLogResponse GetFuelOrderServiceLogByTransactionId (int? transactionId);
        /// <summary>
        /// Fetch fuel price service log by location. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="icaos"></param>
        /// <returns>FuelPriceServiceLogResponse</returns>
        FuelPriceServiceLogResponse GetFuelPriceServiceLogByLocation (int? userId, string icaos);
        /// <summary>
        /// /// &lt;summary&gt;  Fetch fuel price service log by userId.  &lt;/summary&gt; 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>FuelPriceServiceLogResponse</returns>
        FuelPriceServiceLogResponse GetFuelPriceServiceLogByUserId (int? userId);
        /// <summary>
        /// Fetch iFlightPlanner route request service log&#39; 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="tailNumber"></param>
        /// <param name="departureAirport"></param>
        /// <param name="arrivalAirport"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns>IFlightPlannerRouteRequestServiceLogResponse</returns>
        IFlightPlannerRouteRequestServiceLogResponse GetIFlightPlannerRouteServiceLog (int? companyId, string tailNumber, string departureAirport, string arrivalAirport, DateTime? startDateTime, DateTime? endDateTime);
        /// <summary>
        /// Fetch scheduling integration dispatch service log by companyId. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>SchedulingIntegrationDispatchServiceLogResponse</returns>
        SchedulingIntegrationDispatchServiceLogResponse GetSchedulingIntegrationDispatchServiceLogByCompanyId (int? companyId);
        /// <summary>
        /// Fetch scheduling integration dispatch service log by Date. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="dateTimeRecorded"></param>
        /// <returns>SchedulingIntegrationDispatchServiceLogResponse</returns>
        SchedulingIntegrationDispatchServiceLogResponse GetSchedulingIntegrationDispatchServiceLogByDate (int? companyId, DateTime? dateTimeRecorded);
        /// <summary>
        /// Fetch scheduling integration service log by companyId. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>SchedulingIntegrationServiceLogResponse</returns>
        SchedulingIntegrationServiceLogResponse GetSchedulingIntegrationServiceLogByCompanyId (int? companyId);
        /// <summary>
        /// Fetch scheduling integration service log by date. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="dateTimeRecorded"></param>
        /// <returns>SchedulingIntegrationServiceLogResponse</returns>
        SchedulingIntegrationServiceLogResponse GetSchedulingIntegrationServiceLogByDate (int? companyId, DateTime? dateTimeRecorded);
        /// <summary>
        /// Fetch tankering api calculation log. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns>TankeringApiCalculationLogResponse</returns>
        TankeringApiCalculationLogResponse GetTankeringApiCalculationLog (int? companyId, DateTime? startDateTime, DateTime? endDateTime);
        /// <summary>
        /// Post company aircraft change log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyAircraftChangeLogResponse</returns>
        PostCompanyAircraftChangeLogResponse PostCompanyAircraftChangeLogAsync (PostCompanyAircraftChangeLogRequest body);
        /// <summary>
        /// Post company fueler change log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostCompanyFuelerChangeLogResponse</returns>
        PostCompanyFuelerChangeLogResponse PostCompanyFuelerChangeLogAsync (PostCompanyFuelerChangeLogRequest body);
        /// <summary>
        /// Post dispatch email log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostDispatchEmailLogResponse</returns>
        PostDispatchEmailLogResponse PostDispatchEmailLogAsync (PostDispatchEmailLogRequest body);
        /// <summary>
        /// Post fuel order service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostFuelOrderServiceLogResponse</returns>
        PostFuelOrderServiceLogResponse PostFuelOrderServiceLogAsync (PostFuelOrderServiceLogRequest body);
        /// <summary>
        /// Post fuel price service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostFuelPriceServiceLogResponse</returns>
        PostFuelPriceServiceLogResponse PostFuelPriceServiceLogAsync (PostFuelPriceServiceLogRequest body);
        /// <summary>
        /// Post iFlightPlanner route request service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostIFlightPlannerRouteRequestServiceLogResponse</returns>
        PostIFlightPlannerRouteRequestServiceLogResponse PostIFlightPlannerRouteServiceLogAsync (PostIFlightPlannerRouteRequestServiceLogRequest body);
        /// <summary>
        /// Post scheduling integration dispatch service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSchedulingIntegrationDispatchServiceLogResponse</returns>
        PostSchedulingIntegrationDispatchServiceLogResponse PostSchedulingIntegrationDispatchServiceLogAsync (PostSchedulingIntegrationDispatchServiceLogRequest body);
        /// <summary>
        /// Post scheduling integration service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSchedulingIntegrationServiceLogResponse</returns>
        PostSchedulingIntegrationServiceLogResponse PostSchedulingIntegrationServiceLogAsync (PostSchedulingIntegrationServiceLogRequest body);
        /// <summary>
        /// Post tankering api calculation log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTankeringApiCalculationLogResponse</returns>
        PostTankeringApiCalculationLogResponse PostTankeringApiCalculationLogAsync (PostTankeringApiCalculationLogRequest body);
        /// <summary>
        /// Update the company aircraft change log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyAircraftChangeLogResponse</returns>
        UpdateCompanyAircraftChangeLogResponse UpdateCompanyAircraftChangeLog (UpdateCompanyAircraftChangeLogRequest body);
        /// <summary>
        /// Update the company fueler change log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateCompanyFuelerChangeLogResponse</returns>
        UpdateCompanyFuelerChangeLogResponse UpdateCompanyFuelerChangeLog (UpdateCompanyFuelerChangeLogRequest body);
        /// <summary>
        /// Update the dispatch email log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateDispatchEmailLogResponse</returns>
        UpdateDispatchEmailLogResponse UpdateDispatchEmailLog (UpdateDispatchEmailLogRequest body);
        /// <summary>
        /// Update the fuel order service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateFuelOrderServiceLogResponse</returns>
        UpdateFuelOrderServiceLogResponse UpdateFuelOrderServiceLog (UpdateFuelOrderServiceLogRequest body);
        /// <summary>
        /// Update the fuel price service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateFuelPriceServiceLogResponse</returns>
        UpdateFuelPriceServiceLogResponse UpdateFuelPriceServiceLog (UpdateFuelPriceServiceLogRequest body);
        /// <summary>
        /// Update the iFlightPlanner route request service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateIFlightPlannerRouteRequestServiceLogResponse</returns>
        UpdateIFlightPlannerRouteRequestServiceLogResponse UpdateIFlightPlannerRouteServiceLog (UpdateIFlightPlannerRouteRequestServiceLogRequest body);
        /// <summary>
        /// Update the scheduling integration dispatch service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSchedulingIntegrationDispatchServiceLogResponse</returns>
        UpdateSchedulingIntegrationDispatchServiceLogResponse UpdateSchedulingIntegrationDispatchServiceLog (UpdateSchedulingIntegrationDispatchServiceLogRequest body);
        /// <summary>
        /// Update the scheduling integration service log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSchedulingIntegrationServiceLogResponse</returns>
        UpdateSchedulingIntegrationServiceLogResponse UpdateSchedulingIntegrationServiceLog (UpdateSchedulingIntegrationServiceLogRequest body);
        /// <summary>
        /// Update the tankering api calculation log. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateTankeringApiCalculationLogResponse</returns>
        UpdateTankeringApiCalculationLogResponse UpdateTankeringApiCalculationLog (UpdateTankeringApiCalculationLogRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ServiceLogsApi : IServiceLogsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ServiceLogsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ServiceLogsApi(String basePath)
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
        /// Delete a company aircraft change log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteCompanyAircraftChangeLogResponse</returns>            
        public DeleteCompanyAircraftChangeLogResponse DeleteCompanyAircraftChangeLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteCompanyAircraftChangeLog");
            
    
            var path = "/api/ServiceLogs/companyAircraftChangeLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyAircraftChangeLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyAircraftChangeLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyAircraftChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyAircraftChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a company fueler change log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteCompanyFuelerChangeLogResponse</returns>            
        public DeleteCompanyFuelerChangeLogResponse DeleteCompanyFuelerChangeLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteCompanyFuelerChangeLog");
            
    
            var path = "/api/ServiceLogs/companyFuelerChangeLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerChangeLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCompanyFuelerChangeLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteCompanyFuelerChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteCompanyFuelerChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a dispatch email log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteDispatchEmailLogResponse</returns>            
        public DeleteDispatchEmailLogResponse DeleteDispatchEmailLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteDispatchEmailLog");
            
    
            var path = "/api/ServiceLogs/dispatchEmailLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteDispatchEmailLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteDispatchEmailLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteDispatchEmailLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteDispatchEmailLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a fuel order service log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteFuelOrderServiceLogResponse</returns>            
        public DeleteFuelOrderServiceLogResponse DeleteFuelOrderServiceLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteFuelOrderServiceLog");
            
    
            var path = "/api/ServiceLogs/fuelOrderServiceLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFuelOrderServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFuelOrderServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteFuelOrderServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteFuelOrderServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a fuel price service log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteFuelPriceServiceLogResponse</returns>            
        public DeleteFuelPriceServiceLogResponse DeleteFuelPriceServiceLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteFuelPriceServiceLog");
            
    
            var path = "/api/ServiceLogs/fuelPriceServiceLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFuelPriceServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteFuelPriceServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteFuelPriceServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteFuelPriceServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a iFlightPlanner route request service log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteIFlightPlannerRouteRequestServiceLogResponse</returns>            
        public DeleteIFlightPlannerRouteRequestServiceLogResponse DeleteIFlightPlannerRouteServiceLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteIFlightPlannerRouteServiceLog");
            
    
            var path = "/api/ServiceLogs/iFlightPlannerRouteRequestServiceLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteIFlightPlannerRouteServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteIFlightPlannerRouteServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteIFlightPlannerRouteRequestServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteIFlightPlannerRouteRequestServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a scheduling integration dispatch service log record by record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSchedulingIntegrationDispatchServiceLogResponse</returns>            
        public DeleteSchedulingIntegrationDispatchServiceLogResponse DeleteSchedulingIntegrationDispatchServiceLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSchedulingIntegrationDispatchServiceLog");
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationDispatchServiceLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSchedulingIntegrationDispatchServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSchedulingIntegrationDispatchServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSchedulingIntegrationDispatchServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSchedulingIntegrationDispatchServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a scheduling integration service log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSchedulingIntegrationServiceLogResponse</returns>            
        public DeleteSchedulingIntegrationServiceLogResponse DeleteSchedulingIntegrationServiceLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSchedulingIntegrationServiceLog");
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationServiceLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSchedulingIntegrationServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSchedulingIntegrationServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSchedulingIntegrationServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSchedulingIntegrationServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete a tankering api calculation log record by the record id. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteTankeringApiCalculationLogResponse</returns>            
        public DeleteTankeringApiCalculationLogResponse DeleteTankeringApiCalculationLog (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteTankeringApiCalculationLog");
            
    
            var path = "/api/ServiceLogs/tankeringApiCalculationLog/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTankeringApiCalculationLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTankeringApiCalculationLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteTankeringApiCalculationLogResponse) ApiClient.Deserialize(response.Content, typeof(DeleteTankeringApiCalculationLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company aircraft change log by tailNumber. 
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <returns>CompanyAircraftChangeLogResponse</returns>            
        public CompanyAircraftChangeLogResponse GetCompanyAircraftChangeLogByTailNumber (string tailNumber)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetCompanyAircraftChangeLogByTailNumber");
            
    
            var path = "/api/ServiceLogs/companyAircraftChangeLog/by-tailNumber/{tailNumber}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyAircraftChangeLogByTailNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyAircraftChangeLogByTailNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyAircraftChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(CompanyAircraftChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company aircraft change log by userId. 
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>CompanyAircraftChangeLogResponse</returns>            
        public CompanyAircraftChangeLogResponse GetCompanyAircraftChangeLogByUserId (int? userId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling GetCompanyAircraftChangeLogByUserId");
            
    
            var path = "/api/ServiceLogs/companyAircraftChangeLog/by-userId/{userId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyAircraftChangeLogByUserId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyAircraftChangeLogByUserId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyAircraftChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(CompanyAircraftChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company fueler change log by companyId. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>CompanyFuelerChangeLogResponse</returns>            
        public CompanyFuelerChangeLogResponse GetCompanyFuelerChangeLogByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetCompanyFuelerChangeLogByCompanyId");
            
    
            var path = "/api/ServiceLogs/companyFuelerChangeLog/by-companyId/{companyId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerChangeLogByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerChangeLogByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch company fueler change log by fuelerId. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="fuelerId"></param> 
        /// <returns>CompanyFuelerChangeLogResponse</returns>            
        public CompanyFuelerChangeLogResponse GetCompanyFuelerChangeLogByFuelerId (int? companyId, int? fuelerId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetCompanyFuelerChangeLogByFuelerId");
            
            // verify the required parameter 'fuelerId' is set
            if (fuelerId == null) throw new ApiException(400, "Missing required parameter 'fuelerId' when calling GetCompanyFuelerChangeLogByFuelerId");
            
    
            var path = "/api/ServiceLogs/companyFuelerChangeLog/by-fuelerId/{companyId}/{fuelerId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
path = path.Replace("{" + "fuelerId" + "}", ApiClient.ParameterToString(fuelerId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerChangeLogByFuelerId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCompanyFuelerChangeLogByFuelerId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CompanyFuelerChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(CompanyFuelerChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch dispatch email log by tailNumber. 
        /// </summary>
        /// <param name="tailNumber"></param> 
        /// <returns>DispatchEmailLogResponse</returns>            
        public DispatchEmailLogResponse GetDispatchEmailLogByTailNumber (string tailNumber)
        {
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetDispatchEmailLogByTailNumber");
            
    
            var path = "/api/ServiceLogs/dispatchEmailLog/by-tailNumber/{tailNumber}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetDispatchEmailLogByTailNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetDispatchEmailLogByTailNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DispatchEmailLogResponse) ApiClient.Deserialize(response.Content, typeof(DispatchEmailLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch dispatch email log by transactionId. 
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="transactionId"></param> 
        /// <returns>DispatchEmailLogResponse</returns>            
        public DispatchEmailLogResponse GetDispatchEmailLogByTransactionId (int? userId, int? transactionId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling GetDispatchEmailLogByTransactionId");
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling GetDispatchEmailLogByTransactionId");
            
    
            var path = "/api/ServiceLogs/dispatchEmailLog/by-userId/{userId}/{transactionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));
path = path.Replace("{" + "transactionId" + "}", ApiClient.ParameterToString(transactionId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetDispatchEmailLogByTransactionId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetDispatchEmailLogByTransactionId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DispatchEmailLogResponse) ApiClient.Deserialize(response.Content, typeof(DispatchEmailLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch dispatch email log by userId. 
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>DispatchEmailLogResponse</returns>            
        public DispatchEmailLogResponse GetDispatchEmailLogByUserId (int? userId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling GetDispatchEmailLogByUserId");
            
    
            var path = "/api/ServiceLogs/dispatchEmailLog/by-userId/{userId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetDispatchEmailLogByUserId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetDispatchEmailLogByUserId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DispatchEmailLogResponse) ApiClient.Deserialize(response.Content, typeof(DispatchEmailLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch fuel order service log by transactionId. 
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>FuelOrderServiceLogResponse</returns>            
        public FuelOrderServiceLogResponse GetFuelOrderServiceLogByTransactionId (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling GetFuelOrderServiceLogByTransactionId");
            
    
            var path = "/api/ServiceLogs/fuelOrderServiceLog/by-transactionId/{transactionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "transactionId" + "}", ApiClient.ParameterToString(transactionId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelOrderServiceLogByTransactionId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelOrderServiceLogByTransactionId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FuelOrderServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(FuelOrderServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch fuel price service log by location. 
        /// </summary>
        /// <param name="userId"></param> 
        /// <param name="icaos"></param> 
        /// <returns>FuelPriceServiceLogResponse</returns>            
        public FuelPriceServiceLogResponse GetFuelPriceServiceLogByLocation (int? userId, string icaos)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling GetFuelPriceServiceLogByLocation");
            
            // verify the required parameter 'icaos' is set
            if (icaos == null) throw new ApiException(400, "Missing required parameter 'icaos' when calling GetFuelPriceServiceLogByLocation");
            
    
            var path = "/api/ServiceLogs/fuelPriceServiceLog/by-location/{userId}/{icaos}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));
path = path.Replace("{" + "icaos" + "}", ApiClient.ParameterToString(icaos));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelPriceServiceLogByLocation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelPriceServiceLogByLocation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FuelPriceServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(FuelPriceServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// /// &lt;summary&gt;  Fetch fuel price service log by userId.  &lt;/summary&gt; 
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>FuelPriceServiceLogResponse</returns>            
        public FuelPriceServiceLogResponse GetFuelPriceServiceLogByUserId (int? userId)
        {
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling GetFuelPriceServiceLogByUserId");
            
    
            var path = "/api/ServiceLogs/fuelPriceServiceLog/by-userId/{userId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelPriceServiceLogByUserId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetFuelPriceServiceLogByUserId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (FuelPriceServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(FuelPriceServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch iFlightPlanner route request service log&#39; 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="tailNumber"></param> 
        /// <param name="departureAirport"></param> 
        /// <param name="arrivalAirport"></param> 
        /// <param name="startDateTime"></param> 
        /// <param name="endDateTime"></param> 
        /// <returns>IFlightPlannerRouteRequestServiceLogResponse</returns>            
        public IFlightPlannerRouteRequestServiceLogResponse GetIFlightPlannerRouteServiceLog (int? companyId, string tailNumber, string departureAirport, string arrivalAirport, DateTime? startDateTime, DateTime? endDateTime)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetIFlightPlannerRouteServiceLog");
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetIFlightPlannerRouteServiceLog");
            
            // verify the required parameter 'departureAirport' is set
            if (departureAirport == null) throw new ApiException(400, "Missing required parameter 'departureAirport' when calling GetIFlightPlannerRouteServiceLog");
            
            // verify the required parameter 'arrivalAirport' is set
            if (arrivalAirport == null) throw new ApiException(400, "Missing required parameter 'arrivalAirport' when calling GetIFlightPlannerRouteServiceLog");
            
    
            var path = "/api/ServiceLogs/iFlightPlannerRouteRequestServiceLog/{companyId}/{tailNumber}/{departureAirport}/{arrivalAirport}";
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
    
             if (startDateTime != null) queryParams.Add("startDateTime", ApiClient.ParameterToString(startDateTime)); // query parameter
 if (endDateTime != null) queryParams.Add("endDateTime", ApiClient.ParameterToString(endDateTime)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerRouteServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIFlightPlannerRouteServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IFlightPlannerRouteRequestServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(IFlightPlannerRouteRequestServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch scheduling integration dispatch service log by companyId. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>SchedulingIntegrationDispatchServiceLogResponse</returns>            
        public SchedulingIntegrationDispatchServiceLogResponse GetSchedulingIntegrationDispatchServiceLogByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetSchedulingIntegrationDispatchServiceLogByCompanyId");
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationDispatchServiceLog/by-companyId/{companyId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationDispatchServiceLogByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationDispatchServiceLogByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SchedulingIntegrationDispatchServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(SchedulingIntegrationDispatchServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch scheduling integration dispatch service log by Date. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="dateTimeRecorded"></param> 
        /// <returns>SchedulingIntegrationDispatchServiceLogResponse</returns>            
        public SchedulingIntegrationDispatchServiceLogResponse GetSchedulingIntegrationDispatchServiceLogByDate (int? companyId, DateTime? dateTimeRecorded)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetSchedulingIntegrationDispatchServiceLogByDate");
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationDispatchServiceLog/by-date/{companyId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (dateTimeRecorded != null) queryParams.Add("dateTimeRecorded", ApiClient.ParameterToString(dateTimeRecorded)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationDispatchServiceLogByDate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationDispatchServiceLogByDate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SchedulingIntegrationDispatchServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(SchedulingIntegrationDispatchServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch scheduling integration service log by companyId. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <returns>SchedulingIntegrationServiceLogResponse</returns>            
        public SchedulingIntegrationServiceLogResponse GetSchedulingIntegrationServiceLogByCompanyId (int? companyId)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetSchedulingIntegrationServiceLogByCompanyId");
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationServiceLog/by-companyId/{companyId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationServiceLogByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationServiceLogByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SchedulingIntegrationServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(SchedulingIntegrationServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch scheduling integration service log by date. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="dateTimeRecorded"></param> 
        /// <returns>SchedulingIntegrationServiceLogResponse</returns>            
        public SchedulingIntegrationServiceLogResponse GetSchedulingIntegrationServiceLogByDate (int? companyId, DateTime? dateTimeRecorded)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetSchedulingIntegrationServiceLogByDate");
            
            // verify the required parameter 'dateTimeRecorded' is set
            if (dateTimeRecorded == null) throw new ApiException(400, "Missing required parameter 'dateTimeRecorded' when calling GetSchedulingIntegrationServiceLogByDate");
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationServiceLog/by-date/{companyId}/{dateTimeRecorded}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
path = path.Replace("{" + "dateTimeRecorded" + "}", ApiClient.ParameterToString(dateTimeRecorded));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationServiceLogByDate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSchedulingIntegrationServiceLogByDate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SchedulingIntegrationServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(SchedulingIntegrationServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch tankering api calculation log. 
        /// </summary>
        /// <param name="companyId"></param> 
        /// <param name="startDateTime"></param> 
        /// <param name="endDateTime"></param> 
        /// <returns>TankeringApiCalculationLogResponse</returns>            
        public TankeringApiCalculationLogResponse GetTankeringApiCalculationLog (int? companyId, DateTime? startDateTime, DateTime? endDateTime)
        {
            
            // verify the required parameter 'companyId' is set
            if (companyId == null) throw new ApiException(400, "Missing required parameter 'companyId' when calling GetTankeringApiCalculationLog");
            
            // verify the required parameter 'startDateTime' is set
            if (startDateTime == null) throw new ApiException(400, "Missing required parameter 'startDateTime' when calling GetTankeringApiCalculationLog");
            
            // verify the required parameter 'endDateTime' is set
            if (endDateTime == null) throw new ApiException(400, "Missing required parameter 'endDateTime' when calling GetTankeringApiCalculationLog");
            
    
            var path = "/api/ServiceLogs/tankeringApiCalculationLog/{companyId}/{startDateTime}/{endDateTime}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "companyId" + "}", ApiClient.ParameterToString(companyId));
path = path.Replace("{" + "startDateTime" + "}", ApiClient.ParameterToString(startDateTime));
path = path.Replace("{" + "endDateTime" + "}", ApiClient.ParameterToString(endDateTime));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTankeringApiCalculationLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTankeringApiCalculationLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TankeringApiCalculationLogResponse) ApiClient.Deserialize(response.Content, typeof(TankeringApiCalculationLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post company aircraft change log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyAircraftChangeLogResponse</returns>            
        public PostCompanyAircraftChangeLogResponse PostCompanyAircraftChangeLogAsync (PostCompanyAircraftChangeLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/companyAircraftChangeLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyAircraftChangeLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyAircraftChangeLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyAircraftChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyAircraftChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post company fueler change log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostCompanyFuelerChangeLogResponse</returns>            
        public PostCompanyFuelerChangeLogResponse PostCompanyFuelerChangeLogAsync (PostCompanyFuelerChangeLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/companyFuelerChangeLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerChangeLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCompanyFuelerChangeLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostCompanyFuelerChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(PostCompanyFuelerChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post dispatch email log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostDispatchEmailLogResponse</returns>            
        public PostDispatchEmailLogResponse PostDispatchEmailLogAsync (PostDispatchEmailLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/dispatchEmailLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostDispatchEmailLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostDispatchEmailLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostDispatchEmailLogResponse) ApiClient.Deserialize(response.Content, typeof(PostDispatchEmailLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post fuel order service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostFuelOrderServiceLogResponse</returns>            
        public PostFuelOrderServiceLogResponse PostFuelOrderServiceLogAsync (PostFuelOrderServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/fuelOrderServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostFuelOrderServiceLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostFuelOrderServiceLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostFuelOrderServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(PostFuelOrderServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post fuel price service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostFuelPriceServiceLogResponse</returns>            
        public PostFuelPriceServiceLogResponse PostFuelPriceServiceLogAsync (PostFuelPriceServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/fuelPriceServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostFuelPriceServiceLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostFuelPriceServiceLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostFuelPriceServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(PostFuelPriceServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post iFlightPlanner route request service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostIFlightPlannerRouteRequestServiceLogResponse</returns>            
        public PostIFlightPlannerRouteRequestServiceLogResponse PostIFlightPlannerRouteServiceLogAsync (PostIFlightPlannerRouteRequestServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/iFlightPlannerRouteRequestServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostIFlightPlannerRouteServiceLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostIFlightPlannerRouteServiceLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostIFlightPlannerRouteRequestServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(PostIFlightPlannerRouteRequestServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post scheduling integration dispatch service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSchedulingIntegrationDispatchServiceLogResponse</returns>            
        public PostSchedulingIntegrationDispatchServiceLogResponse PostSchedulingIntegrationDispatchServiceLogAsync (PostSchedulingIntegrationDispatchServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationDispatchServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSchedulingIntegrationDispatchServiceLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSchedulingIntegrationDispatchServiceLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSchedulingIntegrationDispatchServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(PostSchedulingIntegrationDispatchServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post scheduling integration service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSchedulingIntegrationServiceLogResponse</returns>            
        public PostSchedulingIntegrationServiceLogResponse PostSchedulingIntegrationServiceLogAsync (PostSchedulingIntegrationServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSchedulingIntegrationServiceLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSchedulingIntegrationServiceLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSchedulingIntegrationServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(PostSchedulingIntegrationServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Post tankering api calculation log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTankeringApiCalculationLogResponse</returns>            
        public PostTankeringApiCalculationLogResponse PostTankeringApiCalculationLogAsync (PostTankeringApiCalculationLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/tankeringApiCalculationLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTankeringApiCalculationLogAsync: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTankeringApiCalculationLogAsync: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTankeringApiCalculationLogResponse) ApiClient.Deserialize(response.Content, typeof(PostTankeringApiCalculationLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the company aircraft change log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyAircraftChangeLogResponse</returns>            
        public UpdateCompanyAircraftChangeLogResponse UpdateCompanyAircraftChangeLog (UpdateCompanyAircraftChangeLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/companyAircraftChangeLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyAircraftChangeLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyAircraftChangeLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyAircraftChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyAircraftChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the company fueler change log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateCompanyFuelerChangeLogResponse</returns>            
        public UpdateCompanyFuelerChangeLogResponse UpdateCompanyFuelerChangeLog (UpdateCompanyFuelerChangeLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/companyFuelerChangeLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerChangeLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCompanyFuelerChangeLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateCompanyFuelerChangeLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateCompanyFuelerChangeLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the dispatch email log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateDispatchEmailLogResponse</returns>            
        public UpdateDispatchEmailLogResponse UpdateDispatchEmailLog (UpdateDispatchEmailLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/dispatchEmailLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateDispatchEmailLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateDispatchEmailLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateDispatchEmailLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateDispatchEmailLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the fuel order service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateFuelOrderServiceLogResponse</returns>            
        public UpdateFuelOrderServiceLogResponse UpdateFuelOrderServiceLog (UpdateFuelOrderServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/fuelOrderServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelOrderServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelOrderServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateFuelOrderServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateFuelOrderServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the fuel price service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateFuelPriceServiceLogResponse</returns>            
        public UpdateFuelPriceServiceLogResponse UpdateFuelPriceServiceLog (UpdateFuelPriceServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/fuelPriceServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelPriceServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateFuelPriceServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateFuelPriceServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateFuelPriceServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the iFlightPlanner route request service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateIFlightPlannerRouteRequestServiceLogResponse</returns>            
        public UpdateIFlightPlannerRouteRequestServiceLogResponse UpdateIFlightPlannerRouteServiceLog (UpdateIFlightPlannerRouteRequestServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/iFlightPlannerRouteRequestServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIFlightPlannerRouteServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateIFlightPlannerRouteServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateIFlightPlannerRouteRequestServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateIFlightPlannerRouteRequestServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the scheduling integration dispatch service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSchedulingIntegrationDispatchServiceLogResponse</returns>            
        public UpdateSchedulingIntegrationDispatchServiceLogResponse UpdateSchedulingIntegrationDispatchServiceLog (UpdateSchedulingIntegrationDispatchServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationDispatchServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSchedulingIntegrationDispatchServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSchedulingIntegrationDispatchServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSchedulingIntegrationDispatchServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSchedulingIntegrationDispatchServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the scheduling integration service log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSchedulingIntegrationServiceLogResponse</returns>            
        public UpdateSchedulingIntegrationServiceLogResponse UpdateSchedulingIntegrationServiceLog (UpdateSchedulingIntegrationServiceLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/schedulingIntegrationServiceLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSchedulingIntegrationServiceLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSchedulingIntegrationServiceLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSchedulingIntegrationServiceLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSchedulingIntegrationServiceLogResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the tankering api calculation log. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateTankeringApiCalculationLogResponse</returns>            
        public UpdateTankeringApiCalculationLogResponse UpdateTankeringApiCalculationLog (UpdateTankeringApiCalculationLogRequest body)
        {
            
    
            var path = "/api/ServiceLogs/tankeringApiCalculationLog";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTankeringApiCalculationLog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTankeringApiCalculationLog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateTankeringApiCalculationLogResponse) ApiClient.Deserialize(response.Content, typeof(UpdateTankeringApiCalculationLogResponse), response.Headers);
        }
    
    }
}

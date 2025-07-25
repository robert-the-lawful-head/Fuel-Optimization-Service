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
    public interface IAnalysisApi
    {
        /// <summary>
        /// Delete a custom report by the provided {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteReportResponse</returns>
        DeleteReportResponse DeleteCustomReport (int? id);
        /// <summary>
        /// Internal use only - Delete the subscriber record for an email blast.  This will reset the subscriber list to a company-default for the blast. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteEmailBlastEmailAddressesResponse</returns>
        DeleteEmailBlastEmailAddressesResponse DeleteEmailAddressesForEmailedAnalysis (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteReportDistributionAssociationResponse</returns>
        DeleteReportDistributionAssociationResponse DeleteReportDistributionAssociation (int? id);
        /// <summary>
        /// Delete Report Group Associations 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteReportGroupAssociationsResponse</returns>
        DeleteReportGroupAssociationsResponse DeleteReportGroupAssociations (int? id);
        /// <summary>
        /// Delete Report Groups By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteReportGroupResponse</returns>
        DeleteReportGroupResponse DeleteReportGroups (int? id);
        /// <summary>
        /// Internal use only - Delete a scheduled report distribution record. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteReportScheduledDistributionResponse</returns>
        DeleteReportScheduledDistributionResponse DeleteReportScheduledDistribution (int? id);
        /// <summary>
        /// Fetch a custom report by it&#39;s {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReportResponse</returns>
        ReportResponse GetCustomReportById (int? id);
        /// <summary>
        /// Internal use only - Fetch all subscribers for a particular email blast. 
        /// </summary>
        /// <param name="emailBlastId"></param>
        /// <returns>EmailBlastEmailAddressesResponse</returns>
        EmailBlastEmailAddressesResponse GetEmailAddressesForMonthlyAnalysis (int? emailBlastId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>IndustryAveragePriceByTransactionResponse</returns>
        IndustryAveragePriceByTransactionResponse GetIndustryAveragePriceByTransaction (int? transactionId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>ReportDataResponse</returns>
        ReportDataResponse GetReportData (ReportDataJsonRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="distributionId"></param>
        /// <returns>ReportDataResponse</returns>
        ReportDataResponse GetReportDataForDistribution (int? reportId, int? distributionId);
        /// <summary>
        /// Get Report Group Associations By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReportGroupAssociationsResponse</returns>
        ReportGroupAssociationsResponse GetReportGroupAssociationsById (int? id);
        /// <summary>
        /// Get report groups for the authenticated company 
        /// </summary>
        /// <returns>ReportGroupListResponse</returns>
        ReportGroupListResponse GetReportGroupListForCompany ();
        /// <summary>
        /// Get Report Groups By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReportGroupResponse</returns>
        ReportGroupResponse GetReportGroupsById (int? id);
        /// <summary>
        /// Fetch a list of reports for the authenticated company. 
        /// </summary>
        /// <returns>ReportListResponse</returns>
        ReportListResponse GetReportList ();
        /// <summary>
        /// Internal use only - Fetch reports scheduled for distribution by the scheduled distribution {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReportScheduledDistributionResponse</returns>
        ReportScheduledDistributionResponse GetReportScheduledDistributionById (int? id);
        /// <summary>
        /// Internal use only - Fetch reports that are scheduled for distribution. 
        /// </summary>
        /// <returns>ReportScheduledDistributionListResponse</returns>
        ReportScheduledDistributionListResponse GetReportScheduledDistributionList ();
        /// <summary>
        /// Internal use only - Fetch all reports that are scheduled for distribution and need to be sent. 
        /// </summary>
        /// <returns>ReportScheduledDistributionListResponse</returns>
        ReportScheduledDistributionListResponse GetReportScheduledDistributionListAllRequiringSending ();
        /// <summary>
        /// Add a new custom report for the authenticated company. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostReportResponse</returns>
        PostReportResponse PostCustomReport (PostReportRequest body);
        /// <summary>
        /// Internal use only - Add a new subscriber-set record to an email blast. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostEmailBlastEmailAddressesResponse</returns>
        PostEmailBlastEmailAddressesResponse PostEmailAddressesForMonthlyAnalysis (PostEmailBlastEmailAddressesRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostReportDistributionAssociationResponse</returns>
        PostReportDistributionAssociationResponse PostReportDistributionAssociation (PostReportDistributionAssociationRequest body);
        /// <summary>
        /// Post Report Group Associations 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostReportGroupAssociationsResponse</returns>
        PostReportGroupAssociationsResponse PostReportGroupAssociations (PostReportGroupAssociationsRequest body);
        /// <summary>
        /// Post Report Groups 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostReportGroupResponse</returns>
        PostReportGroupResponse PostReportGroups (PostReportGroupRequest body);
        /// <summary>
        /// Internal use only - Post a new scheduled report distribution record. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostReportScheduledDistributionResponse</returns>
        PostReportScheduledDistributionResponse PostReportScheduledDistribution (PostReportScheduledDistributionRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <returns>SendReportScheduledDistributionResponse</returns>
        SendReportScheduledDistributionResponse SendReportScheduledDistribution (SendReportScheduledDistributionRequest body);
        /// <summary>
        /// Update a custom report. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateReportResponse</returns>
        UpdateReportResponse UpdateCustomReport (UpdateReportRequest body);
        /// <summary>
        /// Internal use only - Update an existing record of subscribers for an email blast. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>UpdateEmailBlastEmailAddressesResponse</returns>
        UpdateEmailBlastEmailAddressesResponse UpdateEmailAddressesForEmailedAnalysis (int? id, UpdateEmailBlastEmailAddressesRequest body);
        /// <summary>
        /// Update Report Group Associations 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateReportGroupAssociationsResponse</returns>
        UpdateReportGroupAssociationsResponse UpdateReportGroupAssociations (UpdateReportGroupAssociationsRequest body);
        /// <summary>
        /// Update Report Groups 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateReportGroupResponse</returns>
        UpdateReportGroupResponse UpdateReportGroups (UpdateReportGroupRequest body);
        /// <summary>
        /// Internal use only - Update a scheduled report distribution record. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateReportScheduledDistributionResponse</returns>
        UpdateReportScheduledDistributionResponse UpdateReportScheduledDistribution (UpdateReportScheduledDistributionRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class AnalysisApi : IAnalysisApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public AnalysisApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisApi"/> class.
        /// </summary>
        /// <returns></returns>
        public AnalysisApi(String basePath)
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
        /// Delete a custom report by the provided {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteReportResponse</returns>            
        public DeleteReportResponse DeleteCustomReport (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteCustomReport");
            
    
            var path = "/api/Analysis/custom-reports/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCustomReport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteCustomReport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteReportResponse) ApiClient.Deserialize(response.Content, typeof(DeleteReportResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Delete the subscriber record for an email blast.  This will reset the subscriber list to a company-default for the blast. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteEmailBlastEmailAddressesResponse</returns>            
        public DeleteEmailBlastEmailAddressesResponse DeleteEmailAddressesForEmailedAnalysis (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteEmailAddressesForEmailedAnalysis");
            
    
            var path = "/api/Analysis/email-blast/email-addresses/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteEmailAddressesForEmailedAnalysis: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteEmailAddressesForEmailedAnalysis: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteEmailBlastEmailAddressesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteEmailBlastEmailAddressesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteReportDistributionAssociationResponse</returns>            
        public DeleteReportDistributionAssociationResponse DeleteReportDistributionAssociation (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteReportDistributionAssociation");
            
    
            var path = "/api/Analysis/custom-reports/distribution-association/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportDistributionAssociation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportDistributionAssociation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteReportDistributionAssociationResponse) ApiClient.Deserialize(response.Content, typeof(DeleteReportDistributionAssociationResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete Report Group Associations 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteReportGroupAssociationsResponse</returns>            
        public DeleteReportGroupAssociationsResponse DeleteReportGroupAssociations (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteReportGroupAssociations");
            
    
            var path = "/api/Analysis/report-groups-associations/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportGroupAssociations: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportGroupAssociations: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteReportGroupAssociationsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteReportGroupAssociationsResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete Report Groups By Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteReportGroupResponse</returns>            
        public DeleteReportGroupResponse DeleteReportGroups (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteReportGroups");
            
    
            var path = "/api/Analysis/report-groups/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportGroups: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportGroups: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteReportGroupResponse) ApiClient.Deserialize(response.Content, typeof(DeleteReportGroupResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Delete a scheduled report distribution record. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteReportScheduledDistributionResponse</returns>            
        public DeleteReportScheduledDistributionResponse DeleteReportScheduledDistribution (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteReportScheduledDistribution");
            
    
            var path = "/api/Analysis/custom-reports/distribution/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportScheduledDistribution: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteReportScheduledDistribution: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteReportScheduledDistributionResponse) ApiClient.Deserialize(response.Content, typeof(DeleteReportScheduledDistributionResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a custom report by it&#39;s {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>ReportResponse</returns>            
        public ReportResponse GetCustomReportById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetCustomReportById");
            
    
            var path = "/api/Analysis/custom-reports/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomReportById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetCustomReportById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportResponse) ApiClient.Deserialize(response.Content, typeof(ReportResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all subscribers for a particular email blast. 
        /// </summary>
        /// <param name="emailBlastId"></param> 
        /// <returns>EmailBlastEmailAddressesResponse</returns>            
        public EmailBlastEmailAddressesResponse GetEmailAddressesForMonthlyAnalysis (int? emailBlastId)
        {
            
            // verify the required parameter 'emailBlastId' is set
            if (emailBlastId == null) throw new ApiException(400, "Missing required parameter 'emailBlastId' when calling GetEmailAddressesForMonthlyAnalysis");
            
    
            var path = "/api/Analysis/email-blast/{emailBlastId}/email-addresses";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "emailBlastId" + "}", ApiClient.ParameterToString(emailBlastId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetEmailAddressesForMonthlyAnalysis: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetEmailAddressesForMonthlyAnalysis: " + response.ErrorMessage, response.ErrorMessage);
    
            return (EmailBlastEmailAddressesResponse) ApiClient.Deserialize(response.Content, typeof(EmailBlastEmailAddressesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>IndustryAveragePriceByTransactionResponse</returns>            
        public IndustryAveragePriceByTransactionResponse GetIndustryAveragePriceByTransaction (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling GetIndustryAveragePriceByTransaction");
            
    
            var path = "/api/Analysis/industry-average/by-transaction/{transactionId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetIndustryAveragePriceByTransaction: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetIndustryAveragePriceByTransaction: " + response.ErrorMessage, response.ErrorMessage);
    
            return (IndustryAveragePriceByTransactionResponse) ApiClient.Deserialize(response.Content, typeof(IndustryAveragePriceByTransactionResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>ReportDataResponse</returns>            
        public ReportDataResponse GetReportData (ReportDataJsonRequest body)
        {
            
    
            var path = "/api/Analysis/custom-reports/data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportDataResponse) ApiClient.Deserialize(response.Content, typeof(ReportDataResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="reportId"></param> 
        /// <param name="distributionId"></param> 
        /// <returns>ReportDataResponse</returns>            
        public ReportDataResponse GetReportDataForDistribution (int? reportId, int? distributionId)
        {
            
            // verify the required parameter 'reportId' is set
            if (reportId == null) throw new ApiException(400, "Missing required parameter 'reportId' when calling GetReportDataForDistribution");
            
            // verify the required parameter 'distributionId' is set
            if (distributionId == null) throw new ApiException(400, "Missing required parameter 'distributionId' when calling GetReportDataForDistribution");
            
    
            var path = "/api/Analysis/custom-reports/data/{reportId}/distribution/{distributionId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "reportId" + "}", ApiClient.ParameterToString(reportId));
path = path.Replace("{" + "distributionId" + "}", ApiClient.ParameterToString(distributionId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportDataForDistribution: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportDataForDistribution: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportDataResponse) ApiClient.Deserialize(response.Content, typeof(ReportDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Get Report Group Associations By Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>ReportGroupAssociationsResponse</returns>            
        public ReportGroupAssociationsResponse GetReportGroupAssociationsById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetReportGroupAssociationsById");
            
    
            var path = "/api/Analysis/report-groups-associations/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportGroupAssociationsById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportGroupAssociationsById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportGroupAssociationsResponse) ApiClient.Deserialize(response.Content, typeof(ReportGroupAssociationsResponse), response.Headers);
        }
    
        /// <summary>
        /// Get report groups for the authenticated company 
        /// </summary>
        /// <returns>ReportGroupListResponse</returns>            
        public ReportGroupListResponse GetReportGroupListForCompany ()
        {
            
    
            var path = "/api/Analysis/report-groups/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportGroupListForCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportGroupListForCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportGroupListResponse) ApiClient.Deserialize(response.Content, typeof(ReportGroupListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get Report Groups By Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>ReportGroupResponse</returns>            
        public ReportGroupResponse GetReportGroupsById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetReportGroupsById");
            
    
            var path = "/api/Analysis/report-groups/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportGroupsById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportGroupsById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportGroupResponse) ApiClient.Deserialize(response.Content, typeof(ReportGroupResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a list of reports for the authenticated company. 
        /// </summary>
        /// <returns>ReportListResponse</returns>            
        public ReportListResponse GetReportList ()
        {
            
    
            var path = "/api/Analysis/custom-reports/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportListResponse) ApiClient.Deserialize(response.Content, typeof(ReportListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch reports scheduled for distribution by the scheduled distribution {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>ReportScheduledDistributionResponse</returns>            
        public ReportScheduledDistributionResponse GetReportScheduledDistributionById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetReportScheduledDistributionById");
            
    
            var path = "/api/Analysis/custom-reports/distribution/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportScheduledDistributionById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportScheduledDistributionById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportScheduledDistributionResponse) ApiClient.Deserialize(response.Content, typeof(ReportScheduledDistributionResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch reports that are scheduled for distribution. 
        /// </summary>
        /// <returns>ReportScheduledDistributionListResponse</returns>            
        public ReportScheduledDistributionListResponse GetReportScheduledDistributionList ()
        {
            
    
            var path = "/api/Analysis/custom-reports/distribution/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportScheduledDistributionList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportScheduledDistributionList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportScheduledDistributionListResponse) ApiClient.Deserialize(response.Content, typeof(ReportScheduledDistributionListResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all reports that are scheduled for distribution and need to be sent. 
        /// </summary>
        /// <returns>ReportScheduledDistributionListResponse</returns>            
        public ReportScheduledDistributionListResponse GetReportScheduledDistributionListAllRequiringSending ()
        {
            
    
            var path = "/api/Analysis/custom-reports/distribution/list/all/require-sending";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportScheduledDistributionListAllRequiringSending: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportScheduledDistributionListAllRequiringSending: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportScheduledDistributionListResponse) ApiClient.Deserialize(response.Content, typeof(ReportScheduledDistributionListResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new custom report for the authenticated company. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostReportResponse</returns>            
        public PostReportResponse PostCustomReport (PostReportRequest body)
        {
            
    
            var path = "/api/Analysis/custom-reports";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostCustomReport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostCustomReport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostReportResponse) ApiClient.Deserialize(response.Content, typeof(PostReportResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a new subscriber-set record to an email blast. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostEmailBlastEmailAddressesResponse</returns>            
        public PostEmailBlastEmailAddressesResponse PostEmailAddressesForMonthlyAnalysis (PostEmailBlastEmailAddressesRequest body)
        {
            
    
            var path = "/api/Analysis/email-blast/email-addresses";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostEmailAddressesForMonthlyAnalysis: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostEmailAddressesForMonthlyAnalysis: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostEmailBlastEmailAddressesResponse) ApiClient.Deserialize(response.Content, typeof(PostEmailBlastEmailAddressesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostReportDistributionAssociationResponse</returns>            
        public PostReportDistributionAssociationResponse PostReportDistributionAssociation (PostReportDistributionAssociationRequest body)
        {
            
    
            var path = "/api/Analysis/custom-reports/distribution-association";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportDistributionAssociation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportDistributionAssociation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostReportDistributionAssociationResponse) ApiClient.Deserialize(response.Content, typeof(PostReportDistributionAssociationResponse), response.Headers);
        }
    
        /// <summary>
        /// Post Report Group Associations 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostReportGroupAssociationsResponse</returns>            
        public PostReportGroupAssociationsResponse PostReportGroupAssociations (PostReportGroupAssociationsRequest body)
        {
            
    
            var path = "/api/Analysis/report-groups-associations";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportGroupAssociations: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportGroupAssociations: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostReportGroupAssociationsResponse) ApiClient.Deserialize(response.Content, typeof(PostReportGroupAssociationsResponse), response.Headers);
        }
    
        /// <summary>
        /// Post Report Groups 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostReportGroupResponse</returns>            
        public PostReportGroupResponse PostReportGroups (PostReportGroupRequest body)
        {
            
    
            var path = "/api/Analysis/report-groups";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportGroups: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportGroups: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostReportGroupResponse) ApiClient.Deserialize(response.Content, typeof(PostReportGroupResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Post a new scheduled report distribution record. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostReportScheduledDistributionResponse</returns>            
        public PostReportScheduledDistributionResponse PostReportScheduledDistribution (PostReportScheduledDistributionRequest body)
        {
            
    
            var path = "/api/Analysis/custom-reports/distribution";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportScheduledDistribution: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostReportScheduledDistribution: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostReportScheduledDistributionResponse) ApiClient.Deserialize(response.Content, typeof(PostReportScheduledDistributionResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>SendReportScheduledDistributionResponse</returns>            
        public SendReportScheduledDistributionResponse SendReportScheduledDistribution (SendReportScheduledDistributionRequest body)
        {
            
    
            var path = "/api/Analysis/custom-reports/distribution/send";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SendReportScheduledDistribution: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SendReportScheduledDistribution: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SendReportScheduledDistributionResponse) ApiClient.Deserialize(response.Content, typeof(SendReportScheduledDistributionResponse), response.Headers);
        }
    
        /// <summary>
        /// Update a custom report. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateReportResponse</returns>            
        public UpdateReportResponse UpdateCustomReport (UpdateReportRequest body)
        {
            
    
            var path = "/api/Analysis/custom-reports";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCustomReport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateCustomReport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateReportResponse) ApiClient.Deserialize(response.Content, typeof(UpdateReportResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update an existing record of subscribers for an email blast. 
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="body"></param> 
        /// <returns>UpdateEmailBlastEmailAddressesResponse</returns>            
        public UpdateEmailBlastEmailAddressesResponse UpdateEmailAddressesForEmailedAnalysis (int? id, UpdateEmailBlastEmailAddressesRequest body)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UpdateEmailAddressesForEmailedAnalysis");
            
    
            var path = "/api/Analysis/email-blast/email-addresses/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateEmailAddressesForEmailedAnalysis: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateEmailAddressesForEmailedAnalysis: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateEmailBlastEmailAddressesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateEmailBlastEmailAddressesResponse), response.Headers);
        }
    
        /// <summary>
        /// Update Report Group Associations 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateReportGroupAssociationsResponse</returns>            
        public UpdateReportGroupAssociationsResponse UpdateReportGroupAssociations (UpdateReportGroupAssociationsRequest body)
        {
            
    
            var path = "/api/Analysis/report-groups-associations";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateReportGroupAssociations: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateReportGroupAssociations: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateReportGroupAssociationsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateReportGroupAssociationsResponse), response.Headers);
        }
    
        /// <summary>
        /// Update Report Groups 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateReportGroupResponse</returns>            
        public UpdateReportGroupResponse UpdateReportGroups (UpdateReportGroupRequest body)
        {
            
    
            var path = "/api/Analysis/report-groups";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateReportGroups: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateReportGroups: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateReportGroupResponse) ApiClient.Deserialize(response.Content, typeof(UpdateReportGroupResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update a scheduled report distribution record. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateReportScheduledDistributionResponse</returns>            
        public UpdateReportScheduledDistributionResponse UpdateReportScheduledDistribution (UpdateReportScheduledDistributionRequest body)
        {
            
    
            var path = "/api/Analysis/custom-reports/distribution";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateReportScheduledDistribution: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateReportScheduledDistribution: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateReportScheduledDistributionResponse) ApiClient.Deserialize(response.Content, typeof(UpdateReportScheduledDistributionResponse), response.Headers);
        }
    
    }
}

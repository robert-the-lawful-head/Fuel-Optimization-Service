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
        /// Fetch a list of reports for the authenticated company. 
        /// </summary>
        /// <returns>ReportListResponse</returns>
        ReportListResponse GetReportListByCompanyId ();
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
        /// Fetch a list of reports for the authenticated company. 
        /// </summary>
        /// <returns>ReportListResponse</returns>            
        public ReportListResponse GetReportListByCompanyId ()
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportListByCompanyId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetReportListByCompanyId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReportListResponse) ApiClient.Deserialize(response.Content, typeof(ReportListResponse), response.Headers);
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
    
    }
}

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
    public interface IJobQueueApi
    {
        /// <summary>
        /// Delete job queue by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteJobQueueResponse</returns>
        DeleteJobQueueResponse DeleteJobQueue (int? id);
        /// <summary>
        /// Delete job queue files by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteJobQueueFilesResponse</returns>
        DeleteJobQueueFilesResponse DeleteJobQueueFiles (int? id);
        /// <summary>
        /// Deletes job queue result events by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteJobQueueResultEventsResponse</returns>
        DeleteJobQueueResultEventsResponse DeleteJobQueueResultEvents (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteJobQueueResultsResponse</returns>
        DeleteJobQueueResultsResponse DeleteJobQueueResults (int? id);
        /// <summary>
        /// Get job queue by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JobQueueResponse</returns>
        JobQueueResponse GetJobQueue (int? id);
        /// <summary>
        /// Get job queue files by jobQueueId 
        /// </summary>
        /// <param name="jobQueueId"></param>
        /// <returns>JobQueueFilesResponse</returns>
        JobQueueFilesResponse GetJobQueueFiles (int? jobQueueId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="startDateUtc"></param>
        /// <param name="endDateUtc"></param>
        /// <returns>JobQueueListResponse</returns>
        JobQueueListResponse GetJobQueueListByDateRange (DateTime? startDateUtc, DateTime? endDateUtc);
        /// <summary>
        /// Get job queue result events by jobQueueResultId 
        /// </summary>
        /// <param name="jobQueueResultId"></param>
        /// <returns>JobQueueResultEventsResponse</returns>
        JobQueueResultEventsResponse GetJobQueueResultEvents (int? jobQueueResultId);
        /// <summary>
        /// Get job queue results by jobQueueId 
        /// </summary>
        /// <param name="jobQueueId"></param>
        /// <returns>JobQueueResultsResponse</returns>
        JobQueueResultsResponse GetJobQueueResults (int? jobQueueId);
        /// <summary>
        /// Post job queue 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostJobQueueResponse</returns>
        PostJobQueueResponse PostJobQueue (PostJobQueueRequest body);
        /// <summary>
        /// Post job queue files 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostJobQueueFilesResponse</returns>
        PostJobQueueFilesResponse PostJobQueueFiles (PostJobQueueFilesRequest body);
        /// <summary>
        /// Post job queue result events 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostJobQueueResultEventsResponse</returns>
        PostJobQueueResultEventsResponse PostJobQueueResultEvents (PostJobQueueResultEventsRequest body);
        /// <summary>
        /// Post job queue results 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostJobQueueResultsResponse</returns>
        PostJobQueueResultsResponse PostJobQueueResults (PostJobQueueResultsRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RunJobQueueResponse</returns>
        RunJobQueueResponse RunJobById (int? id);
        /// <summary>
        /// Update job queue 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateJobQueueResponse</returns>
        UpdateJobQueueResponse UpdateJobQueue (UpdateJobQueueRequest body);
        /// <summary>
        /// Update job queue files 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateJobQueueFilesResponse</returns>
        UpdateJobQueueFilesResponse UpdateJobQueueFiles (UpdateJobQueueFilesRequest body);
        /// <summary>
        /// Updates job queue result events 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateJobQueueResultEventsResponse</returns>
        UpdateJobQueueResultEventsResponse UpdateJobQueueResultEvents (UpdateJobQueueResultEventsRequest body);
        /// <summary>
        /// Updates job queue results 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateJobQueueResultsResponse</returns>
        UpdateJobQueueResultsResponse UpdateJobQueueResults (UpdateJobQueueResultsRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class JobQueueApi : IJobQueueApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobQueueApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public JobQueueApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="JobQueueApi"/> class.
        /// </summary>
        /// <returns></returns>
        public JobQueueApi(String basePath)
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
        /// Delete job queue by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteJobQueueResponse</returns>            
        public DeleteJobQueueResponse DeleteJobQueue (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteJobQueue");
            
    
            var path = "/api/JobQueue/job-queue/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueue: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteJobQueueResponse) ApiClient.Deserialize(response.Content, typeof(DeleteJobQueueResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete job queue files by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteJobQueueFilesResponse</returns>            
        public DeleteJobQueueFilesResponse DeleteJobQueueFiles (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteJobQueueFiles");
            
    
            var path = "/api/JobQueue/job-queue-files/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueueFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueueFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteJobQueueFilesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteJobQueueFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes job queue result events by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteJobQueueResultEventsResponse</returns>            
        public DeleteJobQueueResultEventsResponse DeleteJobQueueResultEvents (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteJobQueueResultEvents");
            
    
            var path = "/api/JobQueue/job-queue-result-events/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueueResultEvents: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueueResultEvents: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteJobQueueResultEventsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteJobQueueResultEventsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteJobQueueResultsResponse</returns>            
        public DeleteJobQueueResultsResponse DeleteJobQueueResults (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteJobQueueResults");
            
    
            var path = "/api/JobQueue/job-queue-results/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueueResults: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteJobQueueResults: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteJobQueueResultsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteJobQueueResultsResponse), response.Headers);
        }
    
        /// <summary>
        /// Get job queue by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>JobQueueResponse</returns>            
        public JobQueueResponse GetJobQueue (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetJobQueue");
            
    
            var path = "/api/JobQueue/job-queue/by-id/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueue: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobQueueResponse) ApiClient.Deserialize(response.Content, typeof(JobQueueResponse), response.Headers);
        }
    
        /// <summary>
        /// Get job queue files by jobQueueId 
        /// </summary>
        /// <param name="jobQueueId"></param> 
        /// <returns>JobQueueFilesResponse</returns>            
        public JobQueueFilesResponse GetJobQueueFiles (int? jobQueueId)
        {
            
            // verify the required parameter 'jobQueueId' is set
            if (jobQueueId == null) throw new ApiException(400, "Missing required parameter 'jobQueueId' when calling GetJobQueueFiles");
            
    
            var path = "/api/JobQueue/job-queue-files/by-jobQueueId/{jobQueueId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "jobQueueId" + "}", ApiClient.ParameterToString(jobQueueId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobQueueFilesResponse) ApiClient.Deserialize(response.Content, typeof(JobQueueFilesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="startDateUtc"></param> 
        /// <param name="endDateUtc"></param> 
        /// <returns>JobQueueListResponse</returns>            
        public JobQueueListResponse GetJobQueueListByDateRange (DateTime? startDateUtc, DateTime? endDateUtc)
        {
            
    
            var path = "/api/JobQueue/job-queue/by-date-range";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (startDateUtc != null) queryParams.Add("startDateUtc", ApiClient.ParameterToString(startDateUtc)); // query parameter
 if (endDateUtc != null) queryParams.Add("endDateUtc", ApiClient.ParameterToString(endDateUtc)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueListByDateRange: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueListByDateRange: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobQueueListResponse) ApiClient.Deserialize(response.Content, typeof(JobQueueListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get job queue result events by jobQueueResultId 
        /// </summary>
        /// <param name="jobQueueResultId"></param> 
        /// <returns>JobQueueResultEventsResponse</returns>            
        public JobQueueResultEventsResponse GetJobQueueResultEvents (int? jobQueueResultId)
        {
            
            // verify the required parameter 'jobQueueResultId' is set
            if (jobQueueResultId == null) throw new ApiException(400, "Missing required parameter 'jobQueueResultId' when calling GetJobQueueResultEvents");
            
    
            var path = "/api/JobQueue/job-queue-result-events/by-jobQueueResultId/{jobQueueResultId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "jobQueueResultId" + "}", ApiClient.ParameterToString(jobQueueResultId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueResultEvents: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueResultEvents: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobQueueResultEventsResponse) ApiClient.Deserialize(response.Content, typeof(JobQueueResultEventsResponse), response.Headers);
        }
    
        /// <summary>
        /// Get job queue results by jobQueueId 
        /// </summary>
        /// <param name="jobQueueId"></param> 
        /// <returns>JobQueueResultsResponse</returns>            
        public JobQueueResultsResponse GetJobQueueResults (int? jobQueueId)
        {
            
            // verify the required parameter 'jobQueueId' is set
            if (jobQueueId == null) throw new ApiException(400, "Missing required parameter 'jobQueueId' when calling GetJobQueueResults");
            
    
            var path = "/api/JobQueue/job-queue-results/by-jobQueueId/{jobQueueId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "jobQueueId" + "}", ApiClient.ParameterToString(jobQueueId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueResults: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetJobQueueResults: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobQueueResultsResponse) ApiClient.Deserialize(response.Content, typeof(JobQueueResultsResponse), response.Headers);
        }
    
        /// <summary>
        /// Post job queue 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostJobQueueResponse</returns>            
        public PostJobQueueResponse PostJobQueue (PostJobQueueRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueue: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostJobQueueResponse) ApiClient.Deserialize(response.Content, typeof(PostJobQueueResponse), response.Headers);
        }
    
        /// <summary>
        /// Post job queue files 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostJobQueueFilesResponse</returns>            
        public PostJobQueueFilesResponse PostJobQueueFiles (PostJobQueueFilesRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue-files";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueueFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueueFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostJobQueueFilesResponse) ApiClient.Deserialize(response.Content, typeof(PostJobQueueFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Post job queue result events 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostJobQueueResultEventsResponse</returns>            
        public PostJobQueueResultEventsResponse PostJobQueueResultEvents (PostJobQueueResultEventsRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue-result-events";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueueResultEvents: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueueResultEvents: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostJobQueueResultEventsResponse) ApiClient.Deserialize(response.Content, typeof(PostJobQueueResultEventsResponse), response.Headers);
        }
    
        /// <summary>
        /// Post job queue results 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostJobQueueResultsResponse</returns>            
        public PostJobQueueResultsResponse PostJobQueueResults (PostJobQueueResultsRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue-results";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueueResults: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostJobQueueResults: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostJobQueueResultsResponse) ApiClient.Deserialize(response.Content, typeof(PostJobQueueResultsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>RunJobQueueResponse</returns>            
        public RunJobQueueResponse RunJobById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling RunJobById");
            
    
            var path = "/api/JobQueue/job-queue/run/{id}";
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling RunJobById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling RunJobById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RunJobQueueResponse) ApiClient.Deserialize(response.Content, typeof(RunJobQueueResponse), response.Headers);
        }
    
        /// <summary>
        /// Update job queue 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateJobQueueResponse</returns>            
        public UpdateJobQueueResponse UpdateJobQueue (UpdateJobQueueRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueue: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateJobQueueResponse) ApiClient.Deserialize(response.Content, typeof(UpdateJobQueueResponse), response.Headers);
        }
    
        /// <summary>
        /// Update job queue files 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateJobQueueFilesResponse</returns>            
        public UpdateJobQueueFilesResponse UpdateJobQueueFiles (UpdateJobQueueFilesRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue-files";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueueFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueueFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateJobQueueFilesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateJobQueueFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates job queue result events 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateJobQueueResultEventsResponse</returns>            
        public UpdateJobQueueResultEventsResponse UpdateJobQueueResultEvents (UpdateJobQueueResultEventsRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue-result-events";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueueResultEvents: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueueResultEvents: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateJobQueueResultEventsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateJobQueueResultEventsResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates job queue results 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateJobQueueResultsResponse</returns>            
        public UpdateJobQueueResultsResponse UpdateJobQueueResults (UpdateJobQueueResultsRequest body)
        {
            
    
            var path = "/api/JobQueue/job-queue-results";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueueResults: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJobQueueResults: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateJobQueueResultsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateJobQueueResultsResponse), response.Headers);
        }
    
    }
}

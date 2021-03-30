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
    public interface IInvoiceManagementApi
    {
        /// <summary>
        /// Deletes Supported Invoice Import File Tests by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSupportedInvoiceImportFileTestsResponse</returns>
        DeleteSupportedInvoiceImportFileTestsResponse DeleteSupportedInvoiceImportFileTests (int? id);
        /// <summary>
        /// Deletes Supported Invoice Import Files 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSupportedInvoiceImportFilesResponse</returns>
        DeleteSupportedInvoiceImportFilesResponse DeleteSupportedInvoiceImportFiles (int? id);
        /// <summary>
        /// Get Supported Invoice Import File Tests By supportedInvoiceImportFileId 
        /// </summary>
        /// <param name="supportedInvoiceImportFileId"></param>
        /// <returns>SupportedInvoiceImportFileTestsResponse</returns>
        SupportedInvoiceImportFileTestsResponse GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId (int? supportedInvoiceImportFileId);
        /// <summary>
        /// Gets Supported Invoice Import Files By FuelVendorId 
        /// </summary>
        /// <param name="fuelVendorId"></param>
        /// <returns>SupportedInvoiceImportFilesResponse</returns>
        SupportedInvoiceImportFilesResponse GetSupportedInvoiceImportFilesByFuelVendorId (int? fuelVendorId);
        /// <summary>
        /// Gets Supported Invoice Import Files By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SupportedInvoiceImportFilesResponse</returns>
        SupportedInvoiceImportFilesResponse GetSupportedInvoiceImportFilesById (int? id);
        /// <summary>
        /// Post Supported Invoice Import File Tests 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSupportedInvoiceImportFileTestsResponse</returns>
        PostSupportedInvoiceImportFileTestsResponse PostSupportedInvoiceImportFileTests (PostSupportedInvoiceImportFileTestsRequest body);
        /// <summary>
        /// Post Supported Invoice Import Files 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSupportedInvoiceImportFilesResponse</returns>
        PostSupportedInvoiceImportFilesResponse PostSupportedInvoiceImportFiles (PostSupportedInvoiceImportFilesRequest body);
        /// <summary>
        /// Updates Supported Invoice Import File Tests 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSupportedInvoiceImportFileTestsResponse</returns>
        UpdateSupportedInvoiceImportFileTestsResponse UpdateSupportedInvoiceImportFileTests (UpdateSupportedInvoiceImportFileTestsRequest body);
        /// <summary>
        /// Updates Supported Invoice Import Files 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSupportedInvoiceImportFilesResponse</returns>
        UpdateSupportedInvoiceImportFilesResponse UpdateSupportedInvoiceImportFiles (UpdateSupportedInvoiceImportFilesRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class InvoiceManagementApi : IInvoiceManagementApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceManagementApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public InvoiceManagementApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceManagementApi"/> class.
        /// </summary>
        /// <returns></returns>
        public InvoiceManagementApi(String basePath)
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
        /// Deletes Supported Invoice Import File Tests by Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSupportedInvoiceImportFileTestsResponse</returns>            
        public DeleteSupportedInvoiceImportFileTestsResponse DeleteSupportedInvoiceImportFileTests (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSupportedInvoiceImportFileTests");
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-file-tests/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedInvoiceImportFileTests: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedInvoiceImportFileTests: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSupportedInvoiceImportFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSupportedInvoiceImportFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes Supported Invoice Import Files 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSupportedInvoiceImportFilesResponse</returns>            
        public DeleteSupportedInvoiceImportFilesResponse DeleteSupportedInvoiceImportFiles (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSupportedInvoiceImportFiles");
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-files/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedInvoiceImportFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupportedInvoiceImportFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSupportedInvoiceImportFilesResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSupportedInvoiceImportFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Get Supported Invoice Import File Tests By supportedInvoiceImportFileId 
        /// </summary>
        /// <param name="supportedInvoiceImportFileId"></param> 
        /// <returns>SupportedInvoiceImportFileTestsResponse</returns>            
        public SupportedInvoiceImportFileTestsResponse GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId (int? supportedInvoiceImportFileId)
        {
            
            // verify the required parameter 'supportedInvoiceImportFileId' is set
            if (supportedInvoiceImportFileId == null) throw new ApiException(400, "Missing required parameter 'supportedInvoiceImportFileId' when calling GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId");
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-file-tests/by-supportedInvoiceImportFileId/{supportedInvoiceImportFileId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "supportedInvoiceImportFileId" + "}", ApiClient.ParameterToString(supportedInvoiceImportFileId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedInvoiceImportFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(SupportedInvoiceImportFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets Supported Invoice Import Files By FuelVendorId 
        /// </summary>
        /// <param name="fuelVendorId"></param> 
        /// <returns>SupportedInvoiceImportFilesResponse</returns>            
        public SupportedInvoiceImportFilesResponse GetSupportedInvoiceImportFilesByFuelVendorId (int? fuelVendorId)
        {
            
            // verify the required parameter 'fuelVendorId' is set
            if (fuelVendorId == null) throw new ApiException(400, "Missing required parameter 'fuelVendorId' when calling GetSupportedInvoiceImportFilesByFuelVendorId");
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-files/by-fuel-vendor/{fuelVendorId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "fuelVendorId" + "}", ApiClient.ParameterToString(fuelVendorId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceImportFilesByFuelVendorId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceImportFilesByFuelVendorId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedInvoiceImportFilesResponse) ApiClient.Deserialize(response.Content, typeof(SupportedInvoiceImportFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets Supported Invoice Import Files By Id 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>SupportedInvoiceImportFilesResponse</returns>            
        public SupportedInvoiceImportFilesResponse GetSupportedInvoiceImportFilesById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetSupportedInvoiceImportFilesById");
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-files/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceImportFilesById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupportedInvoiceImportFilesById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupportedInvoiceImportFilesResponse) ApiClient.Deserialize(response.Content, typeof(SupportedInvoiceImportFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Post Supported Invoice Import File Tests 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSupportedInvoiceImportFileTestsResponse</returns>            
        public PostSupportedInvoiceImportFileTestsResponse PostSupportedInvoiceImportFileTests (PostSupportedInvoiceImportFileTestsRequest body)
        {
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-file-tests";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedInvoiceImportFileTests: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedInvoiceImportFileTests: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSupportedInvoiceImportFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(PostSupportedInvoiceImportFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Post Supported Invoice Import Files 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSupportedInvoiceImportFilesResponse</returns>            
        public PostSupportedInvoiceImportFilesResponse PostSupportedInvoiceImportFiles (PostSupportedInvoiceImportFilesRequest body)
        {
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-files";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedInvoiceImportFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupportedInvoiceImportFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSupportedInvoiceImportFilesResponse) ApiClient.Deserialize(response.Content, typeof(PostSupportedInvoiceImportFilesResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates Supported Invoice Import File Tests 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSupportedInvoiceImportFileTestsResponse</returns>            
        public UpdateSupportedInvoiceImportFileTestsResponse UpdateSupportedInvoiceImportFileTests (UpdateSupportedInvoiceImportFileTestsRequest body)
        {
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-file-tests";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedInvoiceImportFileTests: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedInvoiceImportFileTests: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSupportedInvoiceImportFileTestsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSupportedInvoiceImportFileTestsResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates Supported Invoice Import Files 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSupportedInvoiceImportFilesResponse</returns>            
        public UpdateSupportedInvoiceImportFilesResponse UpdateSupportedInvoiceImportFiles (UpdateSupportedInvoiceImportFilesRequest body)
        {
            
    
            var path = "/api/InvoiceManagement/supported-invoice-import-files";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedInvoiceImportFiles: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupportedInvoiceImportFiles: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSupportedInvoiceImportFilesResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSupportedInvoiceImportFilesResponse), response.Headers);
        }
    
    }
}

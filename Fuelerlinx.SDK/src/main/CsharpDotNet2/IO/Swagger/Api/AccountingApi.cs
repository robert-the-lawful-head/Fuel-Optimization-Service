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
    public interface IAccountingApi
    {
        /// <summary>
        /// Deletes supplier-details record based on ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSupplierDetailsResponse</returns>
        DeleteSupplierDetailsResponse DeleteSupplierDetails (int? id);
        /// <summary>
        ///  
        /// </summary>
        /// <returns>OracleAccountingExportResponse</returns>
        OracleAccountingExportResponse GetPendingAccountingExport ();
        /// <summary>
        /// Fetch supplier-details for a particular FBO or Vendor based on the provided [ID]. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SupplierDetailsResponse</returns>
        SupplierDetailsResponse GetSupplierDetailsById (int? id);
        /// <summary>
        /// Adds a new record for supplier-details of an FBO or Vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSupplierDetailsResponse</returns>
        PostSupplierDetailsResponse PostSupplierDetails (PostSupplierDetailsRequest body);
        /// <summary>
        /// Updates current supplier detail record 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateSupplierDetailsResponse</returns>
        UpdateSupplierDetailsResponse UpdateSupplierDetails (UpdateSupplierDetailsRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class AccountingApi : IAccountingApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountingApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public AccountingApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountingApi"/> class.
        /// </summary>
        /// <returns></returns>
        public AccountingApi(String basePath)
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
        /// Deletes supplier-details record based on ID 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteSupplierDetailsResponse</returns>            
        public DeleteSupplierDetailsResponse DeleteSupplierDetails (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteSupplierDetails");
            
    
            var path = "/api/Accounting/supplier-details/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupplierDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteSupplierDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteSupplierDetailsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteSupplierDetailsResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <returns>OracleAccountingExportResponse</returns>            
        public OracleAccountingExportResponse GetPendingAccountingExport ()
        {
            
    
            var path = "/api/Accounting/oracle/accounting-export/pending";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPendingAccountingExport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPendingAccountingExport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (OracleAccountingExportResponse) ApiClient.Deserialize(response.Content, typeof(OracleAccountingExportResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch supplier-details for a particular FBO or Vendor based on the provided [ID]. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>SupplierDetailsResponse</returns>            
        public SupplierDetailsResponse GetSupplierDetailsById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetSupplierDetailsById");
            
    
            var path = "/api/Accounting/supplier-details/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupplierDetailsById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSupplierDetailsById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SupplierDetailsResponse) ApiClient.Deserialize(response.Content, typeof(SupplierDetailsResponse), response.Headers);
        }
    
        /// <summary>
        /// Adds a new record for supplier-details of an FBO or Vendor. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostSupplierDetailsResponse</returns>            
        public PostSupplierDetailsResponse PostSupplierDetails (PostSupplierDetailsRequest body)
        {
            
    
            var path = "/api/Accounting/supplier-details";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupplierDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostSupplierDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostSupplierDetailsResponse) ApiClient.Deserialize(response.Content, typeof(PostSupplierDetailsResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates current supplier detail record 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateSupplierDetailsResponse</returns>            
        public UpdateSupplierDetailsResponse UpdateSupplierDetails (UpdateSupplierDetailsRequest body)
        {
            
    
            var path = "/api/Accounting/supplier-details";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupplierDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSupplierDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateSupplierDetailsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateSupplierDetailsResponse), response.Headers);
        }
    
    }
}

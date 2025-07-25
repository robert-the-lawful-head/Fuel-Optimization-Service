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
        /// Get Sage Credentials 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>CheckAuthorizationInfoResponse</returns>
        CheckAuthorizationInfoResponse CheckSageCredentials (SageCredentialsRequest body);
        /// <summary>
        /// Deletes accounting contract mappings record based on ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteAccountingContractMappingsResponse</returns>
        DeleteAccountingContractMappingsResponse DeleteAccountingContractMappings (int? id);
        /// <summary>
        /// Deletes accounting integration item code record based on ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteAccountingIntegrationItemCodesDetailsResponse</returns>
        DeleteAccountingIntegrationItemCodesDetailsResponse DeleteAccountingIntegrationItemCodes (int? id);
        /// <summary>
        /// Deletes supplier-details record based on ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteSupplierDetailsResponse</returns>
        DeleteSupplierDetailsResponse DeleteSupplierDetails (int? id);
        /// <summary>
        /// Gets accounting contract mappings by company Id 
        /// </summary>
        /// <returns>AccountingContractMappingsResponse</returns>
        AccountingContractMappingsResponse GetAccountingContractMappingList ();
        /// <summary>
        /// Gets accounting contract mappings by comma-delimited {transactionIds} 
        /// </summary>
        /// <param name="transactionIds"></param>
        /// <returns>AccountingContractMappingsResponse</returns>
        AccountingContractMappingsResponse GetAccountingContractMappingListForTransactions (string transactionIds);
        /// <summary>
        /// Get department list from the company&#39;s accounting integration 
        /// </summary>
        /// <returns>AccountingDepartmentListResponse</returns>
        AccountingDepartmentListResponse GetAccountingDepartmentList ();
        /// <summary>
        /// Gets single accounting integration item code record 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>AccountingIntegrationItemCodesDetailsResponse</returns>
        AccountingIntegrationItemCodesDetailsResponse GetAccountingIntegrationItemCodesById (int? id);
        /// <summary>
        /// Gets accounting integration mappings for line items 
        /// </summary>
        /// <returns>AccountingIntegrationItemCodesListResponse</returns>
        AccountingIntegrationItemCodesListResponse GetAccountingItemMappingList ();
        /// <summary>
        /// Gets accounting integration mappings for line items by comma-delimited {transactionIds} 
        /// </summary>
        /// <param name="transactionIds"></param>
        /// <returns>AccountingIntegrationItemCodesListResponse</returns>
        AccountingIntegrationItemCodesListResponse GetAccountingItemMappingListForTransactions (string transactionIds);
        /// <summary>
        /// Get GL account details from the accounting integration if available 
        /// </summary>
        /// <returns>AccountingGeneralLedgerListResponse</returns>
        AccountingGeneralLedgerListResponse GetGeneralLedgerAccountsFromIntegration ();
        /// <summary>
        ///  
        /// </summary>
        /// <returns>OracleAccountingExportResponse</returns>
        OracleAccountingExportResponse GetPendingAccountingExport ();
        /// <summary>
        /// Get Sage Credentials 
        /// </summary>
        /// <returns>AuthorizationInfoResponse</returns>
        AuthorizationInfoResponse GetSageCredentials ();
        /// <summary>
        /// Fetch supplier-details for a particular FBO or Vendor based on the provided [ID]. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SupplierDetailsResponse</returns>
        SupplierDetailsResponse GetSupplierDetailsById (int? id);
        /// <summary>
        /// Get GL vendors from the accounting integration if available 
        /// </summary>
        /// <returns>AccountingVendorListResponse</returns>
        AccountingVendorListResponse GetVendorAccountsFromIntegration ();
        /// <summary>
        /// Adds new accounting contract mapping record 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostAccountingContractMappingsResponse</returns>
        PostAccountingContractMappingsResponse PostAccountingContractMappings (PostAccountingContractMappingsRequest body);
        /// <summary>
        /// Adds new accounting integration item code record 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostAccountingIntegrationItemCodesResponse</returns>
        PostAccountingIntegrationItemCodesResponse PostAccountingIntegrationItemCodesDetails (PostAccountingIntegrationItemCodesRequest body);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>AccountingBillCreationResponse</returns>
        AccountingBillCreationResponse PostBillToAccounting (int? transactionId);
        /// <summary>
        /// Adds a new record for supplier-details of an FBO or Vendor. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostSupplierDetailsResponse</returns>
        PostSupplierDetailsResponse PostSupplierDetails (PostSupplierDetailsRequest body);
        /// <summary>
        /// Updates accounting contract mapping record 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateAccountingContractMappingsResponse</returns>
        UpdateAccountingContractMappingsResponse UpdateAccountingContractMappings (UpdateAccountingContractMappingsRequest body);
        /// <summary>
        /// Updates accounting integration item code record 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostAccountingIntegrationItemCodesResponse</returns>
        PostAccountingIntegrationItemCodesResponse UpdateAccountingIntegrationItemCodesDetails (UpdateAccountingIntegrationItemCodesDetailsRequest body);
        /// <summary>
        /// Get Sage Credentials 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>AuthorizationInfoResponse</returns>
        AuthorizationInfoResponse UpdateSageCredentials (SageCredentialsRequest body);
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
        /// Get Sage Credentials 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>CheckAuthorizationInfoResponse</returns>            
        public CheckAuthorizationInfoResponse CheckSageCredentials (SageCredentialsRequest body)
        {
            
    
            var path = "/api/Accounting/sage/check-credentials";
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
                throw new ApiException ((int)response.StatusCode, "Error calling CheckSageCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CheckSageCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CheckAuthorizationInfoResponse) ApiClient.Deserialize(response.Content, typeof(CheckAuthorizationInfoResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes accounting contract mappings record based on ID 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteAccountingContractMappingsResponse</returns>            
        public DeleteAccountingContractMappingsResponse DeleteAccountingContractMappings (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteAccountingContractMappings");
            
    
            var path = "/api/Accounting/accounting-contract-mappings/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAccountingContractMappings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAccountingContractMappings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteAccountingContractMappingsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteAccountingContractMappingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Deletes accounting integration item code record based on ID 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteAccountingIntegrationItemCodesDetailsResponse</returns>            
        public DeleteAccountingIntegrationItemCodesDetailsResponse DeleteAccountingIntegrationItemCodes (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteAccountingIntegrationItemCodes");
            
    
            var path = "/api/Accounting/integration-item-codes/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAccountingIntegrationItemCodes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAccountingIntegrationItemCodes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteAccountingIntegrationItemCodesDetailsResponse) ApiClient.Deserialize(response.Content, typeof(DeleteAccountingIntegrationItemCodesDetailsResponse), response.Headers);
        }
    
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
        /// Gets accounting contract mappings by company Id 
        /// </summary>
        /// <returns>AccountingContractMappingsResponse</returns>            
        public AccountingContractMappingsResponse GetAccountingContractMappingList ()
        {
            
    
            var path = "/api/Accounting/accounting-contract-mappings/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingContractMappingList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingContractMappingList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingContractMappingsResponse) ApiClient.Deserialize(response.Content, typeof(AccountingContractMappingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets accounting contract mappings by comma-delimited {transactionIds} 
        /// </summary>
        /// <param name="transactionIds"></param> 
        /// <returns>AccountingContractMappingsResponse</returns>            
        public AccountingContractMappingsResponse GetAccountingContractMappingListForTransactions (string transactionIds)
        {
            
            // verify the required parameter 'transactionIds' is set
            if (transactionIds == null) throw new ApiException(400, "Missing required parameter 'transactionIds' when calling GetAccountingContractMappingListForTransactions");
            
    
            var path = "/api/Accounting/accounting-contract-mappings/list/{transactionIds}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "transactionIds" + "}", ApiClient.ParameterToString(transactionIds));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingContractMappingListForTransactions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingContractMappingListForTransactions: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingContractMappingsResponse) ApiClient.Deserialize(response.Content, typeof(AccountingContractMappingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Get department list from the company&#39;s accounting integration 
        /// </summary>
        /// <returns>AccountingDepartmentListResponse</returns>            
        public AccountingDepartmentListResponse GetAccountingDepartmentList ()
        {
            
    
            var path = "/api/Accounting/department/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingDepartmentList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingDepartmentList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingDepartmentListResponse) ApiClient.Deserialize(response.Content, typeof(AccountingDepartmentListResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets single accounting integration item code record 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>AccountingIntegrationItemCodesDetailsResponse</returns>            
        public AccountingIntegrationItemCodesDetailsResponse GetAccountingIntegrationItemCodesById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetAccountingIntegrationItemCodesById");
            
    
            var path = "/api/Accounting/integration-item-codes/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingIntegrationItemCodesById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingIntegrationItemCodesById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingIntegrationItemCodesDetailsResponse) ApiClient.Deserialize(response.Content, typeof(AccountingIntegrationItemCodesDetailsResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets accounting integration mappings for line items 
        /// </summary>
        /// <returns>AccountingIntegrationItemCodesListResponse</returns>            
        public AccountingIntegrationItemCodesListResponse GetAccountingItemMappingList ()
        {
            
    
            var path = "/api/Accounting/mapping/items/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingItemMappingList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingItemMappingList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingIntegrationItemCodesListResponse) ApiClient.Deserialize(response.Content, typeof(AccountingIntegrationItemCodesListResponse), response.Headers);
        }
    
        /// <summary>
        /// Gets accounting integration mappings for line items by comma-delimited {transactionIds} 
        /// </summary>
        /// <param name="transactionIds"></param> 
        /// <returns>AccountingIntegrationItemCodesListResponse</returns>            
        public AccountingIntegrationItemCodesListResponse GetAccountingItemMappingListForTransactions (string transactionIds)
        {
            
            // verify the required parameter 'transactionIds' is set
            if (transactionIds == null) throw new ApiException(400, "Missing required parameter 'transactionIds' when calling GetAccountingItemMappingListForTransactions");
            
    
            var path = "/api/Accounting/mapping/items/list/{transactionIds}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "transactionIds" + "}", ApiClient.ParameterToString(transactionIds));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingItemMappingListForTransactions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAccountingItemMappingListForTransactions: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingIntegrationItemCodesListResponse) ApiClient.Deserialize(response.Content, typeof(AccountingIntegrationItemCodesListResponse), response.Headers);
        }
    
        /// <summary>
        /// Get GL account details from the accounting integration if available 
        /// </summary>
        /// <returns>AccountingGeneralLedgerListResponse</returns>            
        public AccountingGeneralLedgerListResponse GetGeneralLedgerAccountsFromIntegration ()
        {
            
    
            var path = "/api/Accounting/integration/gl-accounts/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetGeneralLedgerAccountsFromIntegration: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetGeneralLedgerAccountsFromIntegration: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingGeneralLedgerListResponse) ApiClient.Deserialize(response.Content, typeof(AccountingGeneralLedgerListResponse), response.Headers);
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
        /// Get Sage Credentials 
        /// </summary>
        /// <returns>AuthorizationInfoResponse</returns>            
        public AuthorizationInfoResponse GetSageCredentials ()
        {
            
    
            var path = "/api/Accounting/sage/credentials";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetSageCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetSageCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AuthorizationInfoResponse) ApiClient.Deserialize(response.Content, typeof(AuthorizationInfoResponse), response.Headers);
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
        /// Get GL vendors from the accounting integration if available 
        /// </summary>
        /// <returns>AccountingVendorListResponse</returns>            
        public AccountingVendorListResponse GetVendorAccountsFromIntegration ()
        {
            
    
            var path = "/api/Accounting/integration/vendor-accounts/list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetVendorAccountsFromIntegration: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetVendorAccountsFromIntegration: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingVendorListResponse) ApiClient.Deserialize(response.Content, typeof(AccountingVendorListResponse), response.Headers);
        }
    
        /// <summary>
        /// Adds new accounting contract mapping record 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostAccountingContractMappingsResponse</returns>            
        public PostAccountingContractMappingsResponse PostAccountingContractMappings (PostAccountingContractMappingsRequest body)
        {
            
    
            var path = "/api/Accounting/accounting-contract-mappings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostAccountingContractMappings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostAccountingContractMappings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostAccountingContractMappingsResponse) ApiClient.Deserialize(response.Content, typeof(PostAccountingContractMappingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Adds new accounting integration item code record 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostAccountingIntegrationItemCodesResponse</returns>            
        public PostAccountingIntegrationItemCodesResponse PostAccountingIntegrationItemCodesDetails (PostAccountingIntegrationItemCodesRequest body)
        {
            
    
            var path = "/api/Accounting/integration-item-codes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostAccountingIntegrationItemCodesDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostAccountingIntegrationItemCodesDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostAccountingIntegrationItemCodesResponse) ApiClient.Deserialize(response.Content, typeof(PostAccountingIntegrationItemCodesResponse), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>AccountingBillCreationResponse</returns>            
        public AccountingBillCreationResponse PostBillToAccounting (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling PostBillToAccounting");
            
    
            var path = "/api/Accounting/integration/bill/{transactionId}";
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PostBillToAccounting: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostBillToAccounting: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AccountingBillCreationResponse) ApiClient.Deserialize(response.Content, typeof(AccountingBillCreationResponse), response.Headers);
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
        /// Updates accounting contract mapping record 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateAccountingContractMappingsResponse</returns>            
        public UpdateAccountingContractMappingsResponse UpdateAccountingContractMappings (UpdateAccountingContractMappingsRequest body)
        {
            
    
            var path = "/api/Accounting/accounting-contract-mappings";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAccountingContractMappings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAccountingContractMappings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateAccountingContractMappingsResponse) ApiClient.Deserialize(response.Content, typeof(UpdateAccountingContractMappingsResponse), response.Headers);
        }
    
        /// <summary>
        /// Updates accounting integration item code record 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostAccountingIntegrationItemCodesResponse</returns>            
        public PostAccountingIntegrationItemCodesResponse UpdateAccountingIntegrationItemCodesDetails (UpdateAccountingIntegrationItemCodesDetailsRequest body)
        {
            
    
            var path = "/api/Accounting/integration-item-codes";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAccountingIntegrationItemCodesDetails: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAccountingIntegrationItemCodesDetails: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostAccountingIntegrationItemCodesResponse) ApiClient.Deserialize(response.Content, typeof(PostAccountingIntegrationItemCodesResponse), response.Headers);
        }
    
        /// <summary>
        /// Get Sage Credentials 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>AuthorizationInfoResponse</returns>            
        public AuthorizationInfoResponse UpdateSageCredentials (SageCredentialsRequest body)
        {
            
    
            var path = "/api/Accounting/sage/update-credentials";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSageCredentials: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateSageCredentials: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AuthorizationInfoResponse) ApiClient.Deserialize(response.Content, typeof(AuthorizationInfoResponse), response.Headers);
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

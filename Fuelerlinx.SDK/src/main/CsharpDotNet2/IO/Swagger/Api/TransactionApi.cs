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
    public interface ITransactionApi
    {
        /// <summary>
        /// Internal use only - Delete a file captured by an invoice import. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteAutoReconciledFileResponse</returns>
        DeleteAutoReconciledFileResponse DeleteAutoReconciledFile (int? id);
        /// <summary>
        /// Internal use only - Delete an invoice import (this will not UNDO the invoice information imported - that will be included in a different method). 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>DeleteAutoReconProcessResponse</returns>
        DeleteAutoReconProcessResponse DeleteInvoiceImport (int? processId);
        /// <summary>
        /// Internal use only - Delete the accounting transfer status with the provided {id}. 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>DeleteTransactionAccountingTransferResponse</returns>
        DeleteTransactionAccountingTransferResponse DeleteTransactionAccountTransferStatus (int? transactionId);
        /// <summary>
        /// Delete accounting data associated with a particular transaction. 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>DeleteTransactionAccountingDataResponse</returns>
        DeleteTransactionAccountingDataResponse DeleteTransactionAccountingData (int? transactionId);
        /// <summary>
        /// Delete an attachment record from a transaction by the attachment&#39;s {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteTransactionAttachmentResponse</returns>
        DeleteTransactionAttachmentResponse DeleteTransactionAttachment (int? id);
        /// <summary>
        /// Delete an existing note from a transaction by it&#39;s {id}.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DeleteTransactionNoteResponse</returns>
        DeleteTransactionNoteResponse DeleteTransactionNote (int? id);
        /// <summary>
        /// Internal use only - Fetch all files captured during an invoice import by the process ID. 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>AutoReconciledFileResponse</returns>
        AutoReconciledFileResponse GetAutoReconciledFile (int? processId);
        /// <summary>
        /// Internal use only - Fetch an invoice import by the process ID. 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>AutoReconProcessResponse</returns>
        AutoReconProcessResponse GetInvoiceImportByProcessId (int? processId);
        /// <summary>
        /// Internal use only - Fetch all pending invoice imports for a company. 
        /// </summary>
        /// <returns>AutoReconProcessListResponse</returns>
        AutoReconProcessListResponse GetPendingInvoiceImportsByCompany ();
        /// <summary>
        /// Fetch accounting data associated with the provided {transactionId}. 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>TransactionAccountingDataResponse</returns>
        TransactionAccountingDataResponse GetTransactionAccountingDataByTransactionId (int? transactionId);
        /// <summary>
        /// Internal use only - Get the accounting transfer status of the specified {transactionId}. 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>TransactionAccountingTransferResponse</returns>
        TransactionAccountingTransferResponse GetTransactionAccountingTransferStatus (int? transactionId);
        /// <summary>
        /// Fetch a transaction attachment by the provided {id}. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TransactionAttachmentResponse</returns>
        TransactionAttachmentResponse GetTransactionAttachment (int? id);
        /// <summary>
        /// Get file data (base64 string) for a particular attachment tied to a transaction.  The [attachmentId] parameter can be found in each TransactionAttachment in the transactionAttachments array of a transaction. This method is not async as this capability is currently unavailable for byte arrays.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns>TransactionAttachmentFileDataResponse</returns>
        TransactionAttachmentFileDataResponse GetTransactionAttachmentFileDataById (int? attachmentId);
        /// <summary>
        /// Fetch a transaction by it&#39;s Id.  This will include fuel data, fees/services, invoice information, and scheduling data associated with the transaction. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TransactionResponse</returns>
        TransactionResponse GetTransactionById (int? id);
        /// <summary>
        /// Fetch all prices quoted for a particular transaction. 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>TransactionFuelPriceResponse</returns>
        TransactionFuelPriceResponse GetTransactionFuelPrices (int? transactionId);
        /// <summary>
        /// Get all general transaction information for the specified date range.  This can include both fuel orders and service-only transactions. 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="processId"></param>
        /// <returns>TransactionGeneralInfoResponse</returns>
        TransactionGeneralInfoResponse GetTransactionGeneralInfoByDateRange (DateTime? startDate, DateTime? endDate, int? processId);
        /// <summary>
        /// Fetch a transaction note by the provided {id} 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TransactionNoteResponse</returns>
        TransactionNoteResponse GetTransactionNote (int? id);
        /// <summary>
        /// Get all transactions for the specified airport, tail, and date range.  This can include both fuel orders and service-only transactions. Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).
        /// </summary>
        /// <param name="airportIdentifier"></param>
        /// <param name="tailNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>TransactionsResponse</returns>
        TransactionsResponse GetTransactionsByAirportAndTailNumber (string airportIdentifier, string tailNumber, DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// Get all transactions for the specified date range.  This can include both fuel orders and service-only transactions. Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>TransactionsResponse</returns>
        TransactionsResponse GetTransactionsByDateRange (DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// Get all transactions for the specified invoice number.  This can include both fuel orders and service-only transactions. Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="fuelerId"></param>
        /// <returns>TransactionsResponse</returns>
        TransactionsResponse GetTransactionsByInvoiceNumber (string invoiceNumber, int? fuelerId);
        /// <summary>
        /// Internal use only - Add a file captured by an invoice import. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostAutoReconciledFileResponse</returns>
        PostAutoReconciledFileResponse PostAutoReconciledFile (PostAutoReconciledFileRequest body);
        /// <summary>
        /// Internal use only - Add an invoice import record. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostAutoReconProcessResponse</returns>
        PostAutoReconProcessResponse PostInvoiceImport (PostAutoReconProcessRequest body);
        /// <summary>
        /// Internal use only - Post a new accounting transfer status for a particular transaction. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTransactionAccountingTransferResponse</returns>
        PostTransactionAccountingTransferResponse PostTransactionAccountTransferStatus (PostTransactionAccountingTransferRequest body);
        /// <summary>
        /// Add accounting data for a particular transaction.  The {AccountingData} object can be in any format. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTransactionAccountingDataResponse</returns>
        PostTransactionAccountingDataResponse PostTransactionAccountingData (PostTransactionAccountingDataRequest body);
        /// <summary>
        /// Add a new attachment to a transaction.  Please use the fileData API for \&quot;TransactionFileData\&quot; to first store any file data into the database and retrieve the [AttachmentFileDataId]. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTransactionAttachmentResponse</returns>
        PostTransactionAttachmentResponse PostTransactionAttachment (PostTransactionAttachmentRequest body);
        /// <summary>
        /// Add a new note to a transaction. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>PostTransactionNoteResponse</returns>
        PostTransactionNoteResponse PostTransactionNote (PostTransactionNoteRequest body);
        /// <summary>
        /// Internal use only - Update a file captured by an invoice import. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateAutoReconciledFileResponse</returns>
        UpdateAutoReconciledFileResponse UpdateAutoReconciledFile (UpdateAutoReconciledFileRequest body);
        /// <summary>
        /// Internal use only - Update an invoice import. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateAutoReconProcessResponse</returns>
        UpdateAutoReconProcessResponse UpdateInvoiceImport (UpdateAutoReconProcessRequest body);
        /// <summary>
        /// Update the accounting data record for a particular transaction. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateTransactionAccountingDataResponse</returns>
        UpdateTransactionAccountingDataResponse UpdateTransactionAccountData (UpdateTransactionAccountingDataRequest body);
        /// <summary>
        /// Internal use only - Update the accounting transfer status of a particular transaction. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateTransactionAccountingTransferResponse</returns>
        UpdateTransactionAccountingTransferResponse UpdateTransactionAccountingTransferStatus (UpdateTransactionAccountingTransferRequest body);
        /// <summary>
        /// Update an existing attachment record for a transaction. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateTransactionAttachmentResponse</returns>
        UpdateTransactionAttachmentResponse UpdateTransactionAttachment (UpdateTransactionAttachmentRequest body);
        /// <summary>
        /// Update an existing note for a transaction. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>UpdateTransactionNoteResponse</returns>
        UpdateTransactionNoteResponse UpdateTransactionNote (UpdateTransactionNoteRequest body);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class TransactionApi : ITransactionApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public TransactionApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionApi"/> class.
        /// </summary>
        /// <returns></returns>
        public TransactionApi(String basePath)
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
        /// Internal use only - Delete a file captured by an invoice import. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteAutoReconciledFileResponse</returns>            
        public DeleteAutoReconciledFileResponse DeleteAutoReconciledFile (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteAutoReconciledFile");
            
    
            var path = "/api/Transaction/invoice-import/file-capture/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAutoReconciledFile: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteAutoReconciledFile: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteAutoReconciledFileResponse) ApiClient.Deserialize(response.Content, typeof(DeleteAutoReconciledFileResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Delete an invoice import (this will not UNDO the invoice information imported - that will be included in a different method). 
        /// </summary>
        /// <param name="processId"></param> 
        /// <returns>DeleteAutoReconProcessResponse</returns>            
        public DeleteAutoReconProcessResponse DeleteInvoiceImport (int? processId)
        {
            
            // verify the required parameter 'processId' is set
            if (processId == null) throw new ApiException(400, "Missing required parameter 'processId' when calling DeleteInvoiceImport");
            
    
            var path = "/api/Transaction/invoice-import/process/{processId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "processId" + "}", ApiClient.ParameterToString(processId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteInvoiceImport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteInvoiceImport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteAutoReconProcessResponse) ApiClient.Deserialize(response.Content, typeof(DeleteAutoReconProcessResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Delete the accounting transfer status with the provided {id}. 
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>DeleteTransactionAccountingTransferResponse</returns>            
        public DeleteTransactionAccountingTransferResponse DeleteTransactionAccountTransferStatus (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling DeleteTransactionAccountTransferStatus");
            
    
            var path = "/api/Transaction/accounting-transfer/{transactionId}";
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionAccountTransferStatus: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionAccountTransferStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteTransactionAccountingTransferResponse) ApiClient.Deserialize(response.Content, typeof(DeleteTransactionAccountingTransferResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete accounting data associated with a particular transaction. 
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>DeleteTransactionAccountingDataResponse</returns>            
        public DeleteTransactionAccountingDataResponse DeleteTransactionAccountingData (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling DeleteTransactionAccountingData");
            
    
            var path = "/api/Transaction/accounting-data/{transactionId}";
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionAccountingData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionAccountingData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteTransactionAccountingDataResponse) ApiClient.Deserialize(response.Content, typeof(DeleteTransactionAccountingDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete an attachment record from a transaction by the attachment&#39;s {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteTransactionAttachmentResponse</returns>            
        public DeleteTransactionAttachmentResponse DeleteTransactionAttachment (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteTransactionAttachment");
            
    
            var path = "/api/Transaction/attachment/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionAttachment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionAttachment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteTransactionAttachmentResponse) ApiClient.Deserialize(response.Content, typeof(DeleteTransactionAttachmentResponse), response.Headers);
        }
    
        /// <summary>
        /// Delete an existing note from a transaction by it&#39;s {id}.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>DeleteTransactionNoteResponse</returns>            
        public DeleteTransactionNoteResponse DeleteTransactionNote (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling DeleteTransactionNote");
            
    
            var path = "/api/Transaction/note/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionNote: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteTransactionNote: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DeleteTransactionNoteResponse) ApiClient.Deserialize(response.Content, typeof(DeleteTransactionNoteResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all files captured during an invoice import by the process ID. 
        /// </summary>
        /// <param name="processId"></param> 
        /// <returns>AutoReconciledFileResponse</returns>            
        public AutoReconciledFileResponse GetAutoReconciledFile (int? processId)
        {
            
            // verify the required parameter 'processId' is set
            if (processId == null) throw new ApiException(400, "Missing required parameter 'processId' when calling GetAutoReconciledFile");
            
    
            var path = "/api/Transaction/invoice-import/file-capture/process/{processId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "processId" + "}", ApiClient.ParameterToString(processId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetAutoReconciledFile: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetAutoReconciledFile: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AutoReconciledFileResponse) ApiClient.Deserialize(response.Content, typeof(AutoReconciledFileResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch an invoice import by the process ID. 
        /// </summary>
        /// <param name="processId"></param> 
        /// <returns>AutoReconProcessResponse</returns>            
        public AutoReconProcessResponse GetInvoiceImportByProcessId (int? processId)
        {
            
            // verify the required parameter 'processId' is set
            if (processId == null) throw new ApiException(400, "Missing required parameter 'processId' when calling GetInvoiceImportByProcessId");
            
    
            var path = "/api/Transaction/invoice-import/process/{processId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "processId" + "}", ApiClient.ParameterToString(processId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetInvoiceImportByProcessId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetInvoiceImportByProcessId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AutoReconProcessResponse) ApiClient.Deserialize(response.Content, typeof(AutoReconProcessResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Fetch all pending invoice imports for a company. 
        /// </summary>
        /// <returns>AutoReconProcessListResponse</returns>            
        public AutoReconProcessListResponse GetPendingInvoiceImportsByCompany ()
        {
            
    
            var path = "/api/Transaction/invoice-import/pending";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetPendingInvoiceImportsByCompany: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetPendingInvoiceImportsByCompany: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AutoReconProcessListResponse) ApiClient.Deserialize(response.Content, typeof(AutoReconProcessListResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch accounting data associated with the provided {transactionId}. 
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>TransactionAccountingDataResponse</returns>            
        public TransactionAccountingDataResponse GetTransactionAccountingDataByTransactionId (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling GetTransactionAccountingDataByTransactionId");
            
    
            var path = "/api/Transaction/accounting-data/{transactionId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAccountingDataByTransactionId: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAccountingDataByTransactionId: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionAccountingDataResponse) ApiClient.Deserialize(response.Content, typeof(TransactionAccountingDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Get the accounting transfer status of the specified {transactionId}. 
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>TransactionAccountingTransferResponse</returns>            
        public TransactionAccountingTransferResponse GetTransactionAccountingTransferStatus (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling GetTransactionAccountingTransferStatus");
            
    
            var path = "/api/Transaction/accounting-transfer/{transactionId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAccountingTransferStatus: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAccountingTransferStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionAccountingTransferResponse) ApiClient.Deserialize(response.Content, typeof(TransactionAccountingTransferResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a transaction attachment by the provided {id}. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>TransactionAttachmentResponse</returns>            
        public TransactionAttachmentResponse GetTransactionAttachment (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetTransactionAttachment");
            
    
            var path = "/api/Transaction/attachment/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAttachment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAttachment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionAttachmentResponse) ApiClient.Deserialize(response.Content, typeof(TransactionAttachmentResponse), response.Headers);
        }
    
        /// <summary>
        /// Get file data (base64 string) for a particular attachment tied to a transaction.  The [attachmentId] parameter can be found in each TransactionAttachment in the transactionAttachments array of a transaction. This method is not async as this capability is currently unavailable for byte arrays.
        /// </summary>
        /// <param name="attachmentId"></param> 
        /// <returns>TransactionAttachmentFileDataResponse</returns>            
        public TransactionAttachmentFileDataResponse GetTransactionAttachmentFileDataById (int? attachmentId)
        {
            
            // verify the required parameter 'attachmentId' is set
            if (attachmentId == null) throw new ApiException(400, "Missing required parameter 'attachmentId' when calling GetTransactionAttachmentFileDataById");
            
    
            var path = "/api/Transaction/attachment/{attachmentId}/fileData";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "attachmentId" + "}", ApiClient.ParameterToString(attachmentId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAttachmentFileDataById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionAttachmentFileDataById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionAttachmentFileDataResponse) ApiClient.Deserialize(response.Content, typeof(TransactionAttachmentFileDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a transaction by it&#39;s Id.  This will include fuel data, fees/services, invoice information, and scheduling data associated with the transaction. 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>TransactionResponse</returns>            
        public TransactionResponse GetTransactionById (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetTransactionById");
            
    
            var path = "/api/Transaction/by-id/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionResponse) ApiClient.Deserialize(response.Content, typeof(TransactionResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch all prices quoted for a particular transaction. 
        /// </summary>
        /// <param name="transactionId"></param> 
        /// <returns>TransactionFuelPriceResponse</returns>            
        public TransactionFuelPriceResponse GetTransactionFuelPrices (int? transactionId)
        {
            
            // verify the required parameter 'transactionId' is set
            if (transactionId == null) throw new ApiException(400, "Missing required parameter 'transactionId' when calling GetTransactionFuelPrices");
            
    
            var path = "/api/Transaction/prices/{transactionId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionFuelPrices: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionFuelPrices: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionFuelPriceResponse) ApiClient.Deserialize(response.Content, typeof(TransactionFuelPriceResponse), response.Headers);
        }
    
        /// <summary>
        /// Get all general transaction information for the specified date range.  This can include both fuel orders and service-only transactions. 
        /// </summary>
        /// <param name="startDate"></param> 
        /// <param name="endDate"></param> 
        /// <param name="processId"></param> 
        /// <returns>TransactionGeneralInfoResponse</returns>            
        public TransactionGeneralInfoResponse GetTransactionGeneralInfoByDateRange (DateTime? startDate, DateTime? endDate, int? processId)
        {
            
    
            var path = "/api/Transaction/general-info/by-date-range";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
 if (processId != null) queryParams.Add("processId", ApiClient.ParameterToString(processId)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] { "ApiKeyScheme", "Bearer" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionGeneralInfoByDateRange: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionGeneralInfoByDateRange: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionGeneralInfoResponse) ApiClient.Deserialize(response.Content, typeof(TransactionGeneralInfoResponse), response.Headers);
        }
    
        /// <summary>
        /// Fetch a transaction note by the provided {id} 
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>TransactionNoteResponse</returns>            
        public TransactionNoteResponse GetTransactionNote (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling GetTransactionNote");
            
    
            var path = "/api/Transaction/note/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionNote: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionNote: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionNoteResponse) ApiClient.Deserialize(response.Content, typeof(TransactionNoteResponse), response.Headers);
        }
    
        /// <summary>
        /// Get all transactions for the specified airport, tail, and date range.  This can include both fuel orders and service-only transactions. Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).
        /// </summary>
        /// <param name="airportIdentifier"></param> 
        /// <param name="tailNumber"></param> 
        /// <param name="startDate"></param> 
        /// <param name="endDate"></param> 
        /// <returns>TransactionsResponse</returns>            
        public TransactionsResponse GetTransactionsByAirportAndTailNumber (string airportIdentifier, string tailNumber, DateTime? startDate, DateTime? endDate)
        {
            
            // verify the required parameter 'airportIdentifier' is set
            if (airportIdentifier == null) throw new ApiException(400, "Missing required parameter 'airportIdentifier' when calling GetTransactionsByAirportAndTailNumber");
            
            // verify the required parameter 'tailNumber' is set
            if (tailNumber == null) throw new ApiException(400, "Missing required parameter 'tailNumber' when calling GetTransactionsByAirportAndTailNumber");
            
    
            var path = "/api/Transaction/by-airport/{airportIdentifier}/tailNumber/{tailNumber}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "airportIdentifier" + "}", ApiClient.ParameterToString(airportIdentifier));
path = path.Replace("{" + "tailNumber" + "}", ApiClient.ParameterToString(tailNumber));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsByAirportAndTailNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsByAirportAndTailNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionsResponse) ApiClient.Deserialize(response.Content, typeof(TransactionsResponse), response.Headers);
        }
    
        /// <summary>
        /// Get all transactions for the specified date range.  This can include both fuel orders and service-only transactions. Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).
        /// </summary>
        /// <param name="startDate"></param> 
        /// <param name="endDate"></param> 
        /// <returns>TransactionsResponse</returns>            
        public TransactionsResponse GetTransactionsByDateRange (DateTime? startDate, DateTime? endDate)
        {
            
    
            var path = "/api/Transaction/by-date-range";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsByDateRange: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsByDateRange: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionsResponse) ApiClient.Deserialize(response.Content, typeof(TransactionsResponse), response.Headers);
        }
    
        /// <summary>
        /// Get all transactions for the specified invoice number.  This can include both fuel orders and service-only transactions. Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).
        /// </summary>
        /// <param name="invoiceNumber"></param> 
        /// <param name="fuelerId"></param> 
        /// <returns>TransactionsResponse</returns>            
        public TransactionsResponse GetTransactionsByInvoiceNumber (string invoiceNumber, int? fuelerId)
        {
            
            // verify the required parameter 'invoiceNumber' is set
            if (invoiceNumber == null) throw new ApiException(400, "Missing required parameter 'invoiceNumber' when calling GetTransactionsByInvoiceNumber");
            
            // verify the required parameter 'fuelerId' is set
            if (fuelerId == null) throw new ApiException(400, "Missing required parameter 'fuelerId' when calling GetTransactionsByInvoiceNumber");
            
    
            var path = "/api/Transaction/by-invoice-number/{invoiceNumber}/fueler/{fuelerId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "invoiceNumber" + "}", ApiClient.ParameterToString(invoiceNumber));
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsByInvoiceNumber: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetTransactionsByInvoiceNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TransactionsResponse) ApiClient.Deserialize(response.Content, typeof(TransactionsResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add a file captured by an invoice import. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostAutoReconciledFileResponse</returns>            
        public PostAutoReconciledFileResponse PostAutoReconciledFile (PostAutoReconciledFileRequest body)
        {
            
    
            var path = "/api/Transaction/invoice-import/file-capture";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostAutoReconciledFile: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostAutoReconciledFile: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostAutoReconciledFileResponse) ApiClient.Deserialize(response.Content, typeof(PostAutoReconciledFileResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Add an invoice import record. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostAutoReconProcessResponse</returns>            
        public PostAutoReconProcessResponse PostInvoiceImport (PostAutoReconProcessRequest body)
        {
            
    
            var path = "/api/Transaction/invoice-import";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostInvoiceImport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostInvoiceImport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostAutoReconProcessResponse) ApiClient.Deserialize(response.Content, typeof(PostAutoReconProcessResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Post a new accounting transfer status for a particular transaction. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTransactionAccountingTransferResponse</returns>            
        public PostTransactionAccountingTransferResponse PostTransactionAccountTransferStatus (PostTransactionAccountingTransferRequest body)
        {
            
    
            var path = "/api/Transaction/accounting-transfer";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionAccountTransferStatus: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionAccountTransferStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTransactionAccountingTransferResponse) ApiClient.Deserialize(response.Content, typeof(PostTransactionAccountingTransferResponse), response.Headers);
        }
    
        /// <summary>
        /// Add accounting data for a particular transaction.  The {AccountingData} object can be in any format. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTransactionAccountingDataResponse</returns>            
        public PostTransactionAccountingDataResponse PostTransactionAccountingData (PostTransactionAccountingDataRequest body)
        {
            
    
            var path = "/api/Transaction/accounting-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionAccountingData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionAccountingData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTransactionAccountingDataResponse) ApiClient.Deserialize(response.Content, typeof(PostTransactionAccountingDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new attachment to a transaction.  Please use the fileData API for \&quot;TransactionFileData\&quot; to first store any file data into the database and retrieve the [AttachmentFileDataId]. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTransactionAttachmentResponse</returns>            
        public PostTransactionAttachmentResponse PostTransactionAttachment (PostTransactionAttachmentRequest body)
        {
            
    
            var path = "/api/Transaction/attachment";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionAttachment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionAttachment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTransactionAttachmentResponse) ApiClient.Deserialize(response.Content, typeof(PostTransactionAttachmentResponse), response.Headers);
        }
    
        /// <summary>
        /// Add a new note to a transaction. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>PostTransactionNoteResponse</returns>            
        public PostTransactionNoteResponse PostTransactionNote (PostTransactionNoteRequest body)
        {
            
    
            var path = "/api/Transaction/note";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionNote: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PostTransactionNote: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PostTransactionNoteResponse) ApiClient.Deserialize(response.Content, typeof(PostTransactionNoteResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update a file captured by an invoice import. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateAutoReconciledFileResponse</returns>            
        public UpdateAutoReconciledFileResponse UpdateAutoReconciledFile (UpdateAutoReconciledFileRequest body)
        {
            
    
            var path = "/api/Transaction/invoice-import/file-capture";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAutoReconciledFile: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateAutoReconciledFile: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateAutoReconciledFileResponse) ApiClient.Deserialize(response.Content, typeof(UpdateAutoReconciledFileResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update an invoice import. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateAutoReconProcessResponse</returns>            
        public UpdateAutoReconProcessResponse UpdateInvoiceImport (UpdateAutoReconProcessRequest body)
        {
            
    
            var path = "/api/Transaction/invoice-import";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateInvoiceImport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateInvoiceImport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateAutoReconProcessResponse) ApiClient.Deserialize(response.Content, typeof(UpdateAutoReconProcessResponse), response.Headers);
        }
    
        /// <summary>
        /// Update the accounting data record for a particular transaction. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateTransactionAccountingDataResponse</returns>            
        public UpdateTransactionAccountingDataResponse UpdateTransactionAccountData (UpdateTransactionAccountingDataRequest body)
        {
            
    
            var path = "/api/Transaction/accounting-data";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionAccountData: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionAccountData: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateTransactionAccountingDataResponse) ApiClient.Deserialize(response.Content, typeof(UpdateTransactionAccountingDataResponse), response.Headers);
        }
    
        /// <summary>
        /// Internal use only - Update the accounting transfer status of a particular transaction. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateTransactionAccountingTransferResponse</returns>            
        public UpdateTransactionAccountingTransferResponse UpdateTransactionAccountingTransferStatus (UpdateTransactionAccountingTransferRequest body)
        {
            
    
            var path = "/api/Transaction/accounting-transfer";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionAccountingTransferStatus: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionAccountingTransferStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateTransactionAccountingTransferResponse) ApiClient.Deserialize(response.Content, typeof(UpdateTransactionAccountingTransferResponse), response.Headers);
        }
    
        /// <summary>
        /// Update an existing attachment record for a transaction. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateTransactionAttachmentResponse</returns>            
        public UpdateTransactionAttachmentResponse UpdateTransactionAttachment (UpdateTransactionAttachmentRequest body)
        {
            
    
            var path = "/api/Transaction/attachment";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionAttachment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionAttachment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateTransactionAttachmentResponse) ApiClient.Deserialize(response.Content, typeof(UpdateTransactionAttachmentResponse), response.Headers);
        }
    
        /// <summary>
        /// Update an existing note for a transaction. 
        /// </summary>
        /// <param name="body"></param> 
        /// <returns>UpdateTransactionNoteResponse</returns>            
        public UpdateTransactionNoteResponse UpdateTransactionNote (UpdateTransactionNoteRequest body)
        {
            
    
            var path = "/api/Transaction/note";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionNote: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateTransactionNote: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UpdateTransactionNoteResponse) ApiClient.Deserialize(response.Content, typeof(UpdateTransactionNoteResponse), response.Headers);
        }
    
    }
}

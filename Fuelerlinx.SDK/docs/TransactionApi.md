# IO.Swagger.Api.TransactionApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteAutoReconciledFile**](TransactionApi.md#deleteautoreconciledfile) | **DELETE** /api/Transaction/invoice-import/file-capture/{id} | Internal use only - Delete a file captured by an invoice import.
[**DeleteInvoiceImport**](TransactionApi.md#deleteinvoiceimport) | **DELETE** /api/Transaction/invoice-import/process/{processId} | Internal use only - Delete an invoice import (this will not UNDO the invoice information imported - that will be included in a different method).
[**DeleteTransactionAccountTransferStatus**](TransactionApi.md#deletetransactionaccounttransferstatus) | **DELETE** /api/Transaction/accounting-transfer/{transactionId} | Internal use only - Delete the accounting transfer status with the provided {id}.
[**DeleteTransactionAccountingData**](TransactionApi.md#deletetransactionaccountingdata) | **DELETE** /api/Transaction/accounting-data/{transactionId} | Delete accounting data associated with a particular transaction.
[**DeleteTransactionAttachment**](TransactionApi.md#deletetransactionattachment) | **DELETE** /api/Transaction/attachment/{id} | Delete an attachment record from a transaction by the attachment&#39;s {id}.
[**DeleteTransactionNote**](TransactionApi.md#deletetransactionnote) | **DELETE** /api/Transaction/note/{id} | Delete an existing note from a transaction by it&#39;s {id}.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database.
[**GetAutoReconciledFile**](TransactionApi.md#getautoreconciledfile) | **GET** /api/Transaction/invoice-import/file-capture/process/{processId} | Internal use only - Fetch all files captured during an invoice import by the process ID.
[**GetInvoiceImportByProcessId**](TransactionApi.md#getinvoiceimportbyprocessid) | **GET** /api/Transaction/invoice-import/process/{processId} | Internal use only - Fetch an invoice import by the process ID.
[**GetInvoiceImportsByDateRange**](TransactionApi.md#getinvoiceimportsbydaterange) | **GET** /api/Transaction/invoice-import/by-date-range/list | Internal use only - Fetch all invoice imports across a date range.
[**GetPendingInvoiceImportsByCompany**](TransactionApi.md#getpendinginvoiceimportsbycompany) | **GET** /api/Transaction/invoice-import/pending | Internal use only - Fetch all pending invoice imports for a company.
[**GetTransactionAccountingDataByTransactionId**](TransactionApi.md#gettransactionaccountingdatabytransactionid) | **GET** /api/Transaction/accounting-data/{transactionId} | Fetch accounting data associated with the provided {transactionId}.
[**GetTransactionAccountingTransferStatus**](TransactionApi.md#gettransactionaccountingtransferstatus) | **GET** /api/Transaction/accounting-transfer/{transactionId} | Internal use only - Get the accounting transfer status of the specified {transactionId}.
[**GetTransactionAttachment**](TransactionApi.md#gettransactionattachment) | **GET** /api/Transaction/attachment/{id} | Fetch a transaction attachment by the provided {id}.
[**GetTransactionAttachmentFileDataById**](TransactionApi.md#gettransactionattachmentfiledatabyid) | **GET** /api/Transaction/attachment/{attachmentId}/fileData | Get file data (base64 string) for a particular attachment tied to a transaction.  The [attachmentId] parameter can be found in each TransactionAttachment in the transactionAttachments array of a transaction.
[**GetTransactionById**](TransactionApi.md#gettransactionbyid) | **GET** /api/Transaction/by-id/{id} | Fetch a transaction by it&#39;s Id.  This will include fuel data, fees/services, invoice information, and scheduling data associated with the transaction.
[**GetTransactionFuelPrices**](TransactionApi.md#gettransactionfuelprices) | **GET** /api/Transaction/prices/{transactionId} | Fetch all prices quoted for a particular transaction.
[**GetTransactionGeneralInfoByDateRange**](TransactionApi.md#gettransactiongeneralinfobydaterange) | **GET** /api/Transaction/general-info/by-date-range | Get all general transaction information for the specified date range.  This can include both fuel orders and service-only transactions.
[**GetTransactionNote**](TransactionApi.md#gettransactionnote) | **GET** /api/Transaction/note/{id} | Fetch a transaction note by the provided {id}
[**GetTransactionsByAirportAndTailNumber**](TransactionApi.md#gettransactionsbyairportandtailnumber) | **GET** /api/Transaction/by-airport/{airportIdentifier}/tailNumber/{tailNumber} | Get all transactions for the specified airport, tail, and date range.  This can include both fuel orders and service-only transactions.
[**GetTransactionsByDateRange**](TransactionApi.md#gettransactionsbydaterange) | **GET** /api/Transaction/by-date-range | Get all transactions for the specified date range.  This can include both fuel orders and service-only transactions.
[**GetTransactionsByInvoiceNumber**](TransactionApi.md#gettransactionsbyinvoicenumber) | **GET** /api/Transaction/by-invoice-number/{invoiceNumber}/fueler/{fuelerId} | Get all transactions for the specified invoice number.  This can include both fuel orders and service-only transactions.
[**GetTransactionsFromInvoiceImport**](TransactionApi.md#gettransactionsfrominvoiceimport) | **GET** /api/Transaction/invoice-import/transaction-list/{processId} | 
[**PostAutoReconciledFile**](TransactionApi.md#postautoreconciledfile) | **POST** /api/Transaction/invoice-import/file-capture | Internal use only - Add a file captured by an invoice import.
[**PostInvoiceImport**](TransactionApi.md#postinvoiceimport) | **POST** /api/Transaction/invoice-import | Internal use only - Add an invoice import record.
[**PostTransactionAccountTransferStatus**](TransactionApi.md#posttransactionaccounttransferstatus) | **POST** /api/Transaction/accounting-transfer | Internal use only - Post a new accounting transfer status for a particular transaction.
[**PostTransactionAccountingData**](TransactionApi.md#posttransactionaccountingdata) | **POST** /api/Transaction/accounting-data | Add accounting data for a particular transaction.  The {AccountingData} object can be in any format.
[**PostTransactionAttachment**](TransactionApi.md#posttransactionattachment) | **POST** /api/Transaction/attachment | Add a new attachment to a transaction.  Please use the fileData API for \&quot;TransactionFileData\&quot; to first store any file data into the database and retrieve the [AttachmentFileDataId].
[**PostTransactionNote**](TransactionApi.md#posttransactionnote) | **POST** /api/Transaction/note | Add a new note to a transaction.
[**UpdateAutoReconciledFile**](TransactionApi.md#updateautoreconciledfile) | **PUT** /api/Transaction/invoice-import/file-capture | Internal use only - Update a file captured by an invoice import.
[**UpdateInvoiceImport**](TransactionApi.md#updateinvoiceimport) | **PUT** /api/Transaction/invoice-import | Internal use only - Update an invoice import.
[**UpdateTransaction**](TransactionApi.md#updatetransaction) | **PUT** /api/Transaction | 
[**UpdateTransactionAccountData**](TransactionApi.md#updatetransactionaccountdata) | **PUT** /api/Transaction/accounting-data | Update the accounting data record for a particular transaction.
[**UpdateTransactionAccountingTransferStatus**](TransactionApi.md#updatetransactionaccountingtransferstatus) | **PUT** /api/Transaction/accounting-transfer | Internal use only - Update the accounting transfer status of a particular transaction.
[**UpdateTransactionAttachment**](TransactionApi.md#updatetransactionattachment) | **PUT** /api/Transaction/attachment | Update an existing attachment record for a transaction.
[**UpdateTransactionNote**](TransactionApi.md#updatetransactionnote) | **PUT** /api/Transaction/note | Update an existing note for a transaction.


<a name="deleteautoreconciledfile"></a>
# **DeleteAutoReconciledFile**
> DeleteAutoReconciledFileResponse DeleteAutoReconciledFile (int? id)

Internal use only - Delete a file captured by an invoice import.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteAutoReconciledFileExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete a file captured by an invoice import.
                DeleteAutoReconciledFileResponse result = apiInstance.DeleteAutoReconciledFile(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.DeleteAutoReconciledFile: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

[**DeleteAutoReconciledFileResponse**](DeleteAutoReconciledFileResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteinvoiceimport"></a>
# **DeleteInvoiceImport**
> DeleteAutoReconProcessResponse DeleteInvoiceImport (int? processId)

Internal use only - Delete an invoice import (this will not UNDO the invoice information imported - that will be included in a different method).

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteInvoiceImportExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var processId = 56;  // int? | 

            try
            {
                // Internal use only - Delete an invoice import (this will not UNDO the invoice information imported - that will be included in a different method).
                DeleteAutoReconProcessResponse result = apiInstance.DeleteInvoiceImport(processId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.DeleteInvoiceImport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **processId** | **int?**|  | 

### Return type

[**DeleteAutoReconProcessResponse**](DeleteAutoReconProcessResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletetransactionaccounttransferstatus"></a>
# **DeleteTransactionAccountTransferStatus**
> DeleteTransactionAccountingTransferResponse DeleteTransactionAccountTransferStatus (int? transactionId)

Internal use only - Delete the accounting transfer status with the provided {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTransactionAccountTransferStatusExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var transactionId = 56;  // int? | 

            try
            {
                // Internal use only - Delete the accounting transfer status with the provided {id}.
                DeleteTransactionAccountingTransferResponse result = apiInstance.DeleteTransactionAccountTransferStatus(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.DeleteTransactionAccountTransferStatus: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionId** | **int?**|  | 

### Return type

[**DeleteTransactionAccountingTransferResponse**](DeleteTransactionAccountingTransferResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletetransactionaccountingdata"></a>
# **DeleteTransactionAccountingData**
> DeleteTransactionAccountingDataResponse DeleteTransactionAccountingData (int? transactionId)

Delete accounting data associated with a particular transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTransactionAccountingDataExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var transactionId = 56;  // int? | 

            try
            {
                // Delete accounting data associated with a particular transaction.
                DeleteTransactionAccountingDataResponse result = apiInstance.DeleteTransactionAccountingData(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.DeleteTransactionAccountingData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionId** | **int?**|  | 

### Return type

[**DeleteTransactionAccountingDataResponse**](DeleteTransactionAccountingDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletetransactionattachment"></a>
# **DeleteTransactionAttachment**
> DeleteTransactionAttachmentResponse DeleteTransactionAttachment (int? id)

Delete an attachment record from a transaction by the attachment's {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTransactionAttachmentExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var id = 56;  // int? | 

            try
            {
                // Delete an attachment record from a transaction by the attachment's {id}.
                DeleteTransactionAttachmentResponse result = apiInstance.DeleteTransactionAttachment(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.DeleteTransactionAttachment: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

[**DeleteTransactionAttachmentResponse**](DeleteTransactionAttachmentResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletetransactionnote"></a>
# **DeleteTransactionNote**
> DeleteTransactionNoteResponse DeleteTransactionNote (int? id)

Delete an existing note from a transaction by it's {id}.  The note will be changed to a \"deleted\" state but will not be removed from the database.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTransactionNoteExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var id = 56;  // int? | 

            try
            {
                // Delete an existing note from a transaction by it's {id}.  The note will be changed to a \"deleted\" state but will not be removed from the database.
                DeleteTransactionNoteResponse result = apiInstance.DeleteTransactionNote(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.DeleteTransactionNote: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

[**DeleteTransactionNoteResponse**](DeleteTransactionNoteResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getautoreconciledfile"></a>
# **GetAutoReconciledFile**
> AutoReconciledFileResponse GetAutoReconciledFile (int? processId)

Internal use only - Fetch all files captured during an invoice import by the process ID.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAutoReconciledFileExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var processId = 56;  // int? | 

            try
            {
                // Internal use only - Fetch all files captured during an invoice import by the process ID.
                AutoReconciledFileResponse result = apiInstance.GetAutoReconciledFile(processId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetAutoReconciledFile: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **processId** | **int?**|  | 

### Return type

[**AutoReconciledFileResponse**](AutoReconciledFileResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getinvoiceimportbyprocessid"></a>
# **GetInvoiceImportByProcessId**
> AutoReconProcessResponse GetInvoiceImportByProcessId (int? processId)

Internal use only - Fetch an invoice import by the process ID.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetInvoiceImportByProcessIdExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var processId = 56;  // int? | 

            try
            {
                // Internal use only - Fetch an invoice import by the process ID.
                AutoReconProcessResponse result = apiInstance.GetInvoiceImportByProcessId(processId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetInvoiceImportByProcessId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **processId** | **int?**|  | 

### Return type

[**AutoReconProcessResponse**](AutoReconProcessResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getinvoiceimportsbydaterange"></a>
# **GetInvoiceImportsByDateRange**
> AutoReconProcessListResponse GetInvoiceImportsByDateRange (DateTime? startDate, DateTime? endDate)

Internal use only - Fetch all invoice imports across a date range.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetInvoiceImportsByDateRangeExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var startDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                // Internal use only - Fetch all invoice imports across a date range.
                AutoReconProcessListResponse result = apiInstance.GetInvoiceImportsByDateRange(startDate, endDate);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetInvoiceImportsByDateRange: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **startDate** | **DateTime?**|  | [optional] 
 **endDate** | **DateTime?**|  | [optional] 

### Return type

[**AutoReconProcessListResponse**](AutoReconProcessListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getpendinginvoiceimportsbycompany"></a>
# **GetPendingInvoiceImportsByCompany**
> AutoReconProcessListResponse GetPendingInvoiceImportsByCompany ()

Internal use only - Fetch all pending invoice imports for a company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetPendingInvoiceImportsByCompanyExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();

            try
            {
                // Internal use only - Fetch all pending invoice imports for a company.
                AutoReconProcessListResponse result = apiInstance.GetPendingInvoiceImportsByCompany();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetPendingInvoiceImportsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**AutoReconProcessListResponse**](AutoReconProcessListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionaccountingdatabytransactionid"></a>
# **GetTransactionAccountingDataByTransactionId**
> TransactionAccountingDataResponse GetTransactionAccountingDataByTransactionId (int? transactionId)

Fetch accounting data associated with the provided {transactionId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionAccountingDataByTransactionIdExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var transactionId = 56;  // int? | 

            try
            {
                // Fetch accounting data associated with the provided {transactionId}.
                TransactionAccountingDataResponse result = apiInstance.GetTransactionAccountingDataByTransactionId(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionAccountingDataByTransactionId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionId** | **int?**|  | 

### Return type

[**TransactionAccountingDataResponse**](TransactionAccountingDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionaccountingtransferstatus"></a>
# **GetTransactionAccountingTransferStatus**
> TransactionAccountingTransferResponse GetTransactionAccountingTransferStatus (int? transactionId)

Internal use only - Get the accounting transfer status of the specified {transactionId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionAccountingTransferStatusExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var transactionId = 56;  // int? | 

            try
            {
                // Internal use only - Get the accounting transfer status of the specified {transactionId}.
                TransactionAccountingTransferResponse result = apiInstance.GetTransactionAccountingTransferStatus(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionAccountingTransferStatus: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionId** | **int?**|  | 

### Return type

[**TransactionAccountingTransferResponse**](TransactionAccountingTransferResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionattachment"></a>
# **GetTransactionAttachment**
> TransactionAttachmentResponse GetTransactionAttachment (int? id)

Fetch a transaction attachment by the provided {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionAttachmentExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch a transaction attachment by the provided {id}.
                TransactionAttachmentResponse result = apiInstance.GetTransactionAttachment(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionAttachment: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

[**TransactionAttachmentResponse**](TransactionAttachmentResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionattachmentfiledatabyid"></a>
# **GetTransactionAttachmentFileDataById**
> TransactionAttachmentFileDataResponse GetTransactionAttachmentFileDataById (int? attachmentId)

Get file data (base64 string) for a particular attachment tied to a transaction.  The [attachmentId] parameter can be found in each TransactionAttachment in the transactionAttachments array of a transaction.

This method is not async as this capability is currently unavailable for byte arrays.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionAttachmentFileDataByIdExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var attachmentId = 56;  // int? | 

            try
            {
                // Get file data (base64 string) for a particular attachment tied to a transaction.  The [attachmentId] parameter can be found in each TransactionAttachment in the transactionAttachments array of a transaction.
                TransactionAttachmentFileDataResponse result = apiInstance.GetTransactionAttachmentFileDataById(attachmentId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionAttachmentFileDataById: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **attachmentId** | **int?**|  | 

### Return type

[**TransactionAttachmentFileDataResponse**](TransactionAttachmentFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionbyid"></a>
# **GetTransactionById**
> TransactionResponse GetTransactionById (int? id)

Fetch a transaction by it's Id.  This will include fuel data, fees/services, invoice information, and scheduling data associated with the transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionByIdExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch a transaction by it's Id.  This will include fuel data, fees/services, invoice information, and scheduling data associated with the transaction.
                TransactionResponse result = apiInstance.GetTransactionById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionById: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

[**TransactionResponse**](TransactionResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionfuelprices"></a>
# **GetTransactionFuelPrices**
> TransactionFuelPriceResponse GetTransactionFuelPrices (int? transactionId)

Fetch all prices quoted for a particular transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionFuelPricesExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var transactionId = 56;  // int? | 

            try
            {
                // Fetch all prices quoted for a particular transaction.
                TransactionFuelPriceResponse result = apiInstance.GetTransactionFuelPrices(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionFuelPrices: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionId** | **int?**|  | 

### Return type

[**TransactionFuelPriceResponse**](TransactionFuelPriceResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactiongeneralinfobydaterange"></a>
# **GetTransactionGeneralInfoByDateRange**
> TransactionGeneralInfoResponse GetTransactionGeneralInfoByDateRange (DateTime? startDate, DateTime? endDate, int? processId)

Get all general transaction information for the specified date range.  This can include both fuel orders and service-only transactions.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionGeneralInfoByDateRangeExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var startDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var processId = 56;  // int? |  (optional) 

            try
            {
                // Get all general transaction information for the specified date range.  This can include both fuel orders and service-only transactions.
                TransactionGeneralInfoResponse result = apiInstance.GetTransactionGeneralInfoByDateRange(startDate, endDate, processId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionGeneralInfoByDateRange: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **startDate** | **DateTime?**|  | [optional] 
 **endDate** | **DateTime?**|  | [optional] 
 **processId** | **int?**|  | [optional] 

### Return type

[**TransactionGeneralInfoResponse**](TransactionGeneralInfoResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionnote"></a>
# **GetTransactionNote**
> TransactionNoteResponse GetTransactionNote (int? id)

Fetch a transaction note by the provided {id}

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionNoteExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch a transaction note by the provided {id}
                TransactionNoteResponse result = apiInstance.GetTransactionNote(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionNote: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

[**TransactionNoteResponse**](TransactionNoteResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionsbyairportandtailnumber"></a>
# **GetTransactionsByAirportAndTailNumber**
> TransactionsResponse GetTransactionsByAirportAndTailNumber (string airportIdentifier, string tailNumber, DateTime? startDate, DateTime? endDate)

Get all transactions for the specified airport, tail, and date range.  This can include both fuel orders and service-only transactions.

Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionsByAirportAndTailNumberExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var airportIdentifier = airportIdentifier_example;  // string | 
            var tailNumber = tailNumber_example;  // string | 
            var startDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                // Get all transactions for the specified airport, tail, and date range.  This can include both fuel orders and service-only transactions.
                TransactionsResponse result = apiInstance.GetTransactionsByAirportAndTailNumber(airportIdentifier, tailNumber, startDate, endDate);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionsByAirportAndTailNumber: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **airportIdentifier** | **string**|  | 
 **tailNumber** | **string**|  | 
 **startDate** | **DateTime?**|  | [optional] 
 **endDate** | **DateTime?**|  | [optional] 

### Return type

[**TransactionsResponse**](TransactionsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionsbydaterange"></a>
# **GetTransactionsByDateRange**
> TransactionsResponse GetTransactionsByDateRange (DateTime? startDate, DateTime? endDate)

Get all transactions for the specified date range.  This can include both fuel orders and service-only transactions.

Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionsByDateRangeExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var startDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                // Get all transactions for the specified date range.  This can include both fuel orders and service-only transactions.
                TransactionsResponse result = apiInstance.GetTransactionsByDateRange(startDate, endDate);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionsByDateRange: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **startDate** | **DateTime?**|  | [optional] 
 **endDate** | **DateTime?**|  | [optional] 

### Return type

[**TransactionsResponse**](TransactionsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionsbyinvoicenumber"></a>
# **GetTransactionsByInvoiceNumber**
> TransactionsResponse GetTransactionsByInvoiceNumber (string invoiceNumber, int? fuelerId)

Get all transactions for the specified invoice number.  This can include both fuel orders and service-only transactions.

Details about each attachment are included in the transaction, but the actual file data must be pulled via the /attachment/{fileDataId} API (GetTransactionAttachmentFileDataById in the SDK).

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionsByInvoiceNumberExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var invoiceNumber = invoiceNumber_example;  // string | 
            var fuelerId = 56;  // int? | 

            try
            {
                // Get all transactions for the specified invoice number.  This can include both fuel orders and service-only transactions.
                TransactionsResponse result = apiInstance.GetTransactionsByInvoiceNumber(invoiceNumber, fuelerId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionsByInvoiceNumber: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **invoiceNumber** | **string**|  | 
 **fuelerId** | **int?**|  | 

### Return type

[**TransactionsResponse**](TransactionsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionsfrominvoiceimport"></a>
# **GetTransactionsFromInvoiceImport**
> TransactionsResponse GetTransactionsFromInvoiceImport (int? processId)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionsFromInvoiceImportExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var processId = 56;  // int? | 

            try
            {
                TransactionsResponse result = apiInstance.GetTransactionsFromInvoiceImport(processId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.GetTransactionsFromInvoiceImport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **processId** | **int?**|  | 

### Return type

[**TransactionsResponse**](TransactionsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postautoreconciledfile"></a>
# **PostAutoReconciledFile**
> PostAutoReconciledFileResponse PostAutoReconciledFile (PostAutoReconciledFileRequest body)

Internal use only - Add a file captured by an invoice import.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostAutoReconciledFileExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new PostAutoReconciledFileRequest(); // PostAutoReconciledFileRequest |  (optional) 

            try
            {
                // Internal use only - Add a file captured by an invoice import.
                PostAutoReconciledFileResponse result = apiInstance.PostAutoReconciledFile(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.PostAutoReconciledFile: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostAutoReconciledFileRequest**](PostAutoReconciledFileRequest.md)|  | [optional] 

### Return type

[**PostAutoReconciledFileResponse**](PostAutoReconciledFileResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postinvoiceimport"></a>
# **PostInvoiceImport**
> PostAutoReconProcessResponse PostInvoiceImport (PostAutoReconProcessRequest body)

Internal use only - Add an invoice import record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostInvoiceImportExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new PostAutoReconProcessRequest(); // PostAutoReconProcessRequest |  (optional) 

            try
            {
                // Internal use only - Add an invoice import record.
                PostAutoReconProcessResponse result = apiInstance.PostInvoiceImport(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.PostInvoiceImport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostAutoReconProcessRequest**](PostAutoReconProcessRequest.md)|  | [optional] 

### Return type

[**PostAutoReconProcessResponse**](PostAutoReconProcessResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttransactionaccounttransferstatus"></a>
# **PostTransactionAccountTransferStatus**
> PostTransactionAccountingTransferResponse PostTransactionAccountTransferStatus (PostTransactionAccountingTransferRequest body)

Internal use only - Post a new accounting transfer status for a particular transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTransactionAccountTransferStatusExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new PostTransactionAccountingTransferRequest(); // PostTransactionAccountingTransferRequest |  (optional) 

            try
            {
                // Internal use only - Post a new accounting transfer status for a particular transaction.
                PostTransactionAccountingTransferResponse result = apiInstance.PostTransactionAccountTransferStatus(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.PostTransactionAccountTransferStatus: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTransactionAccountingTransferRequest**](PostTransactionAccountingTransferRequest.md)|  | [optional] 

### Return type

[**PostTransactionAccountingTransferResponse**](PostTransactionAccountingTransferResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttransactionaccountingdata"></a>
# **PostTransactionAccountingData**
> PostTransactionAccountingDataResponse PostTransactionAccountingData (PostTransactionAccountingDataRequest body)

Add accounting data for a particular transaction.  The {AccountingData} object can be in any format.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTransactionAccountingDataExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new PostTransactionAccountingDataRequest(); // PostTransactionAccountingDataRequest |  (optional) 

            try
            {
                // Add accounting data for a particular transaction.  The {AccountingData} object can be in any format.
                PostTransactionAccountingDataResponse result = apiInstance.PostTransactionAccountingData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.PostTransactionAccountingData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTransactionAccountingDataRequest**](PostTransactionAccountingDataRequest.md)|  | [optional] 

### Return type

[**PostTransactionAccountingDataResponse**](PostTransactionAccountingDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttransactionattachment"></a>
# **PostTransactionAttachment**
> PostTransactionAttachmentResponse PostTransactionAttachment (PostTransactionAttachmentRequest body)

Add a new attachment to a transaction.  Please use the fileData API for \"TransactionFileData\" to first store any file data into the database and retrieve the [AttachmentFileDataId].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTransactionAttachmentExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new PostTransactionAttachmentRequest(); // PostTransactionAttachmentRequest |  (optional) 

            try
            {
                // Add a new attachment to a transaction.  Please use the fileData API for \"TransactionFileData\" to first store any file data into the database and retrieve the [AttachmentFileDataId].
                PostTransactionAttachmentResponse result = apiInstance.PostTransactionAttachment(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.PostTransactionAttachment: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTransactionAttachmentRequest**](PostTransactionAttachmentRequest.md)|  | [optional] 

### Return type

[**PostTransactionAttachmentResponse**](PostTransactionAttachmentResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttransactionnote"></a>
# **PostTransactionNote**
> PostTransactionNoteResponse PostTransactionNote (PostTransactionNoteRequest body)

Add a new note to a transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTransactionNoteExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new PostTransactionNoteRequest(); // PostTransactionNoteRequest |  (optional) 

            try
            {
                // Add a new note to a transaction.
                PostTransactionNoteResponse result = apiInstance.PostTransactionNote(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.PostTransactionNote: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTransactionNoteRequest**](PostTransactionNoteRequest.md)|  | [optional] 

### Return type

[**PostTransactionNoteResponse**](PostTransactionNoteResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateautoreconciledfile"></a>
# **UpdateAutoReconciledFile**
> UpdateAutoReconciledFileResponse UpdateAutoReconciledFile (UpdateAutoReconciledFileRequest body)

Internal use only - Update a file captured by an invoice import.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateAutoReconciledFileExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new UpdateAutoReconciledFileRequest(); // UpdateAutoReconciledFileRequest |  (optional) 

            try
            {
                // Internal use only - Update a file captured by an invoice import.
                UpdateAutoReconciledFileResponse result = apiInstance.UpdateAutoReconciledFile(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.UpdateAutoReconciledFile: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateAutoReconciledFileRequest**](UpdateAutoReconciledFileRequest.md)|  | [optional] 

### Return type

[**UpdateAutoReconciledFileResponse**](UpdateAutoReconciledFileResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateinvoiceimport"></a>
# **UpdateInvoiceImport**
> UpdateAutoReconProcessResponse UpdateInvoiceImport (UpdateAutoReconProcessRequest body)

Internal use only - Update an invoice import.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateInvoiceImportExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new UpdateAutoReconProcessRequest(); // UpdateAutoReconProcessRequest |  (optional) 

            try
            {
                // Internal use only - Update an invoice import.
                UpdateAutoReconProcessResponse result = apiInstance.UpdateInvoiceImport(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.UpdateInvoiceImport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateAutoReconProcessRequest**](UpdateAutoReconProcessRequest.md)|  | [optional] 

### Return type

[**UpdateAutoReconProcessResponse**](UpdateAutoReconProcessResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetransaction"></a>
# **UpdateTransaction**
> UpdateTransactionResponse UpdateTransaction (UpdateTransactionRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTransactionExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new UpdateTransactionRequest(); // UpdateTransactionRequest |  (optional) 

            try
            {
                UpdateTransactionResponse result = apiInstance.UpdateTransaction(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.UpdateTransaction: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTransactionRequest**](UpdateTransactionRequest.md)|  | [optional] 

### Return type

[**UpdateTransactionResponse**](UpdateTransactionResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetransactionaccountdata"></a>
# **UpdateTransactionAccountData**
> UpdateTransactionAccountingDataResponse UpdateTransactionAccountData (UpdateTransactionAccountingDataRequest body)

Update the accounting data record for a particular transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTransactionAccountDataExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new UpdateTransactionAccountingDataRequest(); // UpdateTransactionAccountingDataRequest |  (optional) 

            try
            {
                // Update the accounting data record for a particular transaction.
                UpdateTransactionAccountingDataResponse result = apiInstance.UpdateTransactionAccountData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.UpdateTransactionAccountData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTransactionAccountingDataRequest**](UpdateTransactionAccountingDataRequest.md)|  | [optional] 

### Return type

[**UpdateTransactionAccountingDataResponse**](UpdateTransactionAccountingDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetransactionaccountingtransferstatus"></a>
# **UpdateTransactionAccountingTransferStatus**
> UpdateTransactionAccountingTransferResponse UpdateTransactionAccountingTransferStatus (UpdateTransactionAccountingTransferRequest body)

Internal use only - Update the accounting transfer status of a particular transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTransactionAccountingTransferStatusExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new UpdateTransactionAccountingTransferRequest(); // UpdateTransactionAccountingTransferRequest |  (optional) 

            try
            {
                // Internal use only - Update the accounting transfer status of a particular transaction.
                UpdateTransactionAccountingTransferResponse result = apiInstance.UpdateTransactionAccountingTransferStatus(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.UpdateTransactionAccountingTransferStatus: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTransactionAccountingTransferRequest**](UpdateTransactionAccountingTransferRequest.md)|  | [optional] 

### Return type

[**UpdateTransactionAccountingTransferResponse**](UpdateTransactionAccountingTransferResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetransactionattachment"></a>
# **UpdateTransactionAttachment**
> UpdateTransactionAttachmentResponse UpdateTransactionAttachment (UpdateTransactionAttachmentRequest body)

Update an existing attachment record for a transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTransactionAttachmentExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new UpdateTransactionAttachmentRequest(); // UpdateTransactionAttachmentRequest |  (optional) 

            try
            {
                // Update an existing attachment record for a transaction.
                UpdateTransactionAttachmentResponse result = apiInstance.UpdateTransactionAttachment(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.UpdateTransactionAttachment: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTransactionAttachmentRequest**](UpdateTransactionAttachmentRequest.md)|  | [optional] 

### Return type

[**UpdateTransactionAttachmentResponse**](UpdateTransactionAttachmentResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetransactionnote"></a>
# **UpdateTransactionNote**
> UpdateTransactionNoteResponse UpdateTransactionNote (UpdateTransactionNoteRequest body)

Update an existing note for a transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTransactionNoteExample
    {
        public void main()
        {
            
            // Configure API key authorization: ApiKeyScheme
            Configuration.Default.ApiKey.Add("x-api-key", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("x-api-key", "Bearer");
            // Configure API key authorization: Bearer
            Configuration.Default.ApiKey.Add("Authorization", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // Configuration.Default.ApiKeyPrefix.Add("Authorization", "Bearer");

            var apiInstance = new TransactionApi();
            var body = new UpdateTransactionNoteRequest(); // UpdateTransactionNoteRequest |  (optional) 

            try
            {
                // Update an existing note for a transaction.
                UpdateTransactionNoteResponse result = apiInstance.UpdateTransactionNote(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionApi.UpdateTransactionNote: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTransactionNoteRequest**](UpdateTransactionNoteRequest.md)|  | [optional] 

### Return type

[**UpdateTransactionNoteResponse**](UpdateTransactionNoteResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


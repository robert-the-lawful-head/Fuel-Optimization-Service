# IO.Swagger.Api.AccountingApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**CheckSageCredentials**](AccountingApi.md#checksagecredentials) | **POST** /api/Accounting/sage/check-credentials | Get Sage Credentials
[**DeleteAccountingIntegrationItemCodes**](AccountingApi.md#deleteaccountingintegrationitemcodes) | **DELETE** /api/Accounting/integration-item-codes/{id} | Deletes accounting integration item code record based on ID
[**DeleteSupplierDetails**](AccountingApi.md#deletesupplierdetails) | **DELETE** /api/Accounting/supplier-details/{id} | Deletes supplier-details record based on ID
[**GetAccountingIntegrationItemCodesById**](AccountingApi.md#getaccountingintegrationitemcodesbyid) | **GET** /api/Accounting/integration-item-codes/{id} | Gets single accounting integration item code record
[**GetAccountingItemMappingList**](AccountingApi.md#getaccountingitemmappinglist) | **GET** /api/Accounting/mapping/items/list | Gets accounting integration mappings for line items
[**GetAccountingItemMappingListForTransactions**](AccountingApi.md#getaccountingitemmappinglistfortransactions) | **GET** /api/Accounting/mapping/items/list/{transactionIds} | Gets accounting integration mappings for line items by comma-delimited {transactionIds}
[**GetPendingAccountingExport**](AccountingApi.md#getpendingaccountingexport) | **GET** /api/Accounting/oracle/accounting-export/pending | 
[**GetSageCredentials**](AccountingApi.md#getsagecredentials) | **GET** /api/Accounting/sage/credentials | Get Sage Credentials
[**GetSageGlAccounts**](AccountingApi.md#getsageglaccounts) | **GET** /api/Accounting/sage/gl-accounts | Get Sage GL Account Details
[**GetSageVendorAccounts**](AccountingApi.md#getsagevendoraccounts) | **GET** /api/Accounting/sage/vendor-accounts | Get Sage GL Account Details
[**GetSupplierDetailsById**](AccountingApi.md#getsupplierdetailsbyid) | **GET** /api/Accounting/supplier-details/{id} | Fetch supplier-details for a particular FBO or Vendor based on the provided [ID].
[**PostAccountingIntegrationItemCodesDetails**](AccountingApi.md#postaccountingintegrationitemcodesdetails) | **POST** /api/Accounting/integration-item-codes | Adds new accounting integration item code record
[**PostSageBill**](AccountingApi.md#postsagebill) | **POST** /api/Accounting/sage/bill/{transactionid} | Insert new bill in Sage
[**PostSupplierDetails**](AccountingApi.md#postsupplierdetails) | **POST** /api/Accounting/supplier-details | Adds a new record for supplier-details of an FBO or Vendor.
[**UpdateAccountingIntegrationItemCodesDetails**](AccountingApi.md#updateaccountingintegrationitemcodesdetails) | **PUT** /api/Accounting/integration-item-codes | Updates accounting integration item code record
[**UpdateSageCredentials**](AccountingApi.md#updatesagecredentials) | **POST** /api/Accounting/sage/update-credentials | Get Sage Credentials
[**UpdateSupplierDetails**](AccountingApi.md#updatesupplierdetails) | **PUT** /api/Accounting/supplier-details | Updates current supplier detail record


<a name="checksagecredentials"></a>
# **CheckSageCredentials**
> CheckAuthorizationInfoResponse CheckSageCredentials (SageCredentialsRequest body)

Get Sage Credentials

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CheckSageCredentialsExample
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

            var apiInstance = new AccountingApi();
            var body = new SageCredentialsRequest(); // SageCredentialsRequest |  (optional) 

            try
            {
                // Get Sage Credentials
                CheckAuthorizationInfoResponse result = apiInstance.CheckSageCredentials(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.CheckSageCredentials: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**SageCredentialsRequest**](SageCredentialsRequest.md)|  | [optional] 

### Return type

[**CheckAuthorizationInfoResponse**](CheckAuthorizationInfoResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteaccountingintegrationitemcodes"></a>
# **DeleteAccountingIntegrationItemCodes**
> DeleteAccountingIntegrationItemCodesDetailsResponse DeleteAccountingIntegrationItemCodes (int? id)

Deletes accounting integration item code record based on ID

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteAccountingIntegrationItemCodesExample
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

            var apiInstance = new AccountingApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes accounting integration item code record based on ID
                DeleteAccountingIntegrationItemCodesDetailsResponse result = apiInstance.DeleteAccountingIntegrationItemCodes(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.DeleteAccountingIntegrationItemCodes: " + e.Message );
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

[**DeleteAccountingIntegrationItemCodesDetailsResponse**](DeleteAccountingIntegrationItemCodesDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletesupplierdetails"></a>
# **DeleteSupplierDetails**
> DeleteSupplierDetailsResponse DeleteSupplierDetails (int? id)

Deletes supplier-details record based on ID

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSupplierDetailsExample
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

            var apiInstance = new AccountingApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes supplier-details record based on ID
                DeleteSupplierDetailsResponse result = apiInstance.DeleteSupplierDetails(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.DeleteSupplierDetails: " + e.Message );
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

[**DeleteSupplierDetailsResponse**](DeleteSupplierDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaccountingintegrationitemcodesbyid"></a>
# **GetAccountingIntegrationItemCodesById**
> AccountingIntegrationItemCodesDetailsResponse GetAccountingIntegrationItemCodesById (int? id)

Gets single accounting integration item code record

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAccountingIntegrationItemCodesByIdExample
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

            var apiInstance = new AccountingApi();
            var id = 56;  // int? | 

            try
            {
                // Gets single accounting integration item code record
                AccountingIntegrationItemCodesDetailsResponse result = apiInstance.GetAccountingIntegrationItemCodesById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetAccountingIntegrationItemCodesById: " + e.Message );
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

[**AccountingIntegrationItemCodesDetailsResponse**](AccountingIntegrationItemCodesDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaccountingitemmappinglist"></a>
# **GetAccountingItemMappingList**
> AccountingIntegrationItemCodesListResponse GetAccountingItemMappingList ()

Gets accounting integration mappings for line items

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAccountingItemMappingListExample
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

            var apiInstance = new AccountingApi();

            try
            {
                // Gets accounting integration mappings for line items
                AccountingIntegrationItemCodesListResponse result = apiInstance.GetAccountingItemMappingList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetAccountingItemMappingList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**AccountingIntegrationItemCodesListResponse**](AccountingIntegrationItemCodesListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaccountingitemmappinglistfortransactions"></a>
# **GetAccountingItemMappingListForTransactions**
> AccountingIntegrationItemCodesListResponse GetAccountingItemMappingListForTransactions (string transactionIds)

Gets accounting integration mappings for line items by comma-delimited {transactionIds}

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAccountingItemMappingListForTransactionsExample
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

            var apiInstance = new AccountingApi();
            var transactionIds = transactionIds_example;  // string | 

            try
            {
                // Gets accounting integration mappings for line items by comma-delimited {transactionIds}
                AccountingIntegrationItemCodesListResponse result = apiInstance.GetAccountingItemMappingListForTransactions(transactionIds);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetAccountingItemMappingListForTransactions: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionIds** | **string**|  | 

### Return type

[**AccountingIntegrationItemCodesListResponse**](AccountingIntegrationItemCodesListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getpendingaccountingexport"></a>
# **GetPendingAccountingExport**
> OracleAccountingExportResponse GetPendingAccountingExport ()



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetPendingAccountingExportExample
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

            var apiInstance = new AccountingApi();

            try
            {
                OracleAccountingExportResponse result = apiInstance.GetPendingAccountingExport();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetPendingAccountingExport: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**OracleAccountingExportResponse**](OracleAccountingExportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsagecredentials"></a>
# **GetSageCredentials**
> AuthorizationInfoResponse GetSageCredentials ()

Get Sage Credentials

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSageCredentialsExample
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

            var apiInstance = new AccountingApi();

            try
            {
                // Get Sage Credentials
                AuthorizationInfoResponse result = apiInstance.GetSageCredentials();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetSageCredentials: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**AuthorizationInfoResponse**](AuthorizationInfoResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsageglaccounts"></a>
# **GetSageGlAccounts**
> SageGeneralLedgerResponse GetSageGlAccounts ()

Get Sage GL Account Details

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSageGlAccountsExample
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

            var apiInstance = new AccountingApi();

            try
            {
                // Get Sage GL Account Details
                SageGeneralLedgerResponse result = apiInstance.GetSageGlAccounts();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetSageGlAccounts: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**SageGeneralLedgerResponse**](SageGeneralLedgerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsagevendoraccounts"></a>
# **GetSageVendorAccounts**
> SageVendorResponse GetSageVendorAccounts ()

Get Sage GL Account Details

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSageVendorAccountsExample
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

            var apiInstance = new AccountingApi();

            try
            {
                // Get Sage GL Account Details
                SageVendorResponse result = apiInstance.GetSageVendorAccounts();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetSageVendorAccounts: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**SageVendorResponse**](SageVendorResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupplierdetailsbyid"></a>
# **GetSupplierDetailsById**
> SupplierDetailsResponse GetSupplierDetailsById (int? id)

Fetch supplier-details for a particular FBO or Vendor based on the provided [ID].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupplierDetailsByIdExample
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

            var apiInstance = new AccountingApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch supplier-details for a particular FBO or Vendor based on the provided [ID].
                SupplierDetailsResponse result = apiInstance.GetSupplierDetailsById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.GetSupplierDetailsById: " + e.Message );
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

[**SupplierDetailsResponse**](SupplierDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postaccountingintegrationitemcodesdetails"></a>
# **PostAccountingIntegrationItemCodesDetails**
> PostAccountingIntegrationItemCodesResponse PostAccountingIntegrationItemCodesDetails (PostAccountingIntegrationItemCodesRequest body)

Adds new accounting integration item code record

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostAccountingIntegrationItemCodesDetailsExample
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

            var apiInstance = new AccountingApi();
            var body = new PostAccountingIntegrationItemCodesRequest(); // PostAccountingIntegrationItemCodesRequest |  (optional) 

            try
            {
                // Adds new accounting integration item code record
                PostAccountingIntegrationItemCodesResponse result = apiInstance.PostAccountingIntegrationItemCodesDetails(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.PostAccountingIntegrationItemCodesDetails: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostAccountingIntegrationItemCodesRequest**](PostAccountingIntegrationItemCodesRequest.md)|  | [optional] 

### Return type

[**PostAccountingIntegrationItemCodesResponse**](PostAccountingIntegrationItemCodesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsagebill"></a>
# **PostSageBill**
> SageCreateBillResponse PostSageBill (int? transactionid)

Insert new bill in Sage

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSageBillExample
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

            var apiInstance = new AccountingApi();
            var transactionid = 56;  // int? | 

            try
            {
                // Insert new bill in Sage
                SageCreateBillResponse result = apiInstance.PostSageBill(transactionid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.PostSageBill: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionid** | **int?**|  | 

### Return type

[**SageCreateBillResponse**](SageCreateBillResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsupplierdetails"></a>
# **PostSupplierDetails**
> PostSupplierDetailsResponse PostSupplierDetails (PostSupplierDetailsRequest body)

Adds a new record for supplier-details of an FBO or Vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSupplierDetailsExample
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

            var apiInstance = new AccountingApi();
            var body = new PostSupplierDetailsRequest(); // PostSupplierDetailsRequest |  (optional) 

            try
            {
                // Adds a new record for supplier-details of an FBO or Vendor.
                PostSupplierDetailsResponse result = apiInstance.PostSupplierDetails(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.PostSupplierDetails: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSupplierDetailsRequest**](PostSupplierDetailsRequest.md)|  | [optional] 

### Return type

[**PostSupplierDetailsResponse**](PostSupplierDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateaccountingintegrationitemcodesdetails"></a>
# **UpdateAccountingIntegrationItemCodesDetails**
> PostAccountingIntegrationItemCodesResponse UpdateAccountingIntegrationItemCodesDetails (UpdateAccountingIntegrationItemCodesDetailsRequest body)

Updates accounting integration item code record

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateAccountingIntegrationItemCodesDetailsExample
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

            var apiInstance = new AccountingApi();
            var body = new UpdateAccountingIntegrationItemCodesDetailsRequest(); // UpdateAccountingIntegrationItemCodesDetailsRequest |  (optional) 

            try
            {
                // Updates accounting integration item code record
                PostAccountingIntegrationItemCodesResponse result = apiInstance.UpdateAccountingIntegrationItemCodesDetails(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.UpdateAccountingIntegrationItemCodesDetails: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateAccountingIntegrationItemCodesDetailsRequest**](UpdateAccountingIntegrationItemCodesDetailsRequest.md)|  | [optional] 

### Return type

[**PostAccountingIntegrationItemCodesResponse**](PostAccountingIntegrationItemCodesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesagecredentials"></a>
# **UpdateSageCredentials**
> AuthorizationInfoResponse UpdateSageCredentials (SageCredentialsRequest body)

Get Sage Credentials

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSageCredentialsExample
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

            var apiInstance = new AccountingApi();
            var body = new SageCredentialsRequest(); // SageCredentialsRequest |  (optional) 

            try
            {
                // Get Sage Credentials
                AuthorizationInfoResponse result = apiInstance.UpdateSageCredentials(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.UpdateSageCredentials: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**SageCredentialsRequest**](SageCredentialsRequest.md)|  | [optional] 

### Return type

[**AuthorizationInfoResponse**](AuthorizationInfoResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesupplierdetails"></a>
# **UpdateSupplierDetails**
> UpdateSupplierDetailsResponse UpdateSupplierDetails (UpdateSupplierDetailsRequest body)

Updates current supplier detail record

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSupplierDetailsExample
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

            var apiInstance = new AccountingApi();
            var body = new UpdateSupplierDetailsRequest(); // UpdateSupplierDetailsRequest |  (optional) 

            try
            {
                // Updates current supplier detail record
                UpdateSupplierDetailsResponse result = apiInstance.UpdateSupplierDetails(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountingApi.UpdateSupplierDetails: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSupplierDetailsRequest**](UpdateSupplierDetailsRequest.md)|  | [optional] 

### Return type

[**UpdateSupplierDetailsResponse**](UpdateSupplierDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


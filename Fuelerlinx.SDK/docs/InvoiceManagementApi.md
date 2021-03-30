# IO.Swagger.Api.InvoiceManagementApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteSupportedInvoiceImportFileTests**](InvoiceManagementApi.md#deletesupportedinvoiceimportfiletests) | **DELETE** /api/InvoiceManagement/supported-invoice-import-file-tests/{id} | Deletes Supported Invoice Import File Tests by Id
[**DeleteSupportedInvoiceImportFiles**](InvoiceManagementApi.md#deletesupportedinvoiceimportfiles) | **DELETE** /api/InvoiceManagement/supported-invoice-import-files/{id} | Deletes Supported Invoice Import Files
[**GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId**](InvoiceManagementApi.md#getsupportedinvoiceimportfiletestsbysupportedinvoiceimportfileid) | **GET** /api/InvoiceManagement/supported-invoice-import-file-tests/by-supportedInvoiceImportFileId/{supportedInvoiceImportFileId} | Get Supported Invoice Import File Tests By supportedInvoiceImportFileId
[**GetSupportedInvoiceImportFilesByFuelVendorId**](InvoiceManagementApi.md#getsupportedinvoiceimportfilesbyfuelvendorid) | **GET** /api/InvoiceManagement/supported-invoice-import-files/by-fuel-vendor/{fuelVendorId} | Gets Supported Invoice Import Files By FuelVendorId
[**GetSupportedInvoiceImportFilesById**](InvoiceManagementApi.md#getsupportedinvoiceimportfilesbyid) | **GET** /api/InvoiceManagement/supported-invoice-import-files/{id} | Gets Supported Invoice Import Files By Id
[**PostSupportedInvoiceImportFileTests**](InvoiceManagementApi.md#postsupportedinvoiceimportfiletests) | **POST** /api/InvoiceManagement/supported-invoice-import-file-tests | Post Supported Invoice Import File Tests
[**PostSupportedInvoiceImportFiles**](InvoiceManagementApi.md#postsupportedinvoiceimportfiles) | **POST** /api/InvoiceManagement/supported-invoice-import-files | Post Supported Invoice Import Files
[**UpdateSupportedInvoiceImportFileTests**](InvoiceManagementApi.md#updatesupportedinvoiceimportfiletests) | **PUT** /api/InvoiceManagement/supported-invoice-import-file-tests | Updates Supported Invoice Import File Tests
[**UpdateSupportedInvoiceImportFiles**](InvoiceManagementApi.md#updatesupportedinvoiceimportfiles) | **PUT** /api/InvoiceManagement/supported-invoice-import-files | Updates Supported Invoice Import Files


<a name="deletesupportedinvoiceimportfiletests"></a>
# **DeleteSupportedInvoiceImportFileTests**
> DeleteSupportedInvoiceImportFileTestsResponse DeleteSupportedInvoiceImportFileTests (int? id)

Deletes Supported Invoice Import File Tests by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSupportedInvoiceImportFileTestsExample
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

            var apiInstance = new InvoiceManagementApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes Supported Invoice Import File Tests by Id
                DeleteSupportedInvoiceImportFileTestsResponse result = apiInstance.DeleteSupportedInvoiceImportFileTests(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.DeleteSupportedInvoiceImportFileTests: " + e.Message );
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

[**DeleteSupportedInvoiceImportFileTestsResponse**](DeleteSupportedInvoiceImportFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletesupportedinvoiceimportfiles"></a>
# **DeleteSupportedInvoiceImportFiles**
> DeleteSupportedInvoiceImportFilesResponse DeleteSupportedInvoiceImportFiles (int? id)

Deletes Supported Invoice Import Files

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSupportedInvoiceImportFilesExample
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

            var apiInstance = new InvoiceManagementApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes Supported Invoice Import Files
                DeleteSupportedInvoiceImportFilesResponse result = apiInstance.DeleteSupportedInvoiceImportFiles(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.DeleteSupportedInvoiceImportFiles: " + e.Message );
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

[**DeleteSupportedInvoiceImportFilesResponse**](DeleteSupportedInvoiceImportFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedinvoiceimportfiletestsbysupportedinvoiceimportfileid"></a>
# **GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId**
> SupportedInvoiceImportFileTestsResponse GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId (int? supportedInvoiceImportFileId)

Get Supported Invoice Import File Tests By supportedInvoiceImportFileId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileIdExample
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

            var apiInstance = new InvoiceManagementApi();
            var supportedInvoiceImportFileId = 56;  // int? | 

            try
            {
                // Get Supported Invoice Import File Tests By supportedInvoiceImportFileId
                SupportedInvoiceImportFileTestsResponse result = apiInstance.GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId(supportedInvoiceImportFileId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.GetSupportedInvoiceImportFileTestsBySupportedInvoiceImportFileId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **supportedInvoiceImportFileId** | **int?**|  | 

### Return type

[**SupportedInvoiceImportFileTestsResponse**](SupportedInvoiceImportFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedinvoiceimportfilesbyfuelvendorid"></a>
# **GetSupportedInvoiceImportFilesByFuelVendorId**
> SupportedInvoiceImportFilesResponse GetSupportedInvoiceImportFilesByFuelVendorId (int? fuelVendorId)

Gets Supported Invoice Import Files By FuelVendorId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedInvoiceImportFilesByFuelVendorIdExample
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

            var apiInstance = new InvoiceManagementApi();
            var fuelVendorId = 56;  // int? | 

            try
            {
                // Gets Supported Invoice Import Files By FuelVendorId
                SupportedInvoiceImportFilesResponse result = apiInstance.GetSupportedInvoiceImportFilesByFuelVendorId(fuelVendorId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.GetSupportedInvoiceImportFilesByFuelVendorId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fuelVendorId** | **int?**|  | 

### Return type

[**SupportedInvoiceImportFilesResponse**](SupportedInvoiceImportFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedinvoiceimportfilesbyid"></a>
# **GetSupportedInvoiceImportFilesById**
> SupportedInvoiceImportFilesResponse GetSupportedInvoiceImportFilesById (int? id)

Gets Supported Invoice Import Files By Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedInvoiceImportFilesByIdExample
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

            var apiInstance = new InvoiceManagementApi();
            var id = 56;  // int? | 

            try
            {
                // Gets Supported Invoice Import Files By Id
                SupportedInvoiceImportFilesResponse result = apiInstance.GetSupportedInvoiceImportFilesById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.GetSupportedInvoiceImportFilesById: " + e.Message );
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

[**SupportedInvoiceImportFilesResponse**](SupportedInvoiceImportFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsupportedinvoiceimportfiletests"></a>
# **PostSupportedInvoiceImportFileTests**
> PostSupportedInvoiceImportFileTestsResponse PostSupportedInvoiceImportFileTests (PostSupportedInvoiceImportFileTestsRequest body)

Post Supported Invoice Import File Tests

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSupportedInvoiceImportFileTestsExample
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

            var apiInstance = new InvoiceManagementApi();
            var body = new PostSupportedInvoiceImportFileTestsRequest(); // PostSupportedInvoiceImportFileTestsRequest |  (optional) 

            try
            {
                // Post Supported Invoice Import File Tests
                PostSupportedInvoiceImportFileTestsResponse result = apiInstance.PostSupportedInvoiceImportFileTests(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.PostSupportedInvoiceImportFileTests: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSupportedInvoiceImportFileTestsRequest**](PostSupportedInvoiceImportFileTestsRequest.md)|  | [optional] 

### Return type

[**PostSupportedInvoiceImportFileTestsResponse**](PostSupportedInvoiceImportFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsupportedinvoiceimportfiles"></a>
# **PostSupportedInvoiceImportFiles**
> PostSupportedInvoiceImportFilesResponse PostSupportedInvoiceImportFiles (PostSupportedInvoiceImportFilesRequest body)

Post Supported Invoice Import Files

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSupportedInvoiceImportFilesExample
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

            var apiInstance = new InvoiceManagementApi();
            var body = new PostSupportedInvoiceImportFilesRequest(); // PostSupportedInvoiceImportFilesRequest |  (optional) 

            try
            {
                // Post Supported Invoice Import Files
                PostSupportedInvoiceImportFilesResponse result = apiInstance.PostSupportedInvoiceImportFiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.PostSupportedInvoiceImportFiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSupportedInvoiceImportFilesRequest**](PostSupportedInvoiceImportFilesRequest.md)|  | [optional] 

### Return type

[**PostSupportedInvoiceImportFilesResponse**](PostSupportedInvoiceImportFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesupportedinvoiceimportfiletests"></a>
# **UpdateSupportedInvoiceImportFileTests**
> UpdateSupportedInvoiceImportFileTestsResponse UpdateSupportedInvoiceImportFileTests (UpdateSupportedInvoiceImportFileTestsRequest body)

Updates Supported Invoice Import File Tests

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSupportedInvoiceImportFileTestsExample
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

            var apiInstance = new InvoiceManagementApi();
            var body = new UpdateSupportedInvoiceImportFileTestsRequest(); // UpdateSupportedInvoiceImportFileTestsRequest |  (optional) 

            try
            {
                // Updates Supported Invoice Import File Tests
                UpdateSupportedInvoiceImportFileTestsResponse result = apiInstance.UpdateSupportedInvoiceImportFileTests(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.UpdateSupportedInvoiceImportFileTests: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSupportedInvoiceImportFileTestsRequest**](UpdateSupportedInvoiceImportFileTestsRequest.md)|  | [optional] 

### Return type

[**UpdateSupportedInvoiceImportFileTestsResponse**](UpdateSupportedInvoiceImportFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesupportedinvoiceimportfiles"></a>
# **UpdateSupportedInvoiceImportFiles**
> UpdateSupportedInvoiceImportFilesResponse UpdateSupportedInvoiceImportFiles (UpdateSupportedInvoiceImportFilesRequest body)

Updates Supported Invoice Import Files

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSupportedInvoiceImportFilesExample
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

            var apiInstance = new InvoiceManagementApi();
            var body = new UpdateSupportedInvoiceImportFilesRequest(); // UpdateSupportedInvoiceImportFilesRequest |  (optional) 

            try
            {
                // Updates Supported Invoice Import Files
                UpdateSupportedInvoiceImportFilesResponse result = apiInstance.UpdateSupportedInvoiceImportFiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling InvoiceManagementApi.UpdateSupportedInvoiceImportFiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSupportedInvoiceImportFilesRequest**](UpdateSupportedInvoiceImportFilesRequest.md)|  | [optional] 

### Return type

[**UpdateSupportedInvoiceImportFilesResponse**](UpdateSupportedInvoiceImportFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


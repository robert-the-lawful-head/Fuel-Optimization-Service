# IO.Swagger.Api.FileDataApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteImageFileData**](FileDataApi.md#deleteimagefiledata) | **DELETE** /api/FileData/image-file-data/{id} | Internal use only - Delete image file data by {id}.
[**DeleteImportFileCapture**](FileDataApi.md#deleteimportfilecapture) | **DELETE** /api/FileData/import-file-capture/{id} | Internal use only - Delete a file captured during an import.
[**DeleteJobFileData**](FileDataApi.md#deletejobfiledata) | **DELETE** /api/FileData/job-file-data/{id} | Deletes job file data by Id
[**DeletePriceSheetFileData**](FileDataApi.md#deletepricesheetfiledata) | **DELETE** /api/FileData/price-sheet-file-data/{id} | Delete price sheet file data by the provided {id}.
[**DeleteSupportedInvoiceFileDataTemplate**](FileDataApi.md#deletesupportedinvoicefiledatatemplate) | **DELETE** /api/FileData/invoice-file/supported-template/{id} | Internal use only - Delete a supported invoice file template by it&#39;s Id.
[**DeleteTransactionFileData**](FileDataApi.md#deletetransactionfiledata) | **DELETE** /api/FileData/transaction-file-data/{id} | Delete transaction file data by the provided {id}.
[**GetImageFileDataById**](FileDataApi.md#getimagefiledatabyid) | **GET** /api/FileData/image-file-data/{id} | Internal use only - Fetch image file data by {id}.
[**GetImportFileCaptureById**](FileDataApi.md#getimportfilecapturebyid) | **GET** /api/FileData/import-file-capture/{id} | Internal use only - Fetch a captured file import by Id.
[**GetPriceSheetFileData**](FileDataApi.md#getpricesheetfiledata) | **GET** /api/FileData/price-sheet-file-data/{id} | Fetch price sheet file data captured during an upload of pricing info.
[**GetSupportedInvoiceFileTemplatesByCompany**](FileDataApi.md#getsupportedinvoicefiletemplatesbycompany) | **GET** /api/FileData/invoice-file/supported-template/fueler/{fuelerProcessName}/company/{companyId} | Internal use only - Fetch a supported invoice file template by fuel vendor and company.
[**GetSupportedInvoiceFileTemplatesByFuelVendor**](FileDataApi.md#getsupportedinvoicefiletemplatesbyfuelvendor) | **GET** /api/FileData/invoice-file/supported-template/fueler/{fuelerProcessName} | Internal use only - Fetch a supported invoice file template by fuel vendor.
[**GetTransactionFileData**](FileDataApi.md#gettransactionfiledata) | **GET** /api/FileData/transaction-file-data/{id} | Fetch transaction file data for an invoice, receipt, or fuel release.
[**GetsJobFileDataById**](FileDataApi.md#getsjobfiledatabyid) | **GET** /api/FileData/job-file-data/by-id/{id} | Gets job file data by Id
[**PostImageFileData**](FileDataApi.md#postimagefiledata) | **POST** /api/FileData/image-file-data | Internal use only - Post new image file data.
[**PostImportFileCapture**](FileDataApi.md#postimportfilecapture) | **POST** /api/FileData/import-file-capture | Internal use only - Add a captured file that was recently imported.
[**PostJobFileData**](FileDataApi.md#postjobfiledata) | **POST** /api/FileData/job-file-data | Post job file data
[**PostPriceSheetFileData**](FileDataApi.md#postpricesheetfiledata) | **POST** /api/FileData/price-sheet-file-data | Add price sheet file data for an uploaded fuel price sheet.  The file data should be passed as a base64 string.  This is for capturing purposes only and will NOT update pricing.
[**PostSupportedInvoiceFileTemplate**](FileDataApi.md#postsupportedinvoicefiletemplate) | **POST** /api/FileData/invoice-file/supported-template | Internal use only - Add a supported invoice file template.
[**PostTransactionFileData**](FileDataApi.md#posttransactionfiledata) | **POST** /api/FileData/transaction-file-data | Add transaction file data for an invoice, receipt, or fuel release.  The file data should be passed as a base64 string.
[**UpdateImageFileData**](FileDataApi.md#updateimagefiledata) | **PUT** /api/FileData/image-file-data/{id} | Internal use only - Update an existing record of image file data.
[**UpdateImportFileCapture**](FileDataApi.md#updateimportfilecapture) | **PUT** /api/FileData/import-file-capture/{id} | Internal use only - Update a file captured during an import.
[**UpdateJobFileData**](FileDataApi.md#updatejobfiledata) | **PUT** /api/FileData/job-file-data | Updates job file data
[**UpdatePriceSheetFileData**](FileDataApi.md#updatepricesheetfiledata) | **PUT** /api/FileData/price-sheet-file-data | Update price sheet file data for an uploaded fuel price sheet.  This is for capturing purposes only and will NOT update pricing.
[**UpdateSupportedInvoiceFileDataTemplate**](FileDataApi.md#updatesupportedinvoicefiledatatemplate) | **PUT** /api/FileData/invoice-file/supported-template | Internal use only - Update a supported invoice file template.
[**UpdateTransactionFileData**](FileDataApi.md#updatetransactionfiledata) | **PUT** /api/FileData/transaction-file-data | Update transaction file data for an invoice, receipt, or fuel release.


<a name="deleteimagefiledata"></a>
# **DeleteImageFileData**
> DeleteImageFileDataResponse DeleteImageFileData (int? id)

Internal use only - Delete image file data by {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteImageFileDataExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete image file data by {id}.
                DeleteImageFileDataResponse result = apiInstance.DeleteImageFileData(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.DeleteImageFileData: " + e.Message );
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

[**DeleteImageFileDataResponse**](DeleteImageFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteimportfilecapture"></a>
# **DeleteImportFileCapture**
> DeleteImportFileCaptureResponse DeleteImportFileCapture (int? id)

Internal use only - Delete a file captured during an import.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteImportFileCaptureExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete a file captured during an import.
                DeleteImportFileCaptureResponse result = apiInstance.DeleteImportFileCapture(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.DeleteImportFileCapture: " + e.Message );
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

[**DeleteImportFileCaptureResponse**](DeleteImportFileCaptureResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletejobfiledata"></a>
# **DeleteJobFileData**
> DeleteJobFileDataResponse DeleteJobFileData (int? id)

Deletes job file data by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteJobFileDataExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes job file data by Id
                DeleteJobFileDataResponse result = apiInstance.DeleteJobFileData(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.DeleteJobFileData: " + e.Message );
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

[**DeleteJobFileDataResponse**](DeleteJobFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletepricesheetfiledata"></a>
# **DeletePriceSheetFileData**
> DeletePriceSheetFileDataResponse DeletePriceSheetFileData (int? id)

Delete price sheet file data by the provided {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeletePriceSheetFileDataExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Delete price sheet file data by the provided {id}.
                DeletePriceSheetFileDataResponse result = apiInstance.DeletePriceSheetFileData(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.DeletePriceSheetFileData: " + e.Message );
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

[**DeletePriceSheetFileDataResponse**](DeletePriceSheetFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletesupportedinvoicefiledatatemplate"></a>
# **DeleteSupportedInvoiceFileDataTemplate**
> DeleteSupportedInvoiceFileTemplateResponse DeleteSupportedInvoiceFileDataTemplate (int? id)

Internal use only - Delete a supported invoice file template by it's Id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSupportedInvoiceFileDataTemplateExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete a supported invoice file template by it's Id.
                DeleteSupportedInvoiceFileTemplateResponse result = apiInstance.DeleteSupportedInvoiceFileDataTemplate(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.DeleteSupportedInvoiceFileDataTemplate: " + e.Message );
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

[**DeleteSupportedInvoiceFileTemplateResponse**](DeleteSupportedInvoiceFileTemplateResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletetransactionfiledata"></a>
# **DeleteTransactionFileData**
> DeleteTransactionFileDataResponse DeleteTransactionFileData (int? id)

Delete transaction file data by the provided {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTransactionFileDataExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Delete transaction file data by the provided {id}.
                DeleteTransactionFileDataResponse result = apiInstance.DeleteTransactionFileData(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.DeleteTransactionFileData: " + e.Message );
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

[**DeleteTransactionFileDataResponse**](DeleteTransactionFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getimagefiledatabyid"></a>
# **GetImageFileDataById**
> ImageFileDataResponse GetImageFileDataById (int? id)

Internal use only - Fetch image file data by {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetImageFileDataByIdExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Fetch image file data by {id}.
                ImageFileDataResponse result = apiInstance.GetImageFileDataById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.GetImageFileDataById: " + e.Message );
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

[**ImageFileDataResponse**](ImageFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getimportfilecapturebyid"></a>
# **GetImportFileCaptureById**
> ImportFileCaptureResponse GetImportFileCaptureById (int? id)

Internal use only - Fetch a captured file import by Id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetImportFileCaptureByIdExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Fetch a captured file import by Id.
                ImportFileCaptureResponse result = apiInstance.GetImportFileCaptureById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.GetImportFileCaptureById: " + e.Message );
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

[**ImportFileCaptureResponse**](ImportFileCaptureResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getpricesheetfiledata"></a>
# **GetPriceSheetFileData**
> PriceSheetFileDataResponse GetPriceSheetFileData (int? id)

Fetch price sheet file data captured during an upload of pricing info.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetPriceSheetFileDataExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch price sheet file data captured during an upload of pricing info.
                PriceSheetFileDataResponse result = apiInstance.GetPriceSheetFileData(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.GetPriceSheetFileData: " + e.Message );
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

[**PriceSheetFileDataResponse**](PriceSheetFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedinvoicefiletemplatesbycompany"></a>
# **GetSupportedInvoiceFileTemplatesByCompany**
> SupportedInvoiceFileTemplateResponse GetSupportedInvoiceFileTemplatesByCompany (string fuelerProcessName, int? companyId)

Internal use only - Fetch a supported invoice file template by fuel vendor and company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedInvoiceFileTemplatesByCompanyExample
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

            var apiInstance = new FileDataApi();
            var fuelerProcessName = fuelerProcessName_example;  // string | 
            var companyId = 56;  // int? | 

            try
            {
                // Internal use only - Fetch a supported invoice file template by fuel vendor and company.
                SupportedInvoiceFileTemplateResponse result = apiInstance.GetSupportedInvoiceFileTemplatesByCompany(fuelerProcessName, companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.GetSupportedInvoiceFileTemplatesByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fuelerProcessName** | **string**|  | 
 **companyId** | **int?**|  | 

### Return type

[**SupportedInvoiceFileTemplateResponse**](SupportedInvoiceFileTemplateResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedinvoicefiletemplatesbyfuelvendor"></a>
# **GetSupportedInvoiceFileTemplatesByFuelVendor**
> SupportedInvoiceFileTemplateResponse GetSupportedInvoiceFileTemplatesByFuelVendor (string fuelerProcessName)

Internal use only - Fetch a supported invoice file template by fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedInvoiceFileTemplatesByFuelVendorExample
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

            var apiInstance = new FileDataApi();
            var fuelerProcessName = fuelerProcessName_example;  // string | 

            try
            {
                // Internal use only - Fetch a supported invoice file template by fuel vendor.
                SupportedInvoiceFileTemplateResponse result = apiInstance.GetSupportedInvoiceFileTemplatesByFuelVendor(fuelerProcessName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.GetSupportedInvoiceFileTemplatesByFuelVendor: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fuelerProcessName** | **string**|  | 

### Return type

[**SupportedInvoiceFileTemplateResponse**](SupportedInvoiceFileTemplateResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionfiledata"></a>
# **GetTransactionFileData**
> TransactionFileDataResponse GetTransactionFileData (int? id)

Fetch transaction file data for an invoice, receipt, or fuel release.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionFileDataExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch transaction file data for an invoice, receipt, or fuel release.
                TransactionFileDataResponse result = apiInstance.GetTransactionFileData(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.GetTransactionFileData: " + e.Message );
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

[**TransactionFileDataResponse**](TransactionFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsjobfiledatabyid"></a>
# **GetsJobFileDataById**
> JobFileDataResponse GetsJobFileDataById (int? id)

Gets job file data by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetsJobFileDataByIdExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 

            try
            {
                // Gets job file data by Id
                JobFileDataResponse result = apiInstance.GetsJobFileDataById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.GetsJobFileDataById: " + e.Message );
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

[**JobFileDataResponse**](JobFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postimagefiledata"></a>
# **PostImageFileData**
> PostImageFileDataResponse PostImageFileData (PostImageFileDataRequest body)

Internal use only - Post new image file data.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostImageFileDataExample
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

            var apiInstance = new FileDataApi();
            var body = new PostImageFileDataRequest(); // PostImageFileDataRequest |  (optional) 

            try
            {
                // Internal use only - Post new image file data.
                PostImageFileDataResponse result = apiInstance.PostImageFileData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.PostImageFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostImageFileDataRequest**](PostImageFileDataRequest.md)|  | [optional] 

### Return type

[**PostImageFileDataResponse**](PostImageFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postimportfilecapture"></a>
# **PostImportFileCapture**
> PostImportFileCaptureResponse PostImportFileCapture (PostImportFileCaptureRequest body)

Internal use only - Add a captured file that was recently imported.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostImportFileCaptureExample
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

            var apiInstance = new FileDataApi();
            var body = new PostImportFileCaptureRequest(); // PostImportFileCaptureRequest |  (optional) 

            try
            {
                // Internal use only - Add a captured file that was recently imported.
                PostImportFileCaptureResponse result = apiInstance.PostImportFileCapture(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.PostImportFileCapture: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostImportFileCaptureRequest**](PostImportFileCaptureRequest.md)|  | [optional] 

### Return type

[**PostImportFileCaptureResponse**](PostImportFileCaptureResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postjobfiledata"></a>
# **PostJobFileData**
> PostJobFileDataResponse PostJobFileData (PostJobFileDataRequest body)

Post job file data

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostJobFileDataExample
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

            var apiInstance = new FileDataApi();
            var body = new PostJobFileDataRequest(); // PostJobFileDataRequest |  (optional) 

            try
            {
                // Post job file data
                PostJobFileDataResponse result = apiInstance.PostJobFileData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.PostJobFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostJobFileDataRequest**](PostJobFileDataRequest.md)|  | [optional] 

### Return type

[**PostJobFileDataResponse**](PostJobFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postpricesheetfiledata"></a>
# **PostPriceSheetFileData**
> PostPriceSheetFileDataResponse PostPriceSheetFileData (PostPriceSheetFileDataRequest body)

Add price sheet file data for an uploaded fuel price sheet.  The file data should be passed as a base64 string.  This is for capturing purposes only and will NOT update pricing.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostPriceSheetFileDataExample
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

            var apiInstance = new FileDataApi();
            var body = new PostPriceSheetFileDataRequest(); // PostPriceSheetFileDataRequest |  (optional) 

            try
            {
                // Add price sheet file data for an uploaded fuel price sheet.  The file data should be passed as a base64 string.  This is for capturing purposes only and will NOT update pricing.
                PostPriceSheetFileDataResponse result = apiInstance.PostPriceSheetFileData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.PostPriceSheetFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostPriceSheetFileDataRequest**](PostPriceSheetFileDataRequest.md)|  | [optional] 

### Return type

[**PostPriceSheetFileDataResponse**](PostPriceSheetFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsupportedinvoicefiletemplate"></a>
# **PostSupportedInvoiceFileTemplate**
> PostSupportedInvoiceFileTemplateResponse PostSupportedInvoiceFileTemplate (PostSupportedInvoiceFileTemplateRequest body)

Internal use only - Add a supported invoice file template.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSupportedInvoiceFileTemplateExample
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

            var apiInstance = new FileDataApi();
            var body = new PostSupportedInvoiceFileTemplateRequest(); // PostSupportedInvoiceFileTemplateRequest |  (optional) 

            try
            {
                // Internal use only - Add a supported invoice file template.
                PostSupportedInvoiceFileTemplateResponse result = apiInstance.PostSupportedInvoiceFileTemplate(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.PostSupportedInvoiceFileTemplate: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSupportedInvoiceFileTemplateRequest**](PostSupportedInvoiceFileTemplateRequest.md)|  | [optional] 

### Return type

[**PostSupportedInvoiceFileTemplateResponse**](PostSupportedInvoiceFileTemplateResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttransactionfiledata"></a>
# **PostTransactionFileData**
> PostTransactionFileDataResponse PostTransactionFileData (PostTransactionFileDataRequest body)

Add transaction file data for an invoice, receipt, or fuel release.  The file data should be passed as a base64 string.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTransactionFileDataExample
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

            var apiInstance = new FileDataApi();
            var body = new PostTransactionFileDataRequest(); // PostTransactionFileDataRequest |  (optional) 

            try
            {
                // Add transaction file data for an invoice, receipt, or fuel release.  The file data should be passed as a base64 string.
                PostTransactionFileDataResponse result = apiInstance.PostTransactionFileData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.PostTransactionFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTransactionFileDataRequest**](PostTransactionFileDataRequest.md)|  | [optional] 

### Return type

[**PostTransactionFileDataResponse**](PostTransactionFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateimagefiledata"></a>
# **UpdateImageFileData**
> UpdateImageFileDataResponse UpdateImageFileData (int? id, UpdateImageFileDataRequest body)

Internal use only - Update an existing record of image file data.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateImageFileDataExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 
            var body = new UpdateImageFileDataRequest(); // UpdateImageFileDataRequest |  (optional) 

            try
            {
                // Internal use only - Update an existing record of image file data.
                UpdateImageFileDataResponse result = apiInstance.UpdateImageFileData(id, body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.UpdateImageFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 
 **body** | [**UpdateImageFileDataRequest**](UpdateImageFileDataRequest.md)|  | [optional] 

### Return type

[**UpdateImageFileDataResponse**](UpdateImageFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateimportfilecapture"></a>
# **UpdateImportFileCapture**
> UpdateImportFileCaptureResponse UpdateImportFileCapture (int? id, UpdateImportFileCaptureRequest body)

Internal use only - Update a file captured during an import.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateImportFileCaptureExample
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

            var apiInstance = new FileDataApi();
            var id = 56;  // int? | 
            var body = new UpdateImportFileCaptureRequest(); // UpdateImportFileCaptureRequest |  (optional) 

            try
            {
                // Internal use only - Update a file captured during an import.
                UpdateImportFileCaptureResponse result = apiInstance.UpdateImportFileCapture(id, body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.UpdateImportFileCapture: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 
 **body** | [**UpdateImportFileCaptureRequest**](UpdateImportFileCaptureRequest.md)|  | [optional] 

### Return type

[**UpdateImportFileCaptureResponse**](UpdateImportFileCaptureResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatejobfiledata"></a>
# **UpdateJobFileData**
> UpdateJobFileDataResponse UpdateJobFileData (UpdateJobFileDataRequest body)

Updates job file data

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateJobFileDataExample
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

            var apiInstance = new FileDataApi();
            var body = new UpdateJobFileDataRequest(); // UpdateJobFileDataRequest |  (optional) 

            try
            {
                // Updates job file data
                UpdateJobFileDataResponse result = apiInstance.UpdateJobFileData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.UpdateJobFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateJobFileDataRequest**](UpdateJobFileDataRequest.md)|  | [optional] 

### Return type

[**UpdateJobFileDataResponse**](UpdateJobFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatepricesheetfiledata"></a>
# **UpdatePriceSheetFileData**
> UpdatePriceSheetFileDataResponse UpdatePriceSheetFileData (UpdatePriceSheetFileDataRequest body)

Update price sheet file data for an uploaded fuel price sheet.  This is for capturing purposes only and will NOT update pricing.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdatePriceSheetFileDataExample
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

            var apiInstance = new FileDataApi();
            var body = new UpdatePriceSheetFileDataRequest(); // UpdatePriceSheetFileDataRequest |  (optional) 

            try
            {
                // Update price sheet file data for an uploaded fuel price sheet.  This is for capturing purposes only and will NOT update pricing.
                UpdatePriceSheetFileDataResponse result = apiInstance.UpdatePriceSheetFileData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.UpdatePriceSheetFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdatePriceSheetFileDataRequest**](UpdatePriceSheetFileDataRequest.md)|  | [optional] 

### Return type

[**UpdatePriceSheetFileDataResponse**](UpdatePriceSheetFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesupportedinvoicefiledatatemplate"></a>
# **UpdateSupportedInvoiceFileDataTemplate**
> UpdateSupportedInvoiceFileTemplateResponse UpdateSupportedInvoiceFileDataTemplate (UpdateSupportedFileTemplateRequest body)

Internal use only - Update a supported invoice file template.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSupportedInvoiceFileDataTemplateExample
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

            var apiInstance = new FileDataApi();
            var body = new UpdateSupportedFileTemplateRequest(); // UpdateSupportedFileTemplateRequest |  (optional) 

            try
            {
                // Internal use only - Update a supported invoice file template.
                UpdateSupportedInvoiceFileTemplateResponse result = apiInstance.UpdateSupportedInvoiceFileDataTemplate(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.UpdateSupportedInvoiceFileDataTemplate: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSupportedFileTemplateRequest**](UpdateSupportedFileTemplateRequest.md)|  | [optional] 

### Return type

[**UpdateSupportedInvoiceFileTemplateResponse**](UpdateSupportedInvoiceFileTemplateResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetransactionfiledata"></a>
# **UpdateTransactionFileData**
> UpdateTransactionFileDataResponse UpdateTransactionFileData (UpdateTransactionFileDataRequest body)

Update transaction file data for an invoice, receipt, or fuel release.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTransactionFileDataExample
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

            var apiInstance = new FileDataApi();
            var body = new UpdateTransactionFileDataRequest(); // UpdateTransactionFileDataRequest |  (optional) 

            try
            {
                // Update transaction file data for an invoice, receipt, or fuel release.
                UpdateTransactionFileDataResponse result = apiInstance.UpdateTransactionFileData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FileDataApi.UpdateTransactionFileData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTransactionFileDataRequest**](UpdateTransactionFileDataRequest.md)|  | [optional] 

### Return type

[**UpdateTransactionFileDataResponse**](UpdateTransactionFileDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


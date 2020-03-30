# IO.Swagger.Api.AnalysisApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCustomReport**](AnalysisApi.md#deletecustomreport) | **DELETE** /api/Analysis/custom-reports/{id} | Delete a custom report by the provided {id}.
[**DeleteEmailAddressesForEmailedAnalysis**](AnalysisApi.md#deleteemailaddressesforemailedanalysis) | **DELETE** /api/Analysis/email-blast/email-addresses/{id} | Internal use only - Delete the subscriber record for an email blast.  This will reset the subscriber list to a company-default for the blast.
[**GetCustomReportById**](AnalysisApi.md#getcustomreportbyid) | **GET** /api/Analysis/custom-reports/{id} | Fetch a custom report by it&#39;s {id}.
[**GetEmailAddressesForMonthlyAnalysis**](AnalysisApi.md#getemailaddressesformonthlyanalysis) | **GET** /api/Analysis/email-blast/{emailBlastId}/email-addresses | Internal use only - Fetch all subscribers for a particular email blast.
[**GetIndustryAveragePriceByTransaction**](AnalysisApi.md#getindustryaveragepricebytransaction) | **GET** /api/Analysis/industry-average/by-transaction/{transactionId} | 
[**GetReportListByCompanyId**](AnalysisApi.md#getreportlistbycompanyid) | **GET** /api/Analysis/custom-reports/list | Fetch a list of reports for the authenticated company.
[**PostCustomReport**](AnalysisApi.md#postcustomreport) | **POST** /api/Analysis/custom-reports | Add a new custom report for the authenticated company.
[**PostEmailAddressesForMonthlyAnalysis**](AnalysisApi.md#postemailaddressesformonthlyanalysis) | **POST** /api/Analysis/email-blast/email-addresses | Internal use only - Add a new subscriber-set record to an email blast.
[**UpdateCustomReport**](AnalysisApi.md#updatecustomreport) | **PUT** /api/Analysis/custom-reports | Update a custom report.
[**UpdateEmailAddressesForEmailedAnalysis**](AnalysisApi.md#updateemailaddressesforemailedanalysis) | **PUT** /api/Analysis/email-blast/email-addresses/{id} | Internal use only - Update an existing record of subscribers for an email blast.


<a name="deletecustomreport"></a>
# **DeleteCustomReport**
> DeleteReportResponse DeleteCustomReport (int? id)

Delete a custom report by the provided {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCustomReportExample
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

            var apiInstance = new AnalysisApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a custom report by the provided {id}.
                DeleteReportResponse result = apiInstance.DeleteCustomReport(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.DeleteCustomReport: " + e.Message );
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

[**DeleteReportResponse**](DeleteReportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteemailaddressesforemailedanalysis"></a>
# **DeleteEmailAddressesForEmailedAnalysis**
> DeleteEmailBlastEmailAddressesResponse DeleteEmailAddressesForEmailedAnalysis (int? id)

Internal use only - Delete the subscriber record for an email blast.  This will reset the subscriber list to a company-default for the blast.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteEmailAddressesForEmailedAnalysisExample
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

            var apiInstance = new AnalysisApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete the subscriber record for an email blast.  This will reset the subscriber list to a company-default for the blast.
                DeleteEmailBlastEmailAddressesResponse result = apiInstance.DeleteEmailAddressesForEmailedAnalysis(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.DeleteEmailAddressesForEmailedAnalysis: " + e.Message );
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

[**DeleteEmailBlastEmailAddressesResponse**](DeleteEmailBlastEmailAddressesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcustomreportbyid"></a>
# **GetCustomReportById**
> ReportResponse GetCustomReportById (int? id)

Fetch a custom report by it's {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCustomReportByIdExample
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

            var apiInstance = new AnalysisApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch a custom report by it's {id}.
                ReportResponse result = apiInstance.GetCustomReportById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetCustomReportById: " + e.Message );
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

[**ReportResponse**](ReportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getemailaddressesformonthlyanalysis"></a>
# **GetEmailAddressesForMonthlyAnalysis**
> EmailBlastEmailAddressesResponse GetEmailAddressesForMonthlyAnalysis (int? emailBlastId)

Internal use only - Fetch all subscribers for a particular email blast.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetEmailAddressesForMonthlyAnalysisExample
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

            var apiInstance = new AnalysisApi();
            var emailBlastId = 56;  // int? | 

            try
            {
                // Internal use only - Fetch all subscribers for a particular email blast.
                EmailBlastEmailAddressesResponse result = apiInstance.GetEmailAddressesForMonthlyAnalysis(emailBlastId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetEmailAddressesForMonthlyAnalysis: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **emailBlastId** | **int?**|  | 

### Return type

[**EmailBlastEmailAddressesResponse**](EmailBlastEmailAddressesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getindustryaveragepricebytransaction"></a>
# **GetIndustryAveragePriceByTransaction**
> IndustryAveragePriceByTransactionResponse GetIndustryAveragePriceByTransaction (int? transactionId)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIndustryAveragePriceByTransactionExample
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

            var apiInstance = new AnalysisApi();
            var transactionId = 56;  // int? | 

            try
            {
                IndustryAveragePriceByTransactionResponse result = apiInstance.GetIndustryAveragePriceByTransaction(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetIndustryAveragePriceByTransaction: " + e.Message );
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

[**IndustryAveragePriceByTransactionResponse**](IndustryAveragePriceByTransactionResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getreportlistbycompanyid"></a>
# **GetReportListByCompanyId**
> ReportListResponse GetReportListByCompanyId ()

Fetch a list of reports for the authenticated company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetReportListByCompanyIdExample
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

            var apiInstance = new AnalysisApi();

            try
            {
                // Fetch a list of reports for the authenticated company.
                ReportListResponse result = apiInstance.GetReportListByCompanyId();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetReportListByCompanyId: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**ReportListResponse**](ReportListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcustomreport"></a>
# **PostCustomReport**
> PostReportResponse PostCustomReport (PostReportRequest body)

Add a new custom report for the authenticated company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCustomReportExample
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

            var apiInstance = new AnalysisApi();
            var body = new PostReportRequest(); // PostReportRequest |  (optional) 

            try
            {
                // Add a new custom report for the authenticated company.
                PostReportResponse result = apiInstance.PostCustomReport(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.PostCustomReport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostReportRequest**](PostReportRequest.md)|  | [optional] 

### Return type

[**PostReportResponse**](PostReportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postemailaddressesformonthlyanalysis"></a>
# **PostEmailAddressesForMonthlyAnalysis**
> PostEmailBlastEmailAddressesResponse PostEmailAddressesForMonthlyAnalysis (PostEmailBlastEmailAddressesRequest body)

Internal use only - Add a new subscriber-set record to an email blast.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostEmailAddressesForMonthlyAnalysisExample
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

            var apiInstance = new AnalysisApi();
            var body = new PostEmailBlastEmailAddressesRequest(); // PostEmailBlastEmailAddressesRequest |  (optional) 

            try
            {
                // Internal use only - Add a new subscriber-set record to an email blast.
                PostEmailBlastEmailAddressesResponse result = apiInstance.PostEmailAddressesForMonthlyAnalysis(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.PostEmailAddressesForMonthlyAnalysis: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostEmailBlastEmailAddressesRequest**](PostEmailBlastEmailAddressesRequest.md)|  | [optional] 

### Return type

[**PostEmailBlastEmailAddressesResponse**](PostEmailBlastEmailAddressesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecustomreport"></a>
# **UpdateCustomReport**
> UpdateReportResponse UpdateCustomReport (UpdateReportRequest body)

Update a custom report.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCustomReportExample
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

            var apiInstance = new AnalysisApi();
            var body = new UpdateReportRequest(); // UpdateReportRequest |  (optional) 

            try
            {
                // Update a custom report.
                UpdateReportResponse result = apiInstance.UpdateCustomReport(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.UpdateCustomReport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateReportRequest**](UpdateReportRequest.md)|  | [optional] 

### Return type

[**UpdateReportResponse**](UpdateReportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateemailaddressesforemailedanalysis"></a>
# **UpdateEmailAddressesForEmailedAnalysis**
> UpdateEmailBlastEmailAddressesResponse UpdateEmailAddressesForEmailedAnalysis (int? id, UpdateEmailBlastEmailAddressesRequest body)

Internal use only - Update an existing record of subscribers for an email blast.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateEmailAddressesForEmailedAnalysisExample
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

            var apiInstance = new AnalysisApi();
            var id = 56;  // int? | 
            var body = new UpdateEmailBlastEmailAddressesRequest(); // UpdateEmailBlastEmailAddressesRequest |  (optional) 

            try
            {
                // Internal use only - Update an existing record of subscribers for an email blast.
                UpdateEmailBlastEmailAddressesResponse result = apiInstance.UpdateEmailAddressesForEmailedAnalysis(id, body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.UpdateEmailAddressesForEmailedAnalysis: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 
 **body** | [**UpdateEmailBlastEmailAddressesRequest**](UpdateEmailBlastEmailAddressesRequest.md)|  | [optional] 

### Return type

[**UpdateEmailBlastEmailAddressesResponse**](UpdateEmailBlastEmailAddressesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


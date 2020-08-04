# IO.Swagger.Api.AnalysisApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCustomReport**](AnalysisApi.md#deletecustomreport) | **DELETE** /api/Analysis/custom-reports/{id} | Delete a custom report by the provided {id}.
[**DeleteEmailAddressesForEmailedAnalysis**](AnalysisApi.md#deleteemailaddressesforemailedanalysis) | **DELETE** /api/Analysis/email-blast/email-addresses/{id} | Internal use only - Delete the subscriber record for an email blast.  This will reset the subscriber list to a company-default for the blast.
[**DeleteReportDistributionAssociation**](AnalysisApi.md#deletereportdistributionassociation) | **DELETE** /api/Analysis/custom-reports/distribution-association/{id} | 
[**DeleteReportScheduledDistribution**](AnalysisApi.md#deletereportscheduleddistribution) | **DELETE** /api/Analysis/custom-reports/distribution/{id} | Internal use only - Delete a scheduled report distribution record.
[**GetCustomReportById**](AnalysisApi.md#getcustomreportbyid) | **GET** /api/Analysis/custom-reports/{id} | Fetch a custom report by it&#39;s {id}.
[**GetEmailAddressesForMonthlyAnalysis**](AnalysisApi.md#getemailaddressesformonthlyanalysis) | **GET** /api/Analysis/email-blast/{emailBlastId}/email-addresses | Internal use only - Fetch all subscribers for a particular email blast.
[**GetIndustryAveragePriceByTransaction**](AnalysisApi.md#getindustryaveragepricebytransaction) | **GET** /api/Analysis/industry-average/by-transaction/{transactionId} | 
[**GetReportData**](AnalysisApi.md#getreportdata) | **POST** /api/Analysis/custom-reports/data | 
[**GetReportDataForDistribution**](AnalysisApi.md#getreportdatafordistribution) | **GET** /api/Analysis/custom-reports/data/{reportId}/distribution/{distributionId} | 
[**GetReportList**](AnalysisApi.md#getreportlist) | **GET** /api/Analysis/custom-reports/list | Fetch a list of reports for the authenticated company.
[**GetReportScheduledDistributionById**](AnalysisApi.md#getreportscheduleddistributionbyid) | **GET** /api/Analysis/custom-reports/distribution/{id} | Internal use only - Fetch reports scheduled for distribution by the scheduled distribution {id}.
[**GetReportScheduledDistributionList**](AnalysisApi.md#getreportscheduleddistributionlist) | **GET** /api/Analysis/custom-reports/distribution/list | Internal use only - Fetch reports that are scheduled for distribution.
[**GetReportScheduledDistributionListAllRequiringSending**](AnalysisApi.md#getreportscheduleddistributionlistallrequiringsending) | **GET** /api/Analysis/custom-reports/distribution/list/all/require-sending | Internal use only - Fetch all reports that are scheduled for distribution and need to be sent.
[**PostCustomReport**](AnalysisApi.md#postcustomreport) | **POST** /api/Analysis/custom-reports | Add a new custom report for the authenticated company.
[**PostEmailAddressesForMonthlyAnalysis**](AnalysisApi.md#postemailaddressesformonthlyanalysis) | **POST** /api/Analysis/email-blast/email-addresses | Internal use only - Add a new subscriber-set record to an email blast.
[**PostReportDistributionAssociation**](AnalysisApi.md#postreportdistributionassociation) | **POST** /api/Analysis/custom-reports/distribution-association | 
[**PostReportScheduledDistribution**](AnalysisApi.md#postreportscheduleddistribution) | **POST** /api/Analysis/custom-reports/distribution | Internal use only - Post a new scheduled report distribution record.
[**SendReportScheduledDistribution**](AnalysisApi.md#sendreportscheduleddistribution) | **POST** /api/Analysis/custom-reports/distribution/send | 
[**UpdateCustomReport**](AnalysisApi.md#updatecustomreport) | **PUT** /api/Analysis/custom-reports | Update a custom report.
[**UpdateEmailAddressesForEmailedAnalysis**](AnalysisApi.md#updateemailaddressesforemailedanalysis) | **PUT** /api/Analysis/email-blast/email-addresses/{id} | Internal use only - Update an existing record of subscribers for an email blast.
[**UpdateReportScheduledDistribution**](AnalysisApi.md#updatereportscheduleddistribution) | **PUT** /api/Analysis/custom-reports/distribution | Internal use only - Update a scheduled report distribution record.


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

<a name="deletereportdistributionassociation"></a>
# **DeleteReportDistributionAssociation**
> DeleteReportDistributionAssociationResponse DeleteReportDistributionAssociation (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteReportDistributionAssociationExample
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
                DeleteReportDistributionAssociationResponse result = apiInstance.DeleteReportDistributionAssociation(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.DeleteReportDistributionAssociation: " + e.Message );
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

[**DeleteReportDistributionAssociationResponse**](DeleteReportDistributionAssociationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletereportscheduleddistribution"></a>
# **DeleteReportScheduledDistribution**
> DeleteReportScheduledDistributionResponse DeleteReportScheduledDistribution (int? id)

Internal use only - Delete a scheduled report distribution record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteReportScheduledDistributionExample
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
                // Internal use only - Delete a scheduled report distribution record.
                DeleteReportScheduledDistributionResponse result = apiInstance.DeleteReportScheduledDistribution(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.DeleteReportScheduledDistribution: " + e.Message );
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

[**DeleteReportScheduledDistributionResponse**](DeleteReportScheduledDistributionResponse.md)

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

<a name="getreportdata"></a>
# **GetReportData**
> ReportDataResponse GetReportData (ReportDataJsonRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetReportDataExample
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
            var body = new ReportDataJsonRequest(); // ReportDataJsonRequest |  (optional) 

            try
            {
                ReportDataResponse result = apiInstance.GetReportData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetReportData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**ReportDataJsonRequest**](ReportDataJsonRequest.md)|  | [optional] 

### Return type

[**ReportDataResponse**](ReportDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getreportdatafordistribution"></a>
# **GetReportDataForDistribution**
> ReportDataResponse GetReportDataForDistribution (int? reportId, int? distributionId)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetReportDataForDistributionExample
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
            var reportId = 56;  // int? | 
            var distributionId = 56;  // int? | 

            try
            {
                ReportDataResponse result = apiInstance.GetReportDataForDistribution(reportId, distributionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetReportDataForDistribution: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **reportId** | **int?**|  | 
 **distributionId** | **int?**|  | 

### Return type

[**ReportDataResponse**](ReportDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getreportlist"></a>
# **GetReportList**
> ReportListResponse GetReportList ()

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
    public class GetReportListExample
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
                ReportListResponse result = apiInstance.GetReportList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetReportList: " + e.Message );
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

<a name="getreportscheduleddistributionbyid"></a>
# **GetReportScheduledDistributionById**
> ReportScheduledDistributionResponse GetReportScheduledDistributionById (int? id)

Internal use only - Fetch reports scheduled for distribution by the scheduled distribution {id}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetReportScheduledDistributionByIdExample
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
                // Internal use only - Fetch reports scheduled for distribution by the scheduled distribution {id}.
                ReportScheduledDistributionResponse result = apiInstance.GetReportScheduledDistributionById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetReportScheduledDistributionById: " + e.Message );
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

[**ReportScheduledDistributionResponse**](ReportScheduledDistributionResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getreportscheduleddistributionlist"></a>
# **GetReportScheduledDistributionList**
> ReportScheduledDistributionListResponse GetReportScheduledDistributionList ()

Internal use only - Fetch reports that are scheduled for distribution.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetReportScheduledDistributionListExample
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
                // Internal use only - Fetch reports that are scheduled for distribution.
                ReportScheduledDistributionListResponse result = apiInstance.GetReportScheduledDistributionList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetReportScheduledDistributionList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**ReportScheduledDistributionListResponse**](ReportScheduledDistributionListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getreportscheduleddistributionlistallrequiringsending"></a>
# **GetReportScheduledDistributionListAllRequiringSending**
> ReportScheduledDistributionListResponse GetReportScheduledDistributionListAllRequiringSending ()

Internal use only - Fetch all reports that are scheduled for distribution and need to be sent.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetReportScheduledDistributionListAllRequiringSendingExample
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
                // Internal use only - Fetch all reports that are scheduled for distribution and need to be sent.
                ReportScheduledDistributionListResponse result = apiInstance.GetReportScheduledDistributionListAllRequiringSending();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.GetReportScheduledDistributionListAllRequiringSending: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**ReportScheduledDistributionListResponse**](ReportScheduledDistributionListResponse.md)

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

<a name="postreportdistributionassociation"></a>
# **PostReportDistributionAssociation**
> PostReportDistributionAssociationResponse PostReportDistributionAssociation (PostReportDistributionAssociationRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostReportDistributionAssociationExample
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
            var body = new PostReportDistributionAssociationRequest(); // PostReportDistributionAssociationRequest |  (optional) 

            try
            {
                PostReportDistributionAssociationResponse result = apiInstance.PostReportDistributionAssociation(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.PostReportDistributionAssociation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostReportDistributionAssociationRequest**](PostReportDistributionAssociationRequest.md)|  | [optional] 

### Return type

[**PostReportDistributionAssociationResponse**](PostReportDistributionAssociationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postreportscheduleddistribution"></a>
# **PostReportScheduledDistribution**
> PostReportScheduledDistributionResponse PostReportScheduledDistribution (PostReportScheduledDistributionRequest body)

Internal use only - Post a new scheduled report distribution record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostReportScheduledDistributionExample
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
            var body = new PostReportScheduledDistributionRequest(); // PostReportScheduledDistributionRequest |  (optional) 

            try
            {
                // Internal use only - Post a new scheduled report distribution record.
                PostReportScheduledDistributionResponse result = apiInstance.PostReportScheduledDistribution(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.PostReportScheduledDistribution: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostReportScheduledDistributionRequest**](PostReportScheduledDistributionRequest.md)|  | [optional] 

### Return type

[**PostReportScheduledDistributionResponse**](PostReportScheduledDistributionResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="sendreportscheduleddistribution"></a>
# **SendReportScheduledDistribution**
> SendReportScheduledDistributionResponse SendReportScheduledDistribution (SendReportScheduledDistributionRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class SendReportScheduledDistributionExample
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
            var body = new SendReportScheduledDistributionRequest(); // SendReportScheduledDistributionRequest |  (optional) 

            try
            {
                SendReportScheduledDistributionResponse result = apiInstance.SendReportScheduledDistribution(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.SendReportScheduledDistribution: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**SendReportScheduledDistributionRequest**](SendReportScheduledDistributionRequest.md)|  | [optional] 

### Return type

[**SendReportScheduledDistributionResponse**](SendReportScheduledDistributionResponse.md)

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

<a name="updatereportscheduleddistribution"></a>
# **UpdateReportScheduledDistribution**
> UpdateReportScheduledDistributionResponse UpdateReportScheduledDistribution (UpdateReportScheduledDistributionRequest body)

Internal use only - Update a scheduled report distribution record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateReportScheduledDistributionExample
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
            var body = new UpdateReportScheduledDistributionRequest(); // UpdateReportScheduledDistributionRequest |  (optional) 

            try
            {
                // Internal use only - Update a scheduled report distribution record.
                UpdateReportScheduledDistributionResponse result = apiInstance.UpdateReportScheduledDistribution(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalysisApi.UpdateReportScheduledDistribution: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateReportScheduledDistributionRequest**](UpdateReportScheduledDistributionRequest.md)|  | [optional] 

### Return type

[**UpdateReportScheduledDistributionResponse**](UpdateReportScheduledDistributionResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


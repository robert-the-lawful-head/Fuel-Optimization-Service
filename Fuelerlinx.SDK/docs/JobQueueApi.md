# IO.Swagger.Api.JobQueueApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteJobQueue**](JobQueueApi.md#deletejobqueue) | **DELETE** /api/JobQueue/job-queue/{id} | Delete job queue by Id
[**DeleteJobQueueFiles**](JobQueueApi.md#deletejobqueuefiles) | **DELETE** /api/JobQueue/job-queue-files/{id} | Delete job queue files by Id
[**DeleteJobQueueResultEvents**](JobQueueApi.md#deletejobqueueresultevents) | **DELETE** /api/JobQueue/job-queue-result-events/{id} | Deletes job queue result events by Id
[**DeleteJobQueueResults**](JobQueueApi.md#deletejobqueueresults) | **DELETE** /api/JobQueue/job-queue-results/{id} | 
[**GetJobQueue**](JobQueueApi.md#getjobqueue) | **GET** /api/JobQueue/job-queue/by-id/{id} | Get job queue by Id
[**GetJobQueueFiles**](JobQueueApi.md#getjobqueuefiles) | **GET** /api/JobQueue/job-queue-files/by-jobQueueId/{jobQueueId} | Get job queue files by jobQueueId
[**GetJobQueueListByDateRange**](JobQueueApi.md#getjobqueuelistbydaterange) | **GET** /api/JobQueue/job-queue/by-date-range | 
[**GetJobQueueResultEvents**](JobQueueApi.md#getjobqueueresultevents) | **GET** /api/JobQueue/job-queue-result-events/by-jobQueueResultId/{jobQueueResultId} | Get job queue result events by jobQueueResultId
[**GetJobQueueResults**](JobQueueApi.md#getjobqueueresults) | **GET** /api/JobQueue/job-queue-results/by-jobQueueId/{jobQueueId} | Get job queue results by jobQueueId
[**PostJobQueue**](JobQueueApi.md#postjobqueue) | **POST** /api/JobQueue/job-queue | Post job queue
[**PostJobQueueFiles**](JobQueueApi.md#postjobqueuefiles) | **POST** /api/JobQueue/job-queue-files | Post job queue files
[**PostJobQueueResultEvents**](JobQueueApi.md#postjobqueueresultevents) | **POST** /api/JobQueue/job-queue-result-events | Post job queue result events
[**PostJobQueueResults**](JobQueueApi.md#postjobqueueresults) | **POST** /api/JobQueue/job-queue-results | Post job queue results
[**RunJobById**](JobQueueApi.md#runjobbyid) | **POST** /api/JobQueue/job-queue/run/{id} | 
[**UpdateJobQueue**](JobQueueApi.md#updatejobqueue) | **PUT** /api/JobQueue/job-queue | Update job queue
[**UpdateJobQueueFiles**](JobQueueApi.md#updatejobqueuefiles) | **PUT** /api/JobQueue/job-queue-files | Update job queue files
[**UpdateJobQueueResultEvents**](JobQueueApi.md#updatejobqueueresultevents) | **PUT** /api/JobQueue/job-queue-result-events | Updates job queue result events
[**UpdateJobQueueResults**](JobQueueApi.md#updatejobqueueresults) | **PUT** /api/JobQueue/job-queue-results | Updates job queue results


<a name="deletejobqueue"></a>
# **DeleteJobQueue**
> DeleteJobQueueResponse DeleteJobQueue (int? id)

Delete job queue by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteJobQueueExample
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

            var apiInstance = new JobQueueApi();
            var id = 56;  // int? | 

            try
            {
                // Delete job queue by Id
                DeleteJobQueueResponse result = apiInstance.DeleteJobQueue(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.DeleteJobQueue: " + e.Message );
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

[**DeleteJobQueueResponse**](DeleteJobQueueResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletejobqueuefiles"></a>
# **DeleteJobQueueFiles**
> DeleteJobQueueFilesResponse DeleteJobQueueFiles (int? id)

Delete job queue files by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteJobQueueFilesExample
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

            var apiInstance = new JobQueueApi();
            var id = 56;  // int? | 

            try
            {
                // Delete job queue files by Id
                DeleteJobQueueFilesResponse result = apiInstance.DeleteJobQueueFiles(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.DeleteJobQueueFiles: " + e.Message );
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

[**DeleteJobQueueFilesResponse**](DeleteJobQueueFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletejobqueueresultevents"></a>
# **DeleteJobQueueResultEvents**
> DeleteJobQueueResultEventsResponse DeleteJobQueueResultEvents (int? id)

Deletes job queue result events by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteJobQueueResultEventsExample
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

            var apiInstance = new JobQueueApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes job queue result events by Id
                DeleteJobQueueResultEventsResponse result = apiInstance.DeleteJobQueueResultEvents(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.DeleteJobQueueResultEvents: " + e.Message );
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

[**DeleteJobQueueResultEventsResponse**](DeleteJobQueueResultEventsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletejobqueueresults"></a>
# **DeleteJobQueueResults**
> DeleteJobQueueResultsResponse DeleteJobQueueResults (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteJobQueueResultsExample
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

            var apiInstance = new JobQueueApi();
            var id = 56;  // int? | 

            try
            {
                DeleteJobQueueResultsResponse result = apiInstance.DeleteJobQueueResults(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.DeleteJobQueueResults: " + e.Message );
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

[**DeleteJobQueueResultsResponse**](DeleteJobQueueResultsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getjobqueue"></a>
# **GetJobQueue**
> JobQueueResponse GetJobQueue (int? id)

Get job queue by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetJobQueueExample
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

            var apiInstance = new JobQueueApi();
            var id = 56;  // int? | 

            try
            {
                // Get job queue by Id
                JobQueueResponse result = apiInstance.GetJobQueue(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.GetJobQueue: " + e.Message );
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

[**JobQueueResponse**](JobQueueResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getjobqueuefiles"></a>
# **GetJobQueueFiles**
> JobQueueFilesResponse GetJobQueueFiles (int? jobQueueId)

Get job queue files by jobQueueId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetJobQueueFilesExample
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

            var apiInstance = new JobQueueApi();
            var jobQueueId = 56;  // int? | 

            try
            {
                // Get job queue files by jobQueueId
                JobQueueFilesResponse result = apiInstance.GetJobQueueFiles(jobQueueId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.GetJobQueueFiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **jobQueueId** | **int?**|  | 

### Return type

[**JobQueueFilesResponse**](JobQueueFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getjobqueuelistbydaterange"></a>
# **GetJobQueueListByDateRange**
> JobQueueListResponse GetJobQueueListByDateRange (DateTime? startDateUtc, DateTime? endDateUtc)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetJobQueueListByDateRangeExample
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

            var apiInstance = new JobQueueApi();
            var startDateUtc = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDateUtc = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                JobQueueListResponse result = apiInstance.GetJobQueueListByDateRange(startDateUtc, endDateUtc);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.GetJobQueueListByDateRange: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **startDateUtc** | **DateTime?**|  | [optional] 
 **endDateUtc** | **DateTime?**|  | [optional] 

### Return type

[**JobQueueListResponse**](JobQueueListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getjobqueueresultevents"></a>
# **GetJobQueueResultEvents**
> JobQueueResultEventsResponse GetJobQueueResultEvents (int? jobQueueResultId)

Get job queue result events by jobQueueResultId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetJobQueueResultEventsExample
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

            var apiInstance = new JobQueueApi();
            var jobQueueResultId = 56;  // int? | 

            try
            {
                // Get job queue result events by jobQueueResultId
                JobQueueResultEventsResponse result = apiInstance.GetJobQueueResultEvents(jobQueueResultId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.GetJobQueueResultEvents: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **jobQueueResultId** | **int?**|  | 

### Return type

[**JobQueueResultEventsResponse**](JobQueueResultEventsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getjobqueueresults"></a>
# **GetJobQueueResults**
> JobQueueResultsResponse GetJobQueueResults (int? jobQueueId)

Get job queue results by jobQueueId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetJobQueueResultsExample
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

            var apiInstance = new JobQueueApi();
            var jobQueueId = 56;  // int? | 

            try
            {
                // Get job queue results by jobQueueId
                JobQueueResultsResponse result = apiInstance.GetJobQueueResults(jobQueueId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.GetJobQueueResults: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **jobQueueId** | **int?**|  | 

### Return type

[**JobQueueResultsResponse**](JobQueueResultsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postjobqueue"></a>
# **PostJobQueue**
> PostJobQueueResponse PostJobQueue (PostJobQueueRequest body)

Post job queue

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostJobQueueExample
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

            var apiInstance = new JobQueueApi();
            var body = new PostJobQueueRequest(); // PostJobQueueRequest |  (optional) 

            try
            {
                // Post job queue
                PostJobQueueResponse result = apiInstance.PostJobQueue(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.PostJobQueue: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostJobQueueRequest**](PostJobQueueRequest.md)|  | [optional] 

### Return type

[**PostJobQueueResponse**](PostJobQueueResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postjobqueuefiles"></a>
# **PostJobQueueFiles**
> PostJobQueueFilesResponse PostJobQueueFiles (PostJobQueueFilesRequest body)

Post job queue files

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostJobQueueFilesExample
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

            var apiInstance = new JobQueueApi();
            var body = new PostJobQueueFilesRequest(); // PostJobQueueFilesRequest |  (optional) 

            try
            {
                // Post job queue files
                PostJobQueueFilesResponse result = apiInstance.PostJobQueueFiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.PostJobQueueFiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostJobQueueFilesRequest**](PostJobQueueFilesRequest.md)|  | [optional] 

### Return type

[**PostJobQueueFilesResponse**](PostJobQueueFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postjobqueueresultevents"></a>
# **PostJobQueueResultEvents**
> PostJobQueueResultEventsResponse PostJobQueueResultEvents (PostJobQueueResultEventsRequest body)

Post job queue result events

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostJobQueueResultEventsExample
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

            var apiInstance = new JobQueueApi();
            var body = new PostJobQueueResultEventsRequest(); // PostJobQueueResultEventsRequest |  (optional) 

            try
            {
                // Post job queue result events
                PostJobQueueResultEventsResponse result = apiInstance.PostJobQueueResultEvents(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.PostJobQueueResultEvents: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostJobQueueResultEventsRequest**](PostJobQueueResultEventsRequest.md)|  | [optional] 

### Return type

[**PostJobQueueResultEventsResponse**](PostJobQueueResultEventsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postjobqueueresults"></a>
# **PostJobQueueResults**
> PostJobQueueResultsResponse PostJobQueueResults (PostJobQueueResultsRequest body)

Post job queue results

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostJobQueueResultsExample
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

            var apiInstance = new JobQueueApi();
            var body = new PostJobQueueResultsRequest(); // PostJobQueueResultsRequest |  (optional) 

            try
            {
                // Post job queue results
                PostJobQueueResultsResponse result = apiInstance.PostJobQueueResults(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.PostJobQueueResults: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostJobQueueResultsRequest**](PostJobQueueResultsRequest.md)|  | [optional] 

### Return type

[**PostJobQueueResultsResponse**](PostJobQueueResultsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="runjobbyid"></a>
# **RunJobById**
> RunJobQueueResponse RunJobById (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class RunJobByIdExample
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

            var apiInstance = new JobQueueApi();
            var id = 56;  // int? | 

            try
            {
                RunJobQueueResponse result = apiInstance.RunJobById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.RunJobById: " + e.Message );
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

[**RunJobQueueResponse**](RunJobQueueResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatejobqueue"></a>
# **UpdateJobQueue**
> UpdateJobQueueResponse UpdateJobQueue (UpdateJobQueueRequest body)

Update job queue

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateJobQueueExample
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

            var apiInstance = new JobQueueApi();
            var body = new UpdateJobQueueRequest(); // UpdateJobQueueRequest |  (optional) 

            try
            {
                // Update job queue
                UpdateJobQueueResponse result = apiInstance.UpdateJobQueue(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.UpdateJobQueue: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateJobQueueRequest**](UpdateJobQueueRequest.md)|  | [optional] 

### Return type

[**UpdateJobQueueResponse**](UpdateJobQueueResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatejobqueuefiles"></a>
# **UpdateJobQueueFiles**
> UpdateJobQueueFilesResponse UpdateJobQueueFiles (UpdateJobQueueFilesRequest body)

Update job queue files

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateJobQueueFilesExample
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

            var apiInstance = new JobQueueApi();
            var body = new UpdateJobQueueFilesRequest(); // UpdateJobQueueFilesRequest |  (optional) 

            try
            {
                // Update job queue files
                UpdateJobQueueFilesResponse result = apiInstance.UpdateJobQueueFiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.UpdateJobQueueFiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateJobQueueFilesRequest**](UpdateJobQueueFilesRequest.md)|  | [optional] 

### Return type

[**UpdateJobQueueFilesResponse**](UpdateJobQueueFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatejobqueueresultevents"></a>
# **UpdateJobQueueResultEvents**
> UpdateJobQueueResultEventsResponse UpdateJobQueueResultEvents (UpdateJobQueueResultEventsRequest body)

Updates job queue result events

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateJobQueueResultEventsExample
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

            var apiInstance = new JobQueueApi();
            var body = new UpdateJobQueueResultEventsRequest(); // UpdateJobQueueResultEventsRequest |  (optional) 

            try
            {
                // Updates job queue result events
                UpdateJobQueueResultEventsResponse result = apiInstance.UpdateJobQueueResultEvents(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.UpdateJobQueueResultEvents: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateJobQueueResultEventsRequest**](UpdateJobQueueResultEventsRequest.md)|  | [optional] 

### Return type

[**UpdateJobQueueResultEventsResponse**](UpdateJobQueueResultEventsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatejobqueueresults"></a>
# **UpdateJobQueueResults**
> UpdateJobQueueResultsResponse UpdateJobQueueResults (UpdateJobQueueResultsRequest body)

Updates job queue results

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateJobQueueResultsExample
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

            var apiInstance = new JobQueueApi();
            var body = new UpdateJobQueueResultsRequest(); // UpdateJobQueueResultsRequest |  (optional) 

            try
            {
                // Updates job queue results
                UpdateJobQueueResultsResponse result = apiInstance.UpdateJobQueueResults(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling JobQueueApi.UpdateJobQueueResults: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateJobQueueResultsRequest**](UpdateJobQueueResultsRequest.md)|  | [optional] 

### Return type

[**UpdateJobQueueResultsResponse**](UpdateJobQueueResultsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


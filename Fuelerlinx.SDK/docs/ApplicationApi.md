# IO.Swagger.Api.ApplicationApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteDeploymentNotes**](ApplicationApi.md#deletedeploymentnotes) | **DELETE** /api/Application/deployment-notes/{id} | Internal use only - Delete the specific deployment notes.
[**GetBuildVersion**](ApplicationApi.md#getbuildversion) | **GET** /api/Application/build-version | Returns the current build version of the API.
[**GetDeploymentNotesAll**](ApplicationApi.md#getdeploymentnotesall) | **GET** /api/Application/deployment-notes/list | Internal use only - Returns a list of deployment notes to be used with a range of build versions on each application.
[**GetDeploymentNotesByVersionNumber**](ApplicationApi.md#getdeploymentnotesbyversionnumber) | **GET** /api/Application/deployment-notes/by-version-number/{buildVersionNumber} | Internal use only - Fetch deployment notes for a provided {buildVersionNumber}.
[**PostDeploymentNotes**](ApplicationApi.md#postdeploymentnotes) | **POST** /api/Application/deployment-notes | Internal use only - Add a new deployment note for the provided build version range.
[**UpdateDeploymentNotes**](ApplicationApi.md#updatedeploymentnotes) | **PUT** /api/Application/deployment-notes | Internal use only - Update an existing deployment note for a build version.


<a name="deletedeploymentnotes"></a>
# **DeleteDeploymentNotes**
> DeleteDeploymentNotesResponse DeleteDeploymentNotes (int? id)

Internal use only - Delete the specific deployment notes.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteDeploymentNotesExample
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

            var apiInstance = new ApplicationApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete the specific deployment notes.
                DeleteDeploymentNotesResponse result = apiInstance.DeleteDeploymentNotes(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ApplicationApi.DeleteDeploymentNotes: " + e.Message );
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

[**DeleteDeploymentNotesResponse**](DeleteDeploymentNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getbuildversion"></a>
# **GetBuildVersion**
> BuildVersionResponse GetBuildVersion ()

Returns the current build version of the API.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetBuildVersionExample
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

            var apiInstance = new ApplicationApi();

            try
            {
                // Returns the current build version of the API.
                BuildVersionResponse result = apiInstance.GetBuildVersion();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ApplicationApi.GetBuildVersion: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**BuildVersionResponse**](BuildVersionResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getdeploymentnotesall"></a>
# **GetDeploymentNotesAll**
> DeploymentNotesListResponse GetDeploymentNotesAll ()

Internal use only - Returns a list of deployment notes to be used with a range of build versions on each application.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDeploymentNotesAllExample
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

            var apiInstance = new ApplicationApi();

            try
            {
                // Internal use only - Returns a list of deployment notes to be used with a range of build versions on each application.
                DeploymentNotesListResponse result = apiInstance.GetDeploymentNotesAll();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ApplicationApi.GetDeploymentNotesAll: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**DeploymentNotesListResponse**](DeploymentNotesListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getdeploymentnotesbyversionnumber"></a>
# **GetDeploymentNotesByVersionNumber**
> DeploymentNotesListResponse GetDeploymentNotesByVersionNumber (int? buildVersionNumber)

Internal use only - Fetch deployment notes for a provided {buildVersionNumber}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDeploymentNotesByVersionNumberExample
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

            var apiInstance = new ApplicationApi();
            var buildVersionNumber = 56;  // int? | 

            try
            {
                // Internal use only - Fetch deployment notes for a provided {buildVersionNumber}.
                DeploymentNotesListResponse result = apiInstance.GetDeploymentNotesByVersionNumber(buildVersionNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ApplicationApi.GetDeploymentNotesByVersionNumber: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **buildVersionNumber** | **int?**|  | 

### Return type

[**DeploymentNotesListResponse**](DeploymentNotesListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postdeploymentnotes"></a>
# **PostDeploymentNotes**
> PostDeploymentNotesResponse PostDeploymentNotes (PostDeploymentNotesRequest body)

Internal use only - Add a new deployment note for the provided build version range.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostDeploymentNotesExample
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

            var apiInstance = new ApplicationApi();
            var body = new PostDeploymentNotesRequest(); // PostDeploymentNotesRequest |  (optional) 

            try
            {
                // Internal use only - Add a new deployment note for the provided build version range.
                PostDeploymentNotesResponse result = apiInstance.PostDeploymentNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ApplicationApi.PostDeploymentNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostDeploymentNotesRequest**](PostDeploymentNotesRequest.md)|  | [optional] 

### Return type

[**PostDeploymentNotesResponse**](PostDeploymentNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatedeploymentnotes"></a>
# **UpdateDeploymentNotes**
> UpdateDeploymentNotesResponse UpdateDeploymentNotes (UpdateDeploymentNotesRequest body)

Internal use only - Update an existing deployment note for a build version.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateDeploymentNotesExample
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

            var apiInstance = new ApplicationApi();
            var body = new UpdateDeploymentNotesRequest(); // UpdateDeploymentNotesRequest |  (optional) 

            try
            {
                // Internal use only - Update an existing deployment note for a build version.
                UpdateDeploymentNotesResponse result = apiInstance.UpdateDeploymentNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ApplicationApi.UpdateDeploymentNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateDeploymentNotesRequest**](UpdateDeploymentNotesRequest.md)|  | [optional] 

### Return type

[**UpdateDeploymentNotesResponse**](UpdateDeploymentNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


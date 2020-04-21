# IO.Swagger.Api.FBOApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteFboAlias**](FBOApi.md#deletefboalias) | **DELETE** /api/FBO/fboAlias/{id} | 
[**DeleteFboDetailsByCompany**](FBOApi.md#deletefbodetailsbycompany) | **DELETE** /api/FBO/company-specific-details/{fboDetailsByCompanyId} | Delete company-specific details for a particular FBO.
[**DeleteFboDetailsByCompanyNotes**](FBOApi.md#deletefbodetailsbycompanynotes) | **DELETE** /api/FBO/company-specific-details/{fboDetailsByCompanyId}/notes/{noteId} | Delete a company-specific note for a particular FBO.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking.
[**GetFBOLinxFboByAcukwikId**](FBOApi.md#getfbolinxfbobyacukwikid) | **GET** /api/FBO/fbolinx/fbo-by-acukwik-id/{acukwikId} | 
[**GetFboAlias**](FBOApi.md#getfboalias) | **GET** /api/FBO/fboAlias/by-airport/{icao} | 
[**GetFboDetailsByCompany**](FBOApi.md#getfbodetailsbycompany) | **GET** /api/FBO/company-specific-details/{fboDetailsByCompanyId} | Fetch company-specific details for a particular FBO based on the provided [fboDetailsByCompanyId].
[**GetFboDetailsByCompanyByLocation**](FBOApi.md#getfbodetailsbycompanybylocation) | **GET** /api/FBO/company-specific-details/by-location/{icao}/{fboName} | Fetch company-specific details for a particular FBO based on the provided [icao] and [fboName].
[**GetFboDetailsByCompanyNotes**](FBOApi.md#getfbodetailsbycompanynotes) | **GET** /api/FBO/company-specific-details/{fboDetailsByCompanyId}/notes | Fetch company-specific notes for a particular FBO based on the provided [fboDetailsByCompanyId].
[**PostFboAliasAsync**](FBOApi.md#postfboaliasasync) | **POST** /api/FBO/fboAlias | 
[**PostFboDetailsByCompany**](FBOApi.md#postfbodetailsbycompany) | **POST** /api/FBO/company-specific-details | Add company-specific details for a particular FBO.  If a record already exists for this FBO, the previous record will be replaced.
[**PostFboDetailsByCompanyNotes**](FBOApi.md#postfbodetailsbycompanynotes) | **POST** /api/FBO/company-specific-details/notes | Add company-specific notes for a particular FBO.  The note must be associated with a company-specific FBO record.
[**UpdateFboAlias**](FBOApi.md#updatefboalias) | **PUT** /api/FBO/fboAlias | 
[**UpdateFboDetailsByCompany**](FBOApi.md#updatefbodetailsbycompany) | **PUT** /api/FBO/company-specific-details | Update company-specific details for a particular FBO.
[**UpdateFboDetailsByCompanyNotes**](FBOApi.md#updatefbodetailsbycompanynotes) | **PUT** /api/FBO/company-specific-details/notes | Update a company-specific note for a particular FBO.


<a name="deletefboalias"></a>
# **DeleteFboAlias**
> DeleteFboAliasResponse DeleteFboAlias (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteFboAliasExample
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

            var apiInstance = new FBOApi();
            var id = 56;  // int? | 

            try
            {
                DeleteFboAliasResponse result = apiInstance.DeleteFboAlias(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.DeleteFboAlias: " + e.Message );
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

[**DeleteFboAliasResponse**](DeleteFboAliasResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletefbodetailsbycompany"></a>
# **DeleteFboDetailsByCompany**
> DeleteFboDetailsByCompanyResponse DeleteFboDetailsByCompany (int? fboDetailsByCompanyId)

Delete company-specific details for a particular FBO.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteFboDetailsByCompanyExample
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

            var apiInstance = new FBOApi();
            var fboDetailsByCompanyId = 56;  // int? | 

            try
            {
                // Delete company-specific details for a particular FBO.
                DeleteFboDetailsByCompanyResponse result = apiInstance.DeleteFboDetailsByCompany(fboDetailsByCompanyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.DeleteFboDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fboDetailsByCompanyId** | **int?**|  | 

### Return type

[**DeleteFboDetailsByCompanyResponse**](DeleteFboDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletefbodetailsbycompanynotes"></a>
# **DeleteFboDetailsByCompanyNotes**
> DeleteFboDetailsByCompanyNotesResponse DeleteFboDetailsByCompanyNotes (int? fboDetailsByCompanyId, int? noteId)

Delete a company-specific note for a particular FBO.  The note will be changed to a \"deleted\" state but will not be removed from the database to allow for change-tracking.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteFboDetailsByCompanyNotesExample
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

            var apiInstance = new FBOApi();
            var fboDetailsByCompanyId = 56;  // int? | 
            var noteId = 56;  // int? | 

            try
            {
                // Delete a company-specific note for a particular FBO.  The note will be changed to a \"deleted\" state but will not be removed from the database to allow for change-tracking.
                DeleteFboDetailsByCompanyNotesResponse result = apiInstance.DeleteFboDetailsByCompanyNotes(fboDetailsByCompanyId, noteId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.DeleteFboDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fboDetailsByCompanyId** | **int?**|  | 
 **noteId** | **int?**|  | 

### Return type

[**DeleteFboDetailsByCompanyNotesResponse**](DeleteFboDetailsByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfbolinxfbobyacukwikid"></a>
# **GetFBOLinxFboByAcukwikId**
> FBOLinxFBOResponse GetFBOLinxFboByAcukwikId (int? acukwikId)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFBOLinxFboByAcukwikIdExample
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

            var apiInstance = new FBOApi();
            var acukwikId = 56;  // int? | 

            try
            {
                FBOLinxFBOResponse result = apiInstance.GetFBOLinxFboByAcukwikId(acukwikId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.GetFBOLinxFboByAcukwikId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **acukwikId** | **int?**|  | 

### Return type

[**FBOLinxFBOResponse**](FBOLinxFBOResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfboalias"></a>
# **GetFboAlias**
> FboAliasResponse GetFboAlias (string icao)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFboAliasExample
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

            var apiInstance = new FBOApi();
            var icao = icao_example;  // string | 

            try
            {
                FboAliasResponse result = apiInstance.GetFboAlias(icao);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.GetFboAlias: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **icao** | **string**|  | 

### Return type

[**FboAliasResponse**](FboAliasResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfbodetailsbycompany"></a>
# **GetFboDetailsByCompany**
> FboDetailsByCompanyResponse GetFboDetailsByCompany (int? fboDetailsByCompanyId)

Fetch company-specific details for a particular FBO based on the provided [fboDetailsByCompanyId].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFboDetailsByCompanyExample
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

            var apiInstance = new FBOApi();
            var fboDetailsByCompanyId = 56;  // int? | 

            try
            {
                // Fetch company-specific details for a particular FBO based on the provided [fboDetailsByCompanyId].
                FboDetailsByCompanyResponse result = apiInstance.GetFboDetailsByCompany(fboDetailsByCompanyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.GetFboDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fboDetailsByCompanyId** | **int?**|  | 

### Return type

[**FboDetailsByCompanyResponse**](FboDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfbodetailsbycompanybylocation"></a>
# **GetFboDetailsByCompanyByLocation**
> FboDetailsByCompanyResponse GetFboDetailsByCompanyByLocation (string icao, string fboName)

Fetch company-specific details for a particular FBO based on the provided [icao] and [fboName].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFboDetailsByCompanyByLocationExample
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

            var apiInstance = new FBOApi();
            var icao = icao_example;  // string | 
            var fboName = fboName_example;  // string | 

            try
            {
                // Fetch company-specific details for a particular FBO based on the provided [icao] and [fboName].
                FboDetailsByCompanyResponse result = apiInstance.GetFboDetailsByCompanyByLocation(icao, fboName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.GetFboDetailsByCompanyByLocation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **icao** | **string**|  | 
 **fboName** | **string**|  | 

### Return type

[**FboDetailsByCompanyResponse**](FboDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfbodetailsbycompanynotes"></a>
# **GetFboDetailsByCompanyNotes**
> FboDetailsByCompanyNotesResponse GetFboDetailsByCompanyNotes (int? fboDetailsByCompanyId)

Fetch company-specific notes for a particular FBO based on the provided [fboDetailsByCompanyId].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFboDetailsByCompanyNotesExample
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

            var apiInstance = new FBOApi();
            var fboDetailsByCompanyId = 56;  // int? | 

            try
            {
                // Fetch company-specific notes for a particular FBO based on the provided [fboDetailsByCompanyId].
                FboDetailsByCompanyNotesResponse result = apiInstance.GetFboDetailsByCompanyNotes(fboDetailsByCompanyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.GetFboDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fboDetailsByCompanyId** | **int?**|  | 

### Return type

[**FboDetailsByCompanyNotesResponse**](FboDetailsByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postfboaliasasync"></a>
# **PostFboAliasAsync**
> PostFboAliasResponse PostFboAliasAsync (PostFboAliasRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostFboAliasAsyncExample
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

            var apiInstance = new FBOApi();
            var body = new PostFboAliasRequest(); // PostFboAliasRequest |  (optional) 

            try
            {
                PostFboAliasResponse result = apiInstance.PostFboAliasAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.PostFboAliasAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostFboAliasRequest**](PostFboAliasRequest.md)|  | [optional] 

### Return type

[**PostFboAliasResponse**](PostFboAliasResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postfbodetailsbycompany"></a>
# **PostFboDetailsByCompany**
> PostFboDetailsByCompanyResponse PostFboDetailsByCompany (PostFboDetailsByCompanyRequest body)

Add company-specific details for a particular FBO.  If a record already exists for this FBO, the previous record will be replaced.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostFboDetailsByCompanyExample
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

            var apiInstance = new FBOApi();
            var body = new PostFboDetailsByCompanyRequest(); // PostFboDetailsByCompanyRequest |  (optional) 

            try
            {
                // Add company-specific details for a particular FBO.  If a record already exists for this FBO, the previous record will be replaced.
                PostFboDetailsByCompanyResponse result = apiInstance.PostFboDetailsByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.PostFboDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostFboDetailsByCompanyRequest**](PostFboDetailsByCompanyRequest.md)|  | [optional] 

### Return type

[**PostFboDetailsByCompanyResponse**](PostFboDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postfbodetailsbycompanynotes"></a>
# **PostFboDetailsByCompanyNotes**
> PostFboDetailsByCompanyNotesResponse PostFboDetailsByCompanyNotes (PostFboDetailsByCompanyNotesRequest body)

Add company-specific notes for a particular FBO.  The note must be associated with a company-specific FBO record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostFboDetailsByCompanyNotesExample
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

            var apiInstance = new FBOApi();
            var body = new PostFboDetailsByCompanyNotesRequest(); // PostFboDetailsByCompanyNotesRequest |  (optional) 

            try
            {
                // Add company-specific notes for a particular FBO.  The note must be associated with a company-specific FBO record.
                PostFboDetailsByCompanyNotesResponse result = apiInstance.PostFboDetailsByCompanyNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.PostFboDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostFboDetailsByCompanyNotesRequest**](PostFboDetailsByCompanyNotesRequest.md)|  | [optional] 

### Return type

[**PostFboDetailsByCompanyNotesResponse**](PostFboDetailsByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatefboalias"></a>
# **UpdateFboAlias**
> UpdateFboAliasResponse UpdateFboAlias (UpdateFboAliasRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFboAliasExample
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

            var apiInstance = new FBOApi();
            var body = new UpdateFboAliasRequest(); // UpdateFboAliasRequest |  (optional) 

            try
            {
                UpdateFboAliasResponse result = apiInstance.UpdateFboAlias(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.UpdateFboAlias: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateFboAliasRequest**](UpdateFboAliasRequest.md)|  | [optional] 

### Return type

[**UpdateFboAliasResponse**](UpdateFboAliasResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatefbodetailsbycompany"></a>
# **UpdateFboDetailsByCompany**
> UpdateFboDetailsByCompanyResponse UpdateFboDetailsByCompany (UpdateFboDetailsByCompanyRequest body)

Update company-specific details for a particular FBO.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFboDetailsByCompanyExample
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

            var apiInstance = new FBOApi();
            var body = new UpdateFboDetailsByCompanyRequest(); // UpdateFboDetailsByCompanyRequest |  (optional) 

            try
            {
                // Update company-specific details for a particular FBO.
                UpdateFboDetailsByCompanyResponse result = apiInstance.UpdateFboDetailsByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.UpdateFboDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateFboDetailsByCompanyRequest**](UpdateFboDetailsByCompanyRequest.md)|  | [optional] 

### Return type

[**UpdateFboDetailsByCompanyResponse**](UpdateFboDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatefbodetailsbycompanynotes"></a>
# **UpdateFboDetailsByCompanyNotes**
> UpdateFboDetailsByCompanyNotesResponse UpdateFboDetailsByCompanyNotes (UpdateFboDetailsByCompanyNotesRequest body)

Update a company-specific note for a particular FBO.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFboDetailsByCompanyNotesExample
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

            var apiInstance = new FBOApi();
            var body = new UpdateFboDetailsByCompanyNotesRequest(); // UpdateFboDetailsByCompanyNotesRequest |  (optional) 

            try
            {
                // Update a company-specific note for a particular FBO.
                UpdateFboDetailsByCompanyNotesResponse result = apiInstance.UpdateFboDetailsByCompanyNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOApi.UpdateFboDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateFboDetailsByCompanyNotesRequest**](UpdateFboDetailsByCompanyNotesRequest.md)|  | [optional] 

### Return type

[**UpdateFboDetailsByCompanyNotesResponse**](UpdateFboDetailsByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


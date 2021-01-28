# IO.Swagger.Api.FlightTypeApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteFlightTypeMapping**](FlightTypeApi.md#deleteflighttypemapping) | **DELETE** /api/FlightType/mapping/{id} | Internal use only - Delete a flight type mapping record by it&#39;s [id].
[**GetFlightTypeMappingByName**](FlightTypeApi.md#getflighttypemappingbyname) | **GET** /api/FlightType/mapping/by-flight-name/{flightTypeName} | Fetch a flight type mapping by it&#39;s name.  This is used to identify if a flight type is private or commercial based on various custom names provided by scheduling systems.
[**GetFlightTypeMappingList**](FlightTypeApi.md#getflighttypemappinglist) | **GET** /api/FlightType/list/mapping | Internal use only - Fetch all flight type mappings.
[**PostFlightTypeMapping**](FlightTypeApi.md#postflighttypemapping) | **POST** /api/FlightType/mapping | Internal use only - Add a flight type mapping record.
[**UpdateFlightTypeMapping**](FlightTypeApi.md#updateflighttypemapping) | **PUT** /api/FlightType/mapping | Internal use only - Update a flight type mapping record.


<a name="deleteflighttypemapping"></a>
# **DeleteFlightTypeMapping**
> DeleteFlightTypeMappingResponse DeleteFlightTypeMapping (int? id)

Internal use only - Delete a flight type mapping record by it's [id].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteFlightTypeMappingExample
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

            var apiInstance = new FlightTypeApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete a flight type mapping record by it's [id].
                DeleteFlightTypeMappingResponse result = apiInstance.DeleteFlightTypeMapping(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightTypeApi.DeleteFlightTypeMapping: " + e.Message );
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

[**DeleteFlightTypeMappingResponse**](DeleteFlightTypeMappingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getflighttypemappingbyname"></a>
# **GetFlightTypeMappingByName**
> FlightTypeMappingResponse GetFlightTypeMappingByName (string flightTypeName)

Fetch a flight type mapping by it's name.  This is used to identify if a flight type is private or commercial based on various custom names provided by scheduling systems.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFlightTypeMappingByNameExample
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

            var apiInstance = new FlightTypeApi();
            var flightTypeName = flightTypeName_example;  // string | 

            try
            {
                // Fetch a flight type mapping by it's name.  This is used to identify if a flight type is private or commercial based on various custom names provided by scheduling systems.
                FlightTypeMappingResponse result = apiInstance.GetFlightTypeMappingByName(flightTypeName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightTypeApi.GetFlightTypeMappingByName: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **flightTypeName** | **string**|  | 

### Return type

[**FlightTypeMappingResponse**](FlightTypeMappingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getflighttypemappinglist"></a>
# **GetFlightTypeMappingList**
> FlightTypeMappingListResponse GetFlightTypeMappingList ()

Internal use only - Fetch all flight type mappings.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFlightTypeMappingListExample
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

            var apiInstance = new FlightTypeApi();

            try
            {
                // Internal use only - Fetch all flight type mappings.
                FlightTypeMappingListResponse result = apiInstance.GetFlightTypeMappingList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightTypeApi.GetFlightTypeMappingList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**FlightTypeMappingListResponse**](FlightTypeMappingListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postflighttypemapping"></a>
# **PostFlightTypeMapping**
> PostFlightTypeMappingResponse PostFlightTypeMapping (PostFlightTypeMappingRequest body)

Internal use only - Add a flight type mapping record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostFlightTypeMappingExample
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

            var apiInstance = new FlightTypeApi();
            var body = new PostFlightTypeMappingRequest(); // PostFlightTypeMappingRequest |  (optional) 

            try
            {
                // Internal use only - Add a flight type mapping record.
                PostFlightTypeMappingResponse result = apiInstance.PostFlightTypeMapping(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightTypeApi.PostFlightTypeMapping: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostFlightTypeMappingRequest**](PostFlightTypeMappingRequest.md)|  | [optional] 

### Return type

[**PostFlightTypeMappingResponse**](PostFlightTypeMappingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateflighttypemapping"></a>
# **UpdateFlightTypeMapping**
> UpdateFlightTypeMappingResponse UpdateFlightTypeMapping (UpdateFlightTypeMappingRequest body)

Internal use only - Update a flight type mapping record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFlightTypeMappingExample
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

            var apiInstance = new FlightTypeApi();
            var body = new UpdateFlightTypeMappingRequest(); // UpdateFlightTypeMappingRequest |  (optional) 

            try
            {
                // Internal use only - Update a flight type mapping record.
                UpdateFlightTypeMappingResponse result = apiInstance.UpdateFlightTypeMapping(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightTypeApi.UpdateFlightTypeMapping: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateFlightTypeMappingRequest**](UpdateFlightTypeMappingRequest.md)|  | [optional] 

### Return type

[**UpdateFlightTypeMappingResponse**](UpdateFlightTypeMappingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


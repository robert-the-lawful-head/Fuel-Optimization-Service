# IO.Swagger.Api.DispatchingApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**CancelFuelOrder**](DispatchingApi.md#cancelfuelorder) | **POST** /api/Dispatching/cancel-fuel-order | Cancel a previously dispatched fuel order by it&#39;s transaction id.
[**OrderFuel**](DispatchingApi.md#orderfuel) | **POST** /api/Dispatching/orderfuel | Places a fuel order based on the requested details.  This will notify the fuel vendor and any additional integrations the customer has enabled.


<a name="cancelfuelorder"></a>
# **CancelFuelOrder**
> CancelFuelOrdersResponse CancelFuelOrder (CancelFuelOrderRequest body)

Cancel a previously dispatched fuel order by it's transaction id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CancelFuelOrderExample
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

            var apiInstance = new DispatchingApi();
            var body = new CancelFuelOrderRequest(); // CancelFuelOrderRequest |  (optional) 

            try
            {
                // Cancel a previously dispatched fuel order by it's transaction id.
                CancelFuelOrdersResponse result = apiInstance.CancelFuelOrder(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DispatchingApi.CancelFuelOrder: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**CancelFuelOrderRequest**](CancelFuelOrderRequest.md)|  | [optional] 

### Return type

[**CancelFuelOrdersResponse**](CancelFuelOrdersResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="orderfuel"></a>
# **OrderFuel**
> DispatchFuelResponse OrderFuel (DispatchFuelRequest body)

Places a fuel order based on the requested details.  This will notify the fuel vendor and any additional integrations the customer has enabled.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class OrderFuelExample
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

            var apiInstance = new DispatchingApi();
            var body = new DispatchFuelRequest(); // DispatchFuelRequest |  (optional) 

            try
            {
                // Places a fuel order based on the requested details.  This will notify the fuel vendor and any additional integrations the customer has enabled.
                DispatchFuelResponse result = apiInstance.OrderFuel(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DispatchingApi.OrderFuel: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**DispatchFuelRequest**](DispatchFuelRequest.md)|  | [optional] 

### Return type

[**DispatchFuelResponse**](DispatchFuelResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


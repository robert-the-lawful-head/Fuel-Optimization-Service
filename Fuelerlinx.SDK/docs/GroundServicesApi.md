# IO.Swagger.Api.GroundServicesApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetUserInfoForFlightBridge**](GroundServicesApi.md#getuserinfoforflightbridge) | **GET** /api/GroundServices/flightbridge/user-info/from-token/{userAuthenticationToken} | 


<a name="getuserinfoforflightbridge"></a>
# **GetUserInfoForFlightBridge**
> FlightBridgeAuthorizationCheckResponse GetUserInfoForFlightBridge (string userAuthenticationToken)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetUserInfoForFlightBridgeExample
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

            var apiInstance = new GroundServicesApi();
            var userAuthenticationToken = userAuthenticationToken_example;  // string | 

            try
            {
                FlightBridgeAuthorizationCheckResponse result = apiInstance.GetUserInfoForFlightBridge(userAuthenticationToken);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling GroundServicesApi.GetUserInfoForFlightBridge: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userAuthenticationToken** | **string**|  | 

### Return type

[**FlightBridgeAuthorizationCheckResponse**](FlightBridgeAuthorizationCheckResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


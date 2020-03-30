# IO.Swagger.Api.FBOLinxApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetTransactionsCountForNearbyAirports**](FBOLinxApi.md#gettransactionscountfornearbyairports) | **POST** /api/FBOLinx/get-nearby-airports | 


<a name="gettransactionscountfornearbyairports"></a>
# **GetTransactionsCountForNearbyAirports**
> FBOLinxNearbyAirportsResponse GetTransactionsCountForNearbyAirports (FBOLinxNearbyAirportsRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionsCountForNearbyAirportsExample
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

            var apiInstance = new FBOLinxApi();
            var body = new FBOLinxNearbyAirportsRequest(); // FBOLinxNearbyAirportsRequest |  (optional) 

            try
            {
                FBOLinxNearbyAirportsResponse result = apiInstance.GetTransactionsCountForNearbyAirports(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetTransactionsCountForNearbyAirports: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxNearbyAirportsRequest**](FBOLinxNearbyAirportsRequest.md)|  | [optional] 

### Return type

[**FBOLinxNearbyAirportsResponse**](FBOLinxNearbyAirportsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


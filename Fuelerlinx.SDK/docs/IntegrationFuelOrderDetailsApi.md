# IO.Swagger.Api.IntegrationFuelOrderDetailsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AddIntegrationFuelOrderDetails**](IntegrationFuelOrderDetailsApi.md#addintegrationfuelorderdetails) | **POST** /api/IntegrationFuelOrderDetails | Internal use only - associates a fuel order transaction with an integration record.
[**CancelIntegrationFuelOrderDetails**](IntegrationFuelOrderDetailsApi.md#cancelintegrationfuelorderdetails) | **POST** /api/IntegrationFuelOrderDetails/cancel | Internal use only - cancel an integration record from being tied to a transaction and notify the partner.
[**UpdateIntegrationFuelOrderDetails**](IntegrationFuelOrderDetailsApi.md#updateintegrationfuelorderdetails) | **PUT** /api/IntegrationFuelOrderDetails | Internal use only - update an integration record associated with a fuel order transaction.


<a name="addintegrationfuelorderdetails"></a>
# **AddIntegrationFuelOrderDetails**
> IntegrationFuelOrderDetailsResponse AddIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body)

Internal use only - associates a fuel order transaction with an integration record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddIntegrationFuelOrderDetailsExample
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

            var apiInstance = new IntegrationFuelOrderDetailsApi();
            var body = new IntegrationFuelOrderDetailsDTO(); // IntegrationFuelOrderDetailsDTO |  (optional) 

            try
            {
                // Internal use only - associates a fuel order transaction with an integration record.
                IntegrationFuelOrderDetailsResponse result = apiInstance.AddIntegrationFuelOrderDetails(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationFuelOrderDetailsApi.AddIntegrationFuelOrderDetails: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**IntegrationFuelOrderDetailsDTO**](IntegrationFuelOrderDetailsDTO.md)|  | [optional] 

### Return type

[**IntegrationFuelOrderDetailsResponse**](IntegrationFuelOrderDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="cancelintegrationfuelorderdetails"></a>
# **CancelIntegrationFuelOrderDetails**
> DeleteIntegrationFuelOrderDetailsResponse CancelIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body)

Internal use only - cancel an integration record from being tied to a transaction and notify the partner.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class CancelIntegrationFuelOrderDetailsExample
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

            var apiInstance = new IntegrationFuelOrderDetailsApi();
            var body = new IntegrationFuelOrderDetailsDTO(); // IntegrationFuelOrderDetailsDTO |  (optional) 

            try
            {
                // Internal use only - cancel an integration record from being tied to a transaction and notify the partner.
                DeleteIntegrationFuelOrderDetailsResponse result = apiInstance.CancelIntegrationFuelOrderDetails(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationFuelOrderDetailsApi.CancelIntegrationFuelOrderDetails: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**IntegrationFuelOrderDetailsDTO**](IntegrationFuelOrderDetailsDTO.md)|  | [optional] 

### Return type

[**DeleteIntegrationFuelOrderDetailsResponse**](DeleteIntegrationFuelOrderDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateintegrationfuelorderdetails"></a>
# **UpdateIntegrationFuelOrderDetails**
> IntegrationFuelOrderDetailsResponse UpdateIntegrationFuelOrderDetails (IntegrationFuelOrderDetailsDTO body)

Internal use only - update an integration record associated with a fuel order transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateIntegrationFuelOrderDetailsExample
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

            var apiInstance = new IntegrationFuelOrderDetailsApi();
            var body = new IntegrationFuelOrderDetailsDTO(); // IntegrationFuelOrderDetailsDTO |  (optional) 

            try
            {
                // Internal use only - update an integration record associated with a fuel order transaction.
                IntegrationFuelOrderDetailsResponse result = apiInstance.UpdateIntegrationFuelOrderDetails(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationFuelOrderDetailsApi.UpdateIntegrationFuelOrderDetails: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**IntegrationFuelOrderDetailsDTO**](IntegrationFuelOrderDetailsDTO.md)|  | [optional] 

### Return type

[**IntegrationFuelOrderDetailsResponse**](IntegrationFuelOrderDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


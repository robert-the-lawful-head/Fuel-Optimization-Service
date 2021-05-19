# IO.Swagger.Api.CurrencyApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetCurrencyList**](CurrencyApi.md#getcurrencylist) | **GET** /api/Currency/list | Fetch a list of currencies with their current rate-to-dollar conversion value.
[**SyncLatestExchangeRates**](CurrencyApi.md#synclatestexchangerates) | **POST** /api/Currency/exchange-rates/sync-latest | Internal use only - Sync the latest exchange rates with the European Central Bank


<a name="getcurrencylist"></a>
# **GetCurrencyList**
> CurrencyListResponse GetCurrencyList ()

Fetch a list of currencies with their current rate-to-dollar conversion value.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCurrencyListExample
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

            var apiInstance = new CurrencyApi();

            try
            {
                // Fetch a list of currencies with their current rate-to-dollar conversion value.
                CurrencyListResponse result = apiInstance.GetCurrencyList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling CurrencyApi.GetCurrencyList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**CurrencyListResponse**](CurrencyListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="synclatestexchangerates"></a>
# **SyncLatestExchangeRates**
> SyncLatestExchangeRatesResponse SyncLatestExchangeRates ()

Internal use only - Sync the latest exchange rates with the European Central Bank

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class SyncLatestExchangeRatesExample
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

            var apiInstance = new CurrencyApi();

            try
            {
                // Internal use only - Sync the latest exchange rates with the European Central Bank
                SyncLatestExchangeRatesResponse result = apiInstance.SyncLatestExchangeRates();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling CurrencyApi.SyncLatestExchangeRates: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**SyncLatestExchangeRatesResponse**](SyncLatestExchangeRatesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


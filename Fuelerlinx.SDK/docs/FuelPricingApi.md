# IO.Swagger.Api.FuelPricingApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCurrentPricingForCompany**](FuelPricingApi.md#deletecurrentpricingforcompany) | **DELETE** /api/FuelPricing/current | Internal use only - delete all cached pricing for a currently authenticated company.
[**DeleteWeeklyPricingForFuelVendor**](FuelPricingApi.md#deleteweeklypricingforfuelvendor) | **DELETE** /api/FuelPricing/weekly-pricing/by-fueler/{fuelVendorId} | Internal use only - delete all weekly pricing records for a particular fuel vendor.
[**GetAssociatedDetailsForFuelOption**](FuelPricingApi.md#getassociateddetailsforfueloption) | **POST** /api/FuelPricing/associated-details | 
[**GetCurrentPricingForLocation**](FuelPricingApi.md#getcurrentpricingforlocation) | **GET** /api/FuelPricing/current/{commaDelimitedIcaos} | Internal use only - Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user.
[**GetCurrentPricingForLocationAndFlightType**](FuelPricingApi.md#getcurrentpricingforlocationandflighttype) | **GET** /api/FuelPricing/current/{commaDelimitedIcaos}/flight-type/{flightType} | Internal use only - Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user.
[**GetLiveQuoteForLocations**](FuelPricingApi.md#getlivequoteforlocations) | **GET** /api/FuelPricing/live-quote/{commaDelimitedIcaos} | Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using their default flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.
[**GetLiveQuoteForLocationsAndFlightType**](FuelPricingApi.md#getlivequoteforlocationsandflighttype) | **GET** /api/FuelPricing/live-quote/{commaDelimitedIcaos}/flight-type/{flightType} | Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.
[**GetLiveQuoteForLocationsAndFlightTypeAndVendor**](FuelPricingApi.md#getlivequoteforlocationsandflighttypeandvendor) | **GET** /api/FuelPricing/live-quote/{commaDelimitedIcaos}/flight-type/{flightType}/fuel-vendor/{fuelVendorId} | Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.  Only quotes the specified fuel vendor based on the provided {fuelVendorId}.
[**GetQuotFromEpic**](FuelPricingApi.md#getquotfromepic) | **GET** /api/FuelPricing/quoting/epic/{commaDelimitedAirportIdentifiers} | Internal use only - Fetch a quote response from the EPIC Aviation web service.
[**GetWeeklyPricingForFuelerAndLocation**](FuelPricingApi.md#getweeklypricingforfuelerandlocation) | **GET** /api/FuelPricing/weekly-pricing/by-fueler/{fuelVendorId}/by-locations/{commaDelimitedIcaos} | If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos] and [fuelVendorId].  These are to be used as a fallback option when a vendor&#39;s service is unavailable.
[**GetWeeklyPricingForLocation**](FuelPricingApi.md#getweeklypricingforlocation) | **GET** /api/FuelPricing/weekly-pricing/by-locations/{commaDelimitedIcaos} | /// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos].  These are to be used as a fallback option when a vendor&#39;s service is unavailable.
[**PostFuelOrder**](FuelPricingApi.md#postfuelorder) | **POST** /api/FuelPricing/fuel-order | Internal use only - Please use the \&quot;dispatching\&quot; API to dispatch a full fuel order.  This API method is strictly for notifying the fuel vendor.


<a name="deletecurrentpricingforcompany"></a>
# **DeleteCurrentPricingForCompany**
> DeleteCurrentPricingResponse DeleteCurrentPricingForCompany ()

Internal use only - delete all cached pricing for a currently authenticated company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCurrentPricingForCompanyExample
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

            var apiInstance = new FuelPricingApi();

            try
            {
                // Internal use only - delete all cached pricing for a currently authenticated company.
                DeleteCurrentPricingResponse result = apiInstance.DeleteCurrentPricingForCompany();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.DeleteCurrentPricingForCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**DeleteCurrentPricingResponse**](DeleteCurrentPricingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteweeklypricingforfuelvendor"></a>
# **DeleteWeeklyPricingForFuelVendor**
> DeleteWeeklyPricingResponse DeleteWeeklyPricingForFuelVendor (int? fuelVendorId)

Internal use only - delete all weekly pricing records for a particular fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteWeeklyPricingForFuelVendorExample
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

            var apiInstance = new FuelPricingApi();
            var fuelVendorId = 56;  // int? | 

            try
            {
                // Internal use only - delete all weekly pricing records for a particular fuel vendor.
                DeleteWeeklyPricingResponse result = apiInstance.DeleteWeeklyPricingForFuelVendor(fuelVendorId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.DeleteWeeklyPricingForFuelVendor: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fuelVendorId** | **int?**|  | 

### Return type

[**DeleteWeeklyPricingResponse**](DeleteWeeklyPricingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getassociateddetailsforfueloption"></a>
# **GetAssociatedDetailsForFuelOption**
> AssociatedDetailsResponse GetAssociatedDetailsForFuelOption (AssociatedDetailsRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAssociatedDetailsForFuelOptionExample
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

            var apiInstance = new FuelPricingApi();
            var body = new AssociatedDetailsRequest(); // AssociatedDetailsRequest |  (optional) 

            try
            {
                AssociatedDetailsResponse result = apiInstance.GetAssociatedDetailsForFuelOption(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetAssociatedDetailsForFuelOption: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**AssociatedDetailsRequest**](AssociatedDetailsRequest.md)|  | [optional] 

### Return type

[**AssociatedDetailsResponse**](AssociatedDetailsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcurrentpricingforlocation"></a>
# **GetCurrentPricingForLocation**
> CurrentPricingResponse GetCurrentPricingForLocation (string commaDelimitedIcaos)

Internal use only - Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCurrentPricingForLocationExample
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

            var apiInstance = new FuelPricingApi();
            var commaDelimitedIcaos = commaDelimitedIcaos_example;  // string | 

            try
            {
                // Internal use only - Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user.
                CurrentPricingResponse result = apiInstance.GetCurrentPricingForLocation(commaDelimitedIcaos);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetCurrentPricingForLocation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **commaDelimitedIcaos** | **string**|  | 

### Return type

[**CurrentPricingResponse**](CurrentPricingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcurrentpricingforlocationandflighttype"></a>
# **GetCurrentPricingForLocationAndFlightType**
> CurrentPricingResponse GetCurrentPricingForLocationAndFlightType (string commaDelimitedIcaos, string flightType)

Internal use only - Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCurrentPricingForLocationAndFlightTypeExample
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

            var apiInstance = new FuelPricingApi();
            var commaDelimitedIcaos = commaDelimitedIcaos_example;  // string | 
            var flightType = flightType_example;  // string | 

            try
            {
                // Internal use only - Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user.
                CurrentPricingResponse result = apiInstance.GetCurrentPricingForLocationAndFlightType(commaDelimitedIcaos, flightType);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetCurrentPricingForLocationAndFlightType: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **commaDelimitedIcaos** | **string**|  | 
 **flightType** | **string**|  | 

### Return type

[**CurrentPricingResponse**](CurrentPricingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getlivequoteforlocations"></a>
# **GetLiveQuoteForLocations**
> CurrentPricingResponse GetLiveQuoteForLocations (string commaDelimitedIcaos)

Retrieves a live quote from all vendor web services tied to the flight department's account using their default flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.

It is always recommended to do a live quote for pricing if one hasn't been done in the last few hours for the desired airports.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetLiveQuoteForLocationsExample
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

            var apiInstance = new FuelPricingApi();
            var commaDelimitedIcaos = commaDelimitedIcaos_example;  // string | 

            try
            {
                // Retrieves a live quote from all vendor web services tied to the flight department's account using their default flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.
                CurrentPricingResponse result = apiInstance.GetLiveQuoteForLocations(commaDelimitedIcaos);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetLiveQuoteForLocations: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **commaDelimitedIcaos** | **string**|  | 

### Return type

[**CurrentPricingResponse**](CurrentPricingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getlivequoteforlocationsandflighttype"></a>
# **GetLiveQuoteForLocationsAndFlightType**
> CurrentPricingResponse GetLiveQuoteForLocationsAndFlightType (string commaDelimitedIcaos, string flightType)

Retrieves a live quote from all vendor web services tied to the flight department's account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.

It is always recommended to do a live quote for pricing if one hasn't been done in the last few hours for the desired airports.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetLiveQuoteForLocationsAndFlightTypeExample
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

            var apiInstance = new FuelPricingApi();
            var commaDelimitedIcaos = commaDelimitedIcaos_example;  // string | 
            var flightType = flightType_example;  // string | 

            try
            {
                // Retrieves a live quote from all vendor web services tied to the flight department's account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.
                CurrentPricingResponse result = apiInstance.GetLiveQuoteForLocationsAndFlightType(commaDelimitedIcaos, flightType);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetLiveQuoteForLocationsAndFlightType: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **commaDelimitedIcaos** | **string**|  | 
 **flightType** | **string**|  | 

### Return type

[**CurrentPricingResponse**](CurrentPricingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getlivequoteforlocationsandflighttypeandvendor"></a>
# **GetLiveQuoteForLocationsAndFlightTypeAndVendor**
> CurrentPricingResponse GetLiveQuoteForLocationsAndFlightTypeAndVendor (string commaDelimitedIcaos, string flightType, int? fuelVendorId)

Retrieves a live quote from all vendor web services tied to the flight department's account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.  Only quotes the specified fuel vendor based on the provided {fuelVendorId}.

It is always recommended to do a live quote for pricing if one hasn't been done in the last few hours for the desired airports.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetLiveQuoteForLocationsAndFlightTypeAndVendorExample
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

            var apiInstance = new FuelPricingApi();
            var commaDelimitedIcaos = commaDelimitedIcaos_example;  // string | 
            var flightType = flightType_example;  // string | 
            var fuelVendorId = 56;  // int? | 

            try
            {
                // Retrieves a live quote from all vendor web services tied to the flight department's account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.  Only quotes the specified fuel vendor based on the provided {fuelVendorId}.
                CurrentPricingResponse result = apiInstance.GetLiveQuoteForLocationsAndFlightTypeAndVendor(commaDelimitedIcaos, flightType, fuelVendorId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetLiveQuoteForLocationsAndFlightTypeAndVendor: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **commaDelimitedIcaos** | **string**|  | 
 **flightType** | **string**|  | 
 **fuelVendorId** | **int?**|  | 

### Return type

[**CurrentPricingResponse**](CurrentPricingResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getquotfromepic"></a>
# **GetQuotFromEpic**
> EpicQuoteResponse GetQuotFromEpic (string commaDelimitedAirportIdentifiers)

Internal use only - Fetch a quote response from the EPIC Aviation web service.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetQuotFromEpicExample
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

            var apiInstance = new FuelPricingApi();
            var commaDelimitedAirportIdentifiers = commaDelimitedAirportIdentifiers_example;  // string | 

            try
            {
                // Internal use only - Fetch a quote response from the EPIC Aviation web service.
                EpicQuoteResponse result = apiInstance.GetQuotFromEpic(commaDelimitedAirportIdentifiers);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetQuotFromEpic: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **commaDelimitedAirportIdentifiers** | **string**|  | 

### Return type

[**EpicQuoteResponse**](EpicQuoteResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getweeklypricingforfuelerandlocation"></a>
# **GetWeeklyPricingForFuelerAndLocation**
> WeeklyPricingListResponse GetWeeklyPricingForFuelerAndLocation (int? fuelVendorId, string commaDelimitedIcaos)

If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos] and [fuelVendorId].  These are to be used as a fallback option when a vendor's service is unavailable.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetWeeklyPricingForFuelerAndLocationExample
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

            var apiInstance = new FuelPricingApi();
            var fuelVendorId = 56;  // int? | 
            var commaDelimitedIcaos = commaDelimitedIcaos_example;  // string | 

            try
            {
                // If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos] and [fuelVendorId].  These are to be used as a fallback option when a vendor's service is unavailable.
                WeeklyPricingListResponse result = apiInstance.GetWeeklyPricingForFuelerAndLocation(fuelVendorId, commaDelimitedIcaos);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetWeeklyPricingForFuelerAndLocation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **fuelVendorId** | **int?**|  | 
 **commaDelimitedIcaos** | **string**|  | 

### Return type

[**WeeklyPricingListResponse**](WeeklyPricingListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getweeklypricingforlocation"></a>
# **GetWeeklyPricingForLocation**
> WeeklyPricingListResponse GetWeeklyPricingForLocation (string commaDelimitedIcaos)

/// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos].  These are to be used as a fallback option when a vendor's service is unavailable.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetWeeklyPricingForLocationExample
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

            var apiInstance = new FuelPricingApi();
            var commaDelimitedIcaos = commaDelimitedIcaos_example;  // string | 

            try
            {
                // /// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos].  These are to be used as a fallback option when a vendor's service is unavailable.
                WeeklyPricingListResponse result = apiInstance.GetWeeklyPricingForLocation(commaDelimitedIcaos);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetWeeklyPricingForLocation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **commaDelimitedIcaos** | **string**|  | 

### Return type

[**WeeklyPricingListResponse**](WeeklyPricingListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postfuelorder"></a>
# **PostFuelOrder**
> PostFuelOrderResponse PostFuelOrder (PostFuelOrderRequest body)

Internal use only - Please use the \"dispatching\" API to dispatch a full fuel order.  This API method is strictly for notifying the fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostFuelOrderExample
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

            var apiInstance = new FuelPricingApi();
            var body = new PostFuelOrderRequest(); // PostFuelOrderRequest |  (optional) 

            try
            {
                // Internal use only - Please use the \"dispatching\" API to dispatch a full fuel order.  This API method is strictly for notifying the fuel vendor.
                PostFuelOrderResponse result = apiInstance.PostFuelOrder(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.PostFuelOrder: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostFuelOrderRequest**](PostFuelOrderRequest.md)|  | [optional] 

### Return type

[**PostFuelOrderResponse**](PostFuelOrderResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


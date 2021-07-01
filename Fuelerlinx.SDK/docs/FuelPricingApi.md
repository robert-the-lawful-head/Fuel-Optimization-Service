# IO.Swagger.Api.FuelPricingApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCurrentPricingForCompany**](FuelPricingApi.md#deletecurrentpricingforcompany) | **DELETE** /api/FuelPricing/current | Internal use only - delete all cached pricing for a currently authenticated company.
[**DeleteSupportedPriceSheetFileTests**](FuelPricingApi.md#deletesupportedpricesheetfiletests) | **DELETE** /api/FuelPricing/supported-price-sheet-file-tests/{id} | Delete Supported Price Sheet File Tests by Id
[**DeleteSupportedPriceSheetFiles**](FuelPricingApi.md#deletesupportedpricesheetfiles) | **DELETE** /api/FuelPricing/supported-price-sheet-files/{id} | Deletes Supported Price Sheet Files by Id
[**DeleteWeeklyPricingForFuelVendor**](FuelPricingApi.md#deleteweeklypricingforfuelvendor) | **DELETE** /api/FuelPricing/weekly-pricing/by-fueler/{fuelVendorId} | Internal use only - delete all weekly pricing records for a particular fuel vendor.
[**GetAssociatedDetailsForFuelOption**](FuelPricingApi.md#getassociateddetailsforfueloption) | **POST** /api/FuelPricing/associated-details | 
[**GetCurrentPricingForLocation**](FuelPricingApi.md#getcurrentpricingforlocation) | **GET** /api/FuelPricing/current/{commaDelimitedIcaos} | Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user.
[**GetCurrentPricingForLocationAndFlightType**](FuelPricingApi.md#getcurrentpricingforlocationandflighttype) | **GET** /api/FuelPricing/current/{commaDelimitedIcaos}/flight-type/{flightType} | Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user.
[**GetLiveQuoteForLocations**](FuelPricingApi.md#getlivequoteforlocations) | **GET** /api/FuelPricing/live-quote/{commaDelimitedIcaos} | Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using their default flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.
[**GetLiveQuoteForLocationsAndFlightType**](FuelPricingApi.md#getlivequoteforlocationsandflighttype) | **GET** /api/FuelPricing/live-quote/{commaDelimitedIcaos}/flight-type/{flightType} | Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.
[**GetLiveQuoteForLocationsAndFlightTypeAndVendor**](FuelPricingApi.md#getlivequoteforlocationsandflighttypeandvendor) | **GET** /api/FuelPricing/live-quote/{commaDelimitedIcaos}/flight-type/{flightType}/fuel-vendor/{fuelVendorId} | Retrieves a live quote from all vendor web services tied to the flight department&#39;s account using the specified flight type.  This method can take up to 60 seconds to complete based on the number of airports, fuel vendor web services, and account settings.  Only quotes the specified fuel vendor based on the provided {fuelVendorId}.
[**GetQuotFromEpic**](FuelPricingApi.md#getquotfromepic) | **GET** /api/FuelPricing/quoting/epic/{commaDelimitedAirportIdentifiers} | Internal use only - Fetch a quote response from the EPIC Aviation web service.
[**GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId**](FuelPricingApi.md#getsupportedpricesheetfiletestsbysupportedpricesheetfileid) | **GET** /api/FuelPricing/supported-price-sheet-file-tests/by-supportedInvoiceImportFileId/{supportedPriceSheetFileId} | Gets Supported Price Sheet File Tests by supportedPriceSheetFileId
[**GetSupportedPriceSheetFilesByByFuelVendorId**](FuelPricingApi.md#getsupportedpricesheetfilesbybyfuelvendorid) | **GET** /api/FuelPricing/supported-price-sheet-files/by-fuel-vendor/{fuelVendorId} | Gets Supported Price Sheet Files by FuelVendorId
[**GetSupportedPriceSheetFilesByById**](FuelPricingApi.md#getsupportedpricesheetfilesbybyid) | **GET** /api/FuelPricing/supported-price-sheet-files/{id} | Get Supported Price Sheet Files by Id
[**GetWeeklyPricingForFuelerAndLocation**](FuelPricingApi.md#getweeklypricingforfuelerandlocation) | **GET** /api/FuelPricing/weekly-pricing/by-fueler/{fuelVendorId}/by-locations/{commaDelimitedIcaos} | If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos] and [fuelVendorId].  These are to be used as a fallback option when a vendor&#39;s service is unavailable.
[**GetWeeklyPricingForLocation**](FuelPricingApi.md#getweeklypricingforlocation) | **GET** /api/FuelPricing/weekly-pricing/by-locations/{commaDelimitedIcaos} | /// If available, will fetch records from the weekly price sheet received each week for the specified [commaDelimitedIcaos].  These are to be used as a fallback option when a vendor&#39;s service is unavailable.
[**PostFuelOrder**](FuelPricingApi.md#postfuelorder) | **POST** /api/FuelPricing/fuel-order | Internal use only - Please use the \&quot;dispatching\&quot; API to dispatch a full fuel order.  This API method is strictly for notifying the fuel vendor.
[**PostSupportedPriceSheetFileTests**](FuelPricingApi.md#postsupportedpricesheetfiletests) | **POST** /api/FuelPricing/supported-price-sheet-file-tests | Post Supported Price Sheet File Tests
[**PostSupportedPriceSheetFiles**](FuelPricingApi.md#postsupportedpricesheetfiles) | **POST** /api/FuelPricing/supported-price-sheet-files | Post Supported Price Sheet Files
[**UpdateSupportedPriceSheetFileTests**](FuelPricingApi.md#updatesupportedpricesheetfiletests) | **PUT** /api/FuelPricing/supported-price-sheet-file-tests | Updates Supported Price Sheet File Tests
[**UpdateSupportedPriceSheetFiles**](FuelPricingApi.md#updatesupportedpricesheetfiles) | **PUT** /api/FuelPricing/supported-price-sheet-files | Updates Supported Price Sheet Files
[**VerifyVendorServiceCredentials**](FuelPricingApi.md#verifyvendorservicecredentials) | **GET** /api/FuelPricing/verify-vendor-service-credentials/{vendorId} | Internal use only - verify credentials for a vendor service.


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

<a name="deletesupportedpricesheetfiletests"></a>
# **DeleteSupportedPriceSheetFileTests**
> DeleteSupportedPriceSheetFileTestsResponse DeleteSupportedPriceSheetFileTests (int? id)

Delete Supported Price Sheet File Tests by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSupportedPriceSheetFileTestsExample
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
            var id = 56;  // int? | 

            try
            {
                // Delete Supported Price Sheet File Tests by Id
                DeleteSupportedPriceSheetFileTestsResponse result = apiInstance.DeleteSupportedPriceSheetFileTests(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.DeleteSupportedPriceSheetFileTests: " + e.Message );
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

[**DeleteSupportedPriceSheetFileTestsResponse**](DeleteSupportedPriceSheetFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletesupportedpricesheetfiles"></a>
# **DeleteSupportedPriceSheetFiles**
> DeleteSupportedPriceSheetFilesResponse DeleteSupportedPriceSheetFiles (int? id)

Deletes Supported Price Sheet Files by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSupportedPriceSheetFilesExample
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
            var id = 56;  // int? | 

            try
            {
                // Deletes Supported Price Sheet Files by Id
                DeleteSupportedPriceSheetFilesResponse result = apiInstance.DeleteSupportedPriceSheetFiles(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.DeleteSupportedPriceSheetFiles: " + e.Message );
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

[**DeleteSupportedPriceSheetFilesResponse**](DeleteSupportedPriceSheetFilesResponse.md)

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

Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user.

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
                // Fetch all cached pricing for the specified comma-delimited ICAOs currently available for the user.
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

Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user.

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
                // Fetch all cached pricing for the specified comma-delimited ICAOs and flight type currently available for the user.
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

<a name="getsupportedpricesheetfiletestsbysupportedpricesheetfileid"></a>
# **GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId**
> SupportedPriceSheetFileTestsResponse GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId (int? supportedPriceSheetFileId)

Gets Supported Price Sheet File Tests by supportedPriceSheetFileId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileIdExample
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
            var supportedPriceSheetFileId = 56;  // int? | 

            try
            {
                // Gets Supported Price Sheet File Tests by supportedPriceSheetFileId
                SupportedPriceSheetFileTestsResponse result = apiInstance.GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId(supportedPriceSheetFileId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetSupportedPriceSheetFileTestsBySupportedPriceSheetFileId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **supportedPriceSheetFileId** | **int?**|  | 

### Return type

[**SupportedPriceSheetFileTestsResponse**](SupportedPriceSheetFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedpricesheetfilesbybyfuelvendorid"></a>
# **GetSupportedPriceSheetFilesByByFuelVendorId**
> SupportedPriceSheetFileListResponse GetSupportedPriceSheetFilesByByFuelVendorId (int? fuelVendorId)

Gets Supported Price Sheet Files by FuelVendorId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedPriceSheetFilesByByFuelVendorIdExample
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
                // Gets Supported Price Sheet Files by FuelVendorId
                SupportedPriceSheetFileListResponse result = apiInstance.GetSupportedPriceSheetFilesByByFuelVendorId(fuelVendorId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetSupportedPriceSheetFilesByByFuelVendorId: " + e.Message );
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

[**SupportedPriceSheetFileListResponse**](SupportedPriceSheetFileListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsupportedpricesheetfilesbybyid"></a>
# **GetSupportedPriceSheetFilesByById**
> SupportedPriceSheetFilesResponse GetSupportedPriceSheetFilesByById (int? id)

Get Supported Price Sheet Files by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSupportedPriceSheetFilesByByIdExample
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
            var id = 56;  // int? | 

            try
            {
                // Get Supported Price Sheet Files by Id
                SupportedPriceSheetFilesResponse result = apiInstance.GetSupportedPriceSheetFilesByById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.GetSupportedPriceSheetFilesByById: " + e.Message );
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

[**SupportedPriceSheetFilesResponse**](SupportedPriceSheetFilesResponse.md)

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

<a name="postsupportedpricesheetfiletests"></a>
# **PostSupportedPriceSheetFileTests**
> PostSupportedPriceSheetFileTestsResponse PostSupportedPriceSheetFileTests (PostSupportedPriceSheetFileTestsRequest body)

Post Supported Price Sheet File Tests

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSupportedPriceSheetFileTestsExample
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
            var body = new PostSupportedPriceSheetFileTestsRequest(); // PostSupportedPriceSheetFileTestsRequest |  (optional) 

            try
            {
                // Post Supported Price Sheet File Tests
                PostSupportedPriceSheetFileTestsResponse result = apiInstance.PostSupportedPriceSheetFileTests(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.PostSupportedPriceSheetFileTests: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSupportedPriceSheetFileTestsRequest**](PostSupportedPriceSheetFileTestsRequest.md)|  | [optional] 

### Return type

[**PostSupportedPriceSheetFileTestsResponse**](PostSupportedPriceSheetFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsupportedpricesheetfiles"></a>
# **PostSupportedPriceSheetFiles**
> PostSupportedPriceSheetFilesResponse PostSupportedPriceSheetFiles (PostSupportedPriceSheetFilesRequest body)

Post Supported Price Sheet Files

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSupportedPriceSheetFilesExample
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
            var body = new PostSupportedPriceSheetFilesRequest(); // PostSupportedPriceSheetFilesRequest |  (optional) 

            try
            {
                // Post Supported Price Sheet Files
                PostSupportedPriceSheetFilesResponse result = apiInstance.PostSupportedPriceSheetFiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.PostSupportedPriceSheetFiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSupportedPriceSheetFilesRequest**](PostSupportedPriceSheetFilesRequest.md)|  | [optional] 

### Return type

[**PostSupportedPriceSheetFilesResponse**](PostSupportedPriceSheetFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesupportedpricesheetfiletests"></a>
# **UpdateSupportedPriceSheetFileTests**
> UpdateSupportedPriceSheetFileTestsResponse UpdateSupportedPriceSheetFileTests (UpdateSupportedPriceSheetFileTestsRequest body)

Updates Supported Price Sheet File Tests

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSupportedPriceSheetFileTestsExample
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
            var body = new UpdateSupportedPriceSheetFileTestsRequest(); // UpdateSupportedPriceSheetFileTestsRequest |  (optional) 

            try
            {
                // Updates Supported Price Sheet File Tests
                UpdateSupportedPriceSheetFileTestsResponse result = apiInstance.UpdateSupportedPriceSheetFileTests(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.UpdateSupportedPriceSheetFileTests: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSupportedPriceSheetFileTestsRequest**](UpdateSupportedPriceSheetFileTestsRequest.md)|  | [optional] 

### Return type

[**UpdateSupportedPriceSheetFileTestsResponse**](UpdateSupportedPriceSheetFileTestsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesupportedpricesheetfiles"></a>
# **UpdateSupportedPriceSheetFiles**
> UpdateSupportedPriceSheetFilesResponse UpdateSupportedPriceSheetFiles (UpdateSupportedPriceSheetFilesRequest body)

Updates Supported Price Sheet Files

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSupportedPriceSheetFilesExample
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
            var body = new UpdateSupportedPriceSheetFilesRequest(); // UpdateSupportedPriceSheetFilesRequest |  (optional) 

            try
            {
                // Updates Supported Price Sheet Files
                UpdateSupportedPriceSheetFilesResponse result = apiInstance.UpdateSupportedPriceSheetFiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.UpdateSupportedPriceSheetFiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSupportedPriceSheetFilesRequest**](UpdateSupportedPriceSheetFilesRequest.md)|  | [optional] 

### Return type

[**UpdateSupportedPriceSheetFilesResponse**](UpdateSupportedPriceSheetFilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="verifyvendorservicecredentials"></a>
# **VerifyVendorServiceCredentials**
> bool? VerifyVendorServiceCredentials (int? vendorId)

Internal use only - verify credentials for a vendor service.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class VerifyVendorServiceCredentialsExample
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
            var vendorId = 56;  // int? | 

            try
            {
                // Internal use only - verify credentials for a vendor service.
                bool? result = apiInstance.VerifyVendorServiceCredentials(vendorId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelPricingApi.VerifyVendorServiceCredentials: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **vendorId** | **int?**|  | 

### Return type

**bool?**

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


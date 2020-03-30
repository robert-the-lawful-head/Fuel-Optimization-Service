# IO.Swagger.Api.ScheduledTripApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteScheduledLegData**](ScheduledTripApi.md#deletescheduledlegdata) | **DELETE** /api/ScheduledTrip/integration/leg/{legIdentifier} | Delete a previously POSTed leg with a matching [legIdentifier].
[**GetCurrentScheduledTrips**](ScheduledTripApi.md#getcurrentscheduledtrips) | **GET** /api/ScheduledTrip/current | Fetch upcoming scheduled trip info pulled from the user&#39;s scheduling system.
[**GetFuelOrderDetailsForScheduledLeg**](ScheduledTripApi.md#getfuelorderdetailsforscheduledleg) | **GET** /api/ScheduledTrip/integration/fuelorderdetails/{legIdentifier} | Fetch the [transaction] and [generatedFuelComment] associated with the fuel order that was placed for the specified leg tied to the [legIdentifier].
[**PostScheduledLegData**](ScheduledTripApi.md#postscheduledlegdata) | **POST** /api/ScheduledTrip/integration/leg | Post a leg from the user&#39;s scheduling system as an object [ScheduledLegData] and it&#39;s corresponding [LegIdentifier].  The scheduling integration partner controls the format of the [ScheduledLegData] and the [LegIdentifier] should be a unique identifier used on the partner&#39;s side.


<a name="deletescheduledlegdata"></a>
# **DeleteScheduledLegData**
> ScheduledTripDeleteResponse DeleteScheduledLegData (string legIdentifier)

Delete a previously POSTed leg with a matching [legIdentifier].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteScheduledLegDataExample
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

            var apiInstance = new ScheduledTripApi();
            var legIdentifier = legIdentifier_example;  // string | 

            try
            {
                // Delete a previously POSTed leg with a matching [legIdentifier].
                ScheduledTripDeleteResponse result = apiInstance.DeleteScheduledLegData(legIdentifier);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.DeleteScheduledLegData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **legIdentifier** | **string**|  | 

### Return type

[**ScheduledTripDeleteResponse**](ScheduledTripDeleteResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcurrentscheduledtrips"></a>
# **GetCurrentScheduledTrips**
> CurrentScheduledTripsResponse GetCurrentScheduledTrips ()

Fetch upcoming scheduled trip info pulled from the user's scheduling system.

Only records that are scheduled to depart after the current time will be returned.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCurrentScheduledTripsExample
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

            var apiInstance = new ScheduledTripApi();

            try
            {
                // Fetch upcoming scheduled trip info pulled from the user's scheduling system.
                CurrentScheduledTripsResponse result = apiInstance.GetCurrentScheduledTrips();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.GetCurrentScheduledTrips: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**CurrentScheduledTripsResponse**](CurrentScheduledTripsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfuelorderdetailsforscheduledleg"></a>
# **GetFuelOrderDetailsForScheduledLeg**
> FuelOrderDetailsForScheduledLegResponse GetFuelOrderDetailsForScheduledLeg (string legIdentifier)

Fetch the [transaction] and [generatedFuelComment] associated with the fuel order that was placed for the specified leg tied to the [legIdentifier].

The [legIdentifier] should be the unique identifier used by the scheduling integration partner to identify the leg within their system.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFuelOrderDetailsForScheduledLegExample
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

            var apiInstance = new ScheduledTripApi();
            var legIdentifier = legIdentifier_example;  // string | 

            try
            {
                // Fetch the [transaction] and [generatedFuelComment] associated with the fuel order that was placed for the specified leg tied to the [legIdentifier].
                FuelOrderDetailsForScheduledLegResponse result = apiInstance.GetFuelOrderDetailsForScheduledLeg(legIdentifier);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.GetFuelOrderDetailsForScheduledLeg: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **legIdentifier** | **string**|  | 

### Return type

[**FuelOrderDetailsForScheduledLegResponse**](FuelOrderDetailsForScheduledLegResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postscheduledlegdata"></a>
# **PostScheduledLegData**
> PostScheduledLegFromIntegrationResponse PostScheduledLegData (PostScheduledLegFromIntegrationRequest body)

Post a leg from the user's scheduling system as an object [ScheduledLegData] and it's corresponding [LegIdentifier].  The scheduling integration partner controls the format of the [ScheduledLegData] and the [LegIdentifier] should be a unique identifier used on the partner's side.

It is recommended to include the tail number, departure airport, arrival airport, and date/time of the departure/arrival as a minimum when sending information.  Additional information (i.e. pax count, cargo, altitude, fuel on board, etc.) is recommended to help enhance the integration.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostScheduledLegDataExample
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

            var apiInstance = new ScheduledTripApi();
            var body = new PostScheduledLegFromIntegrationRequest(); // PostScheduledLegFromIntegrationRequest |  (optional) 

            try
            {
                // Post a leg from the user's scheduling system as an object [ScheduledLegData] and it's corresponding [LegIdentifier].  The scheduling integration partner controls the format of the [ScheduledLegData] and the [LegIdentifier] should be a unique identifier used on the partner's side.
                PostScheduledLegFromIntegrationResponse result = apiInstance.PostScheduledLegData(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.PostScheduledLegData: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostScheduledLegFromIntegrationRequest**](PostScheduledLegFromIntegrationRequest.md)|  | [optional] 

### Return type

[**PostScheduledLegFromIntegrationResponse**](PostScheduledLegFromIntegrationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


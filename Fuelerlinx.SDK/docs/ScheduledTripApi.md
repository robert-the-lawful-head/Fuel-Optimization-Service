# IO.Swagger.Api.ScheduledTripApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteScheduledLegData**](ScheduledTripApi.md#deletescheduledlegdata) | **DELETE** /api/ScheduledTrip/integration/leg/{legIdentifier} | Delete a previously POSTed leg with a matching [legIdentifier].
[**DeleteScheduledTripSettings**](ScheduledTripApi.md#deletescheduledtripsettings) | **DELETE** /api/ScheduledTrip/settings/{id} | 
[**DeleteSchedulingNoteFailures**](ScheduledTripApi.md#deleteschedulingnotefailures) | **DELETE** /api/ScheduledTrip/scheduling-note-failures/{id} | Delete scheduling note failures
[**DeleteTransactionSchedulingNotePlacement**](ScheduledTripApi.md#deletetransactionschedulingnoteplacement) | **DELETE** /api/ScheduledTrip/scheduling-note-placement/{id} | Delete the record containing the scheduling trip/leg where the notes were inserted at the time of order for a transaction.
[**GetCurrentScheduledTrips**](ScheduledTripApi.md#getcurrentscheduledtrips) | **GET** /api/ScheduledTrip/current | Fetch upcoming scheduled trip info pulled from the user&#39;s scheduling system.
[**GetFuelOrderDetailsForScheduledLeg**](ScheduledTripApi.md#getfuelorderdetailsforscheduledleg) | **GET** /api/ScheduledTrip/integration/fuelorderdetails/{legIdentifier} | Fetch the [transaction] and [generatedFuelComment] associated with the fuel order that was placed for the specified leg tied to the [legIdentifier].
[**GetScheduledTripSettings**](ScheduledTripApi.md#getscheduledtripsettings) | **GET** /api/ScheduledTrip/settings | 
[**GetScheduledTripsByDateRange**](ScheduledTripApi.md#getscheduledtripsbydaterange) | **GET** /api/ScheduledTrip/by-date-range | Fetch scheduled trip info pulled from the user&#39;s scheduling system by date range.
[**GetSchedulingNoteFailures**](ScheduledTripApi.md#getschedulingnotefailures) | **GET** /api/ScheduledTrip/scheduling-note-failures/{transactionId}/{userId} | Get scheduling note failures by transactionId and userId
[**GetTransactionSchedulingNotePlacement**](ScheduledTripApi.md#gettransactionschedulingnoteplacement) | **GET** /api/ScheduledTrip/scheduling-note-placement/{transactionId}/list | Fetch the scheduling trip/leg where notes were inserted at the time of order for a transaction.
[**PostScheduledLegData**](ScheduledTripApi.md#postscheduledlegdata) | **POST** /api/ScheduledTrip/integration/leg | Post a leg from the user&#39;s scheduling system as an object [ScheduledLegData] and it&#39;s corresponding [LegIdentifier].  The scheduling integration partner controls the format of the [ScheduledLegData] and the [LegIdentifier] should be a unique identifier used on the partner&#39;s side.
[**PostScheduledTripSettings**](ScheduledTripApi.md#postscheduledtripsettings) | **POST** /api/ScheduledTrip/settings | 
[**PostSchedulingNoteFailures**](ScheduledTripApi.md#postschedulingnotefailures) | **POST** /api/ScheduledTrip/scheduling-note-failures | Post scheduling note failures
[**PostTransactionSchedulingNotePlacement**](ScheduledTripApi.md#posttransactionschedulingnoteplacement) | **POST** /api/ScheduledTrip/scheduling-note-placement | Add the scheduling trip/leg where the notes were inserted at the time of order for a transaction.
[**UpdateScheduledTripSettings**](ScheduledTripApi.md#updatescheduledtripsettings) | **PUT** /api/ScheduledTrip/settings | 
[**UpdateSchedulingNoteFailures**](ScheduledTripApi.md#updateschedulingnotefailures) | **PUT** /api/ScheduledTrip/scheduling-note-failures | Update scheduling note failures
[**UpdateTransactionSchedulingNotePlacement**](ScheduledTripApi.md#updatetransactionschedulingnoteplacement) | **PUT** /api/ScheduledTrip/scheduling-note-placement | Update the scheduling trip/leg where the notes were inserted at the time of order for a transaction.


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

<a name="deletescheduledtripsettings"></a>
# **DeleteScheduledTripSettings**
> DeleteScheduledTripSettingsResponse DeleteScheduledTripSettings (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteScheduledTripSettingsExample
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
            var id = 56;  // int? | 

            try
            {
                DeleteScheduledTripSettingsResponse result = apiInstance.DeleteScheduledTripSettings(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.DeleteScheduledTripSettings: " + e.Message );
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

[**DeleteScheduledTripSettingsResponse**](DeleteScheduledTripSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteschedulingnotefailures"></a>
# **DeleteSchedulingNoteFailures**
> DeleteSchedulingNoteFailuresResponse DeleteSchedulingNoteFailures (int? id)

Delete scheduling note failures

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSchedulingNoteFailuresExample
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
            var id = 56;  // int? | 

            try
            {
                // Delete scheduling note failures
                DeleteSchedulingNoteFailuresResponse result = apiInstance.DeleteSchedulingNoteFailures(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.DeleteSchedulingNoteFailures: " + e.Message );
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

[**DeleteSchedulingNoteFailuresResponse**](DeleteSchedulingNoteFailuresResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletetransactionschedulingnoteplacement"></a>
# **DeleteTransactionSchedulingNotePlacement**
> DeleteTransactionSchedulingNotePlacementResponse DeleteTransactionSchedulingNotePlacement (int? id)

Delete the record containing the scheduling trip/leg where the notes were inserted at the time of order for a transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTransactionSchedulingNotePlacementExample
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
            var id = 56;  // int? | 

            try
            {
                // Delete the record containing the scheduling trip/leg where the notes were inserted at the time of order for a transaction.
                DeleteTransactionSchedulingNotePlacementResponse result = apiInstance.DeleteTransactionSchedulingNotePlacement(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.DeleteTransactionSchedulingNotePlacement: " + e.Message );
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

[**DeleteTransactionSchedulingNotePlacementResponse**](DeleteTransactionSchedulingNotePlacementResponse.md)

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

<a name="getscheduledtripsettings"></a>
# **GetScheduledTripSettings**
> ScheduledTripSettingsResponse GetScheduledTripSettings ()



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetScheduledTripSettingsExample
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
                ScheduledTripSettingsResponse result = apiInstance.GetScheduledTripSettings();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.GetScheduledTripSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**ScheduledTripSettingsResponse**](ScheduledTripSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getscheduledtripsbydaterange"></a>
# **GetScheduledTripsByDateRange**
> CurrentScheduledTripsResponse GetScheduledTripsByDateRange (DateTime? startDate, DateTime? endDate)

Fetch scheduled trip info pulled from the user's scheduling system by date range.

Only records that are scheduled to depart after the startDate and before the endDate will be returned.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetScheduledTripsByDateRangeExample
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
            var startDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                // Fetch scheduled trip info pulled from the user's scheduling system by date range.
                CurrentScheduledTripsResponse result = apiInstance.GetScheduledTripsByDateRange(startDate, endDate);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.GetScheduledTripsByDateRange: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **startDate** | **DateTime?**|  | [optional] 
 **endDate** | **DateTime?**|  | [optional] 

### Return type

[**CurrentScheduledTripsResponse**](CurrentScheduledTripsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getschedulingnotefailures"></a>
# **GetSchedulingNoteFailures**
> SchedulingNoteFailuresResponse GetSchedulingNoteFailures (int? transactionId, int? userId)

Get scheduling note failures by transactionId and userId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSchedulingNoteFailuresExample
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
            var transactionId = 56;  // int? | 
            var userId = 56;  // int? | 

            try
            {
                // Get scheduling note failures by transactionId and userId
                SchedulingNoteFailuresResponse result = apiInstance.GetSchedulingNoteFailures(transactionId, userId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.GetSchedulingNoteFailures: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionId** | **int?**|  | 
 **userId** | **int?**|  | 

### Return type

[**SchedulingNoteFailuresResponse**](SchedulingNoteFailuresResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionschedulingnoteplacement"></a>
# **GetTransactionSchedulingNotePlacement**
> TransactionSchedulingNotePlacementListResponse GetTransactionSchedulingNotePlacement (int? transactionId)

Fetch the scheduling trip/leg where notes were inserted at the time of order for a transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionSchedulingNotePlacementExample
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
            var transactionId = 56;  // int? | 

            try
            {
                // Fetch the scheduling trip/leg where notes were inserted at the time of order for a transaction.
                TransactionSchedulingNotePlacementListResponse result = apiInstance.GetTransactionSchedulingNotePlacement(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.GetTransactionSchedulingNotePlacement: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transactionId** | **int?**|  | 

### Return type

[**TransactionSchedulingNotePlacementListResponse**](TransactionSchedulingNotePlacementListResponse.md)

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

<a name="postscheduledtripsettings"></a>
# **PostScheduledTripSettings**
> PostScheduledTripSettingsResponse PostScheduledTripSettings (PostScheduledTripSettingsRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostScheduledTripSettingsExample
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
            var body = new PostScheduledTripSettingsRequest(); // PostScheduledTripSettingsRequest |  (optional) 

            try
            {
                PostScheduledTripSettingsResponse result = apiInstance.PostScheduledTripSettings(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.PostScheduledTripSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostScheduledTripSettingsRequest**](PostScheduledTripSettingsRequest.md)|  | [optional] 

### Return type

[**PostScheduledTripSettingsResponse**](PostScheduledTripSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postschedulingnotefailures"></a>
# **PostSchedulingNoteFailures**
> PostSchedulingNoteFailuresResponse PostSchedulingNoteFailures (PostSchedulingNoteFailuresRequest body)

Post scheduling note failures

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSchedulingNoteFailuresExample
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
            var body = new PostSchedulingNoteFailuresRequest(); // PostSchedulingNoteFailuresRequest |  (optional) 

            try
            {
                // Post scheduling note failures
                PostSchedulingNoteFailuresResponse result = apiInstance.PostSchedulingNoteFailures(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.PostSchedulingNoteFailures: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSchedulingNoteFailuresRequest**](PostSchedulingNoteFailuresRequest.md)|  | [optional] 

### Return type

[**PostSchedulingNoteFailuresResponse**](PostSchedulingNoteFailuresResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttransactionschedulingnoteplacement"></a>
# **PostTransactionSchedulingNotePlacement**
> PostTransactionSchedulingNotePlacementResponse PostTransactionSchedulingNotePlacement (PostTransactionSchedulingNotePlacementRequest body)

Add the scheduling trip/leg where the notes were inserted at the time of order for a transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTransactionSchedulingNotePlacementExample
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
            var body = new PostTransactionSchedulingNotePlacementRequest(); // PostTransactionSchedulingNotePlacementRequest |  (optional) 

            try
            {
                // Add the scheduling trip/leg where the notes were inserted at the time of order for a transaction.
                PostTransactionSchedulingNotePlacementResponse result = apiInstance.PostTransactionSchedulingNotePlacement(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.PostTransactionSchedulingNotePlacement: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTransactionSchedulingNotePlacementRequest**](PostTransactionSchedulingNotePlacementRequest.md)|  | [optional] 

### Return type

[**PostTransactionSchedulingNotePlacementResponse**](PostTransactionSchedulingNotePlacementResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatescheduledtripsettings"></a>
# **UpdateScheduledTripSettings**
> UpdateScheduledTripSettingsResponse UpdateScheduledTripSettings (UpdateScheduledTripSettingsRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateScheduledTripSettingsExample
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
            var body = new UpdateScheduledTripSettingsRequest(); // UpdateScheduledTripSettingsRequest |  (optional) 

            try
            {
                UpdateScheduledTripSettingsResponse result = apiInstance.UpdateScheduledTripSettings(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.UpdateScheduledTripSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateScheduledTripSettingsRequest**](UpdateScheduledTripSettingsRequest.md)|  | [optional] 

### Return type

[**UpdateScheduledTripSettingsResponse**](UpdateScheduledTripSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateschedulingnotefailures"></a>
# **UpdateSchedulingNoteFailures**
> UpdateSchedulingNoteFailuresResponse UpdateSchedulingNoteFailures (UpdateSchedulingNoteFailuresRequest body)

Update scheduling note failures

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSchedulingNoteFailuresExample
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
            var body = new UpdateSchedulingNoteFailuresRequest(); // UpdateSchedulingNoteFailuresRequest |  (optional) 

            try
            {
                // Update scheduling note failures
                UpdateSchedulingNoteFailuresResponse result = apiInstance.UpdateSchedulingNoteFailures(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.UpdateSchedulingNoteFailures: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSchedulingNoteFailuresRequest**](UpdateSchedulingNoteFailuresRequest.md)|  | [optional] 

### Return type

[**UpdateSchedulingNoteFailuresResponse**](UpdateSchedulingNoteFailuresResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetransactionschedulingnoteplacement"></a>
# **UpdateTransactionSchedulingNotePlacement**
> UpdateTransactionSchedulingNotePlacementResponse UpdateTransactionSchedulingNotePlacement (UpdateTransactionSchedulingNotePlacementRequest body)

Update the scheduling trip/leg where the notes were inserted at the time of order for a transaction.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTransactionSchedulingNotePlacementExample
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
            var body = new UpdateTransactionSchedulingNotePlacementRequest(); // UpdateTransactionSchedulingNotePlacementRequest |  (optional) 

            try
            {
                // Update the scheduling trip/leg where the notes were inserted at the time of order for a transaction.
                UpdateTransactionSchedulingNotePlacementResponse result = apiInstance.UpdateTransactionSchedulingNotePlacement(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ScheduledTripApi.UpdateTransactionSchedulingNotePlacement: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTransactionSchedulingNotePlacementRequest**](UpdateTransactionSchedulingNotePlacementRequest.md)|  | [optional] 

### Return type

[**UpdateTransactionSchedulingNotePlacementResponse**](UpdateTransactionSchedulingNotePlacementResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


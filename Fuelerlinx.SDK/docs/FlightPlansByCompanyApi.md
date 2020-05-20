# IO.Swagger.Api.FlightPlansByCompanyApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteIFlightPlannerRouteRequestServiceLog**](FlightPlansByCompanyApi.md#deleteiflightplannerrouterequestservicelog) | **DELETE** /api/FlightPlansByCompany/service-logs/iflightplanner-route-request | Internal use only - Delete an existing service log record for a iFlightPlanner route request.
[**GetCurrentPlannedFlights**](FlightPlansByCompanyApi.md#getcurrentplannedflights) | **GET** /api/FlightPlansByCompany/current | Fetch upcoming trip info pulled from the user&#39;s flight planning provider.
[**GetCurrentPlannedFlightsByTail**](FlightPlansByCompanyApi.md#getcurrentplannedflightsbytail) | **GET** /api/FlightPlansByCompany/current/tail/{tailNumber} | Fetch upcoming trip info pulled from the user&#39;s flight planning provider by tail number
[**GetIFlightPlannerAviationProfiles**](FlightPlansByCompanyApi.md#getiflightplanneraviationprofiles) | **GET** /api/FlightPlansByCompany/iflightplanner/aviationprofiles | Fetching stored aviation profiles from the user&#39;s iFlightPlanner account
[**GetIFlightPlannerRouteRequestServiceLog**](FlightPlansByCompanyApi.md#getiflightplannerrouterequestservicelog) | **GET** /api/FlightPlansByCompany/service-logs/iflightplanner-route-request/company/{companyId}/tail/{tailNumber}/departure/{departureAirport}/arrival/{arrivalAirport} | Internal use only - Fetch the service logs for iFlightPlanner route requests based on the provided search parameters.
[**GetRecentATCRoutes**](FlightPlansByCompanyApi.md#getrecentatcroutes) | **GET** /api/FlightPlansByCompany/recent-atc/{departureAirportIdentifier}/{arrivalAirportIdentifier} | 
[**PostIFlightPlannerRouteRequestServiceLog**](FlightPlansByCompanyApi.md#postiflightplannerrouterequestservicelog) | **POST** /api/FlightPlansByCompany/service-logs/iflightplanner-route-request | Internal use only - Post a new service log record for a iFlightPlanner route request.
[**PostTripInfo**](FlightPlansByCompanyApi.md#posttripinfo) | **POST** /api/FlightPlansByCompany/tripinfo | Accepts a serialized set of trip/leg information from a user&#39;s flight planning provider.
[**UpdateFuelPurchaseInfoForFlight**](FlightPlansByCompanyApi.md#updatefuelpurchaseinfoforflight) | **POST** /api/FlightPlansByCompany/fuelpurchaseinfo/update | Internal use only - Update fuel purchase information for a flight planning
[**UpdateIFlightPlannerRouteRequestServiceLog**](FlightPlansByCompanyApi.md#updateiflightplannerrouterequestservicelog) | **PUT** /api/FlightPlansByCompany/service-logs/iflightplanner-route-request | Internal use only - Update an existing service log record for a iFlightPlanner route request.


<a name="deleteiflightplannerrouterequestservicelog"></a>
# **DeleteIFlightPlannerRouteRequestServiceLog**
> DeleteIFlightPlannerRouteRequestServiceLogResponse DeleteIFlightPlannerRouteRequestServiceLog (int? id)

Internal use only - Delete an existing service log record for a iFlightPlanner route request.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteIFlightPlannerRouteRequestServiceLogExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete an existing service log record for a iFlightPlanner route request.
                DeleteIFlightPlannerRouteRequestServiceLogResponse result = apiInstance.DeleteIFlightPlannerRouteRequestServiceLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.DeleteIFlightPlannerRouteRequestServiceLog: " + e.Message );
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

[**DeleteIFlightPlannerRouteRequestServiceLogResponse**](DeleteIFlightPlannerRouteRequestServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcurrentplannedflights"></a>
# **GetCurrentPlannedFlights**
> CurrentPlannedFlightsResponse GetCurrentPlannedFlights ()

Fetch upcoming trip info pulled from the user's flight planning provider.

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
    public class GetCurrentPlannedFlightsExample
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

            var apiInstance = new FlightPlansByCompanyApi();

            try
            {
                // Fetch upcoming trip info pulled from the user's flight planning provider.
                CurrentPlannedFlightsResponse result = apiInstance.GetCurrentPlannedFlights();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.GetCurrentPlannedFlights: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**CurrentPlannedFlightsResponse**](CurrentPlannedFlightsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcurrentplannedflightsbytail"></a>
# **GetCurrentPlannedFlightsByTail**
> CurrentPlannedFlightsResponse GetCurrentPlannedFlightsByTail (string tailNumber)

Fetch upcoming trip info pulled from the user's flight planning provider by tail number

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
    public class GetCurrentPlannedFlightsByTailExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var tailNumber = tailNumber_example;  // string | 

            try
            {
                // Fetch upcoming trip info pulled from the user's flight planning provider by tail number
                CurrentPlannedFlightsResponse result = apiInstance.GetCurrentPlannedFlightsByTail(tailNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.GetCurrentPlannedFlightsByTail: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **tailNumber** | **string**|  | 

### Return type

[**CurrentPlannedFlightsResponse**](CurrentPlannedFlightsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getiflightplanneraviationprofiles"></a>
# **GetIFlightPlannerAviationProfiles**
> IFlightPlannerAviationProfilesResponse GetIFlightPlannerAviationProfiles ()

Fetching stored aviation profiles from the user's iFlightPlanner account

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIFlightPlannerAviationProfilesExample
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

            var apiInstance = new FlightPlansByCompanyApi();

            try
            {
                // Fetching stored aviation profiles from the user's iFlightPlanner account
                IFlightPlannerAviationProfilesResponse result = apiInstance.GetIFlightPlannerAviationProfiles();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.GetIFlightPlannerAviationProfiles: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**IFlightPlannerAviationProfilesResponse**](IFlightPlannerAviationProfilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getiflightplannerrouterequestservicelog"></a>
# **GetIFlightPlannerRouteRequestServiceLog**
> IFLightPlannerRouteRequestServiceLogListResponse GetIFlightPlannerRouteRequestServiceLog (int? companyId, string tailNumber, string departureAirport, string arrivalAirport, DateTime? startDate, DateTime? endDate)

Internal use only - Fetch the service logs for iFlightPlanner route requests based on the provided search parameters.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIFlightPlannerRouteRequestServiceLogExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var companyId = 56;  // int? | 
            var tailNumber = tailNumber_example;  // string | 
            var departureAirport = departureAirport_example;  // string | 
            var arrivalAirport = arrivalAirport_example;  // string | 
            var startDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDate = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                // Internal use only - Fetch the service logs for iFlightPlanner route requests based on the provided search parameters.
                IFLightPlannerRouteRequestServiceLogListResponse result = apiInstance.GetIFlightPlannerRouteRequestServiceLog(companyId, tailNumber, departureAirport, arrivalAirport, startDate, endDate);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.GetIFlightPlannerRouteRequestServiceLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **tailNumber** | **string**|  | 
 **departureAirport** | **string**|  | 
 **arrivalAirport** | **string**|  | 
 **startDate** | **DateTime?**|  | [optional] 
 **endDate** | **DateTime?**|  | [optional] 

### Return type

[**IFLightPlannerRouteRequestServiceLogListResponse**](IFLightPlannerRouteRequestServiceLogListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrecentatcroutes"></a>
# **GetRecentATCRoutes**
> RecentATCResponse GetRecentATCRoutes (string departureAirportIdentifier, string arrivalAirportIdentifier)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetRecentATCRoutesExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var departureAirportIdentifier = departureAirportIdentifier_example;  // string | 
            var arrivalAirportIdentifier = arrivalAirportIdentifier_example;  // string | 

            try
            {
                RecentATCResponse result = apiInstance.GetRecentATCRoutes(departureAirportIdentifier, arrivalAirportIdentifier);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.GetRecentATCRoutes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **departureAirportIdentifier** | **string**|  | 
 **arrivalAirportIdentifier** | **string**|  | 

### Return type

[**RecentATCResponse**](RecentATCResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postiflightplannerrouterequestservicelog"></a>
# **PostIFlightPlannerRouteRequestServiceLog**
> PostIFlightPlannerRouteRequestServiceLogResponse PostIFlightPlannerRouteRequestServiceLog (PostIFlightPlannerRouteRequestServiceLogRequest body)

Internal use only - Post a new service log record for a iFlightPlanner route request.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostIFlightPlannerRouteRequestServiceLogExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var body = new PostIFlightPlannerRouteRequestServiceLogRequest(); // PostIFlightPlannerRouteRequestServiceLogRequest |  (optional) 

            try
            {
                // Internal use only - Post a new service log record for a iFlightPlanner route request.
                PostIFlightPlannerRouteRequestServiceLogResponse result = apiInstance.PostIFlightPlannerRouteRequestServiceLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.PostIFlightPlannerRouteRequestServiceLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostIFlightPlannerRouteRequestServiceLogRequest**](PostIFlightPlannerRouteRequestServiceLogRequest.md)|  | [optional] 

### Return type

[**PostIFlightPlannerRouteRequestServiceLogResponse**](PostIFlightPlannerRouteRequestServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttripinfo"></a>
# **PostTripInfo**
> PostTripInfoResponse PostTripInfo (PostTripInfoRequest body)

Accepts a serialized set of trip/leg information from a user's flight planning provider.

If the serialized data is not in the expected \"TripInfo\" structure defined by FlightPlan.com, an error will occur.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTripInfoExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var body = new PostTripInfoRequest(); // PostTripInfoRequest |  (optional) 

            try
            {
                // Accepts a serialized set of trip/leg information from a user's flight planning provider.
                PostTripInfoResponse result = apiInstance.PostTripInfo(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.PostTripInfo: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTripInfoRequest**](PostTripInfoRequest.md)|  | [optional] 

### Return type

[**PostTripInfoResponse**](PostTripInfoResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatefuelpurchaseinfoforflight"></a>
# **UpdateFuelPurchaseInfoForFlight**
> UpdateFuelPurchaseInfoResponse UpdateFuelPurchaseInfoForFlight (UpdateFuelPurchaseInfoRequest body)

Internal use only - Update fuel purchase information for a flight planning

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFuelPurchaseInfoForFlightExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var body = new UpdateFuelPurchaseInfoRequest(); // UpdateFuelPurchaseInfoRequest |  (optional) 

            try
            {
                // Internal use only - Update fuel purchase information for a flight planning
                UpdateFuelPurchaseInfoResponse result = apiInstance.UpdateFuelPurchaseInfoForFlight(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.UpdateFuelPurchaseInfoForFlight: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateFuelPurchaseInfoRequest**](UpdateFuelPurchaseInfoRequest.md)|  | [optional] 

### Return type

[**UpdateFuelPurchaseInfoResponse**](UpdateFuelPurchaseInfoResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateiflightplannerrouterequestservicelog"></a>
# **UpdateIFlightPlannerRouteRequestServiceLog**
> UpdateIFlightPlannerRouteRequestServiceLogResponse UpdateIFlightPlannerRouteRequestServiceLog (UpdateIFlightPlannerRouteRequestServiceLogRequest body)

Internal use only - Update an existing service log record for a iFlightPlanner route request.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateIFlightPlannerRouteRequestServiceLogExample
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

            var apiInstance = new FlightPlansByCompanyApi();
            var body = new UpdateIFlightPlannerRouteRequestServiceLogRequest(); // UpdateIFlightPlannerRouteRequestServiceLogRequest |  (optional) 

            try
            {
                // Internal use only - Update an existing service log record for a iFlightPlanner route request.
                UpdateIFlightPlannerRouteRequestServiceLogResponse result = apiInstance.UpdateIFlightPlannerRouteRequestServiceLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FlightPlansByCompanyApi.UpdateIFlightPlannerRouteRequestServiceLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateIFlightPlannerRouteRequestServiceLogRequest**](UpdateIFlightPlannerRouteRequestServiceLogRequest.md)|  | [optional] 

### Return type

[**UpdateIFlightPlannerRouteRequestServiceLogResponse**](UpdateIFlightPlannerRouteRequestServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


# IO.Swagger.Api.ServiceLogsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCompanyAircraftChangeLog**](ServiceLogsApi.md#deletecompanyaircraftchangelog) | **DELETE** /api/ServiceLogs/companyAircraftChangeLog/{id} | Delete a company aircraft change log record by the record id.
[**DeleteCompanyFboChangeLog**](ServiceLogsApi.md#deletecompanyfbochangelog) | **DELETE** /api/ServiceLogs/company-fbo-change-log/{id} | Delete a company fbo change log record by the record id.
[**DeleteCompanyFuelerChangeLog**](ServiceLogsApi.md#deletecompanyfuelerchangelog) | **DELETE** /api/ServiceLogs/companyFuelerChangeLog/{id} | Delete a company fueler change log record by the record id.
[**DeleteDispatchEmailLog**](ServiceLogsApi.md#deletedispatchemaillog) | **DELETE** /api/ServiceLogs/dispatchEmailLog/{id} | Delete a dispatch email log record by the record id.
[**DeleteFuelOrderServiceLog**](ServiceLogsApi.md#deletefuelorderservicelog) | **DELETE** /api/ServiceLogs/fuelOrderServiceLog/{id} | Delete a fuel order service log record by the record id.
[**DeleteFuelPriceServiceLog**](ServiceLogsApi.md#deletefuelpriceservicelog) | **DELETE** /api/ServiceLogs/fuelPriceServiceLog/{id} | Delete a fuel price service log record by the record id.
[**DeleteIFlightPlannerRouteServiceLog**](ServiceLogsApi.md#deleteiflightplannerrouteservicelog) | **DELETE** /api/ServiceLogs/iFlightPlannerRouteRequestServiceLog/{id} | Delete a iFlightPlanner route request service log record by the record id.
[**DeleteSchedulingIntegrationDispatchServiceLog**](ServiceLogsApi.md#deleteschedulingintegrationdispatchservicelog) | **DELETE** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog/{id} | Delete a scheduling integration dispatch service log record by record id.
[**DeleteSchedulingIntegrationServiceLog**](ServiceLogsApi.md#deleteschedulingintegrationservicelog) | **DELETE** /api/ServiceLogs/schedulingIntegrationServiceLog/{id} | Delete a scheduling integration service log record by the record id.
[**DeleteTankeringApiCalculationLog**](ServiceLogsApi.md#deletetankeringapicalculationlog) | **DELETE** /api/ServiceLogs/tankeringApiCalculationLog/{id} | Delete a tankering api calculation log record by the record id.
[**GetCompanyAircraftChangeLogByTailNumber**](ServiceLogsApi.md#getcompanyaircraftchangelogbytailnumber) | **GET** /api/ServiceLogs/companyAircraftChangeLog/by-tailNumber/{tailNumber} | Fetch company aircraft change log by tailNumber.
[**GetCompanyAircraftChangeLogByUserId**](ServiceLogsApi.md#getcompanyaircraftchangelogbyuserid) | **GET** /api/ServiceLogs/companyAircraftChangeLog/by-userId/{userId} | Fetch company aircraft change log by userId.
[**GetCompanyFboChangeLogByCompanyId**](ServiceLogsApi.md#getcompanyfbochangelogbycompanyid) | **GET** /api/ServiceLogs/company-fbo-change-log/by-companyId/{companyId} | Fetch company fueler change log by company Id
[**GetCompanyFboChangeLogByIcao**](ServiceLogsApi.md#getcompanyfbochangelogbyicao) | **GET** /api/ServiceLogs/company-fbo-change-log/by-icao/{companyId}/{icao} | Fetch company fueler change log by ICAO
[**GetCompanyFuelerChangeLogByCompanyId**](ServiceLogsApi.md#getcompanyfuelerchangelogbycompanyid) | **GET** /api/ServiceLogs/companyFuelerChangeLog/by-companyId/{companyId} | Fetch company fueler change log by companyId.
[**GetCompanyFuelerChangeLogByFuelerId**](ServiceLogsApi.md#getcompanyfuelerchangelogbyfuelerid) | **GET** /api/ServiceLogs/companyFuelerChangeLog/by-fuelerId/{companyId}/{fuelerId} | Fetch company fueler change log by fuelerId.
[**GetDispatchEmailLogByTailNumber**](ServiceLogsApi.md#getdispatchemaillogbytailnumber) | **GET** /api/ServiceLogs/dispatchEmailLog/by-tailNumber/{tailNumber} | Fetch dispatch email log by tailNumber.
[**GetDispatchEmailLogByTransactionId**](ServiceLogsApi.md#getdispatchemaillogbytransactionid) | **GET** /api/ServiceLogs/dispatchEmailLog/by-userId/{userId}/{transactionId} | Fetch dispatch email log by transactionId.
[**GetDispatchEmailLogByUserId**](ServiceLogsApi.md#getdispatchemaillogbyuserid) | **GET** /api/ServiceLogs/dispatchEmailLog/by-userId/{userId} | Fetch dispatch email log by userId.
[**GetFuelOrderServiceLogByTransactionId**](ServiceLogsApi.md#getfuelorderservicelogbytransactionid) | **GET** /api/ServiceLogs/fuelOrderServiceLog/by-transactionId/{transactionId} | Fetch fuel order service log by transactionId.
[**GetFuelPriceServiceLogByLocation**](ServiceLogsApi.md#getfuelpriceservicelogbylocation) | **GET** /api/ServiceLogs/fuelPriceServiceLog/by-location/{userId}/{icaos} | Fetch fuel price service log by location.
[**GetFuelPriceServiceLogByUserId**](ServiceLogsApi.md#getfuelpriceservicelogbyuserid) | **GET** /api/ServiceLogs/fuelPriceServiceLog/by-userId/{userId} | /// &lt;summary&gt;  Fetch fuel price service log by userId.  &lt;/summary&gt;
[**GetIFlightPlannerRouteServiceLog**](ServiceLogsApi.md#getiflightplannerrouteservicelog) | **GET** /api/ServiceLogs/iFlightPlannerRouteRequestServiceLog/{companyId}/{tailNumber}/{departureAirport}/{arrivalAirport} | Fetch iFlightPlanner route request service log&#39;
[**GetSchedulingIntegrationDispatchServiceLogByCompanyId**](ServiceLogsApi.md#getschedulingintegrationdispatchservicelogbycompanyid) | **GET** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog/by-companyId/{companyId} | Fetch scheduling integration dispatch service log by companyId.
[**GetSchedulingIntegrationDispatchServiceLogByDate**](ServiceLogsApi.md#getschedulingintegrationdispatchservicelogbydate) | **GET** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog/by-date/{companyId} | Fetch scheduling integration dispatch service log by Date.
[**GetSchedulingIntegrationServiceLogByCompanyId**](ServiceLogsApi.md#getschedulingintegrationservicelogbycompanyid) | **GET** /api/ServiceLogs/schedulingIntegrationServiceLog/by-companyId/{companyId} | Fetch scheduling integration service log by companyId.
[**GetSchedulingIntegrationServiceLogByDate**](ServiceLogsApi.md#getschedulingintegrationservicelogbydate) | **GET** /api/ServiceLogs/schedulingIntegrationServiceLog/by-date/{companyId}/{dateTimeRecorded} | Fetch scheduling integration service log by date.
[**GetTankeringApiCalculationLog**](ServiceLogsApi.md#gettankeringapicalculationlog) | **GET** /api/ServiceLogs/tankeringApiCalculationLog/{companyId}/{startDateTime}/{endDateTime} | Fetch tankering api calculation log.
[**PostCompanyAircraftChangeLogAsync**](ServiceLogsApi.md#postcompanyaircraftchangelogasync) | **POST** /api/ServiceLogs/companyAircraftChangeLog | Post company aircraft change log.
[**PostCompanyFboChangeLogAsync**](ServiceLogsApi.md#postcompanyfbochangelogasync) | **POST** /api/ServiceLogs/company-fbo-change-log | Post company fbo change log
[**PostCompanyFuelerChangeLogAsync**](ServiceLogsApi.md#postcompanyfuelerchangelogasync) | **POST** /api/ServiceLogs/companyFuelerChangeLog | Post company fueler change log.
[**PostDispatchEmailLogAsync**](ServiceLogsApi.md#postdispatchemaillogasync) | **POST** /api/ServiceLogs/dispatchEmailLog | Post dispatch email log.
[**PostFuelOrderServiceLogAsync**](ServiceLogsApi.md#postfuelorderservicelogasync) | **POST** /api/ServiceLogs/fuelOrderServiceLog | Post fuel order service log.
[**PostFuelPriceServiceLogAsync**](ServiceLogsApi.md#postfuelpriceservicelogasync) | **POST** /api/ServiceLogs/fuelPriceServiceLog | Post fuel price service log.
[**PostIFlightPlannerRouteServiceLogAsync**](ServiceLogsApi.md#postiflightplannerrouteservicelogasync) | **POST** /api/ServiceLogs/iFlightPlannerRouteRequestServiceLog | Post iFlightPlanner route request service log.
[**PostSchedulingIntegrationDispatchServiceLogAsync**](ServiceLogsApi.md#postschedulingintegrationdispatchservicelogasync) | **POST** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog | Post scheduling integration dispatch service log.
[**PostSchedulingIntegrationServiceLogAsync**](ServiceLogsApi.md#postschedulingintegrationservicelogasync) | **POST** /api/ServiceLogs/schedulingIntegrationServiceLog | Post scheduling integration service log.
[**PostTankeringApiCalculationLogAsync**](ServiceLogsApi.md#posttankeringapicalculationlogasync) | **POST** /api/ServiceLogs/tankeringApiCalculationLog | Post tankering api calculation log.
[**UpdateCompanyAircraftChangeLog**](ServiceLogsApi.md#updatecompanyaircraftchangelog) | **PUT** /api/ServiceLogs/companyAircraftChangeLog | Update the company aircraft change log.
[**UpdateCompanyFboChangeLog**](ServiceLogsApi.md#updatecompanyfbochangelog) | **PUT** /api/ServiceLogs/company-fbo-change-log | Update the company fbo change log.
[**UpdateCompanyFuelerChangeLog**](ServiceLogsApi.md#updatecompanyfuelerchangelog) | **PUT** /api/ServiceLogs/companyFuelerChangeLog | Update the company fueler change log.
[**UpdateDispatchEmailLog**](ServiceLogsApi.md#updatedispatchemaillog) | **PUT** /api/ServiceLogs/dispatchEmailLog | Update the dispatch email log.
[**UpdateFuelOrderServiceLog**](ServiceLogsApi.md#updatefuelorderservicelog) | **PUT** /api/ServiceLogs/fuelOrderServiceLog | Update the fuel order service log.
[**UpdateFuelPriceServiceLog**](ServiceLogsApi.md#updatefuelpriceservicelog) | **PUT** /api/ServiceLogs/fuelPriceServiceLog | Update the fuel price service log.
[**UpdateIFlightPlannerRouteServiceLog**](ServiceLogsApi.md#updateiflightplannerrouteservicelog) | **PUT** /api/ServiceLogs/iFlightPlannerRouteRequestServiceLog | Update the iFlightPlanner route request service log.
[**UpdateSchedulingIntegrationDispatchServiceLog**](ServiceLogsApi.md#updateschedulingintegrationdispatchservicelog) | **PUT** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog | Update the scheduling integration dispatch service log.
[**UpdateSchedulingIntegrationServiceLog**](ServiceLogsApi.md#updateschedulingintegrationservicelog) | **PUT** /api/ServiceLogs/schedulingIntegrationServiceLog | Update the scheduling integration service log.
[**UpdateTankeringApiCalculationLog**](ServiceLogsApi.md#updatetankeringapicalculationlog) | **PUT** /api/ServiceLogs/tankeringApiCalculationLog | Update the tankering api calculation log.


<a name="deletecompanyaircraftchangelog"></a>
# **DeleteCompanyAircraftChangeLog**
> DeleteCompanyAircraftChangeLogResponse DeleteCompanyAircraftChangeLog (int? id)

Delete a company aircraft change log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyAircraftChangeLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a company aircraft change log record by the record id.
                DeleteCompanyAircraftChangeLogResponse result = apiInstance.DeleteCompanyAircraftChangeLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteCompanyAircraftChangeLog: " + e.Message );
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

[**DeleteCompanyAircraftChangeLogResponse**](DeleteCompanyAircraftChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletecompanyfbochangelog"></a>
# **DeleteCompanyFboChangeLog**
> DeleteCompanyFboChangeLogResponse DeleteCompanyFboChangeLog (int? id)

Delete a company fbo change log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyFboChangeLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a company fbo change log record by the record id.
                DeleteCompanyFboChangeLogResponse result = apiInstance.DeleteCompanyFboChangeLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteCompanyFboChangeLog: " + e.Message );
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

[**DeleteCompanyFboChangeLogResponse**](DeleteCompanyFboChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletecompanyfuelerchangelog"></a>
# **DeleteCompanyFuelerChangeLog**
> DeleteCompanyFuelerChangeLogResponse DeleteCompanyFuelerChangeLog (int? id)

Delete a company fueler change log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyFuelerChangeLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a company fueler change log record by the record id.
                DeleteCompanyFuelerChangeLogResponse result = apiInstance.DeleteCompanyFuelerChangeLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteCompanyFuelerChangeLog: " + e.Message );
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

[**DeleteCompanyFuelerChangeLogResponse**](DeleteCompanyFuelerChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletedispatchemaillog"></a>
# **DeleteDispatchEmailLog**
> DeleteDispatchEmailLogResponse DeleteDispatchEmailLog (int? id)

Delete a dispatch email log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteDispatchEmailLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a dispatch email log record by the record id.
                DeleteDispatchEmailLogResponse result = apiInstance.DeleteDispatchEmailLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteDispatchEmailLog: " + e.Message );
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

[**DeleteDispatchEmailLogResponse**](DeleteDispatchEmailLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletefuelorderservicelog"></a>
# **DeleteFuelOrderServiceLog**
> DeleteFuelOrderServiceLogResponse DeleteFuelOrderServiceLog (int? id)

Delete a fuel order service log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteFuelOrderServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a fuel order service log record by the record id.
                DeleteFuelOrderServiceLogResponse result = apiInstance.DeleteFuelOrderServiceLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteFuelOrderServiceLog: " + e.Message );
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

[**DeleteFuelOrderServiceLogResponse**](DeleteFuelOrderServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletefuelpriceservicelog"></a>
# **DeleteFuelPriceServiceLog**
> DeleteFuelPriceServiceLogResponse DeleteFuelPriceServiceLog (int? id)

Delete a fuel price service log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteFuelPriceServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a fuel price service log record by the record id.
                DeleteFuelPriceServiceLogResponse result = apiInstance.DeleteFuelPriceServiceLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteFuelPriceServiceLog: " + e.Message );
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

[**DeleteFuelPriceServiceLogResponse**](DeleteFuelPriceServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteiflightplannerrouteservicelog"></a>
# **DeleteIFlightPlannerRouteServiceLog**
> DeleteIFlightPlannerRouteRequestServiceLogResponse DeleteIFlightPlannerRouteServiceLog (int? id)

Delete a iFlightPlanner route request service log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteIFlightPlannerRouteServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a iFlightPlanner route request service log record by the record id.
                DeleteIFlightPlannerRouteRequestServiceLogResponse result = apiInstance.DeleteIFlightPlannerRouteServiceLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteIFlightPlannerRouteServiceLog: " + e.Message );
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

<a name="deleteschedulingintegrationdispatchservicelog"></a>
# **DeleteSchedulingIntegrationDispatchServiceLog**
> DeleteSchedulingIntegrationDispatchServiceLogResponse DeleteSchedulingIntegrationDispatchServiceLog (int? id)

Delete a scheduling integration dispatch service log record by record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSchedulingIntegrationDispatchServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a scheduling integration dispatch service log record by record id.
                DeleteSchedulingIntegrationDispatchServiceLogResponse result = apiInstance.DeleteSchedulingIntegrationDispatchServiceLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteSchedulingIntegrationDispatchServiceLog: " + e.Message );
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

[**DeleteSchedulingIntegrationDispatchServiceLogResponse**](DeleteSchedulingIntegrationDispatchServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteschedulingintegrationservicelog"></a>
# **DeleteSchedulingIntegrationServiceLog**
> DeleteSchedulingIntegrationServiceLogResponse DeleteSchedulingIntegrationServiceLog (int? id)

Delete a scheduling integration service log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSchedulingIntegrationServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a scheduling integration service log record by the record id.
                DeleteSchedulingIntegrationServiceLogResponse result = apiInstance.DeleteSchedulingIntegrationServiceLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteSchedulingIntegrationServiceLog: " + e.Message );
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

[**DeleteSchedulingIntegrationServiceLogResponse**](DeleteSchedulingIntegrationServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletetankeringapicalculationlog"></a>
# **DeleteTankeringApiCalculationLog**
> DeleteTankeringApiCalculationLogResponse DeleteTankeringApiCalculationLog (int? id)

Delete a tankering api calculation log record by the record id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTankeringApiCalculationLogExample
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

            var apiInstance = new ServiceLogsApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a tankering api calculation log record by the record id.
                DeleteTankeringApiCalculationLogResponse result = apiInstance.DeleteTankeringApiCalculationLog(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.DeleteTankeringApiCalculationLog: " + e.Message );
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

[**DeleteTankeringApiCalculationLogResponse**](DeleteTankeringApiCalculationLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyaircraftchangelogbytailnumber"></a>
# **GetCompanyAircraftChangeLogByTailNumber**
> CompanyAircraftChangeLogResponse GetCompanyAircraftChangeLogByTailNumber (string tailNumber)

Fetch company aircraft change log by tailNumber.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyAircraftChangeLogByTailNumberExample
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

            var apiInstance = new ServiceLogsApi();
            var tailNumber = tailNumber_example;  // string | 

            try
            {
                // Fetch company aircraft change log by tailNumber.
                CompanyAircraftChangeLogResponse result = apiInstance.GetCompanyAircraftChangeLogByTailNumber(tailNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetCompanyAircraftChangeLogByTailNumber: " + e.Message );
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

[**CompanyAircraftChangeLogResponse**](CompanyAircraftChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyaircraftchangelogbyuserid"></a>
# **GetCompanyAircraftChangeLogByUserId**
> CompanyAircraftChangeLogResponse GetCompanyAircraftChangeLogByUserId (int? userId)

Fetch company aircraft change log by userId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyAircraftChangeLogByUserIdExample
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

            var apiInstance = new ServiceLogsApi();
            var userId = 56;  // int? | 

            try
            {
                // Fetch company aircraft change log by userId.
                CompanyAircraftChangeLogResponse result = apiInstance.GetCompanyAircraftChangeLogByUserId(userId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetCompanyAircraftChangeLogByUserId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **int?**|  | 

### Return type

[**CompanyAircraftChangeLogResponse**](CompanyAircraftChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfbochangelogbycompanyid"></a>
# **GetCompanyFboChangeLogByCompanyId**
> CompanyFboChangeLogResponse GetCompanyFboChangeLogByCompanyId (int? companyId)

Fetch company fueler change log by company Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFboChangeLogByCompanyIdExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 

            try
            {
                // Fetch company fueler change log by company Id
                CompanyFboChangeLogResponse result = apiInstance.GetCompanyFboChangeLogByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetCompanyFboChangeLogByCompanyId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 

### Return type

[**CompanyFboChangeLogResponse**](CompanyFboChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfbochangelogbyicao"></a>
# **GetCompanyFboChangeLogByIcao**
> CompanyFboChangeLogResponse GetCompanyFboChangeLogByIcao (int? companyId, string icao)

Fetch company fueler change log by ICAO

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFboChangeLogByIcaoExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 
            var icao = icao_example;  // string | 

            try
            {
                // Fetch company fueler change log by ICAO
                CompanyFboChangeLogResponse result = apiInstance.GetCompanyFboChangeLogByIcao(companyId, icao);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetCompanyFboChangeLogByIcao: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **icao** | **string**|  | 

### Return type

[**CompanyFboChangeLogResponse**](CompanyFboChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfuelerchangelogbycompanyid"></a>
# **GetCompanyFuelerChangeLogByCompanyId**
> CompanyFuelerChangeLogResponse GetCompanyFuelerChangeLogByCompanyId (int? companyId)

Fetch company fueler change log by companyId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerChangeLogByCompanyIdExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 

            try
            {
                // Fetch company fueler change log by companyId.
                CompanyFuelerChangeLogResponse result = apiInstance.GetCompanyFuelerChangeLogByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetCompanyFuelerChangeLogByCompanyId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 

### Return type

[**CompanyFuelerChangeLogResponse**](CompanyFuelerChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfuelerchangelogbyfuelerid"></a>
# **GetCompanyFuelerChangeLogByFuelerId**
> CompanyFuelerChangeLogResponse GetCompanyFuelerChangeLogByFuelerId (int? companyId, int? fuelerId)

Fetch company fueler change log by fuelerId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerChangeLogByFuelerIdExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 
            var fuelerId = 56;  // int? | 

            try
            {
                // Fetch company fueler change log by fuelerId.
                CompanyFuelerChangeLogResponse result = apiInstance.GetCompanyFuelerChangeLogByFuelerId(companyId, fuelerId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetCompanyFuelerChangeLogByFuelerId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **fuelerId** | **int?**|  | 

### Return type

[**CompanyFuelerChangeLogResponse**](CompanyFuelerChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getdispatchemaillogbytailnumber"></a>
# **GetDispatchEmailLogByTailNumber**
> DispatchEmailLogResponse GetDispatchEmailLogByTailNumber (string tailNumber)

Fetch dispatch email log by tailNumber.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDispatchEmailLogByTailNumberExample
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

            var apiInstance = new ServiceLogsApi();
            var tailNumber = tailNumber_example;  // string | 

            try
            {
                // Fetch dispatch email log by tailNumber.
                DispatchEmailLogResponse result = apiInstance.GetDispatchEmailLogByTailNumber(tailNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetDispatchEmailLogByTailNumber: " + e.Message );
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

[**DispatchEmailLogResponse**](DispatchEmailLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getdispatchemaillogbytransactionid"></a>
# **GetDispatchEmailLogByTransactionId**
> DispatchEmailLogResponse GetDispatchEmailLogByTransactionId (int? userId, int? transactionId)

Fetch dispatch email log by transactionId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDispatchEmailLogByTransactionIdExample
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

            var apiInstance = new ServiceLogsApi();
            var userId = 56;  // int? | 
            var transactionId = 56;  // int? | 

            try
            {
                // Fetch dispatch email log by transactionId.
                DispatchEmailLogResponse result = apiInstance.GetDispatchEmailLogByTransactionId(userId, transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetDispatchEmailLogByTransactionId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **int?**|  | 
 **transactionId** | **int?**|  | 

### Return type

[**DispatchEmailLogResponse**](DispatchEmailLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getdispatchemaillogbyuserid"></a>
# **GetDispatchEmailLogByUserId**
> DispatchEmailLogResponse GetDispatchEmailLogByUserId (int? userId)

Fetch dispatch email log by userId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDispatchEmailLogByUserIdExample
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

            var apiInstance = new ServiceLogsApi();
            var userId = 56;  // int? | 

            try
            {
                // Fetch dispatch email log by userId.
                DispatchEmailLogResponse result = apiInstance.GetDispatchEmailLogByUserId(userId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetDispatchEmailLogByUserId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **int?**|  | 

### Return type

[**DispatchEmailLogResponse**](DispatchEmailLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfuelorderservicelogbytransactionid"></a>
# **GetFuelOrderServiceLogByTransactionId**
> FuelOrderServiceLogResponse GetFuelOrderServiceLogByTransactionId (int? transactionId)

Fetch fuel order service log by transactionId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFuelOrderServiceLogByTransactionIdExample
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

            var apiInstance = new ServiceLogsApi();
            var transactionId = 56;  // int? | 

            try
            {
                // Fetch fuel order service log by transactionId.
                FuelOrderServiceLogResponse result = apiInstance.GetFuelOrderServiceLogByTransactionId(transactionId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetFuelOrderServiceLogByTransactionId: " + e.Message );
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

[**FuelOrderServiceLogResponse**](FuelOrderServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfuelpriceservicelogbylocation"></a>
# **GetFuelPriceServiceLogByLocation**
> FuelPriceServiceLogResponse GetFuelPriceServiceLogByLocation (int? userId, string icaos)

Fetch fuel price service log by location.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFuelPriceServiceLogByLocationExample
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

            var apiInstance = new ServiceLogsApi();
            var userId = 56;  // int? | 
            var icaos = icaos_example;  // string | 

            try
            {
                // Fetch fuel price service log by location.
                FuelPriceServiceLogResponse result = apiInstance.GetFuelPriceServiceLogByLocation(userId, icaos);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetFuelPriceServiceLogByLocation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **int?**|  | 
 **icaos** | **string**|  | 

### Return type

[**FuelPriceServiceLogResponse**](FuelPriceServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfuelpriceservicelogbyuserid"></a>
# **GetFuelPriceServiceLogByUserId**
> FuelPriceServiceLogResponse GetFuelPriceServiceLogByUserId (int? userId)

/// <summary>  Fetch fuel price service log by userId.  </summary>

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFuelPriceServiceLogByUserIdExample
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

            var apiInstance = new ServiceLogsApi();
            var userId = 56;  // int? | 

            try
            {
                // /// <summary>  Fetch fuel price service log by userId.  </summary>
                FuelPriceServiceLogResponse result = apiInstance.GetFuelPriceServiceLogByUserId(userId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetFuelPriceServiceLogByUserId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **int?**|  | 

### Return type

[**FuelPriceServiceLogResponse**](FuelPriceServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getiflightplannerrouteservicelog"></a>
# **GetIFlightPlannerRouteServiceLog**
> IFlightPlannerRouteRequestServiceLogResponse GetIFlightPlannerRouteServiceLog (int? companyId, string tailNumber, string departureAirport, string arrivalAirport, DateTime? startDateTime, DateTime? endDateTime)

Fetch iFlightPlanner route request service log'

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIFlightPlannerRouteServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 
            var tailNumber = tailNumber_example;  // string | 
            var departureAirport = departureAirport_example;  // string | 
            var arrivalAirport = arrivalAirport_example;  // string | 
            var startDateTime = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 
            var endDateTime = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                // Fetch iFlightPlanner route request service log'
                IFlightPlannerRouteRequestServiceLogResponse result = apiInstance.GetIFlightPlannerRouteServiceLog(companyId, tailNumber, departureAirport, arrivalAirport, startDateTime, endDateTime);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetIFlightPlannerRouteServiceLog: " + e.Message );
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
 **startDateTime** | **DateTime?**|  | [optional] 
 **endDateTime** | **DateTime?**|  | [optional] 

### Return type

[**IFlightPlannerRouteRequestServiceLogResponse**](IFlightPlannerRouteRequestServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getschedulingintegrationdispatchservicelogbycompanyid"></a>
# **GetSchedulingIntegrationDispatchServiceLogByCompanyId**
> SchedulingIntegrationDispatchServiceLogResponse GetSchedulingIntegrationDispatchServiceLogByCompanyId (int? companyId)

Fetch scheduling integration dispatch service log by companyId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSchedulingIntegrationDispatchServiceLogByCompanyIdExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 

            try
            {
                // Fetch scheduling integration dispatch service log by companyId.
                SchedulingIntegrationDispatchServiceLogResponse result = apiInstance.GetSchedulingIntegrationDispatchServiceLogByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetSchedulingIntegrationDispatchServiceLogByCompanyId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 

### Return type

[**SchedulingIntegrationDispatchServiceLogResponse**](SchedulingIntegrationDispatchServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getschedulingintegrationdispatchservicelogbydate"></a>
# **GetSchedulingIntegrationDispatchServiceLogByDate**
> SchedulingIntegrationDispatchServiceLogResponse GetSchedulingIntegrationDispatchServiceLogByDate (int? companyId, DateTime? dateTimeRecorded)

Fetch scheduling integration dispatch service log by Date.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSchedulingIntegrationDispatchServiceLogByDateExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 
            var dateTimeRecorded = 2013-10-20T19:20:30+01:00;  // DateTime? |  (optional) 

            try
            {
                // Fetch scheduling integration dispatch service log by Date.
                SchedulingIntegrationDispatchServiceLogResponse result = apiInstance.GetSchedulingIntegrationDispatchServiceLogByDate(companyId, dateTimeRecorded);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetSchedulingIntegrationDispatchServiceLogByDate: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **dateTimeRecorded** | **DateTime?**|  | [optional] 

### Return type

[**SchedulingIntegrationDispatchServiceLogResponse**](SchedulingIntegrationDispatchServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getschedulingintegrationservicelogbycompanyid"></a>
# **GetSchedulingIntegrationServiceLogByCompanyId**
> SchedulingIntegrationServiceLogResponse GetSchedulingIntegrationServiceLogByCompanyId (int? companyId)

Fetch scheduling integration service log by companyId.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSchedulingIntegrationServiceLogByCompanyIdExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 

            try
            {
                // Fetch scheduling integration service log by companyId.
                SchedulingIntegrationServiceLogResponse result = apiInstance.GetSchedulingIntegrationServiceLogByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetSchedulingIntegrationServiceLogByCompanyId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 

### Return type

[**SchedulingIntegrationServiceLogResponse**](SchedulingIntegrationServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getschedulingintegrationservicelogbydate"></a>
# **GetSchedulingIntegrationServiceLogByDate**
> SchedulingIntegrationServiceLogResponse GetSchedulingIntegrationServiceLogByDate (int? companyId, DateTime? dateTimeRecorded)

Fetch scheduling integration service log by date.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSchedulingIntegrationServiceLogByDateExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 
            var dateTimeRecorded = 2013-10-20T19:20:30+01:00;  // DateTime? | 

            try
            {
                // Fetch scheduling integration service log by date.
                SchedulingIntegrationServiceLogResponse result = apiInstance.GetSchedulingIntegrationServiceLogByDate(companyId, dateTimeRecorded);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetSchedulingIntegrationServiceLogByDate: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **dateTimeRecorded** | **DateTime?**|  | 

### Return type

[**SchedulingIntegrationServiceLogResponse**](SchedulingIntegrationServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettankeringapicalculationlog"></a>
# **GetTankeringApiCalculationLog**
> TankeringApiCalculationLogResponse GetTankeringApiCalculationLog (int? companyId, DateTime? startDateTime, DateTime? endDateTime)

Fetch tankering api calculation log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTankeringApiCalculationLogExample
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

            var apiInstance = new ServiceLogsApi();
            var companyId = 56;  // int? | 
            var startDateTime = 2013-10-20T19:20:30+01:00;  // DateTime? | 
            var endDateTime = 2013-10-20T19:20:30+01:00;  // DateTime? | 

            try
            {
                // Fetch tankering api calculation log.
                TankeringApiCalculationLogResponse result = apiInstance.GetTankeringApiCalculationLog(companyId, startDateTime, endDateTime);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.GetTankeringApiCalculationLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **startDateTime** | **DateTime?**|  | 
 **endDateTime** | **DateTime?**|  | 

### Return type

[**TankeringApiCalculationLogResponse**](TankeringApiCalculationLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcompanyaircraftchangelogasync"></a>
# **PostCompanyAircraftChangeLogAsync**
> PostCompanyAircraftChangeLogResponse PostCompanyAircraftChangeLogAsync (PostCompanyAircraftChangeLogRequest body)

Post company aircraft change log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyAircraftChangeLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostCompanyAircraftChangeLogRequest(); // PostCompanyAircraftChangeLogRequest |  (optional) 

            try
            {
                // Post company aircraft change log.
                PostCompanyAircraftChangeLogResponse result = apiInstance.PostCompanyAircraftChangeLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostCompanyAircraftChangeLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyAircraftChangeLogRequest**](PostCompanyAircraftChangeLogRequest.md)|  | [optional] 

### Return type

[**PostCompanyAircraftChangeLogResponse**](PostCompanyAircraftChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcompanyfbochangelogasync"></a>
# **PostCompanyFboChangeLogAsync**
> PostCompanyFboChangeLogResponse PostCompanyFboChangeLogAsync (PostCompanyFboChangeLogRequest body)

Post company fbo change log

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyFboChangeLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostCompanyFboChangeLogRequest(); // PostCompanyFboChangeLogRequest |  (optional) 

            try
            {
                // Post company fbo change log
                PostCompanyFboChangeLogResponse result = apiInstance.PostCompanyFboChangeLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostCompanyFboChangeLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyFboChangeLogRequest**](PostCompanyFboChangeLogRequest.md)|  | [optional] 

### Return type

[**PostCompanyFboChangeLogResponse**](PostCompanyFboChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcompanyfuelerchangelogasync"></a>
# **PostCompanyFuelerChangeLogAsync**
> PostCompanyFuelerChangeLogResponse PostCompanyFuelerChangeLogAsync (PostCompanyFuelerChangeLogRequest body)

Post company fueler change log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyFuelerChangeLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostCompanyFuelerChangeLogRequest(); // PostCompanyFuelerChangeLogRequest |  (optional) 

            try
            {
                // Post company fueler change log.
                PostCompanyFuelerChangeLogResponse result = apiInstance.PostCompanyFuelerChangeLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostCompanyFuelerChangeLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyFuelerChangeLogRequest**](PostCompanyFuelerChangeLogRequest.md)|  | [optional] 

### Return type

[**PostCompanyFuelerChangeLogResponse**](PostCompanyFuelerChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postdispatchemaillogasync"></a>
# **PostDispatchEmailLogAsync**
> PostDispatchEmailLogResponse PostDispatchEmailLogAsync (PostDispatchEmailLogRequest body)

Post dispatch email log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostDispatchEmailLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostDispatchEmailLogRequest(); // PostDispatchEmailLogRequest |  (optional) 

            try
            {
                // Post dispatch email log.
                PostDispatchEmailLogResponse result = apiInstance.PostDispatchEmailLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostDispatchEmailLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostDispatchEmailLogRequest**](PostDispatchEmailLogRequest.md)|  | [optional] 

### Return type

[**PostDispatchEmailLogResponse**](PostDispatchEmailLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postfuelorderservicelogasync"></a>
# **PostFuelOrderServiceLogAsync**
> PostFuelOrderServiceLogResponse PostFuelOrderServiceLogAsync (PostFuelOrderServiceLogRequest body)

Post fuel order service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostFuelOrderServiceLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostFuelOrderServiceLogRequest(); // PostFuelOrderServiceLogRequest |  (optional) 

            try
            {
                // Post fuel order service log.
                PostFuelOrderServiceLogResponse result = apiInstance.PostFuelOrderServiceLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostFuelOrderServiceLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostFuelOrderServiceLogRequest**](PostFuelOrderServiceLogRequest.md)|  | [optional] 

### Return type

[**PostFuelOrderServiceLogResponse**](PostFuelOrderServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postfuelpriceservicelogasync"></a>
# **PostFuelPriceServiceLogAsync**
> PostFuelPriceServiceLogResponse PostFuelPriceServiceLogAsync (PostFuelPriceServiceLogRequest body)

Post fuel price service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostFuelPriceServiceLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostFuelPriceServiceLogRequest(); // PostFuelPriceServiceLogRequest |  (optional) 

            try
            {
                // Post fuel price service log.
                PostFuelPriceServiceLogResponse result = apiInstance.PostFuelPriceServiceLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostFuelPriceServiceLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostFuelPriceServiceLogRequest**](PostFuelPriceServiceLogRequest.md)|  | [optional] 

### Return type

[**PostFuelPriceServiceLogResponse**](PostFuelPriceServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postiflightplannerrouteservicelogasync"></a>
# **PostIFlightPlannerRouteServiceLogAsync**
> PostIFlightPlannerRouteRequestServiceLogResponse PostIFlightPlannerRouteServiceLogAsync (PostIFlightPlannerRouteRequestServiceLogRequest body)

Post iFlightPlanner route request service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostIFlightPlannerRouteServiceLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostIFlightPlannerRouteRequestServiceLogRequest(); // PostIFlightPlannerRouteRequestServiceLogRequest |  (optional) 

            try
            {
                // Post iFlightPlanner route request service log.
                PostIFlightPlannerRouteRequestServiceLogResponse result = apiInstance.PostIFlightPlannerRouteServiceLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostIFlightPlannerRouteServiceLogAsync: " + e.Message );
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

<a name="postschedulingintegrationdispatchservicelogasync"></a>
# **PostSchedulingIntegrationDispatchServiceLogAsync**
> PostSchedulingIntegrationDispatchServiceLogResponse PostSchedulingIntegrationDispatchServiceLogAsync (PostSchedulingIntegrationDispatchServiceLogRequest body)

Post scheduling integration dispatch service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSchedulingIntegrationDispatchServiceLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostSchedulingIntegrationDispatchServiceLogRequest(); // PostSchedulingIntegrationDispatchServiceLogRequest |  (optional) 

            try
            {
                // Post scheduling integration dispatch service log.
                PostSchedulingIntegrationDispatchServiceLogResponse result = apiInstance.PostSchedulingIntegrationDispatchServiceLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostSchedulingIntegrationDispatchServiceLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSchedulingIntegrationDispatchServiceLogRequest**](PostSchedulingIntegrationDispatchServiceLogRequest.md)|  | [optional] 

### Return type

[**PostSchedulingIntegrationDispatchServiceLogResponse**](PostSchedulingIntegrationDispatchServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postschedulingintegrationservicelogasync"></a>
# **PostSchedulingIntegrationServiceLogAsync**
> PostSchedulingIntegrationServiceLogResponse PostSchedulingIntegrationServiceLogAsync (PostSchedulingIntegrationServiceLogRequest body)

Post scheduling integration service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSchedulingIntegrationServiceLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostSchedulingIntegrationServiceLogRequest(); // PostSchedulingIntegrationServiceLogRequest |  (optional) 

            try
            {
                // Post scheduling integration service log.
                PostSchedulingIntegrationServiceLogResponse result = apiInstance.PostSchedulingIntegrationServiceLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostSchedulingIntegrationServiceLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSchedulingIntegrationServiceLogRequest**](PostSchedulingIntegrationServiceLogRequest.md)|  | [optional] 

### Return type

[**PostSchedulingIntegrationServiceLogResponse**](PostSchedulingIntegrationServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttankeringapicalculationlogasync"></a>
# **PostTankeringApiCalculationLogAsync**
> PostTankeringApiCalculationLogResponse PostTankeringApiCalculationLogAsync (PostTankeringApiCalculationLogRequest body)

Post tankering api calculation log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTankeringApiCalculationLogAsyncExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new PostTankeringApiCalculationLogRequest(); // PostTankeringApiCalculationLogRequest |  (optional) 

            try
            {
                // Post tankering api calculation log.
                PostTankeringApiCalculationLogResponse result = apiInstance.PostTankeringApiCalculationLogAsync(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.PostTankeringApiCalculationLogAsync: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTankeringApiCalculationLogRequest**](PostTankeringApiCalculationLogRequest.md)|  | [optional] 

### Return type

[**PostTankeringApiCalculationLogResponse**](PostTankeringApiCalculationLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecompanyaircraftchangelog"></a>
# **UpdateCompanyAircraftChangeLog**
> UpdateCompanyAircraftChangeLogResponse UpdateCompanyAircraftChangeLog (UpdateCompanyAircraftChangeLogRequest body)

Update the company aircraft change log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCompanyAircraftChangeLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateCompanyAircraftChangeLogRequest(); // UpdateCompanyAircraftChangeLogRequest |  (optional) 

            try
            {
                // Update the company aircraft change log.
                UpdateCompanyAircraftChangeLogResponse result = apiInstance.UpdateCompanyAircraftChangeLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateCompanyAircraftChangeLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCompanyAircraftChangeLogRequest**](UpdateCompanyAircraftChangeLogRequest.md)|  | [optional] 

### Return type

[**UpdateCompanyAircraftChangeLogResponse**](UpdateCompanyAircraftChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecompanyfbochangelog"></a>
# **UpdateCompanyFboChangeLog**
> UpdateCompanyFboChangeLogResponse UpdateCompanyFboChangeLog (UpdateCompanyFboChangeLogRequest body)

Update the company fbo change log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCompanyFboChangeLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateCompanyFboChangeLogRequest(); // UpdateCompanyFboChangeLogRequest |  (optional) 

            try
            {
                // Update the company fbo change log.
                UpdateCompanyFboChangeLogResponse result = apiInstance.UpdateCompanyFboChangeLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateCompanyFboChangeLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCompanyFboChangeLogRequest**](UpdateCompanyFboChangeLogRequest.md)|  | [optional] 

### Return type

[**UpdateCompanyFboChangeLogResponse**](UpdateCompanyFboChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecompanyfuelerchangelog"></a>
# **UpdateCompanyFuelerChangeLog**
> UpdateCompanyFuelerChangeLogResponse UpdateCompanyFuelerChangeLog (UpdateCompanyFuelerChangeLogRequest body)

Update the company fueler change log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCompanyFuelerChangeLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateCompanyFuelerChangeLogRequest(); // UpdateCompanyFuelerChangeLogRequest |  (optional) 

            try
            {
                // Update the company fueler change log.
                UpdateCompanyFuelerChangeLogResponse result = apiInstance.UpdateCompanyFuelerChangeLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateCompanyFuelerChangeLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCompanyFuelerChangeLogRequest**](UpdateCompanyFuelerChangeLogRequest.md)|  | [optional] 

### Return type

[**UpdateCompanyFuelerChangeLogResponse**](UpdateCompanyFuelerChangeLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatedispatchemaillog"></a>
# **UpdateDispatchEmailLog**
> UpdateDispatchEmailLogResponse UpdateDispatchEmailLog (UpdateDispatchEmailLogRequest body)

Update the dispatch email log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateDispatchEmailLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateDispatchEmailLogRequest(); // UpdateDispatchEmailLogRequest |  (optional) 

            try
            {
                // Update the dispatch email log.
                UpdateDispatchEmailLogResponse result = apiInstance.UpdateDispatchEmailLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateDispatchEmailLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateDispatchEmailLogRequest**](UpdateDispatchEmailLogRequest.md)|  | [optional] 

### Return type

[**UpdateDispatchEmailLogResponse**](UpdateDispatchEmailLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatefuelorderservicelog"></a>
# **UpdateFuelOrderServiceLog**
> UpdateFuelOrderServiceLogResponse UpdateFuelOrderServiceLog (UpdateFuelOrderServiceLogRequest body)

Update the fuel order service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFuelOrderServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateFuelOrderServiceLogRequest(); // UpdateFuelOrderServiceLogRequest |  (optional) 

            try
            {
                // Update the fuel order service log.
                UpdateFuelOrderServiceLogResponse result = apiInstance.UpdateFuelOrderServiceLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateFuelOrderServiceLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateFuelOrderServiceLogRequest**](UpdateFuelOrderServiceLogRequest.md)|  | [optional] 

### Return type

[**UpdateFuelOrderServiceLogResponse**](UpdateFuelOrderServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatefuelpriceservicelog"></a>
# **UpdateFuelPriceServiceLog**
> UpdateFuelPriceServiceLogResponse UpdateFuelPriceServiceLog (UpdateFuelPriceServiceLogRequest body)

Update the fuel price service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFuelPriceServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateFuelPriceServiceLogRequest(); // UpdateFuelPriceServiceLogRequest |  (optional) 

            try
            {
                // Update the fuel price service log.
                UpdateFuelPriceServiceLogResponse result = apiInstance.UpdateFuelPriceServiceLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateFuelPriceServiceLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateFuelPriceServiceLogRequest**](UpdateFuelPriceServiceLogRequest.md)|  | [optional] 

### Return type

[**UpdateFuelPriceServiceLogResponse**](UpdateFuelPriceServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateiflightplannerrouteservicelog"></a>
# **UpdateIFlightPlannerRouteServiceLog**
> UpdateIFlightPlannerRouteRequestServiceLogResponse UpdateIFlightPlannerRouteServiceLog (UpdateIFlightPlannerRouteRequestServiceLogRequest body)

Update the iFlightPlanner route request service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateIFlightPlannerRouteServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateIFlightPlannerRouteRequestServiceLogRequest(); // UpdateIFlightPlannerRouteRequestServiceLogRequest |  (optional) 

            try
            {
                // Update the iFlightPlanner route request service log.
                UpdateIFlightPlannerRouteRequestServiceLogResponse result = apiInstance.UpdateIFlightPlannerRouteServiceLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateIFlightPlannerRouteServiceLog: " + e.Message );
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

<a name="updateschedulingintegrationdispatchservicelog"></a>
# **UpdateSchedulingIntegrationDispatchServiceLog**
> UpdateSchedulingIntegrationDispatchServiceLogResponse UpdateSchedulingIntegrationDispatchServiceLog (UpdateSchedulingIntegrationDispatchServiceLogRequest body)

Update the scheduling integration dispatch service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSchedulingIntegrationDispatchServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateSchedulingIntegrationDispatchServiceLogRequest(); // UpdateSchedulingIntegrationDispatchServiceLogRequest |  (optional) 

            try
            {
                // Update the scheduling integration dispatch service log.
                UpdateSchedulingIntegrationDispatchServiceLogResponse result = apiInstance.UpdateSchedulingIntegrationDispatchServiceLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateSchedulingIntegrationDispatchServiceLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSchedulingIntegrationDispatchServiceLogRequest**](UpdateSchedulingIntegrationDispatchServiceLogRequest.md)|  | [optional] 

### Return type

[**UpdateSchedulingIntegrationDispatchServiceLogResponse**](UpdateSchedulingIntegrationDispatchServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateschedulingintegrationservicelog"></a>
# **UpdateSchedulingIntegrationServiceLog**
> UpdateSchedulingIntegrationServiceLogResponse UpdateSchedulingIntegrationServiceLog (UpdateSchedulingIntegrationServiceLogRequest body)

Update the scheduling integration service log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSchedulingIntegrationServiceLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateSchedulingIntegrationServiceLogRequest(); // UpdateSchedulingIntegrationServiceLogRequest |  (optional) 

            try
            {
                // Update the scheduling integration service log.
                UpdateSchedulingIntegrationServiceLogResponse result = apiInstance.UpdateSchedulingIntegrationServiceLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateSchedulingIntegrationServiceLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSchedulingIntegrationServiceLogRequest**](UpdateSchedulingIntegrationServiceLogRequest.md)|  | [optional] 

### Return type

[**UpdateSchedulingIntegrationServiceLogResponse**](UpdateSchedulingIntegrationServiceLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetankeringapicalculationlog"></a>
# **UpdateTankeringApiCalculationLog**
> UpdateTankeringApiCalculationLogResponse UpdateTankeringApiCalculationLog (UpdateTankeringApiCalculationLogRequest body)

Update the tankering api calculation log.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTankeringApiCalculationLogExample
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

            var apiInstance = new ServiceLogsApi();
            var body = new UpdateTankeringApiCalculationLogRequest(); // UpdateTankeringApiCalculationLogRequest |  (optional) 

            try
            {
                // Update the tankering api calculation log.
                UpdateTankeringApiCalculationLogResponse result = apiInstance.UpdateTankeringApiCalculationLog(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ServiceLogsApi.UpdateTankeringApiCalculationLog: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTankeringApiCalculationLogRequest**](UpdateTankeringApiCalculationLogRequest.md)|  | [optional] 

### Return type

[**UpdateTankeringApiCalculationLogResponse**](UpdateTankeringApiCalculationLogResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


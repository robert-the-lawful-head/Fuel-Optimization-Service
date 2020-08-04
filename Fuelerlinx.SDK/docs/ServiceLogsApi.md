# IO.Swagger.Api.ServiceLogsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCompanyAircraftChangeLog**](ServiceLogsApi.md#deletecompanyaircraftchangelog) | **DELETE** /api/ServiceLogs/companyAircraftChangeLog/{id} | 
[**DeleteCompanyFuelerChangeLog**](ServiceLogsApi.md#deletecompanyfuelerchangelog) | **DELETE** /api/ServiceLogs/companyFuelerChangeLog/{id} | 
[**DeleteDispatchEmailLog**](ServiceLogsApi.md#deletedispatchemaillog) | **DELETE** /api/ServiceLogs/dispatchEmailLog/{id} | 
[**DeleteFuelOrderServiceLog**](ServiceLogsApi.md#deletefuelorderservicelog) | **DELETE** /api/ServiceLogs/fuelOrderServiceLog/{id} | 
[**DeleteFuelPriceServiceLog**](ServiceLogsApi.md#deletefuelpriceservicelog) | **DELETE** /api/ServiceLogs/fuelPriceServiceLog/{id} | 
[**DeleteIFlightPlannerRouteServiceLog**](ServiceLogsApi.md#deleteiflightplannerrouteservicelog) | **DELETE** /api/ServiceLogs/iFlightPlannerRouteRequestServiceLog/{id} | 
[**DeleteSchedulingIntegrationDispatchServiceLog**](ServiceLogsApi.md#deleteschedulingintegrationdispatchservicelog) | **DELETE** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog/{id} | 
[**DeleteSchedulingIntegrationServiceLog**](ServiceLogsApi.md#deleteschedulingintegrationservicelog) | **DELETE** /api/ServiceLogs/schedulingIntegrationServiceLog/{id} | 
[**DeleteTankeringApiCalculationLog**](ServiceLogsApi.md#deletetankeringapicalculationlog) | **DELETE** /api/ServiceLogs/tankeringApiCalculationLog/{id} | 
[**GetCompanyAircraftChangeLogByTailNumber**](ServiceLogsApi.md#getcompanyaircraftchangelogbytailnumber) | **GET** /api/ServiceLogs/companyAircraftChangeLog/by-tailNumber/{tailNumber} | Fetch company aircraft change log by tailNumber.
[**GetCompanyAircraftChangeLogByUserId**](ServiceLogsApi.md#getcompanyaircraftchangelogbyuserid) | **GET** /api/ServiceLogs/companyAircraftChangeLog/by-userId/{userId} | Fetch company aircraft change log by userId.
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
[**PostCompanyAircraftChangeLogAsync**](ServiceLogsApi.md#postcompanyaircraftchangelogasync) | **POST** /api/ServiceLogs/companyAircraftChangeLog | 
[**PostCompanyFuelerChangeLogAsync**](ServiceLogsApi.md#postcompanyfuelerchangelogasync) | **POST** /api/ServiceLogs/companyFuelerChangeLog | 
[**PostDispatchEmailLogAsync**](ServiceLogsApi.md#postdispatchemaillogasync) | **POST** /api/ServiceLogs/dispatchEmailLog | 
[**PostFuelOrderServiceLogAsync**](ServiceLogsApi.md#postfuelorderservicelogasync) | **POST** /api/ServiceLogs/fuelOrderServiceLog | 
[**PostFuelPriceServiceLogAsync**](ServiceLogsApi.md#postfuelpriceservicelogasync) | **POST** /api/ServiceLogs/fuelPriceServiceLog | 
[**PostIFlightPlannerRouteServiceLogAsync**](ServiceLogsApi.md#postiflightplannerrouteservicelogasync) | **POST** /api/ServiceLogs/iFlightPlannerRouteRequestServiceLog | 
[**PostSchedulingIntegrationDispatchServiceLogAsync**](ServiceLogsApi.md#postschedulingintegrationdispatchservicelogasync) | **POST** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog | 
[**PostSchedulingIntegrationServiceLogAsync**](ServiceLogsApi.md#postschedulingintegrationservicelogasync) | **POST** /api/ServiceLogs/schedulingIntegrationServiceLog | 
[**PostTankeringApiCalculationLogAsync**](ServiceLogsApi.md#posttankeringapicalculationlogasync) | **POST** /api/ServiceLogs/tankeringApiCalculationLog | 
[**UpdateCompanyAircraftChangeLog**](ServiceLogsApi.md#updatecompanyaircraftchangelog) | **PUT** /api/ServiceLogs/companyAircraftChangeLog | 
[**UpdateCompanyFuelerChangeLog**](ServiceLogsApi.md#updatecompanyfuelerchangelog) | **PUT** /api/ServiceLogs/companyFuelerChangeLog | 
[**UpdateDispatchEmailLog**](ServiceLogsApi.md#updatedispatchemaillog) | **PUT** /api/ServiceLogs/dispatchEmailLog | 
[**UpdateFuelOrderServiceLog**](ServiceLogsApi.md#updatefuelorderservicelog) | **PUT** /api/ServiceLogs/fuelOrderServiceLog | 
[**UpdateFuelPriceServiceLog**](ServiceLogsApi.md#updatefuelpriceservicelog) | **PUT** /api/ServiceLogs/fuelPriceServiceLog | 
[**UpdateIFlightPlannerRouteServiceLog**](ServiceLogsApi.md#updateiflightplannerrouteservicelog) | **PUT** /api/ServiceLogs/iFlightPlannerRouteRequestServiceLog | 
[**UpdateSchedulingIntegrationDispatchServiceLog**](ServiceLogsApi.md#updateschedulingintegrationdispatchservicelog) | **PUT** /api/ServiceLogs/schedulingIntegrationDispatchServiceLog | 
[**UpdateSchedulingIntegrationServiceLog**](ServiceLogsApi.md#updateschedulingintegrationservicelog) | **PUT** /api/ServiceLogs/schedulingIntegrationServiceLog | 
[**UpdateTankeringApiCalculationLog**](ServiceLogsApi.md#updatetankeringapicalculationlog) | **PUT** /api/ServiceLogs/tankeringApiCalculationLog | 


<a name="deletecompanyaircraftchangelog"></a>
# **DeleteCompanyAircraftChangeLog**
> DeleteCompanyAircraftChangeLogResponse DeleteCompanyAircraftChangeLog (int? id)



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

<a name="deletecompanyfuelerchangelog"></a>
# **DeleteCompanyFuelerChangeLog**
> DeleteCompanyFuelerChangeLogResponse DeleteCompanyFuelerChangeLog (int? id)



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

<a name="postcompanyfuelerchangelogasync"></a>
# **PostCompanyFuelerChangeLogAsync**
> PostCompanyFuelerChangeLogResponse PostCompanyFuelerChangeLogAsync (PostCompanyFuelerChangeLogRequest body)



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

<a name="updatecompanyfuelerchangelog"></a>
# **UpdateCompanyFuelerChangeLog**
> UpdateCompanyFuelerChangeLogResponse UpdateCompanyFuelerChangeLog (UpdateCompanyFuelerChangeLogRequest body)



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


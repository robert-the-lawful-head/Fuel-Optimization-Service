# IO.Swagger.Api.FBOLinxApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetAircraftTailsGroupedByCompany**](FBOLinxApi.md#getaircrafttailsgroupedbycompany) | **GET** /api/FBOLinx/get-aircraft-tails-grouped-by-company | 
[**GetContractFuelOrders**](FBOLinxApi.md#getcontractfuelorders) | **POST** /api/FBOLinx/get-contract-orders | 
[**GetContractFuelVendorsTransactionsCount**](FBOLinxApi.md#getcontractfuelvendorstransactionscount) | **POST** /api/FBOLinx/get-contract-fuel-vendors-orders-count-at-airport | 
[**GetContractFuelVendorsTransactionsCountByAirports**](FBOLinxApi.md#getcontractfuelvendorstransactionscountbyairports) | **POST** /api/FBOLinx/get-contract-fuel-vendors-orders-counts-by-airports | 
[**GetCustomerFBOTransactionsCount**](FBOLinxApi.md#getcustomerfbotransactionscount) | **POST** /api/FBOLinx/get-customer-fbo-orders-count-at-airport | 
[**GetCustomerFuelVendors**](FBOLinxApi.md#getcustomerfuelvendors) | **GET** /api/FBOLinx/get-customer-fuel-vendors | 
[**GetCustomerTransactionsCount**](FBOLinxApi.md#getcustomertransactionscount) | **POST** /api/FBOLinx/get-customer-orders-count-at-airport | 
[**GetCustomerTransactionsCountForMultipleAirports**](FBOLinxApi.md#getcustomertransactionscountformultipleairports) | **POST** /api/FBOLinx/get-customer-orders-count-at-multiple-airports | 
[**GetFboTransactionsCount**](FBOLinxApi.md#getfbotransactionscount) | **POST** /api/FBOLinx/get-fbo-orders-count-at-airport | 
[**GetGroupFbosTransactionsCount**](FBOLinxApi.md#getgroupfbostransactionscount) | **POST** /api/FBOLinx/get-fbos-and-airports-orders-count | 
[**GetLatestPullHistoryFlightDepartmentForICAO**](FBOLinxApi.md#getlatestpullhistoryflightdepartmentforicao) | **POST** /api/FBOLinx/get-latest-pullhistory-flight-dept-by-icao | 
[**GetTransactionsCount**](FBOLinxApi.md#gettransactionscount) | **POST** /api/FBOLinx/get-orders-count-at-airport | 
[**GetTransactionsCountForNearbyAirports**](FBOLinxApi.md#gettransactionscountfornearbyairports) | **POST** /api/FBOLinx/get-nearby-airports | FBOLinx only - Fetch transactions associated with a particular airport and airports within X range of that airport.
[**GetTransactionsDirectOrdersCount**](FBOLinxApi.md#gettransactionsdirectorderscount) | **POST** /api/FBOLinx/get-direct-orders-count | 
[**UpdateFuelVendor**](FBOLinxApi.md#updatefuelvendor) | **POST** /api/FBOLinx/update-fuelvendor-emails | 


<a name="getaircrafttailsgroupedbycompany"></a>
# **GetAircraftTailsGroupedByCompany**
> FboLinxAircraftsResponse GetAircraftTailsGroupedByCompany ()



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAircraftTailsGroupedByCompanyExample
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

            try
            {
                FboLinxAircraftsResponse result = apiInstance.GetAircraftTailsGroupedByCompany();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetAircraftTailsGroupedByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**FboLinxAircraftsResponse**](FboLinxAircraftsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcontractfuelorders"></a>
# **GetContractFuelOrders**
> FBOLinxContractFuelOrdersResponse GetContractFuelOrders (FBOLinxOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetContractFuelOrdersExample
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
            var body = new FBOLinxOrdersRequest(); // FBOLinxOrdersRequest |  (optional) 

            try
            {
                FBOLinxContractFuelOrdersResponse result = apiInstance.GetContractFuelOrders(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetContractFuelOrders: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersRequest**](FBOLinxOrdersRequest.md)|  | [optional] 

### Return type

[**FBOLinxContractFuelOrdersResponse**](FBOLinxContractFuelOrdersResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcontractfuelvendorstransactionscount"></a>
# **GetContractFuelVendorsTransactionsCount**
> FboLinxContractFuelVendorsCountResponse GetContractFuelVendorsTransactionsCount (FBOLinxOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetContractFuelVendorsTransactionsCountExample
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
            var body = new FBOLinxOrdersRequest(); // FBOLinxOrdersRequest |  (optional) 

            try
            {
                FboLinxContractFuelVendorsCountResponse result = apiInstance.GetContractFuelVendorsTransactionsCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetContractFuelVendorsTransactionsCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersRequest**](FBOLinxOrdersRequest.md)|  | [optional] 

### Return type

[**FboLinxContractFuelVendorsCountResponse**](FboLinxContractFuelVendorsCountResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcontractfuelvendorstransactionscountbyairports"></a>
# **GetContractFuelVendorsTransactionsCountByAirports**
> FboLinxContractFuelVendorsCountsByAirportsResponse GetContractFuelVendorsTransactionsCountByAirports (FBOLinxOrdersForMultipleAirportsRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetContractFuelVendorsTransactionsCountByAirportsExample
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
            var body = new FBOLinxOrdersForMultipleAirportsRequest(); // FBOLinxOrdersForMultipleAirportsRequest |  (optional) 

            try
            {
                FboLinxContractFuelVendorsCountsByAirportsResponse result = apiInstance.GetContractFuelVendorsTransactionsCountByAirports(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetContractFuelVendorsTransactionsCountByAirports: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersForMultipleAirportsRequest**](FBOLinxOrdersForMultipleAirportsRequest.md)|  | [optional] 

### Return type

[**FboLinxContractFuelVendorsCountsByAirportsResponse**](FboLinxContractFuelVendorsCountsByAirportsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcustomerfbotransactionscount"></a>
# **GetCustomerFBOTransactionsCount**
> FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerFBOTransactionsCount (FBOLinxOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCustomerFBOTransactionsCountExample
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
            var body = new FBOLinxOrdersRequest(); // FBOLinxOrdersRequest |  (optional) 

            try
            {
                FboLinxCustomerTransactionsCountAtAirportResponse result = apiInstance.GetCustomerFBOTransactionsCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetCustomerFBOTransactionsCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersRequest**](FBOLinxOrdersRequest.md)|  | [optional] 

### Return type

[**FboLinxCustomerTransactionsCountAtAirportResponse**](FboLinxCustomerTransactionsCountAtAirportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcustomerfuelvendors"></a>
# **GetCustomerFuelVendors**
> FboLinxCustomerFuelVendorsResponse GetCustomerFuelVendors ()



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCustomerFuelVendorsExample
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

            try
            {
                FboLinxCustomerFuelVendorsResponse result = apiInstance.GetCustomerFuelVendors();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetCustomerFuelVendors: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**FboLinxCustomerFuelVendorsResponse**](FboLinxCustomerFuelVendorsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcustomertransactionscount"></a>
# **GetCustomerTransactionsCount**
> FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerTransactionsCount (FBOLinxOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCustomerTransactionsCountExample
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
            var body = new FBOLinxOrdersRequest(); // FBOLinxOrdersRequest |  (optional) 

            try
            {
                FboLinxCustomerTransactionsCountAtAirportResponse result = apiInstance.GetCustomerTransactionsCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetCustomerTransactionsCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersRequest**](FBOLinxOrdersRequest.md)|  | [optional] 

### Return type

[**FboLinxCustomerTransactionsCountAtAirportResponse**](FboLinxCustomerTransactionsCountAtAirportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcustomertransactionscountformultipleairports"></a>
# **GetCustomerTransactionsCountForMultipleAirports**
> FboLinxCustomerTransactionsCountAtAirportResponse GetCustomerTransactionsCountForMultipleAirports (FBOLinxOrdersForMultipleAirportsRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCustomerTransactionsCountForMultipleAirportsExample
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
            var body = new FBOLinxOrdersForMultipleAirportsRequest(); // FBOLinxOrdersForMultipleAirportsRequest |  (optional) 

            try
            {
                FboLinxCustomerTransactionsCountAtAirportResponse result = apiInstance.GetCustomerTransactionsCountForMultipleAirports(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetCustomerTransactionsCountForMultipleAirports: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersForMultipleAirportsRequest**](FBOLinxOrdersForMultipleAirportsRequest.md)|  | [optional] 

### Return type

[**FboLinxCustomerTransactionsCountAtAirportResponse**](FboLinxCustomerTransactionsCountAtAirportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getfbotransactionscount"></a>
# **GetFboTransactionsCount**
> FboLinxFbosTransactionsCountResponse GetFboTransactionsCount (FBOLinxOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetFboTransactionsCountExample
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
            var body = new FBOLinxOrdersRequest(); // FBOLinxOrdersRequest |  (optional) 

            try
            {
                FboLinxFbosTransactionsCountResponse result = apiInstance.GetFboTransactionsCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetFboTransactionsCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersRequest**](FBOLinxOrdersRequest.md)|  | [optional] 

### Return type

[**FboLinxFbosTransactionsCountResponse**](FboLinxFbosTransactionsCountResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getgroupfbostransactionscount"></a>
# **GetGroupFbosTransactionsCount**
> FBOLinxGroupOrdersResponse GetGroupFbosTransactionsCount (FBOLinxGroupOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetGroupFbosTransactionsCountExample
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
            var body = new FBOLinxGroupOrdersRequest(); // FBOLinxGroupOrdersRequest |  (optional) 

            try
            {
                FBOLinxGroupOrdersResponse result = apiInstance.GetGroupFbosTransactionsCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetGroupFbosTransactionsCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxGroupOrdersRequest**](FBOLinxGroupOrdersRequest.md)|  | [optional] 

### Return type

[**FBOLinxGroupOrdersResponse**](FBOLinxGroupOrdersResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getgroupfbostransactionscount"></a>
# **GetGroupFbosTransactionsCount**
> FBOLinxGroupOrdersResponse GetGroupFbosTransactionsCount (FBOLinxGroupOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetGroupFbosTransactionsCountExample
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
            var body = new FBOLinxGroupOrdersRequest(); // FBOLinxGroupOrdersRequest |  (optional) 

            try
            {
                FBOLinxGroupOrdersResponse result = apiInstance.GetGroupFbosTransactionsCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetGroupFbosTransactionsCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxGroupOrdersRequest**](FBOLinxGroupOrdersRequest.md)|  | [optional] 

### Return type

[**FBOLinxGroupOrdersResponse**](FBOLinxGroupOrdersResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getlatestpullhistoryflightdepartmentforicao"></a>
# **GetLatestPullHistoryFlightDepartmentForICAO**
> int? GetLatestPullHistoryFlightDepartmentForICAO (FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetLatestPullHistoryFlightDepartmentForICAOExample
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
            var body = new FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest(); // FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest |  (optional) 

            try
            {
                int? result = apiInstance.GetLatestPullHistoryFlightDepartmentForICAO(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetLatestPullHistoryFlightDepartmentForICAO: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest**](FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest.md)|  | [optional] 

### Return type

**int?**

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionscount"></a>
# **GetTransactionsCount**
> FBOLinxOrdersResponse GetTransactionsCount (FBOLinxOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionsCountExample
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
            var body = new FBOLinxOrdersRequest(); // FBOLinxOrdersRequest |  (optional) 

            try
            {
                FBOLinxOrdersResponse result = apiInstance.GetTransactionsCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetTransactionsCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersRequest**](FBOLinxOrdersRequest.md)|  | [optional] 

### Return type

[**FBOLinxOrdersResponse**](FBOLinxOrdersResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettransactionscountfornearbyairports"></a>
# **GetTransactionsCountForNearbyAirports**
> FBOLinxNearbyAirportsResponse GetTransactionsCountForNearbyAirports (FBOLinxNearbyAirportsRequest body)

FBOLinx only - Fetch transactions associated with a particular airport and airports within X range of that airport.

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
                // FBOLinx only - Fetch transactions associated with a particular airport and airports within X range of that airport.
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

<a name="gettransactionsdirectorderscount"></a>
# **GetTransactionsDirectOrdersCount**
> FBOLinxOrdersResponse GetTransactionsDirectOrdersCount (FBOLinxOrdersRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTransactionsDirectOrdersCountExample
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
            var body = new FBOLinxOrdersRequest(); // FBOLinxOrdersRequest |  (optional) 

            try
            {
                FBOLinxOrdersResponse result = apiInstance.GetTransactionsDirectOrdersCount(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.GetTransactionsDirectOrdersCount: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxOrdersRequest**](FBOLinxOrdersRequest.md)|  | [optional] 

### Return type

[**FBOLinxOrdersResponse**](FBOLinxOrdersResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatefuelvendor"></a>
# **UpdateFuelVendor**
> FBOLinxFuelVendorUpdateResponse UpdateFuelVendor (FBOLinxFuelVendorUpdateRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateFuelVendorExample
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
            var body = new FBOLinxFuelVendorUpdateRequest(); // FBOLinxFuelVendorUpdateRequest |  (optional) 

            try
            {
                FBOLinxFuelVendorUpdateResponse result = apiInstance.UpdateFuelVendor(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FBOLinxApi.UpdateFuelVendor: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FBOLinxFuelVendorUpdateRequest**](FBOLinxFuelVendorUpdateRequest.md)|  | [optional] 

### Return type

[**FBOLinxFuelVendorUpdateResponse**](FBOLinxFuelVendorUpdateResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


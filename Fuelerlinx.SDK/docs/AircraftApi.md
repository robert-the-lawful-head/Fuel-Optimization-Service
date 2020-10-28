# IO.Swagger.Api.AircraftApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AddAircraftForCompany**](AircraftApi.md#addaircraftforcompany) | **POST** /api/Aircraft/company | Add an aircraft to the authorized company.
[**AddAircraftForUser**](AircraftApi.md#addaircraftforuser) | **POST** /api/Aircraft/user | Add an existing aircraft to the user&#39;s account.
[**AddTail**](AircraftApi.md#addtail) | **POST** /api/Aircraft/tail | Add a new aircraft with a corresponding tail number to be used by a specific company/user.
[**DeleteUsereAircraftTankeringSettings**](AircraftApi.md#deleteusereaircrafttankeringsettings) | **DELETE** /api/Aircraft/tankering-settings/{id} | Delete tankering settings for an aircraft.
[**GetAircraftByAircraftId**](AircraftApi.md#getaircraftbyaircraftid) | **GET** /api/Aircraft/aircraftId/{aircraftId} | 
[**GetAircraftByTailNumber**](AircraftApi.md#getaircraftbytailnumber) | **GET** /api/Aircraft/company/{companyId}/tail/{tailNumber} | Get an aircraft by [companyId] and [tailNumber].
[**GetAircraftByTailNumberId**](AircraftApi.md#getaircraftbytailnumberid) | **GET** /api/Aircraft/tail/{tailNumberId} | Get an aircraft by the provided [tailNumberId].
[**GetAircraftDataForCompany**](AircraftApi.md#getaircraftdataforcompany) | **GET** /api/Aircraft/company | Get all aircraft assigned to the authorized company.
[**GetAircraftDataForCompanyByCompanyId**](AircraftApi.md#getaircraftdataforcompanybycompanyid) | **GET** /api/Aircraft/company/{companyId} | Internal use only - Get all aircraft assigned to the specified {companyId}.
[**GetAircraftForUser**](AircraftApi.md#getaircraftforuser) | **GET** /api/Aircraft/user | Get all aircraft assigned to the authorized user.
[**GetAmstatDataByTailNumber**](AircraftApi.md#getamstatdatabytailnumber) | **GET** /api/Aircraft/amstat/tail/{tailNumber} | Internal use only - Fetch details for an aircraft from the AMSTAT database.
[**GetIFlightPlannerAircraftId**](AircraftApi.md#getiflightplanneraircraftid) | **GET** /api/Aircraft/iflightplanner/aircraftid/{tailNumber} | Internal use only - Get the iFlightPlanner aircraft Id for a specific tail number.
[**GetIFlightPlannerModel**](AircraftApi.md#getiflightplannermodel) | **GET** /api/Aircraft/iflightplanner/model/{aircraftModelId} | Internal use only - Get a specific iFlightPlanner model by it&#39;s Id.
[**GetIFlightPlannerModelsByType**](AircraftApi.md#getiflightplannermodelsbytype) | **GET** /api/Aircraft/iflightplanner/modelsbytype/{aircraftType} | Internal use only - Get a list of iFlightPlanner aircraft models by the aircraft type (ICAO) code.
[**GetIFlightPlannerProfileByTailNumber**](AircraftApi.md#getiflightplannerprofilebytailnumber) | **GET** /api/Aircraft/iflightplanner/aircraftprofile/{tailNumber} | Internal use only - Fetch an aircraft profile from iFlightPlanner.
[**PostUserAircraftTankeringSettings**](AircraftApi.md#postuseraircrafttankeringsettings) | **POST** /api/Aircraft/tankering-settings | Add tankering settings for an aircraft.
[**RegisterAircraftWithIFlightPlanner**](AircraftApi.md#registeraircraftwithiflightplanner) | **POST** /api/Aircraft/iflightplanner/register | Internal use only - Register an aircraft with iFlightPlanner.
[**UpdateTail**](AircraftApi.md#updatetail) | **PUT** /api/Aircraft/tail/{tailNumberId} | Update an existing aircraft with the corresponding [tailNumberId].
[**UpdateUserAircraftTankeringSettings**](AircraftApi.md#updateuseraircrafttankeringsettings) | **PUT** /api/Aircraft/tankering-settings | Update tankering settings for an aircraft.


<a name="addaircraftforcompany"></a>
# **AddAircraftForCompany**
> UserAircraftResponse AddAircraftForCompany (AircraftDataDTO body)

Add an aircraft to the authorized company.

An AircraftData record will be inserted and then assigned to the authorized company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddAircraftForCompanyExample
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

            var apiInstance = new AircraftApi();
            var body = new AircraftDataDTO(); // AircraftDataDTO |  (optional) 

            try
            {
                // Add an aircraft to the authorized company.
                UserAircraftResponse result = apiInstance.AddAircraftForCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.AddAircraftForCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**AircraftDataDTO**](AircraftDataDTO.md)|  | [optional] 

### Return type

[**UserAircraftResponse**](UserAircraftResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="addaircraftforuser"></a>
# **AddAircraftForUser**
> UserAircraftResponse AddAircraftForUser (UserAircraftDTO body)

Add an existing aircraft to the user's account.

The [tailNumberId] is required and should be created by posting to the /tail API beforehand.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddAircraftForUserExample
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

            var apiInstance = new AircraftApi();
            var body = new UserAircraftDTO(); // UserAircraftDTO |  (optional) 

            try
            {
                // Add an existing aircraft to the user's account.
                UserAircraftResponse result = apiInstance.AddAircraftForUser(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.AddAircraftForUser: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UserAircraftDTO**](UserAircraftDTO.md)|  | [optional] 

### Return type

[**UserAircraftResponse**](UserAircraftResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="addtail"></a>
# **AddTail**
> AircraftDataResponse AddTail (AircraftDataDTO body)

Add a new aircraft with a corresponding tail number to be used by a specific company/user.

A tail number is required.  The tail number does not have to be unique.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddTailExample
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

            var apiInstance = new AircraftApi();
            var body = new AircraftDataDTO(); // AircraftDataDTO |  (optional) 

            try
            {
                // Add a new aircraft with a corresponding tail number to be used by a specific company/user.
                AircraftDataResponse result = apiInstance.AddTail(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.AddTail: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**AircraftDataDTO**](AircraftDataDTO.md)|  | [optional] 

### Return type

[**AircraftDataResponse**](AircraftDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteusereaircrafttankeringsettings"></a>
# **DeleteUsereAircraftTankeringSettings**
> DeleteUserAircraftTankeringSettingsResponse DeleteUsereAircraftTankeringSettings (int? id)

Delete tankering settings for an aircraft.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteUsereAircraftTankeringSettingsExample
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

            var apiInstance = new AircraftApi();
            var id = 56;  // int? | 

            try
            {
                // Delete tankering settings for an aircraft.
                DeleteUserAircraftTankeringSettingsResponse result = apiInstance.DeleteUsereAircraftTankeringSettings(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.DeleteUsereAircraftTankeringSettings: " + e.Message );
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

[**DeleteUserAircraftTankeringSettingsResponse**](DeleteUserAircraftTankeringSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaircraftbyaircraftid"></a>
# **GetAircraftByAircraftId**
> UserAircraftListResponse GetAircraftByAircraftId (int? aircraftId)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAircraftByAircraftIdExample
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

            var apiInstance = new AircraftApi();
            var aircraftId = 56;  // int? | 

            try
            {
                UserAircraftListResponse result = apiInstance.GetAircraftByAircraftId(aircraftId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetAircraftByAircraftId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **aircraftId** | **int?**|  | 

### Return type

[**UserAircraftListResponse**](UserAircraftListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaircraftbytailnumber"></a>
# **GetAircraftByTailNumber**
> AircraftDataResponse GetAircraftByTailNumber (int? companyId, string tailNumber)

Get an aircraft by [companyId] and [tailNumber].

The request will fail if the authorized user is not part of the company that the record is attached to.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAircraftByTailNumberExample
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

            var apiInstance = new AircraftApi();
            var companyId = 56;  // int? | 
            var tailNumber = tailNumber_example;  // string | 

            try
            {
                // Get an aircraft by [companyId] and [tailNumber].
                AircraftDataResponse result = apiInstance.GetAircraftByTailNumber(companyId, tailNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetAircraftByTailNumber: " + e.Message );
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

### Return type

[**AircraftDataResponse**](AircraftDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaircraftbytailnumberid"></a>
# **GetAircraftByTailNumberId**
> AircraftDataResponse GetAircraftByTailNumberId (int? tailNumberId)

Get an aircraft by the provided [tailNumberId].

The request will fail if the authorized user is not part of the company that the record is attached to.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAircraftByTailNumberIdExample
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

            var apiInstance = new AircraftApi();
            var tailNumberId = 56;  // int? | 

            try
            {
                // Get an aircraft by the provided [tailNumberId].
                AircraftDataResponse result = apiInstance.GetAircraftByTailNumberId(tailNumberId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetAircraftByTailNumberId: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **tailNumberId** | **int?**|  | 

### Return type

[**AircraftDataResponse**](AircraftDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaircraftdataforcompany"></a>
# **GetAircraftDataForCompany**
> AircraftDataListResponse GetAircraftDataForCompany ()

Get all aircraft assigned to the authorized company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAircraftDataForCompanyExample
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

            var apiInstance = new AircraftApi();

            try
            {
                // Get all aircraft assigned to the authorized company.
                AircraftDataListResponse result = apiInstance.GetAircraftDataForCompany();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetAircraftDataForCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**AircraftDataListResponse**](AircraftDataListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaircraftdataforcompanybycompanyid"></a>
# **GetAircraftDataForCompanyByCompanyId**
> AircraftDataListResponse GetAircraftDataForCompanyByCompanyId (int? companyId)

Internal use only - Get all aircraft assigned to the specified {companyId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAircraftDataForCompanyByCompanyIdExample
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

            var apiInstance = new AircraftApi();
            var companyId = 56;  // int? | 

            try
            {
                // Internal use only - Get all aircraft assigned to the specified {companyId}.
                AircraftDataListResponse result = apiInstance.GetAircraftDataForCompanyByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetAircraftDataForCompanyByCompanyId: " + e.Message );
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

[**AircraftDataListResponse**](AircraftDataListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getaircraftforuser"></a>
# **GetAircraftForUser**
> UserAircraftListResponse GetAircraftForUser ()

Get all aircraft assigned to the authorized user.

Each record contains an [AircraftData] property where the actual aircraft information can be found.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAircraftForUserExample
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

            var apiInstance = new AircraftApi();

            try
            {
                // Get all aircraft assigned to the authorized user.
                UserAircraftListResponse result = apiInstance.GetAircraftForUser();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetAircraftForUser: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**UserAircraftListResponse**](UserAircraftListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getamstatdatabytailnumber"></a>
# **GetAmstatDataByTailNumber**
> AircraftResponse GetAmstatDataByTailNumber (string tailNumber)

Internal use only - Fetch details for an aircraft from the AMSTAT database.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAmstatDataByTailNumberExample
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

            var apiInstance = new AircraftApi();
            var tailNumber = tailNumber_example;  // string | 

            try
            {
                // Internal use only - Fetch details for an aircraft from the AMSTAT database.
                AircraftResponse result = apiInstance.GetAmstatDataByTailNumber(tailNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetAmstatDataByTailNumber: " + e.Message );
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

[**AircraftResponse**](AircraftResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getiflightplanneraircraftid"></a>
# **GetIFlightPlannerAircraftId**
> IFlightPlannerAircraftIdResponse GetIFlightPlannerAircraftId (string tailNumber)

Internal use only - Get the iFlightPlanner aircraft Id for a specific tail number.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIFlightPlannerAircraftIdExample
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

            var apiInstance = new AircraftApi();
            var tailNumber = tailNumber_example;  // string | 

            try
            {
                // Internal use only - Get the iFlightPlanner aircraft Id for a specific tail number.
                IFlightPlannerAircraftIdResponse result = apiInstance.GetIFlightPlannerAircraftId(tailNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetIFlightPlannerAircraftId: " + e.Message );
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

[**IFlightPlannerAircraftIdResponse**](IFlightPlannerAircraftIdResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getiflightplannermodel"></a>
# **GetIFlightPlannerModel**
> IFLightPlannerModelResponse GetIFlightPlannerModel (int? aircraftModelId)

Internal use only - Get a specific iFlightPlanner model by it's Id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIFlightPlannerModelExample
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

            var apiInstance = new AircraftApi();
            var aircraftModelId = 56;  // int? | 

            try
            {
                // Internal use only - Get a specific iFlightPlanner model by it's Id.
                IFLightPlannerModelResponse result = apiInstance.GetIFlightPlannerModel(aircraftModelId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetIFlightPlannerModel: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **aircraftModelId** | **int?**|  | 

### Return type

[**IFLightPlannerModelResponse**](IFLightPlannerModelResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getiflightplannermodelsbytype"></a>
# **GetIFlightPlannerModelsByType**
> IFlightPlannerModelsByTypeResponse GetIFlightPlannerModelsByType (string aircraftType)

Internal use only - Get a list of iFlightPlanner aircraft models by the aircraft type (ICAO) code.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIFlightPlannerModelsByTypeExample
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

            var apiInstance = new AircraftApi();
            var aircraftType = aircraftType_example;  // string | 

            try
            {
                // Internal use only - Get a list of iFlightPlanner aircraft models by the aircraft type (ICAO) code.
                IFlightPlannerModelsByTypeResponse result = apiInstance.GetIFlightPlannerModelsByType(aircraftType);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetIFlightPlannerModelsByType: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **aircraftType** | **string**|  | 

### Return type

[**IFlightPlannerModelsByTypeResponse**](IFlightPlannerModelsByTypeResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getiflightplannerprofilebytailnumber"></a>
# **GetIFlightPlannerProfileByTailNumber**
> IFlightPlannerAircraftProfileResponse GetIFlightPlannerProfileByTailNumber (string tailNumber)

Internal use only - Fetch an aircraft profile from iFlightPlanner.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIFlightPlannerProfileByTailNumberExample
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

            var apiInstance = new AircraftApi();
            var tailNumber = tailNumber_example;  // string | 

            try
            {
                // Internal use only - Fetch an aircraft profile from iFlightPlanner.
                IFlightPlannerAircraftProfileResponse result = apiInstance.GetIFlightPlannerProfileByTailNumber(tailNumber);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.GetIFlightPlannerProfileByTailNumber: " + e.Message );
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

[**IFlightPlannerAircraftProfileResponse**](IFlightPlannerAircraftProfileResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postuseraircrafttankeringsettings"></a>
# **PostUserAircraftTankeringSettings**
> PostUserAircraftTankeringSettingsResponse PostUserAircraftTankeringSettings (PostUserAircraftTankeringSettingsRequest body)

Add tankering settings for an aircraft.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostUserAircraftTankeringSettingsExample
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

            var apiInstance = new AircraftApi();
            var body = new PostUserAircraftTankeringSettingsRequest(); // PostUserAircraftTankeringSettingsRequest |  (optional) 

            try
            {
                // Add tankering settings for an aircraft.
                PostUserAircraftTankeringSettingsResponse result = apiInstance.PostUserAircraftTankeringSettings(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.PostUserAircraftTankeringSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostUserAircraftTankeringSettingsRequest**](PostUserAircraftTankeringSettingsRequest.md)|  | [optional] 

### Return type

[**PostUserAircraftTankeringSettingsResponse**](PostUserAircraftTankeringSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="registeraircraftwithiflightplanner"></a>
# **RegisterAircraftWithIFlightPlanner**
> IFlightPlannerAircraftRegistrationResponse RegisterAircraftWithIFlightPlanner (IFlightPlannerAircraftRegistrationRequest body)

Internal use only - Register an aircraft with iFlightPlanner.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class RegisterAircraftWithIFlightPlannerExample
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

            var apiInstance = new AircraftApi();
            var body = new IFlightPlannerAircraftRegistrationRequest(); // IFlightPlannerAircraftRegistrationRequest |  (optional) 

            try
            {
                // Internal use only - Register an aircraft with iFlightPlanner.
                IFlightPlannerAircraftRegistrationResponse result = apiInstance.RegisterAircraftWithIFlightPlanner(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.RegisterAircraftWithIFlightPlanner: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**IFlightPlannerAircraftRegistrationRequest**](IFlightPlannerAircraftRegistrationRequest.md)|  | [optional] 

### Return type

[**IFlightPlannerAircraftRegistrationResponse**](IFlightPlannerAircraftRegistrationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetail"></a>
# **UpdateTail**
> AircraftDataResponse UpdateTail (int? tailNumberId, AircraftDataDTO body)

Update an existing aircraft with the corresponding [tailNumberId].

A tail number is required.  The tail number does not have to be unique.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTailExample
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

            var apiInstance = new AircraftApi();
            var tailNumberId = 56;  // int? | 
            var body = new AircraftDataDTO(); // AircraftDataDTO |  (optional) 

            try
            {
                // Update an existing aircraft with the corresponding [tailNumberId].
                AircraftDataResponse result = apiInstance.UpdateTail(tailNumberId, body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.UpdateTail: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **tailNumberId** | **int?**|  | 
 **body** | [**AircraftDataDTO**](AircraftDataDTO.md)|  | [optional] 

### Return type

[**AircraftDataResponse**](AircraftDataResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateuseraircrafttankeringsettings"></a>
# **UpdateUserAircraftTankeringSettings**
> UpdateUserAircraftTankeringSettingsResponse UpdateUserAircraftTankeringSettings (UpdateUserAircraftTankeringSettingsRequest body)

Update tankering settings for an aircraft.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateUserAircraftTankeringSettingsExample
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

            var apiInstance = new AircraftApi();
            var body = new UpdateUserAircraftTankeringSettingsRequest(); // UpdateUserAircraftTankeringSettingsRequest |  (optional) 

            try
            {
                // Update tankering settings for an aircraft.
                UpdateUserAircraftTankeringSettingsResponse result = apiInstance.UpdateUserAircraftTankeringSettings(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AircraftApi.UpdateUserAircraftTankeringSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateUserAircraftTankeringSettingsRequest**](UpdateUserAircraftTankeringSettingsRequest.md)|  | [optional] 

### Return type

[**UpdateUserAircraftTankeringSettingsResponse**](UpdateUserAircraftTankeringSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


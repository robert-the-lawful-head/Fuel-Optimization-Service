# IO.Swagger.Api.RampFeeApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AddRampFeeByCompany**](RampFeeApi.md#addrampfeebycompany) | **POST** /api/RampFee/company-specific | Add a company-specific ramp fee.
[**DeleteRampFeeByCompany**](RampFeeApi.md#deleterampfeebycompany) | **DELETE** /api/RampFee/company-specific/{id} | Delete a company-specific ramp fee.
[**DeleteRampFeeByCompanyNote**](RampFeeApi.md#deleterampfeebycompanynote) | **DELETE** /api/RampFee/company-specific/{id}/notes/{noteId} | Delete a company-specific note for a ramp fee.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking.
[**GetCrowdSourcedRampFeeByScenario**](RampFeeApi.md#getcrowdsourcedrampfeebyscenario) | **GET** /api/RampFee/crowd-sourced/tail/{tailNumber}/airport/{icao}/fbo/{fboName} | Fetch a crowd-sourced ramp fee pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName].
[**GetCrowdSourcedRampFeeByScenarioList**](RampFeeApi.md#getcrowdsourcedrampfeebyscenariolist) | **GET** /api/RampFee/crowd-sourced/tail/{tailNumber}/airport/{icao}/fbo/{fboName}/list | Fetch all crowd-sourced ramp fees pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName].
[**GetRampFeeByCompany**](RampFeeApi.md#getrampfeebycompany) | **GET** /api/RampFee/company-specific/{id} | Get a company-specific ramp fee by it&#39;s [id].
[**GetRampFeeByCompanyByScenario**](RampFeeApi.md#getrampfeebycompanybyscenario) | **GET** /api/RampFee/company-specific/tail/{tailNumber}/airport/{icao}/fbo/{fboName} | Fetch a company-specific ramp fee based on the provided {tailNumber}, {airportIdentifier}, and {fbo}.
[**GetRampFeeByCompanyNotes**](RampFeeApi.md#getrampfeebycompanynotes) | **GET** /api/RampFee/company-specific/{id}/notes | Fetch all company-specific notes for the specified [rampFeeByCompanyId].
[**GetRampFeesByCompanyForAirport**](RampFeeApi.md#getrampfeesbycompanyforairport) | **GET** /api/RampFee/company-specific/by-airport/{icao} | Get a list of company-specific ramp fees at the the provided [icao].
[**GetRampFeesByCompanyForLocation**](RampFeeApi.md#getrampfeesbycompanyforlocation) | **GET** /api/RampFee/company-specific/by-location/{icao}/{fboName} | Get a list of company-specific ramp fees at the the provided [icao] and [fboName].
[**PostRampFeeByCompanyNotes**](RampFeeApi.md#postrampfeebycompanynotes) | **POST** /api/RampFee/company-specific/notes | Add a company-specific note to a ramp fee.
[**UpdateRampFeeByCompany**](RampFeeApi.md#updaterampfeebycompany) | **PUT** /api/RampFee/company-specific | Update a company-specific ramp fee.
[**UpdateRampFeeByCompanyNotes**](RampFeeApi.md#updaterampfeebycompanynotes) | **PUT** /api/RampFee/company-specific/notes | Update a company-specific note for a ramp fee.


<a name="addrampfeebycompany"></a>
# **AddRampFeeByCompany**
> RampFeeByCompanyResponse AddRampFeeByCompany (PostRampFeeByCompanyRequest body)

Add a company-specific ramp fee.

If the ramp fee already exists, please use the [PUT] http method.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddRampFeeByCompanyExample
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

            var apiInstance = new RampFeeApi();
            var body = new PostRampFeeByCompanyRequest(); // PostRampFeeByCompanyRequest |  (optional) 

            try
            {
                // Add a company-specific ramp fee.
                RampFeeByCompanyResponse result = apiInstance.AddRampFeeByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.AddRampFeeByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostRampFeeByCompanyRequest**](PostRampFeeByCompanyRequest.md)|  | [optional] 

### Return type

[**RampFeeByCompanyResponse**](RampFeeByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleterampfeebycompany"></a>
# **DeleteRampFeeByCompany**
> DeleteRampFeeByCompanyResponse DeleteRampFeeByCompany (int? id)

Delete a company-specific ramp fee.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteRampFeeByCompanyExample
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

            var apiInstance = new RampFeeApi();
            var id = 56;  // int? | 

            try
            {
                // Delete a company-specific ramp fee.
                DeleteRampFeeByCompanyResponse result = apiInstance.DeleteRampFeeByCompany(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.DeleteRampFeeByCompany: " + e.Message );
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

[**DeleteRampFeeByCompanyResponse**](DeleteRampFeeByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleterampfeebycompanynote"></a>
# **DeleteRampFeeByCompanyNote**
> DeleteRampFeeByCompanyNotesResponse DeleteRampFeeByCompanyNote (int? id, int? noteId)

Delete a company-specific note for a ramp fee.  The note will be changed to a \"deleted\" state but will not be removed from the database to allow for change-tracking.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteRampFeeByCompanyNoteExample
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

            var apiInstance = new RampFeeApi();
            var id = 56;  // int? | 
            var noteId = 56;  // int? | 

            try
            {
                // Delete a company-specific note for a ramp fee.  The note will be changed to a \"deleted\" state but will not be removed from the database to allow for change-tracking.
                DeleteRampFeeByCompanyNotesResponse result = apiInstance.DeleteRampFeeByCompanyNote(id, noteId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.DeleteRampFeeByCompanyNote: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 
 **noteId** | **int?**|  | 

### Return type

[**DeleteRampFeeByCompanyNotesResponse**](DeleteRampFeeByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcrowdsourcedrampfeebyscenario"></a>
# **GetCrowdSourcedRampFeeByScenario**
> CrowdSourcedRampFeeResponse GetCrowdSourcedRampFeeByScenario (string tailNumber, string icao, string fboName)

Fetch a crowd-sourced ramp fee pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCrowdSourcedRampFeeByScenarioExample
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

            var apiInstance = new RampFeeApi();
            var tailNumber = tailNumber_example;  // string | 
            var icao = icao_example;  // string | 
            var fboName = fboName_example;  // string | 

            try
            {
                // Fetch a crowd-sourced ramp fee pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName].
                CrowdSourcedRampFeeResponse result = apiInstance.GetCrowdSourcedRampFeeByScenario(tailNumber, icao, fboName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.GetCrowdSourcedRampFeeByScenario: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **tailNumber** | **string**|  | 
 **icao** | **string**|  | 
 **fboName** | **string**|  | 

### Return type

[**CrowdSourcedRampFeeResponse**](CrowdSourcedRampFeeResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcrowdsourcedrampfeebyscenariolist"></a>
# **GetCrowdSourcedRampFeeByScenarioList**
> CrowdSourcedRampFeeListResponse GetCrowdSourcedRampFeeByScenarioList (string tailNumber, string icao, string fboName)

Fetch all crowd-sourced ramp fees pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCrowdSourcedRampFeeByScenarioListExample
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

            var apiInstance = new RampFeeApi();
            var tailNumber = tailNumber_example;  // string | 
            var icao = icao_example;  // string | 
            var fboName = fboName_example;  // string | 

            try
            {
                // Fetch all crowd-sourced ramp fees pulled from various sources for the provided [tailNumber], [airportIdentifier], and [fboName].
                CrowdSourcedRampFeeListResponse result = apiInstance.GetCrowdSourcedRampFeeByScenarioList(tailNumber, icao, fboName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.GetCrowdSourcedRampFeeByScenarioList: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **tailNumber** | **string**|  | 
 **icao** | **string**|  | 
 **fboName** | **string**|  | 

### Return type

[**CrowdSourcedRampFeeListResponse**](CrowdSourcedRampFeeListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrampfeebycompany"></a>
# **GetRampFeeByCompany**
> RampFeeByCompanyResponse GetRampFeeByCompany (int? id)

Get a company-specific ramp fee by it's [id].

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
    public class GetRampFeeByCompanyExample
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

            var apiInstance = new RampFeeApi();
            var id = 56;  // int? | 

            try
            {
                // Get a company-specific ramp fee by it's [id].
                RampFeeByCompanyResponse result = apiInstance.GetRampFeeByCompany(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.GetRampFeeByCompany: " + e.Message );
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

[**RampFeeByCompanyResponse**](RampFeeByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrampfeebycompanybyscenario"></a>
# **GetRampFeeByCompanyByScenario**
> RampFeeByCompanyResponse GetRampFeeByCompanyByScenario (string tailNumber, string icao, string fboName)

Fetch a company-specific ramp fee based on the provided {tailNumber}, {airportIdentifier}, and {fbo}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetRampFeeByCompanyByScenarioExample
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

            var apiInstance = new RampFeeApi();
            var tailNumber = tailNumber_example;  // string | 
            var icao = icao_example;  // string | 
            var fboName = fboName_example;  // string | 

            try
            {
                // Fetch a company-specific ramp fee based on the provided {tailNumber}, {airportIdentifier}, and {fbo}.
                RampFeeByCompanyResponse result = apiInstance.GetRampFeeByCompanyByScenario(tailNumber, icao, fboName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.GetRampFeeByCompanyByScenario: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **tailNumber** | **string**|  | 
 **icao** | **string**|  | 
 **fboName** | **string**|  | 

### Return type

[**RampFeeByCompanyResponse**](RampFeeByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrampfeebycompanynotes"></a>
# **GetRampFeeByCompanyNotes**
> RampFeeByCompanyNotesResponse GetRampFeeByCompanyNotes (int? id)

Fetch all company-specific notes for the specified [rampFeeByCompanyId].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetRampFeeByCompanyNotesExample
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

            var apiInstance = new RampFeeApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch all company-specific notes for the specified [rampFeeByCompanyId].
                RampFeeByCompanyNotesResponse result = apiInstance.GetRampFeeByCompanyNotes(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.GetRampFeeByCompanyNotes: " + e.Message );
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

[**RampFeeByCompanyNotesResponse**](RampFeeByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrampfeesbycompanyforairport"></a>
# **GetRampFeesByCompanyForAirport**
> RampFeeByCompanyListResponse GetRampFeesByCompanyForAirport (string icao)

Get a list of company-specific ramp fees at the the provided [icao].

The returned ramp fees will be for all FBOs at that [icao].  Ramp fees will be categorized by size, weight range, tail, etc.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetRampFeesByCompanyForAirportExample
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

            var apiInstance = new RampFeeApi();
            var icao = icao_example;  // string | 

            try
            {
                // Get a list of company-specific ramp fees at the the provided [icao].
                RampFeeByCompanyListResponse result = apiInstance.GetRampFeesByCompanyForAirport(icao);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.GetRampFeesByCompanyForAirport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **icao** | **string**|  | 

### Return type

[**RampFeeByCompanyListResponse**](RampFeeByCompanyListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getrampfeesbycompanyforlocation"></a>
# **GetRampFeesByCompanyForLocation**
> RampFeeByCompanyListResponse GetRampFeesByCompanyForLocation (string icao, string fboName)

Get a list of company-specific ramp fees at the the provided [icao] and [fboName].

The returned ramp fees will be for the specific [fboName] at that [icao].  Ramp fees will be categorized by size, weight range, tail, etc.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetRampFeesByCompanyForLocationExample
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

            var apiInstance = new RampFeeApi();
            var icao = icao_example;  // string | 
            var fboName = fboName_example;  // string | 

            try
            {
                // Get a list of company-specific ramp fees at the the provided [icao] and [fboName].
                RampFeeByCompanyListResponse result = apiInstance.GetRampFeesByCompanyForLocation(icao, fboName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.GetRampFeesByCompanyForLocation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **icao** | **string**|  | 
 **fboName** | **string**|  | 

### Return type

[**RampFeeByCompanyListResponse**](RampFeeByCompanyListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postrampfeebycompanynotes"></a>
# **PostRampFeeByCompanyNotes**
> PostRampFeeByCompanyNotesResponse PostRampFeeByCompanyNotes (PostRampFeeByCompanyNotesRequest body)

Add a company-specific note to a ramp fee.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostRampFeeByCompanyNotesExample
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

            var apiInstance = new RampFeeApi();
            var body = new PostRampFeeByCompanyNotesRequest(); // PostRampFeeByCompanyNotesRequest |  (optional) 

            try
            {
                // Add a company-specific note to a ramp fee.
                PostRampFeeByCompanyNotesResponse result = apiInstance.PostRampFeeByCompanyNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.PostRampFeeByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostRampFeeByCompanyNotesRequest**](PostRampFeeByCompanyNotesRequest.md)|  | [optional] 

### Return type

[**PostRampFeeByCompanyNotesResponse**](PostRampFeeByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updaterampfeebycompany"></a>
# **UpdateRampFeeByCompany**
> UpdateRampFeeByCompanyResponse UpdateRampFeeByCompany (UpdateRampFeeByCompanyRequest body)

Update a company-specific ramp fee.

If this is a new ramp fee, please use the [POST] http method.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateRampFeeByCompanyExample
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

            var apiInstance = new RampFeeApi();
            var body = new UpdateRampFeeByCompanyRequest(); // UpdateRampFeeByCompanyRequest |  (optional) 

            try
            {
                // Update a company-specific ramp fee.
                UpdateRampFeeByCompanyResponse result = apiInstance.UpdateRampFeeByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.UpdateRampFeeByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateRampFeeByCompanyRequest**](UpdateRampFeeByCompanyRequest.md)|  | [optional] 

### Return type

[**UpdateRampFeeByCompanyResponse**](UpdateRampFeeByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updaterampfeebycompanynotes"></a>
# **UpdateRampFeeByCompanyNotes**
> UpdateRampFeeByCompanyNotesResponse UpdateRampFeeByCompanyNotes (UpdateRampFeeByCompanyNotesRequest body)

Update a company-specific note for a ramp fee.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateRampFeeByCompanyNotesExample
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

            var apiInstance = new RampFeeApi();
            var body = new UpdateRampFeeByCompanyNotesRequest(); // UpdateRampFeeByCompanyNotesRequest |  (optional) 

            try
            {
                // Update a company-specific note for a ramp fee.
                UpdateRampFeeByCompanyNotesResponse result = apiInstance.UpdateRampFeeByCompanyNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling RampFeeApi.UpdateRampFeeByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateRampFeeByCompanyNotesRequest**](UpdateRampFeeByCompanyNotesRequest.md)|  | [optional] 

### Return type

[**UpdateRampFeeByCompanyNotesResponse**](UpdateRampFeeByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


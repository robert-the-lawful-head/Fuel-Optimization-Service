# IO.Swagger.Api.AirportApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteAirportDetailsByCompany**](AirportApi.md#deleteairportdetailsbycompany) | **DELETE** /api/Airport/company-specific-details/{airportDetailsByCompanyId} | Delete the company-specific details of an airport based on the provided {airportDetailsByCompanyId}.
[**DeleteAirportDetailsByCompanyNotes**](AirportApi.md#deleteairportdetailsbycompanynotes) | **DELETE** /api/Airport/company-specific-details/{airportDetailsByCompanyId}/notes/{noteId} | Delete a company-specific note for a particular airport.  The note will be changed to a \&quot;deleted\&quot; state but will not be removed from the database to allow for change-tracking.
[**GetAcukwikAirport**](AirportApi.md#getacukwikairport) | **GET** /api/Airport/airport/{airportIdentifier} | Fetch information specifically for the airport with the designated {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.
[**GetAirportDetailsByCompany**](AirportApi.md#getairportdetailsbycompany) | **GET** /api/Airport/company-specific-details/{airportDetailsByCompanyId} | Fetch company-specific details for an airport based on the provided {airportDetailsByCompanyId} of the record.  These details are unique for each flight department.
[**GetAirportDetailsByCompanyByIcao**](AirportApi.md#getairportdetailsbycompanybyicao) | **GET** /api/Airport/company-specific-details/icao/{airportIdentifier} | Fetch company-specific details for an airport based on the provided {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.  These details are unique for each flight department.
[**GetAirportDetailsByCompanyNotes**](AirportApi.md#getairportdetailsbycompanynotes) | **GET** /api/Airport/company-specific-details/{airportDetailsByCompanyId}/notes | Fetch the company-specific notes for a particular airport based on the provided {airportDetailsByCompanyId}.
[**GetAllAirports**](AirportApi.md#getallairports) | **GET** /api/Airport/airports | Internal use only - Fetch all airports in the Acukwik database.  This will included FBO/Handler information as well.
[**GetDistinctAirportCountries**](AirportApi.md#getdistinctairportcountries) | **GET** /api/Airport/countries/distinct | 
[**GetGeneralAirportInfoList**](AirportApi.md#getgeneralairportinfolist) | **GET** /api/Airport/general-info/list | 
[**PostAirportDetailsByCompany**](AirportApi.md#postairportdetailsbycompany) | **POST** /api/Airport/company-specific-details | Add a new record for company-specific details of an airport.  These details are unique for each flight department.
[**PostAirportDetailsByCompanyNotes**](AirportApi.md#postairportdetailsbycompanynotes) | **POST** /api/Airport/company-specific-details/notes | Add a new company-specific note for an airport.
[**UpdateAirportDetailsByCompany**](AirportApi.md#updateairportdetailsbycompany) | **PUT** /api/Airport/company-specific-details | Update the company-specific details of an airport.  These details are unique for each flight department.
[**UpdateAirportDetailsByCompanyNotes**](AirportApi.md#updateairportdetailsbycompanynotes) | **PUT** /api/Airport/company-specific-details/notes | Update an existing company-specific note for an airport.


<a name="deleteairportdetailsbycompany"></a>
# **DeleteAirportDetailsByCompany**
> DeleteAirportDetailsByCompanyResponse DeleteAirportDetailsByCompany (int? airportDetailsByCompanyId)

Delete the company-specific details of an airport based on the provided {airportDetailsByCompanyId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteAirportDetailsByCompanyExample
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

            var apiInstance = new AirportApi();
            var airportDetailsByCompanyId = 56;  // int? | 

            try
            {
                // Delete the company-specific details of an airport based on the provided {airportDetailsByCompanyId}.
                DeleteAirportDetailsByCompanyResponse result = apiInstance.DeleteAirportDetailsByCompany(airportDetailsByCompanyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.DeleteAirportDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **airportDetailsByCompanyId** | **int?**|  | 

### Return type

[**DeleteAirportDetailsByCompanyResponse**](DeleteAirportDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteairportdetailsbycompanynotes"></a>
# **DeleteAirportDetailsByCompanyNotes**
> DeleteAirportDetailsByCompanyNoteResponse DeleteAirportDetailsByCompanyNotes (int? airportDetailsByCompanyId, int? noteId)

Delete a company-specific note for a particular airport.  The note will be changed to a \"deleted\" state but will not be removed from the database to allow for change-tracking.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteAirportDetailsByCompanyNotesExample
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

            var apiInstance = new AirportApi();
            var airportDetailsByCompanyId = 56;  // int? | 
            var noteId = 56;  // int? | 

            try
            {
                // Delete a company-specific note for a particular airport.  The note will be changed to a \"deleted\" state but will not be removed from the database to allow for change-tracking.
                DeleteAirportDetailsByCompanyNoteResponse result = apiInstance.DeleteAirportDetailsByCompanyNotes(airportDetailsByCompanyId, noteId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.DeleteAirportDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **airportDetailsByCompanyId** | **int?**|  | 
 **noteId** | **int?**|  | 

### Return type

[**DeleteAirportDetailsByCompanyNoteResponse**](DeleteAirportDetailsByCompanyNoteResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getacukwikairport"></a>
# **GetAcukwikAirport**
> AcukwikAirportResponse GetAcukwikAirport (string airportIdentifier)

Fetch information specifically for the airport with the designated {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAcukwikAirportExample
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

            var apiInstance = new AirportApi();
            var airportIdentifier = airportIdentifier_example;  // string | 

            try
            {
                // Fetch information specifically for the airport with the designated {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.
                AcukwikAirportResponse result = apiInstance.GetAcukwikAirport(airportIdentifier);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.GetAcukwikAirport: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **airportIdentifier** | **string**|  | 

### Return type

[**AcukwikAirportResponse**](AcukwikAirportResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getairportdetailsbycompany"></a>
# **GetAirportDetailsByCompany**
> AirportDetailsByCompanyResponse GetAirportDetailsByCompany (int? airportDetailsByCompanyId)

Fetch company-specific details for an airport based on the provided {airportDetailsByCompanyId} of the record.  These details are unique for each flight department.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAirportDetailsByCompanyExample
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

            var apiInstance = new AirportApi();
            var airportDetailsByCompanyId = 56;  // int? | 

            try
            {
                // Fetch company-specific details for an airport based on the provided {airportDetailsByCompanyId} of the record.  These details are unique for each flight department.
                AirportDetailsByCompanyResponse result = apiInstance.GetAirportDetailsByCompany(airportDetailsByCompanyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.GetAirportDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **airportDetailsByCompanyId** | **int?**|  | 

### Return type

[**AirportDetailsByCompanyResponse**](AirportDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getairportdetailsbycompanybyicao"></a>
# **GetAirportDetailsByCompanyByIcao**
> AirportDetailsByCompanyResponse GetAirportDetailsByCompanyByIcao (string airportIdentifier)

Fetch company-specific details for an airport based on the provided {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.  These details are unique for each flight department.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAirportDetailsByCompanyByIcaoExample
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

            var apiInstance = new AirportApi();
            var airportIdentifier = airportIdentifier_example;  // string | 

            try
            {
                // Fetch company-specific details for an airport based on the provided {airportIdentifier}.  The identifier should be the ICAO, IATA, or FAA ID of the airport.  These details are unique for each flight department.
                AirportDetailsByCompanyResponse result = apiInstance.GetAirportDetailsByCompanyByIcao(airportIdentifier);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.GetAirportDetailsByCompanyByIcao: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **airportIdentifier** | **string**|  | 

### Return type

[**AirportDetailsByCompanyResponse**](AirportDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getairportdetailsbycompanynotes"></a>
# **GetAirportDetailsByCompanyNotes**
> AirportDetailsByCompanyNotesResponse GetAirportDetailsByCompanyNotes (int? airportDetailsByCompanyId)

Fetch the company-specific notes for a particular airport based on the provided {airportDetailsByCompanyId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAirportDetailsByCompanyNotesExample
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

            var apiInstance = new AirportApi();
            var airportDetailsByCompanyId = 56;  // int? | 

            try
            {
                // Fetch the company-specific notes for a particular airport based on the provided {airportDetailsByCompanyId}.
                AirportDetailsByCompanyNotesResponse result = apiInstance.GetAirportDetailsByCompanyNotes(airportDetailsByCompanyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.GetAirportDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **airportDetailsByCompanyId** | **int?**|  | 

### Return type

[**AirportDetailsByCompanyNotesResponse**](AirportDetailsByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getallairports"></a>
# **GetAllAirports**
> List<AcukwikAirportDTO> GetAllAirports ()

Internal use only - Fetch all airports in the Acukwik database.  This will included FBO/Handler information as well.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAllAirportsExample
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

            var apiInstance = new AirportApi();

            try
            {
                // Internal use only - Fetch all airports in the Acukwik database.  This will included FBO/Handler information as well.
                List&lt;AcukwikAirportDTO&gt; result = apiInstance.GetAllAirports();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.GetAllAirports: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<AcukwikAirportDTO>**](AcukwikAirportDTO.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getdistinctairportcountries"></a>
# **GetDistinctAirportCountries**
> DistinctCountryListResponse GetDistinctAirportCountries ()



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetDistinctAirportCountriesExample
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

            var apiInstance = new AirportApi();

            try
            {
                DistinctCountryListResponse result = apiInstance.GetDistinctAirportCountries();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.GetDistinctAirportCountries: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**DistinctCountryListResponse**](DistinctCountryListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getgeneralairportinfolist"></a>
# **GetGeneralAirportInfoList**
> List<GeneralAirportInformation> GetGeneralAirportInfoList ()



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetGeneralAirportInfoListExample
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

            var apiInstance = new AirportApi();

            try
            {
                List&lt;GeneralAirportInformation&gt; result = apiInstance.GetGeneralAirportInfoList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.GetGeneralAirportInfoList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**List<GeneralAirportInformation>**](GeneralAirportInformation.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postairportdetailsbycompany"></a>
# **PostAirportDetailsByCompany**
> PostAirportDetailsByCompanyResponse PostAirportDetailsByCompany (PostAirportDetailsByCompanyRequest body)

Add a new record for company-specific details of an airport.  These details are unique for each flight department.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostAirportDetailsByCompanyExample
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

            var apiInstance = new AirportApi();
            var body = new PostAirportDetailsByCompanyRequest(); // PostAirportDetailsByCompanyRequest |  (optional) 

            try
            {
                // Add a new record for company-specific details of an airport.  These details are unique for each flight department.
                PostAirportDetailsByCompanyResponse result = apiInstance.PostAirportDetailsByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.PostAirportDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostAirportDetailsByCompanyRequest**](PostAirportDetailsByCompanyRequest.md)|  | [optional] 

### Return type

[**PostAirportDetailsByCompanyResponse**](PostAirportDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postairportdetailsbycompanynotes"></a>
# **PostAirportDetailsByCompanyNotes**
> PostAirportDetailsByCompanyNotesResponse PostAirportDetailsByCompanyNotes (PostAirportDetailsByCompanyNotesRequest body)

Add a new company-specific note for an airport.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostAirportDetailsByCompanyNotesExample
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

            var apiInstance = new AirportApi();
            var body = new PostAirportDetailsByCompanyNotesRequest(); // PostAirportDetailsByCompanyNotesRequest |  (optional) 

            try
            {
                // Add a new company-specific note for an airport.
                PostAirportDetailsByCompanyNotesResponse result = apiInstance.PostAirportDetailsByCompanyNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.PostAirportDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostAirportDetailsByCompanyNotesRequest**](PostAirportDetailsByCompanyNotesRequest.md)|  | [optional] 

### Return type

[**PostAirportDetailsByCompanyNotesResponse**](PostAirportDetailsByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateairportdetailsbycompany"></a>
# **UpdateAirportDetailsByCompany**
> UpdateAirportDetailsByCompanyResponse UpdateAirportDetailsByCompany (UpdateAirportDetailsByCompanyRequest body)

Update the company-specific details of an airport.  These details are unique for each flight department.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateAirportDetailsByCompanyExample
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

            var apiInstance = new AirportApi();
            var body = new UpdateAirportDetailsByCompanyRequest(); // UpdateAirportDetailsByCompanyRequest |  (optional) 

            try
            {
                // Update the company-specific details of an airport.  These details are unique for each flight department.
                UpdateAirportDetailsByCompanyResponse result = apiInstance.UpdateAirportDetailsByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.UpdateAirportDetailsByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateAirportDetailsByCompanyRequest**](UpdateAirportDetailsByCompanyRequest.md)|  | [optional] 

### Return type

[**UpdateAirportDetailsByCompanyResponse**](UpdateAirportDetailsByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateairportdetailsbycompanynotes"></a>
# **UpdateAirportDetailsByCompanyNotes**
> UpdateAirportDetailsByCompanyNotesResponse UpdateAirportDetailsByCompanyNotes (UpdateAirportDetailsByCompanyNotesRequest body)

Update an existing company-specific note for an airport.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateAirportDetailsByCompanyNotesExample
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

            var apiInstance = new AirportApi();
            var body = new UpdateAirportDetailsByCompanyNotesRequest(); // UpdateAirportDetailsByCompanyNotesRequest |  (optional) 

            try
            {
                // Update an existing company-specific note for an airport.
                UpdateAirportDetailsByCompanyNotesResponse result = apiInstance.UpdateAirportDetailsByCompanyNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AirportApi.UpdateAirportDetailsByCompanyNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateAirportDetailsByCompanyNotesRequest**](UpdateAirportDetailsByCompanyNotesRequest.md)|  | [optional] 

### Return type

[**UpdateAirportDetailsByCompanyNotesResponse**](UpdateAirportDetailsByCompanyNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


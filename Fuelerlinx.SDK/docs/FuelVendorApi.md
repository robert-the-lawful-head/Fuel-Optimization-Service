# IO.Swagger.Api.FuelVendorApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCompanyFueler**](FuelVendorApi.md#deletecompanyfueler) | **DELETE** /api/FuelVendor/company-specific/{companyFuelerId} | Delete the company-specific details of a fuel vendor based on the provided {companyFuelerId}.
[**DeleteCompanyFuelerNotes**](FuelVendorApi.md#deletecompanyfuelernotes) | **DELETE** /api/FuelVendor/company-specific/{companyFuelerId}/notes/{noteId} | Delete a company-specific note for the provided {companyFuelerId} record.
[**DeleteCompanyFuelerSettings**](FuelVendorApi.md#deletecompanyfuelersettings) | **DELETE** /api/FuelVendor/company-specific/{companyFuelerId}/settings/{settingsId} | Delete a company-specific settings record for a fuel vendor.
[**GetCompanyFuelerByFuelerId**](FuelVendorApi.md#getcompanyfuelerbyfuelerid) | **GET** /api/FuelVendor/company-specific/by-fueler/{fuelVendorId} | Fetch a company-specific record tied to the fuel vendor for the provided {fuelVendorId}.
[**GetCompanyFuelerById**](FuelVendorApi.md#getcompanyfuelerbyid) | **GET** /api/FuelVendor/company-specific/{companyFuelerId} | Fetch a company-specific fuel vendor record for the provided {companyFuelerId}.
[**GetCompanyFuelerList**](FuelVendorApi.md#getcompanyfuelerlist) | **GET** /api/FuelVendor/company-specific/list | Fetch all company-specific records for the authenticated company.
[**GetCompanyFuelerNotes**](FuelVendorApi.md#getcompanyfuelernotes) | **GET** /api/FuelVendor/company-specific/{companyFuelerId}/notes | Fetch the company-specific notes for a particular fuel vendor based on the provided {companyFuelerId}.
[**GetCompanyFuelerSettings**](FuelVendorApi.md#getcompanyfuelersettings) | **GET** /api/FuelVendor/company-specific/{companyFuelerId}/settings | Fetch the company-specific settings for the specified {companyFuelerId} record.
[**PostCompanyFueler**](FuelVendorApi.md#postcompanyfueler) | **POST** /api/FuelVendor/company-specific | Add a company-specific record for a fuel vendor.  These details are unique for each flight department.
[**PostCompanyFuelerNotes**](FuelVendorApi.md#postcompanyfuelernotes) | **POST** /api/FuelVendor/company-specific/notes | Add a new company-specific note for a fuel vendor.
[**PostCompanyFuelerSettings**](FuelVendorApi.md#postcompanyfuelersettings) | **POST** /api/FuelVendor/company-specific/settings | Add a company-specific settings record for a fuel vendor.
[**UpdateCompanyFueler**](FuelVendorApi.md#updatecompanyfueler) | **PUT** /api/FuelVendor/company-specific | Update the company-specific details of a fuel vendor.  These details are unique for each flight department.
[**UpdateCompanyFuelerNotes**](FuelVendorApi.md#updatecompanyfuelernotes) | **PUT** /api/FuelVendor/company-specific/notes | Update an existing company-specific note for a fuel vendor.
[**UpdateCompanyFuelerSettings**](FuelVendorApi.md#updatecompanyfuelersettings) | **PUT** /api/FuelVendor/company-specific/settings | Update a company-specific settings record for a fuel vendor.


<a name="deletecompanyfueler"></a>
# **DeleteCompanyFueler**
> DeleteCompanyFuelerResponse DeleteCompanyFueler (int? companyFuelerId)

Delete the company-specific details of a fuel vendor based on the provided {companyFuelerId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyFuelerExample
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

            var apiInstance = new FuelVendorApi();
            var companyFuelerId = 56;  // int? | 

            try
            {
                // Delete the company-specific details of a fuel vendor based on the provided {companyFuelerId}.
                DeleteCompanyFuelerResponse result = apiInstance.DeleteCompanyFueler(companyFuelerId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.DeleteCompanyFueler: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyFuelerId** | **int?**|  | 

### Return type

[**DeleteCompanyFuelerResponse**](DeleteCompanyFuelerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletecompanyfuelernotes"></a>
# **DeleteCompanyFuelerNotes**
> DeleteCompanyFuelerNotesResponse DeleteCompanyFuelerNotes (int? companyFuelerId, int? noteId)

Delete a company-specific note for the provided {companyFuelerId} record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyFuelerNotesExample
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

            var apiInstance = new FuelVendorApi();
            var companyFuelerId = 56;  // int? | 
            var noteId = 56;  // int? | 

            try
            {
                // Delete a company-specific note for the provided {companyFuelerId} record.
                DeleteCompanyFuelerNotesResponse result = apiInstance.DeleteCompanyFuelerNotes(companyFuelerId, noteId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.DeleteCompanyFuelerNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyFuelerId** | **int?**|  | 
 **noteId** | **int?**|  | 

### Return type

[**DeleteCompanyFuelerNotesResponse**](DeleteCompanyFuelerNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletecompanyfuelersettings"></a>
# **DeleteCompanyFuelerSettings**
> DeleteCompanyFuelerSettingsResponse DeleteCompanyFuelerSettings (int? companyFuelerId, int? settingsId)

Delete a company-specific settings record for a fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyFuelerSettingsExample
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

            var apiInstance = new FuelVendorApi();
            var companyFuelerId = 56;  // int? | 
            var settingsId = 56;  // int? | 

            try
            {
                // Delete a company-specific settings record for a fuel vendor.
                DeleteCompanyFuelerSettingsResponse result = apiInstance.DeleteCompanyFuelerSettings(companyFuelerId, settingsId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.DeleteCompanyFuelerSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyFuelerId** | **int?**|  | 
 **settingsId** | **int?**|  | 

### Return type

[**DeleteCompanyFuelerSettingsResponse**](DeleteCompanyFuelerSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfuelerbyfuelerid"></a>
# **GetCompanyFuelerByFuelerId**
> CompanyFuelerResponse GetCompanyFuelerByFuelerId (int? fuelVendorId)

Fetch a company-specific record tied to the fuel vendor for the provided {fuelVendorId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerByFuelerIdExample
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

            var apiInstance = new FuelVendorApi();
            var fuelVendorId = 56;  // int? | 

            try
            {
                // Fetch a company-specific record tied to the fuel vendor for the provided {fuelVendorId}.
                CompanyFuelerResponse result = apiInstance.GetCompanyFuelerByFuelerId(fuelVendorId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.GetCompanyFuelerByFuelerId: " + e.Message );
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

[**CompanyFuelerResponse**](CompanyFuelerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfuelerbyid"></a>
# **GetCompanyFuelerById**
> CompanyFuelerResponse GetCompanyFuelerById (int? companyFuelerId)

Fetch a company-specific fuel vendor record for the provided {companyFuelerId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerByIdExample
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

            var apiInstance = new FuelVendorApi();
            var companyFuelerId = 56;  // int? | 

            try
            {
                // Fetch a company-specific fuel vendor record for the provided {companyFuelerId}.
                CompanyFuelerResponse result = apiInstance.GetCompanyFuelerById(companyFuelerId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.GetCompanyFuelerById: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyFuelerId** | **int?**|  | 

### Return type

[**CompanyFuelerResponse**](CompanyFuelerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfuelerlist"></a>
# **GetCompanyFuelerList**
> CompanyFuelerListResponse GetCompanyFuelerList ()

Fetch all company-specific records for the authenticated company.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerListExample
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

            var apiInstance = new FuelVendorApi();

            try
            {
                // Fetch all company-specific records for the authenticated company.
                CompanyFuelerListResponse result = apiInstance.GetCompanyFuelerList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.GetCompanyFuelerList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**CompanyFuelerListResponse**](CompanyFuelerListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfuelernotes"></a>
# **GetCompanyFuelerNotes**
> CompanyFuelerNotesResponse GetCompanyFuelerNotes (int? companyFuelerId)

Fetch the company-specific notes for a particular fuel vendor based on the provided {companyFuelerId}.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerNotesExample
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

            var apiInstance = new FuelVendorApi();
            var companyFuelerId = 56;  // int? | 

            try
            {
                // Fetch the company-specific notes for a particular fuel vendor based on the provided {companyFuelerId}.
                CompanyFuelerNotesResponse result = apiInstance.GetCompanyFuelerNotes(companyFuelerId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.GetCompanyFuelerNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyFuelerId** | **int?**|  | 

### Return type

[**CompanyFuelerNotesResponse**](CompanyFuelerNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyfuelersettings"></a>
# **GetCompanyFuelerSettings**
> CompanyFuelerSettingsResponse GetCompanyFuelerSettings (int? companyFuelerId)

Fetch the company-specific settings for the specified {companyFuelerId} record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerSettingsExample
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

            var apiInstance = new FuelVendorApi();
            var companyFuelerId = 56;  // int? | 

            try
            {
                // Fetch the company-specific settings for the specified {companyFuelerId} record.
                CompanyFuelerSettingsResponse result = apiInstance.GetCompanyFuelerSettings(companyFuelerId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.GetCompanyFuelerSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyFuelerId** | **int?**|  | 

### Return type

[**CompanyFuelerSettingsResponse**](CompanyFuelerSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcompanyfueler"></a>
# **PostCompanyFueler**
> PostCompanyFuelerResponse PostCompanyFueler (PostCompanyFuelerRequest body)

Add a company-specific record for a fuel vendor.  These details are unique for each flight department.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyFuelerExample
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

            var apiInstance = new FuelVendorApi();
            var body = new PostCompanyFuelerRequest(); // PostCompanyFuelerRequest |  (optional) 

            try
            {
                // Add a company-specific record for a fuel vendor.  These details are unique for each flight department.
                PostCompanyFuelerResponse result = apiInstance.PostCompanyFueler(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.PostCompanyFueler: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyFuelerRequest**](PostCompanyFuelerRequest.md)|  | [optional] 

### Return type

[**PostCompanyFuelerResponse**](PostCompanyFuelerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcompanyfuelernotes"></a>
# **PostCompanyFuelerNotes**
> PostCompanyFuelerNotesResponse PostCompanyFuelerNotes (PostCompanyFuelerNotesRequest body)

Add a new company-specific note for a fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyFuelerNotesExample
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

            var apiInstance = new FuelVendorApi();
            var body = new PostCompanyFuelerNotesRequest(); // PostCompanyFuelerNotesRequest |  (optional) 

            try
            {
                // Add a new company-specific note for a fuel vendor.
                PostCompanyFuelerNotesResponse result = apiInstance.PostCompanyFuelerNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.PostCompanyFuelerNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyFuelerNotesRequest**](PostCompanyFuelerNotesRequest.md)|  | [optional] 

### Return type

[**PostCompanyFuelerNotesResponse**](PostCompanyFuelerNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcompanyfuelersettings"></a>
# **PostCompanyFuelerSettings**
> PostCompanyFuelerSettingsResponse PostCompanyFuelerSettings (PostCompanyFuelerSettingsRequest body)

Add a company-specific settings record for a fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyFuelerSettingsExample
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

            var apiInstance = new FuelVendorApi();
            var body = new PostCompanyFuelerSettingsRequest(); // PostCompanyFuelerSettingsRequest |  (optional) 

            try
            {
                // Add a company-specific settings record for a fuel vendor.
                PostCompanyFuelerSettingsResponse result = apiInstance.PostCompanyFuelerSettings(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.PostCompanyFuelerSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyFuelerSettingsRequest**](PostCompanyFuelerSettingsRequest.md)|  | [optional] 

### Return type

[**PostCompanyFuelerSettingsResponse**](PostCompanyFuelerSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecompanyfueler"></a>
# **UpdateCompanyFueler**
> UpdateCompanyFuelerResponse UpdateCompanyFueler (UpdateCompanyFuelerRequest body)

Update the company-specific details of a fuel vendor.  These details are unique for each flight department.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCompanyFuelerExample
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

            var apiInstance = new FuelVendorApi();
            var body = new UpdateCompanyFuelerRequest(); // UpdateCompanyFuelerRequest |  (optional) 

            try
            {
                // Update the company-specific details of a fuel vendor.  These details are unique for each flight department.
                UpdateCompanyFuelerResponse result = apiInstance.UpdateCompanyFueler(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.UpdateCompanyFueler: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCompanyFuelerRequest**](UpdateCompanyFuelerRequest.md)|  | [optional] 

### Return type

[**UpdateCompanyFuelerResponse**](UpdateCompanyFuelerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecompanyfuelernotes"></a>
# **UpdateCompanyFuelerNotes**
> UpdateCompanyFuelerNotesResponse UpdateCompanyFuelerNotes (UpdateCompanyFuelerNotesRequest body)

Update an existing company-specific note for a fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCompanyFuelerNotesExample
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

            var apiInstance = new FuelVendorApi();
            var body = new UpdateCompanyFuelerNotesRequest(); // UpdateCompanyFuelerNotesRequest |  (optional) 

            try
            {
                // Update an existing company-specific note for a fuel vendor.
                UpdateCompanyFuelerNotesResponse result = apiInstance.UpdateCompanyFuelerNotes(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.UpdateCompanyFuelerNotes: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCompanyFuelerNotesRequest**](UpdateCompanyFuelerNotesRequest.md)|  | [optional] 

### Return type

[**UpdateCompanyFuelerNotesResponse**](UpdateCompanyFuelerNotesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecompanyfuelersettings"></a>
# **UpdateCompanyFuelerSettings**
> UpdateCompanyFuelerSettingsResponse UpdateCompanyFuelerSettings (UpdateCompanyFuelerSettingsRequest body)

Update a company-specific settings record for a fuel vendor.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCompanyFuelerSettingsExample
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

            var apiInstance = new FuelVendorApi();
            var body = new UpdateCompanyFuelerSettingsRequest(); // UpdateCompanyFuelerSettingsRequest |  (optional) 

            try
            {
                // Update a company-specific settings record for a fuel vendor.
                UpdateCompanyFuelerSettingsResponse result = apiInstance.UpdateCompanyFuelerSettings(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.UpdateCompanyFuelerSettings: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCompanyFuelerSettingsRequest**](UpdateCompanyFuelerSettingsRequest.md)|  | [optional] 

### Return type

[**UpdateCompanyFuelerSettingsResponse**](UpdateCompanyFuelerSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


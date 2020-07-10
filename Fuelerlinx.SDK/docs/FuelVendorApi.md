# IO.Swagger.Api.FuelVendorApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCompanyFueler**](FuelVendorApi.md#deletecompanyfueler) | **DELETE** /api/FuelVendor/company-specific/{companyFuelerId} | Delete the company-specific details of a fuel vendor based on the provided {companyFuelerId}.
[**DeleteCompanyFuelerNotes**](FuelVendorApi.md#deletecompanyfuelernotes) | **DELETE** /api/FuelVendor/company-specific/{companyFuelerId}/notes/{noteId} | Delete a company-specific note for the provided {companyFuelerId} record.
[**DeleteCompanyFuelerPriceAdjustment**](FuelVendorApi.md#deletecompanyfuelerpriceadjustment) | **DELETE** /api/FuelVendor/company-specific/price-adjustment/{id} | Delete a price adjustment for a company fueler.
[**DeleteCompanyFuelerSettings**](FuelVendorApi.md#deletecompanyfuelersettings) | **DELETE** /api/FuelVendor/company-specific/{companyFuelerId}/settings/{settingsId} | Delete a company-specific settings record for a fuel vendor.
[**DeletePaymentInformationByFuelVendorId**](FuelVendorApi.md#deletepaymentinformationbyfuelvendorid) | **DELETE** /api/FuelVendor/{id}/delete-payment-info | Delete fuel vendor payment information by fuel vendor id
[**GetCompanyFuelerByFuelerId**](FuelVendorApi.md#getcompanyfuelerbyfuelerid) | **GET** /api/FuelVendor/company-specific/by-fueler/{fuelVendorId} | Fetch a company-specific record tied to the fuel vendor for the provided {fuelVendorId}.
[**GetCompanyFuelerById**](FuelVendorApi.md#getcompanyfuelerbyid) | **GET** /api/FuelVendor/company-specific/{companyFuelerId} | Fetch a company-specific fuel vendor record for the provided {companyFuelerId}.
[**GetCompanyFuelerList**](FuelVendorApi.md#getcompanyfuelerlist) | **GET** /api/FuelVendor/company-specific/list | Fetch all company-specific records for the authenticated company.
[**GetCompanyFuelerNotes**](FuelVendorApi.md#getcompanyfuelernotes) | **GET** /api/FuelVendor/company-specific/{companyFuelerId}/notes | Fetch the company-specific notes for a particular fuel vendor based on the provided {companyFuelerId}.
[**GetCompanyFuelerPriceAdjustmentList**](FuelVendorApi.md#getcompanyfuelerpriceadjustmentlist) | **GET** /api/FuelVendor/company-specific/{companyFuelerId}/price-adjustment/list | Get all price adjustments for a company fueler.
[**GetCompanyFuelerSettings**](FuelVendorApi.md#getcompanyfuelersettings) | **GET** /api/FuelVendor/company-specific/{companyFuelerId}/settings | Fetch the company-specific settings for the specified {companyFuelerId} record.
[**GetPaymentInformationByFuelVendorId**](FuelVendorApi.md#getpaymentinformationbyfuelvendorid) | **GET** /api/FuelVendor/{id}/get-payment-info | Get fuel vendor payment information by fuel vendor id
[**PostCompanyFueler**](FuelVendorApi.md#postcompanyfueler) | **POST** /api/FuelVendor/company-specific | Add a company-specific record for a fuel vendor.  These details are unique for each flight department.
[**PostCompanyFuelerNotes**](FuelVendorApi.md#postcompanyfuelernotes) | **POST** /api/FuelVendor/company-specific/notes | Add a new company-specific note for a fuel vendor.
[**PostCompanyFuelerPriceAdjustment**](FuelVendorApi.md#postcompanyfuelerpriceadjustment) | **POST** /api/FuelVendor/company-specific/price-adjustment | Add a new price adjustment for a company fueler.  This price adjustment will be applied to the user&#39;s own adjusted price section when reviewing prices.
[**PostCompanyFuelerSettings**](FuelVendorApi.md#postcompanyfuelersettings) | **POST** /api/FuelVendor/company-specific/settings | Add a company-specific settings record for a fuel vendor.
[**PostPaymentInformation**](FuelVendorApi.md#postpaymentinformation) | **POST** /api/FuelVendor/add-payment-info | Add a fuel vendor payment information
[**PutPaymentInformation**](FuelVendorApi.md#putpaymentinformation) | **PUT** /api/FuelVendor/update-payment-info | Update a fuel vendor payment information
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

<a name="deletecompanyfuelerpriceadjustment"></a>
# **DeleteCompanyFuelerPriceAdjustment**
> DeleteCompanyFuelerPriceAdjustmentResponse DeleteCompanyFuelerPriceAdjustment (int? id)

Delete a price adjustment for a company fueler.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyFuelerPriceAdjustmentExample
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
            var id = 56;  // int? | 

            try
            {
                // Delete a price adjustment for a company fueler.
                DeleteCompanyFuelerPriceAdjustmentResponse result = apiInstance.DeleteCompanyFuelerPriceAdjustment(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.DeleteCompanyFuelerPriceAdjustment: " + e.Message );
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

[**DeleteCompanyFuelerPriceAdjustmentResponse**](DeleteCompanyFuelerPriceAdjustmentResponse.md)

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

<a name="deletepaymentinformationbyfuelvendorid"></a>
# **DeletePaymentInformationByFuelVendorId**
> FuelVendorPaymentInformationDTO DeletePaymentInformationByFuelVendorId (int? id)

Delete fuel vendor payment information by fuel vendor id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeletePaymentInformationByFuelVendorIdExample
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
            var id = 56;  // int? | 

            try
            {
                // Delete fuel vendor payment information by fuel vendor id
                FuelVendorPaymentInformationDTO result = apiInstance.DeletePaymentInformationByFuelVendorId(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.DeletePaymentInformationByFuelVendorId: " + e.Message );
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

[**FuelVendorPaymentInformationDTO**](FuelVendorPaymentInformationDTO.md)

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

<a name="getcompanyfuelerpriceadjustmentlist"></a>
# **GetCompanyFuelerPriceAdjustmentList**
> CompanyFuelerPriceAdjustmentListResponse GetCompanyFuelerPriceAdjustmentList (int? companyFuelerId)

Get all price adjustments for a company fueler.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyFuelerPriceAdjustmentListExample
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
                // Get all price adjustments for a company fueler.
                CompanyFuelerPriceAdjustmentListResponse result = apiInstance.GetCompanyFuelerPriceAdjustmentList(companyFuelerId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.GetCompanyFuelerPriceAdjustmentList: " + e.Message );
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

[**CompanyFuelerPriceAdjustmentListResponse**](CompanyFuelerPriceAdjustmentListResponse.md)

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

<a name="getpaymentinformationbyfuelvendorid"></a>
# **GetPaymentInformationByFuelVendorId**
> FuelVendorPaymentInformationDTO GetPaymentInformationByFuelVendorId (int? id)

Get fuel vendor payment information by fuel vendor id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetPaymentInformationByFuelVendorIdExample
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
            var id = 56;  // int? | 

            try
            {
                // Get fuel vendor payment information by fuel vendor id
                FuelVendorPaymentInformationDTO result = apiInstance.GetPaymentInformationByFuelVendorId(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.GetPaymentInformationByFuelVendorId: " + e.Message );
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

[**FuelVendorPaymentInformationDTO**](FuelVendorPaymentInformationDTO.md)

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

<a name="postcompanyfuelerpriceadjustment"></a>
# **PostCompanyFuelerPriceAdjustment**
> PostCompanyFuelerPriceAdjustmentResponse PostCompanyFuelerPriceAdjustment (PostCompanyFuelerPriceAdjustmentRequest body)

Add a new price adjustment for a company fueler.  This price adjustment will be applied to the user's own adjusted price section when reviewing prices.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyFuelerPriceAdjustmentExample
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
            var body = new PostCompanyFuelerPriceAdjustmentRequest(); // PostCompanyFuelerPriceAdjustmentRequest |  (optional) 

            try
            {
                // Add a new price adjustment for a company fueler.  This price adjustment will be applied to the user's own adjusted price section when reviewing prices.
                PostCompanyFuelerPriceAdjustmentResponse result = apiInstance.PostCompanyFuelerPriceAdjustment(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.PostCompanyFuelerPriceAdjustment: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyFuelerPriceAdjustmentRequest**](PostCompanyFuelerPriceAdjustmentRequest.md)|  | [optional] 

### Return type

[**PostCompanyFuelerPriceAdjustmentResponse**](PostCompanyFuelerPriceAdjustmentResponse.md)

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

<a name="postpaymentinformation"></a>
# **PostPaymentInformation**
> PostPaymentInformationResponse PostPaymentInformation (PostPaymentInformationRequest body)

Add a fuel vendor payment information

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostPaymentInformationExample
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
            var body = new PostPaymentInformationRequest(); // PostPaymentInformationRequest |  (optional) 

            try
            {
                // Add a fuel vendor payment information
                PostPaymentInformationResponse result = apiInstance.PostPaymentInformation(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.PostPaymentInformation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostPaymentInformationRequest**](PostPaymentInformationRequest.md)|  | [optional] 

### Return type

[**PostPaymentInformationResponse**](PostPaymentInformationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putpaymentinformation"></a>
# **PutPaymentInformation**
> FuelVendorPaymentInformationDTO PutPaymentInformation (FuelVendorPaymentInformationDTO body)

Update a fuel vendor payment information

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PutPaymentInformationExample
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
            var body = new FuelVendorPaymentInformationDTO(); // FuelVendorPaymentInformationDTO |  (optional) 

            try
            {
                // Update a fuel vendor payment information
                FuelVendorPaymentInformationDTO result = apiInstance.PutPaymentInformation(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FuelVendorApi.PutPaymentInformation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**FuelVendorPaymentInformationDTO**](FuelVendorPaymentInformationDTO.md)|  | [optional] 

### Return type

[**FuelVendorPaymentInformationDTO**](FuelVendorPaymentInformationDTO.md)

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


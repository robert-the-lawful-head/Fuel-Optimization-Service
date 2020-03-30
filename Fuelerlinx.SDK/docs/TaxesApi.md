# IO.Swagger.Api.TaxesApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteTaxByCountry**](TaxesApi.md#deletetaxbycountry) | **DELETE** /api/Taxes/by-country/{id} | Internal use only - Delete a tax-by-country record.
[**GetTaxByCountryById**](TaxesApi.md#gettaxbycountrybyid) | **GET** /api/Taxes/by-country/id/{id} | Fetch taxes by country by it&#39;s [id].  These taxes include MOT/VAT and under what circumstances they are applicable.
[**GetTaxesByCountryList**](TaxesApi.md#gettaxesbycountrylist) | **GET** /api/Taxes/by-country/list | Fetch all tax-by-country records that are available.  These taxes include MOT/VAT and under what circumstances they are applicable.
[**GetTaxesByCountryName**](TaxesApi.md#gettaxesbycountryname) | **GET** /api/Taxes/by-country/country-name/{countryName} | Fetch taxes for a specified [countryName].  These taxes include MOT/VAT and under what circumstances they are applicable.
[**PostTaxByCountry**](TaxesApi.md#posttaxbycountry) | **POST** /api/Taxes/by-country | Internal use only - Add a tax-by-country record to store MOT/VAT and control which circumstances they apply to.
[**UpdateTaxByCountry**](TaxesApi.md#updatetaxbycountry) | **PUT** /api/Taxes/by-country | Internal use only - Update a tax-by-country record to store MOT/VAT and control which circumstances they apply to.


<a name="deletetaxbycountry"></a>
# **DeleteTaxByCountry**
> DeleteTaxesByCountryResponse DeleteTaxByCountry (int? id)

Internal use only - Delete a tax-by-country record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteTaxByCountryExample
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

            var apiInstance = new TaxesApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete a tax-by-country record.
                DeleteTaxesByCountryResponse result = apiInstance.DeleteTaxByCountry(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TaxesApi.DeleteTaxByCountry: " + e.Message );
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

[**DeleteTaxesByCountryResponse**](DeleteTaxesByCountryResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettaxbycountrybyid"></a>
# **GetTaxByCountryById**
> TaxesByCountryResponse GetTaxByCountryById (int? id)

Fetch taxes by country by it's [id].  These taxes include MOT/VAT and under what circumstances they are applicable.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTaxByCountryByIdExample
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

            var apiInstance = new TaxesApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch taxes by country by it's [id].  These taxes include MOT/VAT and under what circumstances they are applicable.
                TaxesByCountryResponse result = apiInstance.GetTaxByCountryById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TaxesApi.GetTaxByCountryById: " + e.Message );
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

[**TaxesByCountryResponse**](TaxesByCountryResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettaxesbycountrylist"></a>
# **GetTaxesByCountryList**
> TaxesByCountryListResponse GetTaxesByCountryList ()

Fetch all tax-by-country records that are available.  These taxes include MOT/VAT and under what circumstances they are applicable.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTaxesByCountryListExample
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

            var apiInstance = new TaxesApi();

            try
            {
                // Fetch all tax-by-country records that are available.  These taxes include MOT/VAT and under what circumstances they are applicable.
                TaxesByCountryListResponse result = apiInstance.GetTaxesByCountryList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TaxesApi.GetTaxesByCountryList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**TaxesByCountryListResponse**](TaxesByCountryListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="gettaxesbycountryname"></a>
# **GetTaxesByCountryName**
> TaxesByCountryResponse GetTaxesByCountryName (string countryName)

Fetch taxes for a specified [countryName].  These taxes include MOT/VAT and under what circumstances they are applicable.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetTaxesByCountryNameExample
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

            var apiInstance = new TaxesApi();
            var countryName = countryName_example;  // string | 

            try
            {
                // Fetch taxes for a specified [countryName].  These taxes include MOT/VAT and under what circumstances they are applicable.
                TaxesByCountryResponse result = apiInstance.GetTaxesByCountryName(countryName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TaxesApi.GetTaxesByCountryName: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **countryName** | **string**|  | 

### Return type

[**TaxesByCountryResponse**](TaxesByCountryResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="posttaxbycountry"></a>
# **PostTaxByCountry**
> PostTaxesByCountryResponse PostTaxByCountry (PostTaxesByCountryRequest body)

Internal use only - Add a tax-by-country record to store MOT/VAT and control which circumstances they apply to.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostTaxByCountryExample
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

            var apiInstance = new TaxesApi();
            var body = new PostTaxesByCountryRequest(); // PostTaxesByCountryRequest |  (optional) 

            try
            {
                // Internal use only - Add a tax-by-country record to store MOT/VAT and control which circumstances they apply to.
                PostTaxesByCountryResponse result = apiInstance.PostTaxByCountry(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TaxesApi.PostTaxByCountry: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostTaxesByCountryRequest**](PostTaxesByCountryRequest.md)|  | [optional] 

### Return type

[**PostTaxesByCountryResponse**](PostTaxesByCountryResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatetaxbycountry"></a>
# **UpdateTaxByCountry**
> UpdateTaxesByCountryResponse UpdateTaxByCountry (UpdateTaxesByCountryRequest body)

Internal use only - Update a tax-by-country record to store MOT/VAT and control which circumstances they apply to.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateTaxByCountryExample
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

            var apiInstance = new TaxesApi();
            var body = new UpdateTaxesByCountryRequest(); // UpdateTaxesByCountryRequest |  (optional) 

            try
            {
                // Internal use only - Update a tax-by-country record to store MOT/VAT and control which circumstances they apply to.
                UpdateTaxesByCountryResponse result = apiInstance.UpdateTaxByCountry(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TaxesApi.UpdateTaxByCountry: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateTaxesByCountryRequest**](UpdateTaxesByCountryRequest.md)|  | [optional] 

### Return type

[**UpdateTaxesByCountryResponse**](UpdateTaxesByCountryResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


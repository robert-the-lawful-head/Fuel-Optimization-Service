# IO.Swagger.Api.FeeApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteServicesAndFeesByCompany**](FeeApi.md#deleteservicesandfeesbycompany) | **DELETE** /api/Fee/company-specific/{feeId} | Delete a company-specific service/fee.
[**GetServicesAndFeesByCompany**](FeeApi.md#getservicesandfeesbycompany) | **GET** /api/Fee/company-specific/{feeId} | Fetch a company-specific service/fee by it&#39;s Id.
[**GetServicesAndFeesByCompanyByLocation**](FeeApi.md#getservicesandfeesbycompanybylocation) | **GET** /api/Fee/company-specific/by-location/{icao}/{fboName} | Fetch a company-specific service/fee by the ICAO and FBO.
[**GetServicesAndFeesByCompanyId**](FeeApi.md#getservicesandfeesbycompanyid) | **GET** /api/Fee/company-specific/by-company-id/{companyId} | 
[**PostServicesAndFeesByCompany**](FeeApi.md#postservicesandfeesbycompany) | **POST** /api/Fee/company-specific | Add a new company-specific service/fee.
[**UpdateServicesAndFeesByCompany**](FeeApi.md#updateservicesandfeesbycompany) | **PUT** /api/Fee/company-specific | Update a company-specific service/fee.


<a name="deleteservicesandfeesbycompany"></a>
# **DeleteServicesAndFeesByCompany**
> DeleteServicesAndFeesByCompanyResponse DeleteServicesAndFeesByCompany (int? feeId)

Delete a company-specific service/fee.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteServicesAndFeesByCompanyExample
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

            var apiInstance = new FeeApi();
            var feeId = 56;  // int? | 

            try
            {
                // Delete a company-specific service/fee.
                DeleteServicesAndFeesByCompanyResponse result = apiInstance.DeleteServicesAndFeesByCompany(feeId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeeApi.DeleteServicesAndFeesByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **feeId** | **int?**|  | 

### Return type

[**DeleteServicesAndFeesByCompanyResponse**](DeleteServicesAndFeesByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getservicesandfeesbycompany"></a>
# **GetServicesAndFeesByCompany**
> ServicesAndFeesByCompanyResponse GetServicesAndFeesByCompany (int? feeId)

Fetch a company-specific service/fee by it's Id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetServicesAndFeesByCompanyExample
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

            var apiInstance = new FeeApi();
            var feeId = 56;  // int? | 

            try
            {
                // Fetch a company-specific service/fee by it's Id.
                ServicesAndFeesByCompanyResponse result = apiInstance.GetServicesAndFeesByCompany(feeId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeeApi.GetServicesAndFeesByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **feeId** | **int?**|  | 

### Return type

[**ServicesAndFeesByCompanyResponse**](ServicesAndFeesByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getservicesandfeesbycompanybylocation"></a>
# **GetServicesAndFeesByCompanyByLocation**
> ServicesAndFeesByCompanyListResponse GetServicesAndFeesByCompanyByLocation (string icao, string fboName)

Fetch a company-specific service/fee by the ICAO and FBO.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetServicesAndFeesByCompanyByLocationExample
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

            var apiInstance = new FeeApi();
            var icao = icao_example;  // string | 
            var fboName = fboName_example;  // string | 

            try
            {
                // Fetch a company-specific service/fee by the ICAO and FBO.
                ServicesAndFeesByCompanyListResponse result = apiInstance.GetServicesAndFeesByCompanyByLocation(icao, fboName);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeeApi.GetServicesAndFeesByCompanyByLocation: " + e.Message );
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

[**ServicesAndFeesByCompanyListResponse**](ServicesAndFeesByCompanyListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getservicesandfeesbycompanyid"></a>
# **GetServicesAndFeesByCompanyId**
> ServicesAndFeesByCompanyListResponse GetServicesAndFeesByCompanyId (int? companyId)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetServicesAndFeesByCompanyIdExample
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

            var apiInstance = new FeeApi();
            var companyId = 56;  // int? | 

            try
            {
                ServicesAndFeesByCompanyListResponse result = apiInstance.GetServicesAndFeesByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeeApi.GetServicesAndFeesByCompanyId: " + e.Message );
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

[**ServicesAndFeesByCompanyListResponse**](ServicesAndFeesByCompanyListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postservicesandfeesbycompany"></a>
# **PostServicesAndFeesByCompany**
> PostServicesAndFeesByCompanyResponse PostServicesAndFeesByCompany (PostServicesAndFeesByCompanyRequest body)

Add a new company-specific service/fee.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostServicesAndFeesByCompanyExample
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

            var apiInstance = new FeeApi();
            var body = new PostServicesAndFeesByCompanyRequest(); // PostServicesAndFeesByCompanyRequest |  (optional) 

            try
            {
                // Add a new company-specific service/fee.
                PostServicesAndFeesByCompanyResponse result = apiInstance.PostServicesAndFeesByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeeApi.PostServicesAndFeesByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostServicesAndFeesByCompanyRequest**](PostServicesAndFeesByCompanyRequest.md)|  | [optional] 

### Return type

[**PostServicesAndFeesByCompanyResponse**](PostServicesAndFeesByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateservicesandfeesbycompany"></a>
# **UpdateServicesAndFeesByCompany**
> UpdateServicesAndFeesByCompanyResponse UpdateServicesAndFeesByCompany (UpdateServicesAndFeesByCompanyRequest body)

Update a company-specific service/fee.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateServicesAndFeesByCompanyExample
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

            var apiInstance = new FeeApi();
            var body = new UpdateServicesAndFeesByCompanyRequest(); // UpdateServicesAndFeesByCompanyRequest |  (optional) 

            try
            {
                // Update a company-specific service/fee.
                UpdateServicesAndFeesByCompanyResponse result = apiInstance.UpdateServicesAndFeesByCompany(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling FeeApi.UpdateServicesAndFeesByCompany: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateServicesAndFeesByCompanyRequest**](UpdateServicesAndFeesByCompanyRequest.md)|  | [optional] 

### Return type

[**UpdateServicesAndFeesByCompanyResponse**](UpdateServicesAndFeesByCompanyResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


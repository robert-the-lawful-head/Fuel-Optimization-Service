# IO.Swagger.Api.IntegrationAuthorizationApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AddIntegrationAuthorization**](IntegrationAuthorizationApi.md#addintegrationauthorization) | **POST** /api/IntegrationAuthorization | Internal use only
[**DeleteIntegrationAuthorization**](IntegrationAuthorizationApi.md#deleteintegrationauthorization) | **DELETE** /api/IntegrationAuthorization/{id} | Internal use only
[**GetIntegrationAuthorizationByAffiliation**](IntegrationAuthorizationApi.md#getintegrationauthorizationbyaffiliation) | **GET** /api/IntegrationAuthorization/company/{companyId}/integrationtype/{integrationPartnerType}/affiliation/{affiliation} | Internal use only
[**GetIntegrationAuthorizationByCompanyId**](IntegrationAuthorizationApi.md#getintegrationauthorizationbycompanyid) | **GET** /api/IntegrationAuthorization/company/{companyId} | Internal use only
[**GetIntegrationAuthorizationById**](IntegrationAuthorizationApi.md#getintegrationauthorizationbyid) | **GET** /api/IntegrationAuthorization/{id} | Internal use only
[**GetIntegrationAuthorizationByPartnerType**](IntegrationAuthorizationApi.md#getintegrationauthorizationbypartnertype) | **GET** /api/IntegrationAuthorization/company/{companyId}/integrationtype/{integrationPartnerType} | Internal use only
[**UpdateIntegrationAuthorization**](IntegrationAuthorizationApi.md#updateintegrationauthorization) | **PUT** /api/IntegrationAuthorization/{id} | Internal use only


<a name="addintegrationauthorization"></a>
# **AddIntegrationAuthorization**
> IntegrationAuthorizationResponse AddIntegrationAuthorization (IntegrationAuthorizationDTO body)

Internal use only

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AddIntegrationAuthorizationExample
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

            var apiInstance = new IntegrationAuthorizationApi();
            var body = new IntegrationAuthorizationDTO(); // IntegrationAuthorizationDTO |  (optional) 

            try
            {
                // Internal use only
                IntegrationAuthorizationResponse result = apiInstance.AddIntegrationAuthorization(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationAuthorizationApi.AddIntegrationAuthorization: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**IntegrationAuthorizationDTO**](IntegrationAuthorizationDTO.md)|  | [optional] 

### Return type

[**IntegrationAuthorizationResponse**](IntegrationAuthorizationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteintegrationauthorization"></a>
# **DeleteIntegrationAuthorization**
> DeleteIntegrationAuthorizationResponse DeleteIntegrationAuthorization (int? id)

Internal use only

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteIntegrationAuthorizationExample
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

            var apiInstance = new IntegrationAuthorizationApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only
                DeleteIntegrationAuthorizationResponse result = apiInstance.DeleteIntegrationAuthorization(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationAuthorizationApi.DeleteIntegrationAuthorization: " + e.Message );
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

[**DeleteIntegrationAuthorizationResponse**](DeleteIntegrationAuthorizationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getintegrationauthorizationbyaffiliation"></a>
# **GetIntegrationAuthorizationByAffiliation**
> IntegrationAuthorizationListResponse GetIntegrationAuthorizationByAffiliation (int? companyId, int? integrationPartnerType, int? affiliation)

Internal use only

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIntegrationAuthorizationByAffiliationExample
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

            var apiInstance = new IntegrationAuthorizationApi();
            var companyId = 56;  // int? | 
            var integrationPartnerType = 56;  // int? | 
            var affiliation = 56;  // int? | 

            try
            {
                // Internal use only
                IntegrationAuthorizationListResponse result = apiInstance.GetIntegrationAuthorizationByAffiliation(companyId, integrationPartnerType, affiliation);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationAuthorizationApi.GetIntegrationAuthorizationByAffiliation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **integrationPartnerType** | **int?**|  | 
 **affiliation** | **int?**|  | 

### Return type

[**IntegrationAuthorizationListResponse**](IntegrationAuthorizationListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getintegrationauthorizationbycompanyid"></a>
# **GetIntegrationAuthorizationByCompanyId**
> IntegrationAuthorizationListResponse GetIntegrationAuthorizationByCompanyId (int? companyId)

Internal use only

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIntegrationAuthorizationByCompanyIdExample
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

            var apiInstance = new IntegrationAuthorizationApi();
            var companyId = 56;  // int? | 

            try
            {
                // Internal use only
                IntegrationAuthorizationListResponse result = apiInstance.GetIntegrationAuthorizationByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationAuthorizationApi.GetIntegrationAuthorizationByCompanyId: " + e.Message );
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

[**IntegrationAuthorizationListResponse**](IntegrationAuthorizationListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getintegrationauthorizationbyid"></a>
# **GetIntegrationAuthorizationById**
> IntegrationAuthorizationResponse GetIntegrationAuthorizationById (int? id)

Internal use only

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIntegrationAuthorizationByIdExample
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

            var apiInstance = new IntegrationAuthorizationApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only
                IntegrationAuthorizationResponse result = apiInstance.GetIntegrationAuthorizationById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationAuthorizationApi.GetIntegrationAuthorizationById: " + e.Message );
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

[**IntegrationAuthorizationResponse**](IntegrationAuthorizationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getintegrationauthorizationbypartnertype"></a>
# **GetIntegrationAuthorizationByPartnerType**
> IntegrationAuthorizationListResponse GetIntegrationAuthorizationByPartnerType (int? companyId, int? integrationPartnerType)

Internal use only

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetIntegrationAuthorizationByPartnerTypeExample
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

            var apiInstance = new IntegrationAuthorizationApi();
            var companyId = 56;  // int? | 
            var integrationPartnerType = 56;  // int? | 

            try
            {
                // Internal use only
                IntegrationAuthorizationListResponse result = apiInstance.GetIntegrationAuthorizationByPartnerType(companyId, integrationPartnerType);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationAuthorizationApi.GetIntegrationAuthorizationByPartnerType: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **companyId** | **int?**|  | 
 **integrationPartnerType** | **int?**|  | 

### Return type

[**IntegrationAuthorizationListResponse**](IntegrationAuthorizationListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updateintegrationauthorization"></a>
# **UpdateIntegrationAuthorization**
> IntegrationAuthorizationResponse UpdateIntegrationAuthorization (int? id, IntegrationAuthorizationDTO body)

Internal use only

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateIntegrationAuthorizationExample
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

            var apiInstance = new IntegrationAuthorizationApi();
            var id = 56;  // int? | 
            var body = new IntegrationAuthorizationDTO(); // IntegrationAuthorizationDTO |  (optional) 

            try
            {
                // Internal use only
                IntegrationAuthorizationResponse result = apiInstance.UpdateIntegrationAuthorization(id, body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling IntegrationAuthorizationApi.UpdateIntegrationAuthorization: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 
 **body** | [**IntegrationAuthorizationDTO**](IntegrationAuthorizationDTO.md)|  | [optional] 

### Return type

[**IntegrationAuthorizationResponse**](IntegrationAuthorizationResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


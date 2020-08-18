# IO.Swagger.Api.PartnerApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**ApplyPartnerCredentialsByTypeAndAffiliation**](PartnerApi.md#applypartnercredentialsbytypeandaffiliation) | **POST** /api/Partner/credentials | Apply credential changes for a certain type/afiiliation of integration partner.
[**GetAvailablePartners**](PartnerApi.md#getavailablepartners) | **GET** /api/Partner/list/type/{partnerType} | Fetch all integration partners of a certain type.
[**GetAvailablePartnersByType**](PartnerApi.md#getavailablepartnersbytype) | **GET** /api/Partner/list | Fetch all integration partners.
[**GetPartnerCredentialsByTypeAndAffiliation**](PartnerApi.md#getpartnercredentialsbytypeandaffiliation) | **GET** /api/Partner/credentials/type/{partnerType}/affiliation/{affiliation} | Fetch the credentials model for a certain type/affiliation of integration partner.  If the authenticated user has anything setup for that partner then the model will contain the user&#39;s data.
[**GetPartnerInfo**](PartnerApi.md#getpartnerinfo) | **GET** /api/Partner | Fetch the integration partner by the provided API key.


<a name="applypartnercredentialsbytypeandaffiliation"></a>
# **ApplyPartnerCredentialsByTypeAndAffiliation**
> PostIntegrationPartnerCredentialsResponse ApplyPartnerCredentialsByTypeAndAffiliation (PostIntegrationPartnerCredentialsRequest body)

Apply credential changes for a certain type/afiiliation of integration partner.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ApplyPartnerCredentialsByTypeAndAffiliationExample
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

            var apiInstance = new PartnerApi();
            var body = new PostIntegrationPartnerCredentialsRequest(); // PostIntegrationPartnerCredentialsRequest |  (optional) 

            try
            {
                // Apply credential changes for a certain type/afiiliation of integration partner.
                PostIntegrationPartnerCredentialsResponse result = apiInstance.ApplyPartnerCredentialsByTypeAndAffiliation(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PartnerApi.ApplyPartnerCredentialsByTypeAndAffiliation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostIntegrationPartnerCredentialsRequest**](PostIntegrationPartnerCredentialsRequest.md)|  | [optional] 

### Return type

[**PostIntegrationPartnerCredentialsResponse**](PostIntegrationPartnerCredentialsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getavailablepartners"></a>
# **GetAvailablePartners**
> IntegrationPartnerListResponse GetAvailablePartners (int? partnerType)

Fetch all integration partners of a certain type.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAvailablePartnersExample
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

            var apiInstance = new PartnerApi();
            var partnerType = 56;  // int? | 

            try
            {
                // Fetch all integration partners of a certain type.
                IntegrationPartnerListResponse result = apiInstance.GetAvailablePartners(partnerType);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PartnerApi.GetAvailablePartners: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **partnerType** | **int?**|  | 

### Return type

[**IntegrationPartnerListResponse**](IntegrationPartnerListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getavailablepartnersbytype"></a>
# **GetAvailablePartnersByType**
> IntegrationPartnerListResponse GetAvailablePartnersByType ()

Fetch all integration partners.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetAvailablePartnersByTypeExample
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

            var apiInstance = new PartnerApi();

            try
            {
                // Fetch all integration partners.
                IntegrationPartnerListResponse result = apiInstance.GetAvailablePartnersByType();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PartnerApi.GetAvailablePartnersByType: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**IntegrationPartnerListResponse**](IntegrationPartnerListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getpartnercredentialsbytypeandaffiliation"></a>
# **GetPartnerCredentialsByTypeAndAffiliation**
> IntegrationPartnerCredentialsResponse GetPartnerCredentialsByTypeAndAffiliation (int? partnerType, int? affiliation)

Fetch the credentials model for a certain type/affiliation of integration partner.  If the authenticated user has anything setup for that partner then the model will contain the user's data.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetPartnerCredentialsByTypeAndAffiliationExample
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

            var apiInstance = new PartnerApi();
            var partnerType = 56;  // int? | 
            var affiliation = 56;  // int? | 

            try
            {
                // Fetch the credentials model for a certain type/affiliation of integration partner.  If the authenticated user has anything setup for that partner then the model will contain the user's data.
                IntegrationPartnerCredentialsResponse result = apiInstance.GetPartnerCredentialsByTypeAndAffiliation(partnerType, affiliation);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PartnerApi.GetPartnerCredentialsByTypeAndAffiliation: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **partnerType** | **int?**|  | 
 **affiliation** | **int?**|  | 

### Return type

[**IntegrationPartnerCredentialsResponse**](IntegrationPartnerCredentialsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getpartnerinfo"></a>
# **GetPartnerInfo**
> void GetPartnerInfo ()

Fetch the integration partner by the provided API key.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetPartnerInfoExample
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

            var apiInstance = new PartnerApi();

            try
            {
                // Fetch the integration partner by the provided API key.
                apiInstance.GetPartnerInfo();
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PartnerApi.GetPartnerInfo: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

void (empty response body)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


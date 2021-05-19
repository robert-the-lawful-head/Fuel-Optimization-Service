# IO.Swagger.Api.MobileAppSettingsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteSetting**](MobileAppSettingsApi.md#deletesetting) | **DELETE** /api/MobileAppSettings/setting/{id} | Internal use only - Delete a mobile app setting by id.
[**GetSettingByKey**](MobileAppSettingsApi.md#getsettingbykey) | **GET** /api/MobileAppSettings/setting/{key} | Internal use only - Fetch a mobile app setting by a key.
[**PostSetting**](MobileAppSettingsApi.md#postsetting) | **POST** /api/MobileAppSettings/setting | Internal use only - Add a mobile app setting.
[**UpdateSetting**](MobileAppSettingsApi.md#updatesetting) | **PUT** /api/MobileAppSettings/setting | Internal use only - Update a mobile app setting.


<a name="deletesetting"></a>
# **DeleteSetting**
> DeleteMobileAppSettingsResponse DeleteSetting (int? id)

Internal use only - Delete a mobile app setting by id.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSettingExample
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

            var apiInstance = new MobileAppSettingsApi();
            var id = 56;  // int? | 

            try
            {
                // Internal use only - Delete a mobile app setting by id.
                DeleteMobileAppSettingsResponse result = apiInstance.DeleteSetting(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MobileAppSettingsApi.DeleteSetting: " + e.Message );
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

[**DeleteMobileAppSettingsResponse**](DeleteMobileAppSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsettingbykey"></a>
# **GetSettingByKey**
> MobileAppSettingsResponse GetSettingByKey (string key)

Internal use only - Fetch a mobile app setting by a key.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSettingByKeyExample
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

            var apiInstance = new MobileAppSettingsApi();
            var key = key_example;  // string | 

            try
            {
                // Internal use only - Fetch a mobile app setting by a key.
                MobileAppSettingsResponse result = apiInstance.GetSettingByKey(key);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MobileAppSettingsApi.GetSettingByKey: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **key** | **string**|  | 

### Return type

[**MobileAppSettingsResponse**](MobileAppSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsetting"></a>
# **PostSetting**
> PostMobileAppSettingsResponse PostSetting (PostMobileAppSettingsRequest body)

Internal use only - Add a mobile app setting.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSettingExample
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

            var apiInstance = new MobileAppSettingsApi();
            var body = new PostMobileAppSettingsRequest(); // PostMobileAppSettingsRequest |  (optional) 

            try
            {
                // Internal use only - Add a mobile app setting.
                PostMobileAppSettingsResponse result = apiInstance.PostSetting(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MobileAppSettingsApi.PostSetting: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostMobileAppSettingsRequest**](PostMobileAppSettingsRequest.md)|  | [optional] 

### Return type

[**PostMobileAppSettingsResponse**](PostMobileAppSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesetting"></a>
# **UpdateSetting**
> UpdateMobileAppSettingsResponse UpdateSetting (UpdateMobileAppSettingsRequest body)

Internal use only - Update a mobile app setting.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSettingExample
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

            var apiInstance = new MobileAppSettingsApi();
            var body = new UpdateMobileAppSettingsRequest(); // UpdateMobileAppSettingsRequest |  (optional) 

            try
            {
                // Internal use only - Update a mobile app setting.
                UpdateMobileAppSettingsResponse result = apiInstance.UpdateSetting(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MobileAppSettingsApi.UpdateSetting: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateMobileAppSettingsRequest**](UpdateMobileAppSettingsRequest.md)|  | [optional] 

### Return type

[**UpdateMobileAppSettingsResponse**](UpdateMobileAppSettingsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


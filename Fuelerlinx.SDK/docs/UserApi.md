# IO.Swagger.Api.UserApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteCompanyUserProfiles**](UserApi.md#deletecompanyuserprofiles) | **DELETE** /api/User/company-user-profiles/{id} | Deletes company user profile based on Id
[**DeleteCredentials**](UserApi.md#deletecredentials) | **DELETE** /api/User/credentials/{id} | Deletes user credentials based on Id
[**DeleteUserFromIFlightPlanner**](UserApi.md#deleteuserfromiflightplanner) | **DELETE** /api/User/iflightplanner/user | Internal use only - Delete a user from iFlightPlanner to stop the flight planning integration.
[**ExchangeRefreshToken**](UserApi.md#exchangerefreshtoken) | **POST** /api/User/refreshtoken | Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken].
[**GetCompanyUserProfiles**](UserApi.md#getcompanyuserprofiles) | **GET** /api/User/company-user-profiles/by-company/list | Fetches all user profiles by companyId
[**GetCredentials**](UserApi.md#getcredentials) | **GET** /api/User/credentials/{id} | Fetches user credentials by Id
[**GetCredentialsList**](UserApi.md#getcredentialslist) | **GET** /api/User/credentials/list | Fetches all user credentials
[**GetUser**](UserApi.md#getuser) | **GET** /api/User/{id} | Fetch a user by their [id].
[**GetUserByCredentials**](UserApi.md#getuserbycredentials) | **GET** /api/User/by-credentials/{username}/{password} | 
[**PostCompanyUserProfiles**](UserApi.md#postcompanyuserprofiles) | **POST** /api/User/company-user-profiles | 
[**PostCredentials**](UserApi.md#postcredentials) | **POST** /api/User/credentials | 
[**SaveCompanyToIFlightPlanner**](UserApi.md#savecompanytoiflightplanner) | **POST** /api/User/iflightplanner/company | Internal use only - Save a company to IFlightPlanner to use the flight planning integration.
[**SaveUserToIFlightPlanner**](UserApi.md#saveusertoiflightplanner) | **POST** /api/User/iflightplanner/user | Internal use only - Save a user to IFlightPlanner to use the flight planning integration.
[**UpdateCompanyUserProfiles**](UserApi.md#updatecompanyuserprofiles) | **PUT** /api/User/company-user-profiles | Updates company user profile based on companyId
[**UpdateCredentials**](UserApi.md#updatecredentials) | **PUT** /api/User/credentials | Updates user credentials based on Id
[**UserAuthToken**](UserApi.md#userauthtoken) | **POST** /api/User/token | Authenticates a user by username and password.
[**UserAuthTokenFromAccessToken**](UserApi.md#userauthtokenfromaccesstoken) | **POST** /api/User/accesstoken | Authenticates a user from an existing [AccessToken].
[**UserAuthTokenFromIFlightPlannerGUID**](UserApi.md#userauthtokenfromiflightplannerguid) | **GET** /api/User/iflightplanner/user/{guid} | Authenticate a user via the iFlightPlanner integration


<a name="deletecompanyuserprofiles"></a>
# **DeleteCompanyUserProfiles**
> DeleteCompanyUserProfilesResponse DeleteCompanyUserProfiles (int? id)

Deletes company user profile based on Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCompanyUserProfilesExample
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

            var apiInstance = new UserApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes company user profile based on Id
                DeleteCompanyUserProfilesResponse result = apiInstance.DeleteCompanyUserProfiles(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.DeleteCompanyUserProfiles: " + e.Message );
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

[**DeleteCompanyUserProfilesResponse**](DeleteCompanyUserProfilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletecredentials"></a>
# **DeleteCredentials**
> DeleteCredentialsResponse DeleteCredentials (int? id)

Deletes user credentials based on Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteCredentialsExample
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

            var apiInstance = new UserApi();
            var id = 56;  // int? | 

            try
            {
                // Deletes user credentials based on Id
                DeleteCredentialsResponse result = apiInstance.DeleteCredentials(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.DeleteCredentials: " + e.Message );
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

[**DeleteCredentialsResponse**](DeleteCredentialsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deleteuserfromiflightplanner"></a>
# **DeleteUserFromIFlightPlanner**
> DeleteUserFromIFlightPlannerResponse DeleteUserFromIFlightPlanner ()

Internal use only - Delete a user from iFlightPlanner to stop the flight planning integration.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteUserFromIFlightPlannerExample
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

            var apiInstance = new UserApi();

            try
            {
                // Internal use only - Delete a user from iFlightPlanner to stop the flight planning integration.
                DeleteUserFromIFlightPlannerResponse result = apiInstance.DeleteUserFromIFlightPlanner();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.DeleteUserFromIFlightPlanner: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**DeleteUserFromIFlightPlannerResponse**](DeleteUserFromIFlightPlannerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="exchangerefreshtoken"></a>
# **ExchangeRefreshToken**
> ExchangeRefreshTokenResponse ExchangeRefreshToken (ExchangeRefreshTokenRequest body)

Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken].

Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class ExchangeRefreshTokenExample
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

            var apiInstance = new UserApi();
            var body = new ExchangeRefreshTokenRequest(); // ExchangeRefreshTokenRequest |  (optional) 

            try
            {
                // Exchanges a valid [RefreshToken] and expired [AccessToken] for a new [RefreshToken] and [AccessToken].
                ExchangeRefreshTokenResponse result = apiInstance.ExchangeRefreshToken(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.ExchangeRefreshToken: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**ExchangeRefreshTokenRequest**](ExchangeRefreshTokenRequest.md)|  | [optional] 

### Return type

[**ExchangeRefreshTokenResponse**](ExchangeRefreshTokenResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcompanyuserprofiles"></a>
# **GetCompanyUserProfiles**
> CompanyUserProfileListResponse GetCompanyUserProfiles ()

Fetches all user profiles by companyId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCompanyUserProfilesExample
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

            var apiInstance = new UserApi();

            try
            {
                // Fetches all user profiles by companyId
                CompanyUserProfileListResponse result = apiInstance.GetCompanyUserProfiles();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.GetCompanyUserProfiles: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**CompanyUserProfileListResponse**](CompanyUserProfileListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcredentials"></a>
# **GetCredentials**
> CredentialsResponse GetCredentials (int? id)

Fetches user credentials by Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCredentialsExample
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

            var apiInstance = new UserApi();
            var id = 56;  // int? | 

            try
            {
                // Fetches user credentials by Id
                CredentialsResponse result = apiInstance.GetCredentials(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.GetCredentials: " + e.Message );
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

[**CredentialsResponse**](CredentialsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getcredentialslist"></a>
# **GetCredentialsList**
> CredentialsListResponse GetCredentialsList ()

Fetches all user credentials

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetCredentialsListExample
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

            var apiInstance = new UserApi();

            try
            {
                // Fetches all user credentials
                CredentialsListResponse result = apiInstance.GetCredentialsList();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.GetCredentialsList: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**CredentialsListResponse**](CredentialsListResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getuser"></a>
# **GetUser**
> UserDTO GetUser (int? id)

Fetch a user by their [id].

The authenticated user must have access to view this user's record.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetUserExample
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

            var apiInstance = new UserApi();
            var id = 56;  // int? | 

            try
            {
                // Fetch a user by their [id].
                UserDTO result = apiInstance.GetUser(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.GetUser: " + e.Message );
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

[**UserDTO**](UserDTO.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getuserbycredentials"></a>
# **GetUserByCredentials**
> UserResponse GetUserByCredentials (string username, string password)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetUserByCredentialsExample
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

            var apiInstance = new UserApi();
            var username = username_example;  // string | 
            var password = password_example;  // string | 

            try
            {
                UserResponse result = apiInstance.GetUserByCredentials(username, password);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.GetUserByCredentials: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **username** | **string**|  | 
 **password** | **string**|  | 

### Return type

[**UserResponse**](UserResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcompanyuserprofiles"></a>
# **PostCompanyUserProfiles**
> PostCompanyUserProfilesResponse PostCompanyUserProfiles (PostCompanyUserProfilesRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCompanyUserProfilesExample
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

            var apiInstance = new UserApi();
            var body = new PostCompanyUserProfilesRequest(); // PostCompanyUserProfilesRequest |  (optional) 

            try
            {
                PostCompanyUserProfilesResponse result = apiInstance.PostCompanyUserProfiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.PostCompanyUserProfiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCompanyUserProfilesRequest**](PostCompanyUserProfilesRequest.md)|  | [optional] 

### Return type

[**PostCompanyUserProfilesResponse**](PostCompanyUserProfilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postcredentials"></a>
# **PostCredentials**
> PostCredentialsResponse PostCredentials (PostCredentialsRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostCredentialsExample
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

            var apiInstance = new UserApi();
            var body = new PostCredentialsRequest(); // PostCredentialsRequest |  (optional) 

            try
            {
                PostCredentialsResponse result = apiInstance.PostCredentials(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.PostCredentials: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostCredentialsRequest**](PostCredentialsRequest.md)|  | [optional] 

### Return type

[**PostCredentialsResponse**](PostCredentialsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="savecompanytoiflightplanner"></a>
# **SaveCompanyToIFlightPlanner**
> SaveCompanyToIFlightPlannerResponse SaveCompanyToIFlightPlanner (SaveCompanyToIFlightPlannerRequest body)

Internal use only - Save a company to IFlightPlanner to use the flight planning integration.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class SaveCompanyToIFlightPlannerExample
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

            var apiInstance = new UserApi();
            var body = new SaveCompanyToIFlightPlannerRequest(); // SaveCompanyToIFlightPlannerRequest |  (optional) 

            try
            {
                // Internal use only - Save a company to IFlightPlanner to use the flight planning integration.
                SaveCompanyToIFlightPlannerResponse result = apiInstance.SaveCompanyToIFlightPlanner(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.SaveCompanyToIFlightPlanner: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**SaveCompanyToIFlightPlannerRequest**](SaveCompanyToIFlightPlannerRequest.md)|  | [optional] 

### Return type

[**SaveCompanyToIFlightPlannerResponse**](SaveCompanyToIFlightPlannerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="saveusertoiflightplanner"></a>
# **SaveUserToIFlightPlanner**
> SaveUserToIFlightPlannerResponse SaveUserToIFlightPlanner (SaveUserToIFlightPlannerRequest body)

Internal use only - Save a user to IFlightPlanner to use the flight planning integration.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class SaveUserToIFlightPlannerExample
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

            var apiInstance = new UserApi();
            var body = new SaveUserToIFlightPlannerRequest(); // SaveUserToIFlightPlannerRequest |  (optional) 

            try
            {
                // Internal use only - Save a user to IFlightPlanner to use the flight planning integration.
                SaveUserToIFlightPlannerResponse result = apiInstance.SaveUserToIFlightPlanner(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.SaveUserToIFlightPlanner: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**SaveUserToIFlightPlannerRequest**](SaveUserToIFlightPlannerRequest.md)|  | [optional] 

### Return type

[**SaveUserToIFlightPlannerResponse**](SaveUserToIFlightPlannerResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecompanyuserprofiles"></a>
# **UpdateCompanyUserProfiles**
> UpdateCompanyUserProfilesResponse UpdateCompanyUserProfiles (UpdateCompanyUserProfilesRequest body)

Updates company user profile based on companyId

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCompanyUserProfilesExample
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

            var apiInstance = new UserApi();
            var body = new UpdateCompanyUserProfilesRequest(); // UpdateCompanyUserProfilesRequest |  (optional) 

            try
            {
                // Updates company user profile based on companyId
                UpdateCompanyUserProfilesResponse result = apiInstance.UpdateCompanyUserProfiles(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.UpdateCompanyUserProfiles: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCompanyUserProfilesRequest**](UpdateCompanyUserProfilesRequest.md)|  | [optional] 

### Return type

[**UpdateCompanyUserProfilesResponse**](UpdateCompanyUserProfilesResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatecredentials"></a>
# **UpdateCredentials**
> UpdateCredentialsResponse UpdateCredentials (UpdateCredentialsRequest body)

Updates user credentials based on Id

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateCredentialsExample
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

            var apiInstance = new UserApi();
            var body = new UpdateCredentialsRequest(); // UpdateCredentialsRequest |  (optional) 

            try
            {
                // Updates user credentials based on Id
                UpdateCredentialsResponse result = apiInstance.UpdateCredentials(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.UpdateCredentials: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateCredentialsRequest**](UpdateCredentialsRequest.md)|  | [optional] 

### Return type

[**UpdateCredentialsResponse**](UpdateCredentialsResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="userauthtoken"></a>
# **UserAuthToken**
> UserAuthTokenResponse UserAuthToken (UserAuthTokenFromCredentialsRequest body)

Authenticates a user by username and password.

The returned object contains a [AccessToken] (JWT) that can be used to identify the user for other API requests using the [Bearer] authorization header.  The response also contains a [RefreshToken] that can be used in the /api/user/refreshtoken api to receive a new [Token].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UserAuthTokenExample
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

            var apiInstance = new UserApi();
            var body = new UserAuthTokenFromCredentialsRequest(); // UserAuthTokenFromCredentialsRequest |  (optional) 

            try
            {
                // Authenticates a user by username and password.
                UserAuthTokenResponse result = apiInstance.UserAuthToken(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.UserAuthToken: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UserAuthTokenFromCredentialsRequest**](UserAuthTokenFromCredentialsRequest.md)|  | [optional] 

### Return type

[**UserAuthTokenResponse**](UserAuthTokenResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="userauthtokenfromaccesstoken"></a>
# **UserAuthTokenFromAccessToken**
> UserAuthTokenResponse UserAuthTokenFromAccessToken (UserAuthTokenFromAccessTokenRequest body)

Authenticates a user from an existing [AccessToken].

The returned object contains a [AccessToken] (JWT) that can be used to identify the user for other API requests using the [Bearer] authorization header.  The response also contains a [RefreshToken] that can be used in the /api/user/refreshtoken api to receive a new [Token].

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UserAuthTokenFromAccessTokenExample
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

            var apiInstance = new UserApi();
            var body = new UserAuthTokenFromAccessTokenRequest(); // UserAuthTokenFromAccessTokenRequest |  (optional) 

            try
            {
                // Authenticates a user from an existing [AccessToken].
                UserAuthTokenResponse result = apiInstance.UserAuthTokenFromAccessToken(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.UserAuthTokenFromAccessToken: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UserAuthTokenFromAccessTokenRequest**](UserAuthTokenFromAccessTokenRequest.md)|  | [optional] 

### Return type

[**UserAuthTokenResponse**](UserAuthTokenResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="userauthtokenfromiflightplannerguid"></a>
# **UserAuthTokenFromIFlightPlannerGUID**
> UserAuthTokenResponse UserAuthTokenFromIFlightPlannerGUID (string guid)

Authenticate a user via the iFlightPlanner integration

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UserAuthTokenFromIFlightPlannerGUIDExample
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

            var apiInstance = new UserApi();
            var guid = guid_example;  // string | 

            try
            {
                // Authenticate a user via the iFlightPlanner integration
                UserAuthTokenResponse result = apiInstance.UserAuthTokenFromIFlightPlannerGUID(guid);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserApi.UserAuthTokenFromIFlightPlannerGUID: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **guid** | **string**|  | 

### Return type

[**UserAuthTokenResponse**](UserAuthTokenResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


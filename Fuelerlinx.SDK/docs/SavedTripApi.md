# IO.Swagger.Api.SavedTripApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**DeleteSavedTrip**](SavedTripApi.md#deletesavedtrip) | **DELETE** /api/SavedTrip/{id} | 
[**DeleteSavedTripLeg**](SavedTripApi.md#deletesavedtripleg) | **DELETE** /api/SavedTrip/legs/{id} | 
[**GetSavedTripById**](SavedTripApi.md#getsavedtripbyid) | **GET** /api/SavedTrip/{id} | 
[**GetSavedTripLegById**](SavedTripApi.md#getsavedtriplegbyid) | **GET** /api/SavedTrip/legs/{id} | 
[**GetSavedTripsByCompanyId**](SavedTripApi.md#getsavedtripsbycompanyid) | **GET** /api/SavedTrip/by-company/{companyId} | 
[**PostSavedTrip**](SavedTripApi.md#postsavedtrip) | **POST** /api/SavedTrip | 
[**PostSavedTripLeg**](SavedTripApi.md#postsavedtripleg) | **POST** /api/SavedTrip/legs | 
[**UpdateSavedTrip**](SavedTripApi.md#updatesavedtrip) | **PUT** /api/SavedTrip | 
[**UpdateSavedTripLeg**](SavedTripApi.md#updatesavedtripleg) | **PUT** /api/SavedTrip/legs | 


<a name="deletesavedtrip"></a>
# **DeleteSavedTrip**
> DeleteSavedTripResponse DeleteSavedTrip (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSavedTripExample
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

            var apiInstance = new SavedTripApi();
            var id = 56;  // int? | 

            try
            {
                DeleteSavedTripResponse result = apiInstance.DeleteSavedTrip(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.DeleteSavedTrip: " + e.Message );
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

[**DeleteSavedTripResponse**](DeleteSavedTripResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="deletesavedtripleg"></a>
# **DeleteSavedTripLeg**
> DeleteSavedTripLegResponse DeleteSavedTripLeg (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteSavedTripLegExample
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

            var apiInstance = new SavedTripApi();
            var id = 56;  // int? | 

            try
            {
                DeleteSavedTripLegResponse result = apiInstance.DeleteSavedTripLeg(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.DeleteSavedTripLeg: " + e.Message );
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

[**DeleteSavedTripLegResponse**](DeleteSavedTripLegResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsavedtripbyid"></a>
# **GetSavedTripById**
> SavedTripResponse GetSavedTripById (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSavedTripByIdExample
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

            var apiInstance = new SavedTripApi();
            var id = 56;  // int? | 

            try
            {
                SavedTripResponse result = apiInstance.GetSavedTripById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.GetSavedTripById: " + e.Message );
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

[**SavedTripResponse**](SavedTripResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsavedtriplegbyid"></a>
# **GetSavedTripLegById**
> SavedTripLegResponse GetSavedTripLegById (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSavedTripLegByIdExample
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

            var apiInstance = new SavedTripApi();
            var id = 56;  // int? | 

            try
            {
                SavedTripLegResponse result = apiInstance.GetSavedTripLegById(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.GetSavedTripLegById: " + e.Message );
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

[**SavedTripLegResponse**](SavedTripLegResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getsavedtripsbycompanyid"></a>
# **GetSavedTripsByCompanyId**
> SavedTripResponse GetSavedTripsByCompanyId (int? companyId)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetSavedTripsByCompanyIdExample
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

            var apiInstance = new SavedTripApi();
            var companyId = 56;  // int? | 

            try
            {
                SavedTripResponse result = apiInstance.GetSavedTripsByCompanyId(companyId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.GetSavedTripsByCompanyId: " + e.Message );
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

[**SavedTripResponse**](SavedTripResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsavedtrip"></a>
# **PostSavedTrip**
> PostSavedTripResponse PostSavedTrip (PostSavedTripRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSavedTripExample
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

            var apiInstance = new SavedTripApi();
            var body = new PostSavedTripRequest(); // PostSavedTripRequest |  (optional) 

            try
            {
                PostSavedTripResponse result = apiInstance.PostSavedTrip(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.PostSavedTrip: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSavedTripRequest**](PostSavedTripRequest.md)|  | [optional] 

### Return type

[**PostSavedTripResponse**](PostSavedTripResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postsavedtripleg"></a>
# **PostSavedTripLeg**
> PostSavedTripLegResponse PostSavedTripLeg (PostSavedTripLegRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostSavedTripLegExample
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

            var apiInstance = new SavedTripApi();
            var body = new PostSavedTripLegRequest(); // PostSavedTripLegRequest |  (optional) 

            try
            {
                PostSavedTripLegResponse result = apiInstance.PostSavedTripLeg(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.PostSavedTripLeg: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**PostSavedTripLegRequest**](PostSavedTripLegRequest.md)|  | [optional] 

### Return type

[**PostSavedTripLegResponse**](PostSavedTripLegResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesavedtrip"></a>
# **UpdateSavedTrip**
> UpdateSavedTripResponse UpdateSavedTrip (int? savedTripId, string savedTripName, int? savedTripCompanyId, int? savedTripUserId, int? savedTripUserAircraftId, string savedTripFuelUnitType, double? savedTripStartingFuel, string savedTripMltmodelPropertiesJson, List<Object> savedTripSavedTripLegs, int? savedTripAircraftDataId, string savedTripAircraftDataTailNumber, int? savedTripAircraftDataMinLandingRunwayLengthUnit, double? savedTripAircraftDataMinLandingRunwayLengthAmount, string savedTripAircraftDataMinLandingRunwayLengthUnitDescription, double? savedTripAircraftDataMaintCostPerHour, int? savedTripAircraftDataMaxRangeUnit, double? savedTripAircraftDataMaxRangeAmount, string savedTripAircraftDataMaxRangeUnitDescription, int? savedTripAircraftDataSize, double? savedTripAircraftDataFuelBurnRate, int? savedTripAircraftDataFuelCapacityUnit, double? savedTripAircraftDataFuelCapacityAmount, string savedTripAircraftDataFuelCapacityUnitDescription, double? savedTripAircraftDataPayloadBurnRate, int? savedTripAircraftDataAircraftId, int? savedTripAircraftDataMaxTakeoffWeightUnit, double? savedTripAircraftDataMaxTakeoffWeightAmount, string savedTripAircraftDataMaxTakeoffWeightUnitDescription, int? savedTripAircraftDataMaxLandingWeightUnit, double? savedTripAircraftDataMaxLandingWeightAmount, string savedTripAircraftDataMaxLandingWeightUnitDescription, int? savedTripAircraftDataAircraftCeilingUnit, double? savedTripAircraftDataAircraftCeilingAmount, string savedTripAircraftDataAircraftCeilingUnitDescription, int? savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit, double? savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount, string savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription, int? savedTripAircraftDataMinTakeoffRunwayLengthUnit, double? savedTripAircraftDataMinTakeoffRunwayLengthAmount, string savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription, int? savedTripAircraftDataZeroFuelWeightUnit, double? savedTripAircraftDataZeroFuelWeightAmount, string savedTripAircraftDataZeroFuelWeightUnitDescription, int? savedTripAircraftDataBasicOperatingWeightUnit, double? savedTripAircraftDataBasicOperatingWeightAmount, string savedTripAircraftDataBasicOperatingWeightUnitDescription, string savedTripAircraftDataQbclass, int? savedTripAircraftDataIfrreseveUnit, double? savedTripAircraftDataIfrreseveAmount, string savedTripAircraftDataIfrreseveUnitDescription, bool? savedTripAircraftDataDefault, int? savedTripAircraftDataNormalCruiseTasUnit, double? savedTripAircraftDataNormalCruiseTasAmount, string savedTripAircraftDataNormalCruiseTasUnitDescription, string savedTripAircraftDataApcode, int? savedTripAircraftDataMinimumUpliftUnit, double? savedTripAircraftDataMinimumUpliftAmount, string savedTripAircraftDataMinimumUpliftUnitDescription, string savedTripAircraftDataAircraftTypeEngineName, string savedTripAircraftDataFleetGroup, int? savedTripAircraftDataFactorySpecificationsAircraftId, string savedTripAircraftDataFactorySpecificationsMake, string savedTripAircraftDataFactorySpecificationsModel, int? savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit, double? savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount, string savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription, int? savedTripAircraftDataFactorySpecificationsFuelCapacityUnit, double? savedTripAircraftDataFactorySpecificationsFuelCapacityAmount, string savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit, double? savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount, string savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription, double? savedTripAircraftDataFactorySpecificationsRangeNm, double? savedTripAircraftDataFactorySpecificationsRangePerGal, double? savedTripAircraftDataFactorySpecificationsMaxRangeHours, double? savedTripAircraftDataFactorySpecificationsMaxRangeMinutes, double? savedTripAircraftDataFactorySpecificationsReserveMinutes, double? savedTripAircraftDataFactorySpecificationsReserveNm, int? savedTripAircraftDataFactorySpecificationsSize, int? savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit, double? savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount, string savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit, double? savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount, string savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit, double? savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount, string savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit, double? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount, string savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription, int? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit, double? savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount, string savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription, int? savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit, double? savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount, string savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription, int? savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit, double? savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount, string savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription, string savedTripAircraftDataFactorySpecificationsFuelType, int? savedTripAircraftDataFactorySpecificationsIfrreseveUnit, double? savedTripAircraftDataFactorySpecificationsIfrreseveAmount, string savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription, string savedTripAircraftDataFactorySpecificationsIcao, int? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid, int? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines, string savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine, double? savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSavedTripExample
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

            var apiInstance = new SavedTripApi();
            var savedTripId = 56;  // int? | 
            var savedTripName = savedTripName_example;  // string | 
            var savedTripCompanyId = 56;  // int? | 
            var savedTripUserId = 56;  // int? | 
            var savedTripUserAircraftId = 56;  // int? | 
            var savedTripFuelUnitType = savedTripFuelUnitType_example;  // string | 
            var savedTripStartingFuel = 1.2;  // double? | 
            var savedTripMltmodelPropertiesJson = savedTripMltmodelPropertiesJson_example;  // string | 
            var savedTripSavedTripLegs = new List<Object>(); // List<Object> | 
            var savedTripAircraftDataId = 56;  // int? | 
            var savedTripAircraftDataTailNumber = savedTripAircraftDataTailNumber_example;  // string | 
            var savedTripAircraftDataMinLandingRunwayLengthUnit = 56;  // int? | 
            var savedTripAircraftDataMinLandingRunwayLengthAmount = 1.2;  // double? | 
            var savedTripAircraftDataMinLandingRunwayLengthUnitDescription = savedTripAircraftDataMinLandingRunwayLengthUnitDescription_example;  // string | 
            var savedTripAircraftDataMaintCostPerHour = 1.2;  // double? | 
            var savedTripAircraftDataMaxRangeUnit = 56;  // int? | 
            var savedTripAircraftDataMaxRangeAmount = 1.2;  // double? | 
            var savedTripAircraftDataMaxRangeUnitDescription = savedTripAircraftDataMaxRangeUnitDescription_example;  // string | 
            var savedTripAircraftDataSize = 56;  // int? | 
            var savedTripAircraftDataFuelBurnRate = 1.2;  // double? | 
            var savedTripAircraftDataFuelCapacityUnit = 56;  // int? | 
            var savedTripAircraftDataFuelCapacityAmount = 1.2;  // double? | 
            var savedTripAircraftDataFuelCapacityUnitDescription = savedTripAircraftDataFuelCapacityUnitDescription_example;  // string | 
            var savedTripAircraftDataPayloadBurnRate = 1.2;  // double? | 
            var savedTripAircraftDataAircraftId = 56;  // int? | 
            var savedTripAircraftDataMaxTakeoffWeightUnit = 56;  // int? | 
            var savedTripAircraftDataMaxTakeoffWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataMaxTakeoffWeightUnitDescription = savedTripAircraftDataMaxTakeoffWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataMaxLandingWeightUnit = 56;  // int? | 
            var savedTripAircraftDataMaxLandingWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataMaxLandingWeightUnitDescription = savedTripAircraftDataMaxLandingWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataAircraftCeilingUnit = 56;  // int? | 
            var savedTripAircraftDataAircraftCeilingAmount = 1.2;  // double? | 
            var savedTripAircraftDataAircraftCeilingUnitDescription = savedTripAircraftDataAircraftCeilingUnitDescription_example;  // string | 
            var savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit = 56;  // int? | 
            var savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount = 1.2;  // double? | 
            var savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription = savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription_example;  // string | 
            var savedTripAircraftDataMinTakeoffRunwayLengthUnit = 56;  // int? | 
            var savedTripAircraftDataMinTakeoffRunwayLengthAmount = 1.2;  // double? | 
            var savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription = savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription_example;  // string | 
            var savedTripAircraftDataZeroFuelWeightUnit = 56;  // int? | 
            var savedTripAircraftDataZeroFuelWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataZeroFuelWeightUnitDescription = savedTripAircraftDataZeroFuelWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataBasicOperatingWeightUnit = 56;  // int? | 
            var savedTripAircraftDataBasicOperatingWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataBasicOperatingWeightUnitDescription = savedTripAircraftDataBasicOperatingWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataQbclass = savedTripAircraftDataQbclass_example;  // string | 
            var savedTripAircraftDataIfrreseveUnit = 56;  // int? | 
            var savedTripAircraftDataIfrreseveAmount = 1.2;  // double? | 
            var savedTripAircraftDataIfrreseveUnitDescription = savedTripAircraftDataIfrreseveUnitDescription_example;  // string | 
            var savedTripAircraftDataDefault = true;  // bool? | 
            var savedTripAircraftDataNormalCruiseTasUnit = 56;  // int? | 
            var savedTripAircraftDataNormalCruiseTasAmount = 1.2;  // double? | 
            var savedTripAircraftDataNormalCruiseTasUnitDescription = savedTripAircraftDataNormalCruiseTasUnitDescription_example;  // string | 
            var savedTripAircraftDataApcode = savedTripAircraftDataApcode_example;  // string | 
            var savedTripAircraftDataMinimumUpliftUnit = 56;  // int? | 
            var savedTripAircraftDataMinimumUpliftAmount = 1.2;  // double? | 
            var savedTripAircraftDataMinimumUpliftUnitDescription = savedTripAircraftDataMinimumUpliftUnitDescription_example;  // string | 
            var savedTripAircraftDataAircraftTypeEngineName = savedTripAircraftDataAircraftTypeEngineName_example;  // string | 
            var savedTripAircraftDataFleetGroup = savedTripAircraftDataFleetGroup_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAircraftId = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsMake = savedTripAircraftDataFactorySpecificationsMake_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsModel = savedTripAircraftDataFactorySpecificationsModel_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription = savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsFuelCapacityUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsFuelCapacityAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription = savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription = savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsRangeNm = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsRangePerGal = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsMaxRangeHours = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsMaxRangeMinutes = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsReserveMinutes = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsReserveNm = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsSize = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription = savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription = savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription = savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription = savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription = savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription = savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription = savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsFuelType = savedTripAircraftDataFactorySpecificationsFuelType_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsIfrreseveUnit = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsIfrreseveAmount = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription = savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsIcao = savedTripAircraftDataFactorySpecificationsIcao_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId = 56;  // int? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName = savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches = savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches = savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches = savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS = savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS_example;  // string | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine = 1.2;  // double? | 
            var savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours = 1.2;  // double? | 

            try
            {
                UpdateSavedTripResponse result = apiInstance.UpdateSavedTrip(savedTripId, savedTripName, savedTripCompanyId, savedTripUserId, savedTripUserAircraftId, savedTripFuelUnitType, savedTripStartingFuel, savedTripMltmodelPropertiesJson, savedTripSavedTripLegs, savedTripAircraftDataId, savedTripAircraftDataTailNumber, savedTripAircraftDataMinLandingRunwayLengthUnit, savedTripAircraftDataMinLandingRunwayLengthAmount, savedTripAircraftDataMinLandingRunwayLengthUnitDescription, savedTripAircraftDataMaintCostPerHour, savedTripAircraftDataMaxRangeUnit, savedTripAircraftDataMaxRangeAmount, savedTripAircraftDataMaxRangeUnitDescription, savedTripAircraftDataSize, savedTripAircraftDataFuelBurnRate, savedTripAircraftDataFuelCapacityUnit, savedTripAircraftDataFuelCapacityAmount, savedTripAircraftDataFuelCapacityUnitDescription, savedTripAircraftDataPayloadBurnRate, savedTripAircraftDataAircraftId, savedTripAircraftDataMaxTakeoffWeightUnit, savedTripAircraftDataMaxTakeoffWeightAmount, savedTripAircraftDataMaxTakeoffWeightUnitDescription, savedTripAircraftDataMaxLandingWeightUnit, savedTripAircraftDataMaxLandingWeightAmount, savedTripAircraftDataMaxLandingWeightUnitDescription, savedTripAircraftDataAircraftCeilingUnit, savedTripAircraftDataAircraftCeilingAmount, savedTripAircraftDataAircraftCeilingUnitDescription, savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit, savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount, savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription, savedTripAircraftDataMinTakeoffRunwayLengthUnit, savedTripAircraftDataMinTakeoffRunwayLengthAmount, savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription, savedTripAircraftDataZeroFuelWeightUnit, savedTripAircraftDataZeroFuelWeightAmount, savedTripAircraftDataZeroFuelWeightUnitDescription, savedTripAircraftDataBasicOperatingWeightUnit, savedTripAircraftDataBasicOperatingWeightAmount, savedTripAircraftDataBasicOperatingWeightUnitDescription, savedTripAircraftDataQbclass, savedTripAircraftDataIfrreseveUnit, savedTripAircraftDataIfrreseveAmount, savedTripAircraftDataIfrreseveUnitDescription, savedTripAircraftDataDefault, savedTripAircraftDataNormalCruiseTasUnit, savedTripAircraftDataNormalCruiseTasAmount, savedTripAircraftDataNormalCruiseTasUnitDescription, savedTripAircraftDataApcode, savedTripAircraftDataMinimumUpliftUnit, savedTripAircraftDataMinimumUpliftAmount, savedTripAircraftDataMinimumUpliftUnitDescription, savedTripAircraftDataAircraftTypeEngineName, savedTripAircraftDataFleetGroup, savedTripAircraftDataFactorySpecificationsAircraftId, savedTripAircraftDataFactorySpecificationsMake, savedTripAircraftDataFactorySpecificationsModel, savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit, savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount, savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription, savedTripAircraftDataFactorySpecificationsFuelCapacityUnit, savedTripAircraftDataFactorySpecificationsFuelCapacityAmount, savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription, savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit, savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount, savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription, savedTripAircraftDataFactorySpecificationsRangeNm, savedTripAircraftDataFactorySpecificationsRangePerGal, savedTripAircraftDataFactorySpecificationsMaxRangeHours, savedTripAircraftDataFactorySpecificationsMaxRangeMinutes, savedTripAircraftDataFactorySpecificationsReserveMinutes, savedTripAircraftDataFactorySpecificationsReserveNm, savedTripAircraftDataFactorySpecificationsSize, savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit, savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount, savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription, savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit, savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount, savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription, savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit, savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount, savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription, savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit, savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount, savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription, savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit, savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount, savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription, savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit, savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount, savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription, savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit, savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount, savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription, savedTripAircraftDataFactorySpecificationsFuelType, savedTripAircraftDataFactorySpecificationsIfrreseveUnit, savedTripAircraftDataFactorySpecificationsIfrreseveAmount, savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription, savedTripAircraftDataFactorySpecificationsIcao, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine, savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.UpdateSavedTrip: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **savedTripId** | **int?**|  | 
 **savedTripName** | **string**|  | 
 **savedTripCompanyId** | **int?**|  | 
 **savedTripUserId** | **int?**|  | 
 **savedTripUserAircraftId** | **int?**|  | 
 **savedTripFuelUnitType** | **string**|  | 
 **savedTripStartingFuel** | **double?**|  | 
 **savedTripMltmodelPropertiesJson** | **string**|  | 
 **savedTripSavedTripLegs** | [**List<Object>**](Object.md)|  | 
 **savedTripAircraftDataId** | **int?**|  | 
 **savedTripAircraftDataTailNumber** | **string**|  | 
 **savedTripAircraftDataMinLandingRunwayLengthUnit** | **int?**|  | 
 **savedTripAircraftDataMinLandingRunwayLengthAmount** | **double?**|  | 
 **savedTripAircraftDataMinLandingRunwayLengthUnitDescription** | **string**|  | 
 **savedTripAircraftDataMaintCostPerHour** | **double?**|  | 
 **savedTripAircraftDataMaxRangeUnit** | **int?**|  | 
 **savedTripAircraftDataMaxRangeAmount** | **double?**|  | 
 **savedTripAircraftDataMaxRangeUnitDescription** | **string**|  | 
 **savedTripAircraftDataSize** | **int?**|  | 
 **savedTripAircraftDataFuelBurnRate** | **double?**|  | 
 **savedTripAircraftDataFuelCapacityUnit** | **int?**|  | 
 **savedTripAircraftDataFuelCapacityAmount** | **double?**|  | 
 **savedTripAircraftDataFuelCapacityUnitDescription** | **string**|  | 
 **savedTripAircraftDataPayloadBurnRate** | **double?**|  | 
 **savedTripAircraftDataAircraftId** | **int?**|  | 
 **savedTripAircraftDataMaxTakeoffWeightUnit** | **int?**|  | 
 **savedTripAircraftDataMaxTakeoffWeightAmount** | **double?**|  | 
 **savedTripAircraftDataMaxTakeoffWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataMaxLandingWeightUnit** | **int?**|  | 
 **savedTripAircraftDataMaxLandingWeightAmount** | **double?**|  | 
 **savedTripAircraftDataMaxLandingWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataAircraftCeilingUnit** | **int?**|  | 
 **savedTripAircraftDataAircraftCeilingAmount** | **double?**|  | 
 **savedTripAircraftDataAircraftCeilingUnitDescription** | **string**|  | 
 **savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnit** | **int?**|  | 
 **savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelAmount** | **double?**|  | 
 **savedTripAircraftDataMinTakeoffRunwayLengthAtSeaLevelUnitDescription** | **string**|  | 
 **savedTripAircraftDataMinTakeoffRunwayLengthUnit** | **int?**|  | 
 **savedTripAircraftDataMinTakeoffRunwayLengthAmount** | **double?**|  | 
 **savedTripAircraftDataMinTakeoffRunwayLengthUnitDescription** | **string**|  | 
 **savedTripAircraftDataZeroFuelWeightUnit** | **int?**|  | 
 **savedTripAircraftDataZeroFuelWeightAmount** | **double?**|  | 
 **savedTripAircraftDataZeroFuelWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataBasicOperatingWeightUnit** | **int?**|  | 
 **savedTripAircraftDataBasicOperatingWeightAmount** | **double?**|  | 
 **savedTripAircraftDataBasicOperatingWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataQbclass** | **string**|  | 
 **savedTripAircraftDataIfrreseveUnit** | **int?**|  | 
 **savedTripAircraftDataIfrreseveAmount** | **double?**|  | 
 **savedTripAircraftDataIfrreseveUnitDescription** | **string**|  | 
 **savedTripAircraftDataDefault** | **bool?**|  | 
 **savedTripAircraftDataNormalCruiseTasUnit** | **int?**|  | 
 **savedTripAircraftDataNormalCruiseTasAmount** | **double?**|  | 
 **savedTripAircraftDataNormalCruiseTasUnitDescription** | **string**|  | 
 **savedTripAircraftDataApcode** | **string**|  | 
 **savedTripAircraftDataMinimumUpliftUnit** | **int?**|  | 
 **savedTripAircraftDataMinimumUpliftAmount** | **double?**|  | 
 **savedTripAircraftDataMinimumUpliftUnitDescription** | **string**|  | 
 **savedTripAircraftDataAircraftTypeEngineName** | **string**|  | 
 **savedTripAircraftDataFleetGroup** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAircraftId** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsMake** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsModel** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsNormalCruiseTasAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsNormalCruiseTasUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsFuelCapacityUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsFuelCapacityAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsFuelCapacityUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsMinLandingRunwayLengthUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsRangeNm** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsRangePerGal** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxRangeHours** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxRangeMinutes** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsReserveMinutes** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsReserveNm** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsSize** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxTakeoffWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxLandingWeightAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsMaxLandingWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAircraftCeilingUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsAircraftCeilingAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAircraftCeilingUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAtSeaLevelUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsMinTakeoffRunwayLengthUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsZeroFuelWeightAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsZeroFuelWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsBasicOperatingWeightAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsBasicOperatingWeightUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsFuelType** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsIfrreseveUnit** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsIfrreseveAmount** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsIfrreseveUnitDescription** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsIcao** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsOid** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsAircraftId** | **int?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsModelName** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsLengthFt** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsHeightFt** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuselageDimensionsWingSpanFt** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsLengthFtInches** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsHeightFtInches** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCabinDimensionsWidthFtInches** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCrewConfiguration** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPassengerConfiguration** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsPressurizationPsi** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardLbs** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityStandardGal** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalLbs** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsFuelCapacityOptionalGal** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxRampWeightLbs** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxTakeoffWeightLbs** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsZeroFuelWeightLbs** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsBasicOperatingWeightLbs** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsMaxLandingWeightLbs** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsCleanSpeedKnots** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVsoLandingSpeedKnots** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalCruiseTasSpeedKnots** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsVmoMaxOpIasSpeedKnots** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNormalClimbFpm** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineOutClimbFpm** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsCeilingFt** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsLandingPerfFaaFieldLengthFt** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerfSlIsaBfl** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsTakeoffPerf500020cBfl** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsRangeNm** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsNumberOfEngines** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineModelS** | **string**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineThrustLbsPerEngine** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineShaftHpPerEngine** | **double?**|  | 
 **savedTripAircraftDataFactorySpecificationsAdditionalSpecificationsEngineCommonTboHours** | **double?**|  | 

### Return type

[**UpdateSavedTripResponse**](UpdateSavedTripResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="updatesavedtripleg"></a>
# **UpdateSavedTripLeg**
> UpdateSavedTripLegResponse UpdateSavedTripLeg (UpdateSavedTripLegRequest body)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class UpdateSavedTripLegExample
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

            var apiInstance = new SavedTripApi();
            var body = new UpdateSavedTripLegRequest(); // UpdateSavedTripLegRequest |  (optional) 

            try
            {
                UpdateSavedTripLegResponse result = apiInstance.UpdateSavedTripLeg(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SavedTripApi.UpdateSavedTripLeg: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**UpdateSavedTripLegRequest**](UpdateSavedTripLegRequest.md)|  | [optional] 

### Return type

[**UpdateSavedTripLegResponse**](UpdateSavedTripLegResponse.md)

### Authorization

[ApiKeyScheme](../README.md#ApiKeyScheme), [Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


# IO.Swagger.Model.PostRampFeeByCompanyRequest
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Icao** | **string** |  | 
**FboHandlerName** | **string** |  | 
**RampFee** | **double?** |  | [optional] 
**RampFeeWaived** | [**Weight**](Weight.md) |  | [optional] 
**CategoryType** | **int?** | 0 &#x3D; Not Specified             1 &#x3D; By Size             2 &#x3D; By Aircraft Type             3 &#x3D; By Weight Range (Lbs)             4 &#x3D; By Wingspan (feet)             5 &#x3D; By Tail Number    * &#x60;NotSpecified&#x60; - Not Specified  * &#x60;BySize&#x60; - By Size  * &#x60;ByAircraftType&#x60; - By Aircraft Type  * &#x60;ByWeightRange&#x60; - By Weight Range (Lbs)  * &#x60;ByWingSpan&#x60; - By Wingspan (feet)  * &#x60;ByTailNumberList&#x60; - By Tailnumber   | [optional] 
**CategoryMinValue** | **int?** |  | [optional] 
**CategoryMaxValue** | **int?** |  | [optional] 
**CategoryStringValue** | **string** |  | [optional] 
**Notes** | **string** |  | [optional] 
**FacilityFee** | **double?** |  | [optional] 
**ExpirationDate** | **string** |  | [optional] 
**Applicable** | **bool?** |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


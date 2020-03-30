# IO.Swagger.Model.ServicesAndFeesByCompanyDTO
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **int?** |  | [optional] 
**CompanyId** | **int?** |  | [optional] 
**Icao** | **string** |  | [optional] 
**FboHandlerName** | **string** |  | [optional] 
**Fbo** | **string** |  | [optional] 
**Name** | **string** |  | [optional] 
**Price** | **double?** |  | [optional] 
**VolumeToAvoidFee** | [**Weight**](Weight.md) |  | [optional] 
**PriceCurrency** | **string** |  | [optional] 
**PreferredWeightUnitFormat** | **int?** | Weight units:  0 &#x3D; Gallons  1 &#x3D; Pounds  2 &#x3D; Kilograms  3 &#x3D; Tonnes  4 &#x3D; Liters | [optional] 
**CategoryType** | **int?** | 0 &#x3D; Not Specified  1 &#x3D; By Size  2 &#x3D; By Aircraft Type  3 &#x3D; By Weight Range (Lbs)  4 &#x3D; By Wingspan (feet)  5 &#x3D; By Tail Number | [optional] 
**CategoryMinValue** | **int?** |  | [optional] 
**CategoryMaxValue** | **int?** |  | [optional] 
**CategoryStringValue** | **string** |  | [optional] 
**ApplicableTailNumbers** | **List&lt;string&gt;** |  | [optional] 
**CategorizationDescription** | **string** |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


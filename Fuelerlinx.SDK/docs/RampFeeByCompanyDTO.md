# IO.Swagger.Model.RampFeeByCompanyDTO
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **int?** |  | [optional] 
**Icao** | **string** |  | 
**FboHandlerName** | **string** |  | 
**RampFee** | **double?** |  | [optional] 
**RampFeeWaived** | [**Weight**](Weight.md) |  | [optional] 
**CompanyId** | **int?** |  | [optional] 
**UserId** | **int?** |  | [optional] 
**Updated** | **DateTime?** |  | [optional] 
**CategoryType** | **int?** | 0 &#x3D; Not Specified  1 &#x3D; By Size  2 &#x3D; By Aircraft Type  3 &#x3D; By Weight Range (Lbs)  4 &#x3D; By Wingspan (feet)  5 &#x3D; By Tail Number | [optional] 
**CategoryMinValue** | **int?** |  | [optional] 
**CategoryMaxValue** | **int?** |  | [optional] 
**LandingFee** | **double?** |  | [optional] 
**FacilityFee** | **double?** |  | [optional] 
**ExpirationDate** | **string** |  | [optional] 
**Applicable** | **bool?** |  | [optional] 
**CategoryStringValue** | **string** |  | [optional] 
**RampFeeByCompanyNotes** | [**List&lt;RampFeeByCompanyNoteDTO&gt;**](RampFeeByCompanyNoteDTO.md) |  | [optional] 
**ApplicableTailNumbers** | **List&lt;string&gt;** |  | [optional] 
**CategorizationDescription** | **string** |  | [optional] 
**AddedByName** | **string** |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


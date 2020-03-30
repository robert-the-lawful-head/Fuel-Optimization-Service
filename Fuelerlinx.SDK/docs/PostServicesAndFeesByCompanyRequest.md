# IO.Swagger.Model.PostServicesAndFeesByCompanyRequest
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Icao** | **string** |  | [optional] 
**FboHandlerName** | **string** |  | [optional] 
**Name** | **string** |  | [optional] 
**Price** | **double?** |  | [optional] 
**VolumeToAvoidFee** | [**Weight**](Weight.md) |  | [optional] 
**PriceCurrency** | **string** |  | [optional] 
**PreferredWeightUnitFormat** | **int?** | Weight units:             0 &#x3D; Gallons             1 &#x3D; Pounds             2 &#x3D; Kilograms             3 &#x3D; Tonnes             4 &#x3D; Liters    * &#x60;Gallons&#x60; - Gallons  * &#x60;Pounds&#x60; - Pounds  * &#x60;Kilograms&#x60; - Kilograms  * &#x60;Tonnes&#x60; - Tonnes  * &#x60;Liters&#x60; - Liters   | [optional] 
**CategoryType** | **int?** | 0 &#x3D; Not Specified             1 &#x3D; By Size             2 &#x3D; By Aircraft Type             3 &#x3D; By Weight Range (Lbs)             4 &#x3D; By Wingspan (feet)             5 &#x3D; By Tail Number    * &#x60;NotSpecified&#x60; - Not Specified  * &#x60;BySize&#x60; - By Size  * &#x60;ByAircraftType&#x60; - By Aircraft Type  * &#x60;ByWeightRange&#x60; - By Weight Range (Lbs)  * &#x60;ByWingSpan&#x60; - By Wingspan (feet)  * &#x60;ByTailNumberList&#x60; - By Tailnumber   | [optional] 
**CategoryMinValue** | **int?** | Provide the minimum range here for wingspan or weight.  Provide the aircraftId here if by aircraft type. Provide the size enum here if by size. | [optional] 
**CategoryMaxValue** | **int?** | Provide the maximum range here for wingspan or weight. | [optional] 
**CategoryStringValue** | **string** | Provide a comma delimited list of tails here if the categoryType is by tail number (5) | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


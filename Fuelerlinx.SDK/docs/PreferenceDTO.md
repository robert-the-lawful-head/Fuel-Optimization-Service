# IO.Swagger.Model.PreferenceDTO
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**UserID** | **int?** |  | [optional] 
**FuelOptionSorting** | **int?** |  | [optional] 
**IsAllInclusivePricingPrioritized** | **bool?** |  | [optional] 
**SortByAllInclusivePricing** | **bool?** |  | [optional] 
**IsPriceMasked** | **bool?** |  | [optional] 
**OmitFromDispatchConfirmation** | **bool?** |  | [optional] 
**FuelOn** | **string** |  | [optional] 
**ExcludeAllEmailsByDefault** | **bool?** |  | [optional] 
**SiteMode** | **int?** | Site Modes:             Live &#x3D; 0,             Test &#x3D; 1,             Local &#x3D; 2,             Beta &#x3D; 3,             Staging &#x3D; 4,             Classic &#x3D; 5,             Rollback &#x3D; 6    * &#x60;Live&#x60; - Live  * &#x60;Test&#x60; - Test  * &#x60;Local&#x60; - Local  * &#x60;Beta&#x60; - Beta  * &#x60;Staging&#x60; - Staging  * &#x60;Classic&#x60; - Classic  * &#x60;Rollback&#x60; - Rollback   | [optional] 
**HandlerEmails** | [**List&lt;UserEmailDTO&gt;**](UserEmailDTO.md) |  | [optional] 
**Currency** | **string** |  | [optional] 
**ExcludeTakingFuelByDefault** | **bool?** |  | [optional] 
**CollapseLegsByDefault** | **bool?** |  | [optional] 
**CreateTransactionsForSkippedLegs** | **bool?** |  | [optional] 
**AddOneGalToSkipFuel** | **bool?** |  | [optional] 
**CopyPrimaryAccountOnDispatch** | **bool?** |  | [optional] 
**ShowPreferredFBOsOnly** | **bool?** |  | [optional] 
**FuelOptionsWeightUnit** | **int?** | Weight units:             0 &#x3D; Gallons             1 &#x3D; Pounds             2 &#x3D; Kilograms             3 &#x3D; Tonnes             4 &#x3D; Liters    * &#x60;Gallons&#x60; - Gallons  * &#x60;Pounds&#x60; - Pounds  * &#x60;Kilograms&#x60; - Kilograms  * &#x60;Tonnes&#x60; - Tonnes  * &#x60;Liters&#x60; - Liters   | [optional] 
**ShowPostedRetail** | **bool?** |  | [optional] 
**CCOnVendorDispatchEmail** | **bool?** |  | [optional] 
**TimePreference** | **int?** |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


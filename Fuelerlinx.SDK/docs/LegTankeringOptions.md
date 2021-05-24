# IO.Swagger.Model.LegTankeringOptions
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**LandWithMaxFuel** | **bool?** | Maximizes the fuel uplift on this leg, landing with as much fuel as possible. | [optional] 
**OmitFromFuelConsideration** | **bool?** | Omits the leg from consideration of a fuel uplift.  Will skip fueling in all scenarios. | [optional] 
**LockedFuelAmount** | [**Weight**](Weight.md) |  | [optional] 
**MinimumRequiredRampFuel** | [**Weight**](Weight.md) |  | [optional] 
**PriceSearchPreference** | **int?** | Overrides the trip-wide price search preference.  Set to 0 - Selected option only if you want to lock it to the selected option. | [optional] 
**MinExtraReserve** | [**Weight**](Weight.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


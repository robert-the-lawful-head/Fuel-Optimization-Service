# IO.Swagger.Model.Leg
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**LegNumber** | **int?** |  | [optional] 
**LegId** | **int?** |  | [optional] 
**DepartureAirportIdentifier** | **string** |  | [optional] 
**ArrivalAirportIdentifier** | **string** |  | [optional] 
**DepartureDate** | **string** |  | [optional] 
**DepartureTime** | **string** |  | [optional] 
**DepartureType** | **int?** |  | [optional] 
**SchedulingInformation** | [**ScheduledTripDTO**](ScheduledTripDTO.md) |  | [optional] 
**LegPricingData** | [**PricingData**](PricingData.md) |  | [optional] 
**LegTankeringOptions** | [**LegTankeringOptions**](LegTankeringOptions.md) |  | [optional] 
**FlightDataWithoutTankeredFuel** | [**RouteDetailsCalculationWithNavLog**](RouteDetailsCalculationWithNavLog.md) |  | [optional] 
**FlightDataWithTankeredFuel** | [**RouteDetailsCalculationWithNavLog**](RouteDetailsCalculationWithNavLog.md) |  | [optional] 
**WeightAndBalanceOptions** | [**WeightAndBalanceOptions**](WeightAndBalanceOptions.md) |  | [optional] 
**LegFlightPlanningRequest** | [**LegFlightPlanningRequest**](LegFlightPlanningRequest.md) |  | [optional] 
**FlightDataWithoutTankeredFuelJSON** | **string** | This property is to only be used if FuelerLinx has approved your custom flight data in JSON form.  Please notify techsupport@fuelerlinx.com if you&#39;d like to provide your own flight data. | [optional] 
**FlightDataWithTankeredFuelJSON** | **string** | This property is to only be used if FuelerLinx has approved your custom flight data in JSON form.  Please notify techsupport@fuelerlinx.com if you&#39;d like to provide your own flight data. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


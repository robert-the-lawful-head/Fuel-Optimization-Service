# IO.Swagger.Model.CalculateRouteDetailsRequest
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**TailNumber** | **string** |  | 
**DepartureAirportIdentifier** | **string** |  | 
**ArrivalAirportIdentifier** | **string** |  | 
**DepartureDateTime** | **DateTime?** |  | [optional] 
**MaxLandingWeight** | [**Weight**](Weight.md) |  | [optional] 
**MaxTakeoffWeight** | [**Weight**](Weight.md) |  | [optional] 
**CargoWeight** | [**Weight**](Weight.md) |  | [optional] 
**NumberOfPassengers** | **int?** |  | [optional] 
**TotalPassengerWeight** | [**Weight**](Weight.md) |  | [optional] 
**IFlightPlannerCruiseProfileId** | **int?** |  | [optional] 
**RoutingType** | **int?** | Routing Types:             0 &#x3D; Optimal             1 &#x3D; Direct             2 &#x3D; Custom    * &#x60;Optimal&#x60; - Optimal  * &#x60;Direct&#x60; - Direct  * &#x60;Custom&#x60; - Customer  * &#x60;RecentATC&#x60; - Recent ATC   | [optional] 
**CustomRoute** | **string** |  | [optional] 
**AlternateAirport** | **string** |  | [optional] 
**AlternateFuel** | [**Weight**](Weight.md) |  | [optional] 
**ReserveFuel** | [**Weight**](Weight.md) |  | [optional] 
**HoldTime** | [**Time**](Time.md) |  | [optional] 
**CruiseAirSpeed** | [**Speed**](Speed.md) |  | [optional] 
**MaxAltitudeInFeet** | **int?** |  | [optional] 
**SpecificAltitudeInFeet** | **int?** |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


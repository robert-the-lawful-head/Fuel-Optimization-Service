# IO.Swagger.Model.AircraftPerformanceProfile
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **int?** |  | [optional] 
**IsDefault** | **bool?** |  | [optional] 
**Name** | **string** |  | [optional] 
**ProfileType** | **int?** | Aircraft Performance Profile Types:             0 &#x3D; None             1 &#x3D; By Segment             2 &#x3D; By Hour             4 &#x3D; By Altitude             8 &#x3D; SkyPlan    * &#x60;None&#x60; -   * &#x60;BySegment&#x60; -   * &#x60;ByHour&#x60; -   * &#x60;ByAltitude&#x60; -   * &#x60;SkyPlan&#x60; -    | [optional] 
**ClimbAirspeed** | **string** |  | [optional] 
**ClimbAirspeedType** | **int?** | Airspeed Types:             0 &#x3D; None             1 &#x3D; IAS             2 &#x3D; TAS    * &#x60;None&#x60; -   * &#x60;IAS&#x60; -   * &#x60;TAS&#x60; -    | [optional] 
**ClimbFuelBurn** | **double?** |  | [optional] 
**ClimbKTAS** | **double?** |  | [optional] 
**ClimbRate** | **double?** |  | [optional] 
**CruiseAirspeed** | **string** |  | [optional] 
**DescentAirspeedType** | **int?** | Airspeed Types:             0 &#x3D; None             1 &#x3D; IAS             2 &#x3D; TAS    * &#x60;None&#x60; -   * &#x60;IAS&#x60; -   * &#x60;TAS&#x60; -    | [optional] 
**CruiseFuelBurn** | **double?** |  | [optional] 
**CruiseKTAS** | **double?** |  | [optional] 
**DescentAirspeed** | **string** |  | [optional] 
**DescentFuelBurn** | **double?** |  | [optional] 
**DescentKTAS** | **double?** |  | [optional] 
**DescentRate** | **double?** |  | [optional] 
**FuelBurnByHour** | **string** |  | [optional] 
**AirspeedType** | **int?** | Airspeed Types:             0 &#x3D; None             1 &#x3D; IAS             2 &#x3D; TAS    * &#x60;None&#x60; -   * &#x60;IAS&#x60; -   * &#x60;TAS&#x60; -    | [optional] 
**FlightPhase** | **int?** | Flight phase:             0 &#x3D; Unspecified             1 &#x3D; Climb             2 &#x3D; Cruise             4 &#x3D; Descent    * &#x60;Unspecified&#x60; -   * &#x60;Climb&#x60; -   * &#x60;Cruise&#x60; -   * &#x60;Descent&#x60; -    | [optional] 
**Altitudes** | [**List&lt;AircraftPerformanceAltitude&gt;**](AircraftPerformanceAltitude.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


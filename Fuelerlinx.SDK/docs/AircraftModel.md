# IO.Swagger.Model.AircraftModel
## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **int?** |  | [optional] 
**Make** | **string** |  | [optional] 
**SerialNumbers** | **string** |  | [optional] 
**DisplayName** | **string** |  | [optional] 
**AircraftID** | **int?** |  | [optional] 
**AircraftType** | **string** |  | [optional] 
**Maker** | **string** |  | [optional] 
**Model** | **string** |  | [optional] 
**ModelID** | **int?** |  | [optional] 
**TailNumber** | **string** |  | [optional] 
**AirspeedUnit** | **int?** | Airspeed units:             0 &#x3D; Unspecified             4 &#x3D; KMH             8 &#x3D; Knots             16 &#x3D; MPH             32 &#x3D; Mach    * &#x60;Unspecified&#x60; -   * &#x60;KMH&#x60; -   * &#x60;Knots&#x60; -   * &#x60;MPH&#x60; -   * &#x60;Mach&#x60; -    | [optional] 
**EmptyWeight** | **double?** |  | [optional] 
**FuelBurnUnit** | **int?** | Fuel burn units:             1 &#x3D; Gallons per hour             2 &#x3D; Pounds per hour             4 &#x3D; Liters per hour             8 &#x3D; Kilograms per hour    * &#x60;GallonsPerHour&#x60; -   * &#x60;PoundsPerHour&#x60; -   * &#x60;LitersPerHour&#x60; -   * &#x60;KilogramsPerHour&#x60; -    | [optional] 
**FuelCapacity** | **double?** |  | [optional] 
**FuelReserve** | **double?** |  | [optional] 
**FuelWeight** | **double?** |  | [optional] 
**MaxAltitude** | **int?** |  | [optional] 
**MinRunwayLength** | **int?** |  | [optional] 
**PerformanceProfileType** | **int?** | Aircraft Performance Profile Types:             0 &#x3D; None             1 &#x3D; By Segment             2 &#x3D; By Hour             4 &#x3D; By Altitude             8 &#x3D; SkyPlan    * &#x60;None&#x60; -   * &#x60;BySegment&#x60; -   * &#x60;ByHour&#x60; -   * &#x60;ByAltitude&#x60; -   * &#x60;SkyPlan&#x60; -    | [optional] 
**VolumeUnit** | **int?** | Volume units:             0 &#x3D; Gallons             1 &#x3D; Liters    * &#x60;Gallons&#x60; -   * &#x60;Liters&#x60; -    | [optional] 
**WeightUnit** | **int?** | Weight units:             0 &#x3D; Pounds             1 &#x3D; Kilograms    * &#x60;Pounds&#x60; -   * &#x60;Kilograms&#x60; -    | [optional] 
**SkyPlanMachEnabled** | **bool?** |  | [optional] 
**SkyPlanMachMin** | **int?** |  | [optional] 
**SkyPlanMachMax** | **int?** |  | [optional] 
**Performance** | [**List&lt;AircraftPerformanceProfile&gt;**](AircraftPerformanceProfile.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)


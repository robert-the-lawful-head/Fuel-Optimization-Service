USE [paragon_test]
GO
CREATE NONCLUSTERED INDEX [INX_AirportWatchLiveData_AircraftPositionDateTimeUtc]
ON [dbo].[AirportWatchLiveData] ([AircraftPositionDateTimeUtc])
INCLUDE ([BoxTransmissionDateTimeUtc],[AircraftHexCode],[AtcFlightNumber],[AltitudeInStandardPressure],[GroundSpeedKts],[TrackingDegree],[Latitude],[Longitude],[VerticalSpeedKts],[TransponderCode],[BoxName],[AircraftTypeCode],[GpsAltitude],[IsAircraftOnGround],[TailNumber])
GO

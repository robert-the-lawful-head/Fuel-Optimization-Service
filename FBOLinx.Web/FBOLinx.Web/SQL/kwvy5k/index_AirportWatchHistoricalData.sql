CREATE NONCLUSTERED INDEX [INX_AirportWatchHistoricalData_ICAO_PositionDate]
ON [dbo].[AirportWatchHistoricalData] ([AirportICAO],[AircraftPositionDateTimeUtc])
INCLUDE ([AircraftHexCode],[AtcFlightNumber],[AircraftTypeCode],[AircraftStatus])
GO

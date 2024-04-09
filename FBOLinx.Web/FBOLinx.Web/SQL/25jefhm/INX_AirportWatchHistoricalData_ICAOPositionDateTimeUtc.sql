SET STATISTICS TIME ON
SET STATISTICS IO ON
GO
DROP INDEX AirportWatchHistoricalData.INX_AirportWatchHistoricalData_ICAO_PositionDate;
GO
DROP INDEX AirportWatchHistoricalData.INX_AirportWatchHistoricalData_ICAOPositionDateTimeUtc;
GO
CREATE NONCLUSTERED INDEX [INX_AirportWatchHistoricalData_ICAOPositionDateTimeUtc] ON [dbo].[AirportWatchHistoricalData]
(
	[AirportICAO] ASC,
	[AircraftPositionDateTimeUtc] ASC
)
INCLUDE([AircraftHexCode],[AtcFlightNumber],[Latitude],[Longitude],[AircraftTypeCode],[AircraftStatus],[TailNumber],[BoxName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


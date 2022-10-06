USE [Dega]
GO

/****** Object:  Index [IX_SWIMFlightLegs_DepartureICAO_ArrivalICAO_ATD]    Script Date: 9/27/2022 9:52:40 AM ******/
DROP INDEX [IX_SWIMFlightLegs_DepartureICAO_ArrivalICAO_ATD] ON [dbo].[SWIMFlightLegs]
GO

USE [paragon_test]
GO

ALTER TABLE AirportWatchDistinctBoxes
ADD Latitude nvarchar(50) NULL

ALTER TABLE AirportWatchDistinctBoxes
ADD Longitude nvarchar(50) NULL
GO

USE [paragon_test]
GO
/****** Object:  StoredProcedure [dbo].[up_AirportWatchDistinctBoxes_Update]    Script Date: 9/28/2022 11:46:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[up_AirportWatchDistinctBoxes_Update]
AS

DECLARE @pastHalfHour datetime = DATEADD(minute, -30, GETUTCDATE());

--- Insert any new boxes that aren't currently in AirportWatchDistinctBoxes
INSERT INTO AirportWatchDistinctBoxes (BoxName)
SELECT [a].[BoxName]
FROM [AirportWatchLiveData] AS [a]
LEFT JOIN AirportWatchDistinctBoxes ab ON a.BoxName = ab.BoxName
WHERE [a].[AircraftPositionDateTimeUtc] > @pastHalfHour and ab.BoxName is null
GROUP BY [a].[BoxName]

--- Update boxes with live datetimes
UPDATE AirportWatchDistinctBoxes
SET LastLiveDateTime = [AircraftPositionDateTimeUtc]
FROM AirportWatchDistinctBoxes ab
INNER JOIN 
(
    SELECT BoxName, MAX([a].[AircraftPositionDateTimeUtc]) as [AircraftPositionDateTimeUtc]
    FROM [AirportWatchLiveData] AS [a]
    WHERE [a].[AircraftPositionDateTimeUtc] > @pastHalfHour
	GROUP BY BoxName
) a ON a.BoxName = ab.BoxName

-- Update boxes with historical datetimes
UPDATE AirportWatchDistinctBoxes
SET LastHistoricDateTime = [AircraftPositionDateTimeUtc],
	AirportICAO = a.AirportICAO
FROM AirportWatchDistinctBoxes ab
INNER JOIN 
(
    SELECT BoxName, AirportICAO, MAX([a].[AircraftPositionDateTimeUtc]) as [AircraftPositionDateTimeUtc]
    FROM [AirportWatchHistoricalData] AS [a]
    WHERE [a].[AircraftPositionDateTimeUtc] > @pastHalfHour
	GROUP BY BoxName, AirportICAO, Latitude, Longitude
) a ON a.BoxName = ab.BoxName

-- Updated boxes with lat/lng if there's any missing
UPDATE AirportWatchDistinctBoxes
SET Latitude = aa.Latitude, Longitude = aa.Longitude
FROM AirportWatchDistinctBoxes ab
INNER JOIN Dega.dbo.AcukwikAirports aa on (aa.ICAO = ab.AirportICAO or aa.ICAO = ab.AirportICAO or aa.FAA = ab.AirportICAO) and ab.Latitude is null and ab.Longitude is null
GO

USE [paragon_test]
GO

/****** Object:  StoredProcedure [dbo].[up_AirportWatchDistinctBoxes_Update]    Script Date: 9/12/2022 3:36:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[up_AirportWatchDistinctBoxes_Update]
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
SET LastHistoricDateTime = [AircraftPositionDateTimeUtc]
FROM AirportWatchDistinctBoxes ab
INNER JOIN 
(
    SELECT BoxName, MAX([a].[AircraftPositionDateTimeUtc]) as [AircraftPositionDateTimeUtc]
    FROM [AirportWatchHistoricalData] AS [a]
    WHERE [a].[AircraftPositionDateTimeUtc] > @pastHalfHour
	GROUP BY BoxName
) a ON a.BoxName = ab.BoxName

GO



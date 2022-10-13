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

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'CRKF'
WHERE BoxName = 'cykf_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KDAL'
WHERE BoxName = 'kdal_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KDEN'
WHERE BoxName = 'kden_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'kden_a02'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KFRG'
WHERE BoxName = 'kfrg_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KLWB'
WHERE BoxName = 'klwb_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KNKX'
WHERE BoxName = 'knkx_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KOAK'
WHERE BoxName = 'koak_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KOPF'
WHERE BoxName = 'kopf_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KPDX'
WHERE BoxName = 'kpae_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KPWK'
WHERE BoxName = 'kpwk_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KRNO'
WHERE BoxName = 'krno_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'kryy_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KSBA'
WHERE BoxName = 'ksba_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'ksmo_a03'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'ksna_a02'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = 'KTOA'
WHERE BoxName = 'ktoa_a01'

UPDATE AirportWatchDistinctBoxes
SET AirportICAO = NULL
WHERE BoxName = 'mbpv_a01'

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

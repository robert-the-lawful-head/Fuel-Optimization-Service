USE [Dega]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Thain Breese
-- Create date: 3/30/2020
-- Description:	Get airports within mile distance
-- =============================================
ALTER PROCEDURE [dbo].[up_Airports_In_Distance] 
	@ICAO varchar(10),
	@Mile int = null
AS
BEGIN
	
	IF OBJECT_ID('tempdb..#GeoTable') IS NOT NULL DROP TABLE #GeoTable

    SELECT
		*
	  , CASE WHEN substring(Latitude, 1, 1) = 'N' THEN
		   CAST(substring(Latitude, 2, 2) AS numeric(10, 5)) + CAST(substring(Latitude, 5, 2) AS numeric(10, 5))/60 + CAST(substring(Latitude, 8, 2) AS numeric(10, 5))/3600
		ELSE
		   -(CAST(substring(Latitude, 2, 2) AS numeric(10, 5)) + CAST(substring(Latitude, 5, 2) AS numeric(10, 5))/60 + CAST(substring(Latitude, 8, 2) AS numeric(10, 5))/3600)
		END AS lat
      , CASE WHEN substring(Longitude, 1, 1) = 'E' THEN
		   CAST(substring(Longitude, 2, 2) AS numeric(10, 5)) + CAST(substring(Longitude, 5, 2) AS numeric(10, 5))/60 + CAST(substring(Longitude, 8, 2) AS numeric(10, 5))/3600
		ELSE
		   -(CAST(substring(Longitude, 2, 2) AS numeric(10, 5)) + CAST(substring(Longitude, 5, 2) AS numeric(10, 5))/60 + CAST(substring(Longitude, 8, 2) AS numeric(10, 5))/3600)
		END AS long
	INTO #GeoTable
	FROM Dega.dbo.AcukwikAirports;

	DECLARE @OrigLat DECIMAL(10,5);
	DECLARE @OrigLong DECIMAL(10, 5);

	SET @OrigLat = (SELECT lat FROM #GeoTable WHERE ICAO = @ICAO);
	SET @OrigLong = (SELECT long FROM #GeoTable WHERE ICAO = @ICAO);

	SELECT 
	   [Airport_ID]
      ,[ICAO]
      ,[IATA]
      ,[FAA]
      ,[FullAirportName]
      ,[AirportCity]
      ,[State/Subdivision]
      ,[Country]
      ,[AirportType]
      ,[Distance_From_City]
      ,[Latitude]
      ,[Longitude]
      ,[Elevation]
      ,[Variation]
      ,[IntlTimeZone]
      ,[DaylightSavingsYN]
      ,[FuelType]
      ,[AirportOfEntry]
      ,[Customs]
      ,[HandlingMandatory]
      ,[SlotsRequired]
      ,[Open24Hours]
      ,[ControlTowerHours]
      ,[ApproachList]
      ,[PrimaryRunwayID]
      ,[RunwayLength]
      ,[RunwayWidth]
      ,[Lighting]
      ,[AirportNameShort]
	FROM (
		SELECT 
			*
		  , dbo.fn_Calc_Distance_Miles(@OrigLat, @OrigLong, lat, long) AS Distance
		FROM #GeoTable
	) DistanceTable
	WHERE @Mile IS NULL OR Distance < @Mile
END

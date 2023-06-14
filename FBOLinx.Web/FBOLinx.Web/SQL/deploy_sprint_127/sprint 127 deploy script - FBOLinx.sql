USE [paragon_test]
GO

/****** Object:  Table [dbo].[AirportWatchHistoricalParking]    Script Date: 6/1/2023 9:27:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AirportWatchHistoricalParking](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AirportWatchHistoricalDataID] [int] NOT NULL,
	[AcukwikFBOHandlerID] [int] NOT NULL,
	[DateCalculatedUTC] [datetime] NULL,
	[IsConfirmed] [bit] NULL
 CONSTRAINT [PK_AirportWatchHistoricalParking] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [INX_AirportWatchHistoricalParking_AirportWatchHistoricalDataID] on [dbo].AirportWatchHistoricalParking
(
	[AirportWatchHistoricalDataID]
)
INCLUDE ([AcukwikFBOHandlerID], [DateCalculatedUTC], [IsConfirmed])
GO

USE [paragon_test]
GO
CREATE NONCLUSTERED INDEX [INX_Customers_FuelerlinxID]
ON [dbo].[Customers] ([FuelerlinxID])

GO
USE [paragon_test]
GO

/****** Object:  Table [dbo].[ServiceOrderItems]    Script Date: 5/18/2023 6:58:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceOrderItems](
	[OID] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceOrderID] [int] NOT NULL,
	[ServiceName] [varchar](255) NOT NULL,
	[Quantity] int NOT NULL,
	[IsCompleted] [bit] NULL,
	[CompletionDateTimeUtc] [datetime] NULL,
	[CompletedByUserID] int NULL,
	[CompletedByName] varchar(255) NULL
 CONSTRAINT [PK_ServiceOrderItems] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrderItems_ServiceOrderID] on [dbo].[ServiceOrderItems]
(
	ServiceOrderID
)
INCLUDE ([ServiceName], [Quantity], [IsCompleted], [CompletionDateTimeUtc], [CompletedByUserID], [CompletedByName])
GO
USE [paragon_test]
GO

/****** Object:  Table [dbo].[ServiceOrders]    Script Date: 5/18/2023 6:53:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceOrders](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FBOID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[CustomerInfoByGroupID] [int] NOT NULL,
	[ServiceDateTimeUTC] [datetime] NOT NULL,
	[ArrivalDateTimeUTC] [datetime] NOT NULL,
	[DepartureDateTimeUTC] [datetime] NULL,
	[CustomerAircraftID] [int] NOT NULL,
	[AssociatedFuelOrderID] [int] NULL,
	[FuelerLinxTransactionID] [int] NULL,
	[ServiceOn] [smallint] NULL
 CONSTRAINT [PK_ServiceOrders] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_FBOID_ServiceDateTimeUTC] on [dbo].[ServiceOrders]
(
	FBOID,
	ServiceDateTimeUTC
)
INCLUDE (GroupID, CustomerInfoByGroupID, CustomerAircraftID, AssociatedFuelOrderID, FuelerLinxTransactionId, ArrivalDateTimeUTC, DepartureDateTimeUTC, ServiceOn)
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_CustomerInfoByGroupID_ServiceDateTimeUTC] on [dbo].[ServiceOrders]
(
	CustomerInfoByGroupID,
	ServiceDateTimeUTC
)
INCLUDE (FBOID, GroupID, CustomerAircraftID, AssociatedFuelOrderID, FuelerLinxTransactionId, ArrivalDateTimeUTC, DepartureDateTimeUTC, ServiceOn)
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_AssociatedFuelOrderID] on [dbo].[ServiceOrders]
(
	AssociatedFuelOrderID
)
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_FuelerLinxTransactionID] on [dbo].[ServiceOrders]
(
	FuelerLinxTransactionID
)
GO
USE [paragon_test]
GO
/****** Object:  StoredProcedure [dbo].[up_AirportWatchDistinctBoxes_Update]    Script Date: 5/25/2023 2:57:01 PM ******/
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

-- Update new boxes with no ICAOs
UPDATE ad
SET AirportICAO = fa.ICAO
FROM AirportWatchDistinctBoxes ad
INNER JOIN FBOs f ON f.AntennaName = ad.BoxName
INNER JOIN FBOAirports fa on fa.FBOID = f.OID
WHERE ad.AirportICAO is null

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
    SELECT a.BoxName, MAX([a].[AircraftPositionDateTimeUtc]) as [AircraftPositionDateTimeUtc]
    FROM [AirportWatchHistoricalData] AS [a]
	inner join AirportWatchDistinctBoxes b on a.AirportICAO = b.AirportICAO and [a].[AircraftPositionDateTimeUtc] > @pastHalfHour and b.BoxName = a.BoxName
    WHERE [a].[AircraftPositionDateTimeUtc] > @pastHalfHour
	GROUP BY a.BoxName
) a ON a.BoxName = ab.BoxName
GO
GO
use [paragon_test]
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mike Mieglitz
-- Create date: 06/01/2023
-- Description:	Insert/Update parking occurrences from AirportWatchHistoricalData
-- =============================================
CREATE OR ALTER PROCEDURE up_AirportWatchHistoricalParking_Update
	@StartDateTime datetime,
	@EndDateTime datetime,
	@ICAO varchar(10) = null
AS
BEGIN
	--Prepare FBO Geofencing results for review on parking location
	select geography::STGeomFromText('POLYGON((' + ((STUFF((
	select ',' + STR(ag.Longitude, 19, 14) + ' ' + STR(ag.Latitude, 19, 14)
	from paragon_test.dbo.AirportFBOGeoFenceClusters a
	inner join paragon_test.dbo.FBOs f on f.AcukwikFBOHandlerID = a.AcukwikFBOHandlerID
	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates ag on ag.ClusterID = a.OID
	where f.OID = fbo.OID and a.OID = clusterCoord.OID
	order by ag.oid
	FOR XML PATH('')), 1, 1, ''))+ ',' + STR(firstCoordRecord.Longitude, 19, 14) + ' ' + STR(firstCoordRecord.Latitude, 19, 14)) + '))', 4326) as GeoCoordinates, 
	((STUFF((
	select ',' + STR(ag.Longitude, 19, 14) + ' ' + STR(ag.Latitude, 19, 14)
	from paragon_test.dbo.AirportFBOGeoFenceClusters a
	inner join paragon_test.dbo.FBOs f on f.AcukwikFBOHandlerID = a.AcukwikFBOHandlerID
	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates ag on ag.ClusterID = a.OID
	where f.OID = fbo.OID and a.OID = clusterCoord.OID
	order by ag.oid
	FOR XML PATH('')), 1, 1, ''))+ ',' + STR(firstCoordRecord.Longitude, 19, 14) + ' ' + STR(firstCoordRecord.Latitude, 19, 14)) as CoordinatesAsString,
	firstCoord.FBOID,
	fbo.FBO,
	fbo.AcukwikFBOHandlerID,
	fa.ICAO,
	clusterCoord.OID
	into #FBOGeoPolygons
	from paragon_test.dbo.FBOs fbo
	inner join paragon_test.dbo.[Group] g on g.oid = FBO.GroupID
	inner join paragon_test.dbo.FBOAirports fa on fa.FBOID = fbo.OID
	inner join 
	(
	select f.oid as FBOID, f.FBO, min(ag.oid) as FirstCoordRecordID, a.oid as ClusterOID
	from paragon_test.dbo.AirportFBOGeoFenceClusters a
	inner join paragon_test.dbo.FBOs f on f.AcukwikFBOHandlerID = a.AcukwikFBOHandlerID
	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates ag on ag.ClusterID = a.OID
	group by f.oid, f.FBO, a.oid) firstCoord on firstCoord.FBOID = fbo.OID
	inner join paragon_test.dbo.AirportFBOGeoFenceClusters clusterCoord on clusterCoord.OID = firstCoord.ClusterOID
	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates firstCoordRecord on firstCoordRecord.OID = firstCoord.FirstCoordRecordID
	where fbo.GroupID <> 1

	--We have to reverse the polygon direction if the area is too big.  This is due to the left-hand rule SQL uses to fill the polygon.
	update fg
	set fg.GeoCoordinates = fg.GeoCoordinates.MakeValid().ReorientObject()
	from #FBOGeoPolygons fg
	where fg.GeoCoordinates.MakeValid().STArea() > 10000000

	select distinct (case when isnull(ab.AirportICAO, '') = '' then fa.ICAO else ab.AirportICAO end) as ICAO
	into #DistinctAirportsForBoxes
	from paragon_test.dbo.AirportWatchDistinctBoxes ab
	left join paragon_test.dbo.fbos f on f.AntennaName = ab.BoxName
	left join paragon_test.dbo.fboAirports fa on fa.fboid = f.oid
	where (case when isnull(ab.AirportICAO, '') = '' then fa.ICAO else ab.AirportICAO end) is not null
	and (@ICAO is null OR (case when isnull(ab.AirportICAO, '') = '' then fa.ICAO else ab.AirportICAO end) = @ICAO)

	select awParking.OID, --awParking.AirportICAO, awParking.AircraftPositionDateTimeUTC, awParking.AircraftHexCode, awParking.AtcFlightNumber, 
	awParking.Latitude,
	awParking.Longitude,
	aa.ICAO
	into #AirportWatchHistoricalResults
	from paragon_test.dbo.AirportWatchHistoricalData awParking 
	inner join #DistinctAirportsForBoxes aa on aa.ICAO = awParking.AirportICAO 
	where 
	awParking.AirportIcao = aa.ICAO
	and awParking.AircraftPositionDateTimeUTC > @StartDateTime and awParking.AircraftPositionDateTimeUTC < @EndDateTime
	and awParking.AircraftStatus = 2

	drop table #DistinctAirportsForBoxes

	CREATE NONCLUSTERED INDEX [inx_TempAirportWatchHistoricalResults_OID]
	ON [#AirportWatchHistoricalResults] ([OID])
	INCLUDE ([Latitude],[Longitude],[ICAO])

	update awp
	set AcukwikFBOHandlerID = fboGeo.AcukwikFBOHandlerID,
	DateCalculatedUTC = getutcdate()
	from #AirportWatchHistoricalResults a
	inner join #FBOGeoPolygons fboGeo on fboGeo.ICAO = a.ICAO and a.Longitude is not null and  fboGeo.GeoCoordinates.MakeValid().STContains(geography::STGeomFromText('POINT(' + convert(varchar, a.Longitude) + ' ' + convert(varchar, a.Latitude) + ')', 4326).MakeValid()) = 1
	inner join [AirportWatchHistoricalParking] awp on awp.AirportWatchHistoricalDataID = a.OID
	where awp.IsConfirmed is null

	insert into [AirportWatchHistoricalParking] (AirportWatchHistoricalDataID, AcukwikFBOHandlerID, DateCalculatedUTC)
	select a.OID, fboGeo.AcukwikFBOHandlerID, getutcdate()
	from #AirportWatchHistoricalResults a
	inner join #FBOGeoPolygons fboGeo on fboGeo.ICAO = a.ICAO and a.Longitude is not null and  fboGeo.GeoCoordinates.MakeValid().STContains(geography::STGeomFromText('POINT(' + convert(varchar, a.Longitude) + ' ' + convert(varchar, a.Latitude) + ')', 4326).MakeValid()) = 1
	left join [AirportWatchHistoricalParking] awp on awp.AirportWatchHistoricalDataID = a.OID
	where awp.oid is null


	drop table #AirportWatchHistoricalResults
	drop table #FBOGeoPolygons
END
GO
use [paragon_test]
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mike Mieglitz
-- Create date: 06/01/2023
-- Description:	Update 
-- =============================================
CREATE OR ALTER PROCEDURE up_ScheduledJobs_AirportWatchHistoricalParking
	
AS
BEGIN
	declare @StartDateTime datetime = DATEADD(dd, -365, getdate())
	declare @EndDateTime datetime = getdate()
	exec up_AirportWatchHistoricalParking_Update @StartDateTime, @EndDateTime
END
GO
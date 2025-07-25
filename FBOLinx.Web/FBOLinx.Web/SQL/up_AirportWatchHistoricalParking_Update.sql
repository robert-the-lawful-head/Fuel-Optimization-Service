USE [paragon_test]
GO
/****** Object:  StoredProcedure [dbo].[up_AirportWatchHistoricalParking_Update]    Script Date: 4/3/2024 3:33:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mike Mieglitz
-- Create date: 06/01/2023
-- Description:	Insert/Update parking occurrences from AirportWatchHistoricalData
-- =============================================
ALTER PROCEDURE [dbo].[up_AirportWatchHistoricalParking_Update]
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
	inner join paragon_test.dbo.AirportWatchDistinctBoxes ab on ab.AirportICAO = fa.ICAO
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

	select awParking.OID,
	awParking.Latitude,
	awParking.Longitude,
	aa.ICAO,
	geography::STGeomFromText('POINT(' + convert(varchar, awParking.Longitude) + ' ' + convert(varchar, awParking.Latitude) + ')', 4326).MakeValid() as GeoLocation,
	0 as AcukwikFBOHandlerID
	INTO #AirportWatchHistoricalResults
	from paragon_test.dbo.AirportWatchHistoricalData awParking with (nolock) 
	inner join #DistinctAirportsForBoxes aa on aa.ICAO = awParking.AirportICAO 
	where awParking.AircraftPositionDateTimeUTC > @StartDateTime and awParking.AircraftPositionDateTimeUTC < @EndDateTime
	and awParking.AircraftStatus = 2
	and awParking.Longitude is not null

	drop table #DistinctAirportsForBoxes

	update a
	set AcukwikFBOHandlerID = fboGeo.AcukwikFBOHandlerID
	from #AirportWatchHistoricalResults a
	inner join #FBOGeoPolygons fboGeo on fboGeo.ICAO = a.ICAO and a.Longitude is not null 										
										and fboGeo.GeoCoordinates.MakeValid().STContains(a.GeoLocation) = 1

	update awp
	set AcukwikFBOHandlerID = a.AcukwikFBOHandlerID,
	DateCalculatedUTC = getutcdate()
	from [AirportWatchHistoricalParking] awp with (nolock)	
	inner join #AirportWatchHistoricalResults a on a.OID = awp.AirportWatchHistoricalDataID
	where awp.IsConfirmed is null

	insert into [AirportWatchHistoricalParking] (AirportWatchHistoricalDataID, AcukwikFBOHandlerID, DateCalculatedUTC)
	select a.OID, a.AcukwikFBOHandlerID, getutcdate()
	from #AirportWatchHistoricalResults a
	left join [AirportWatchHistoricalParking] awp with (nolock) on awp.AirportWatchHistoricalDataID = a.OID
	where awp.oid is null


	drop table #AirportWatchHistoricalResults
	drop table #FBOGeoPolygons
END

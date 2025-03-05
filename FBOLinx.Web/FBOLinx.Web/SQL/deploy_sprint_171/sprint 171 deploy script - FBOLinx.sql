use paragon_Test
GO
--ALTER TABLE [dbo].[FBOFeesAndTaxes] ADD IsOmitted BIT NULL
--GO
--USE [paragon_test]
--GO
/****** Object:  StoredProcedure [dbo].[up_Insert_HistoricalAirportWatchSwimFlightLeg]    Script Date: 2/21/2025 1:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------------------------------------------------------
-- Created By:	Chau Ly
-- Date Created: 5/8/2024
------------------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE [dbo].[up_Insert_HistoricalAirportWatchSwimFlightLeg]
	
AS

DECLARE @StartDate AS DATETIME
DECLARE @EndDate AS DATETIME

SET @StartDate = DATEADD(MINUTE,-30,GETUTCDATE())
SET @EndDate = GETUTCDATE()

select ah.oid as AirportWatchHistoricalDataId, TailNumber, AircraftPositionDateTimeUtc, ah.AirportICAO, fa.IATA, ah.AtcFlightNumber
--, tailnumber, atd, eta, departureicao, arrivalicao,ah.airporticao
INTO #TempAirportWatchHistoricalData
from airportwatchhistoricaldata ah
inner join airportwatchdistinctboxes a on ah.airporticao=a.airporticao
left join (select distinct iata, icao from fboairports) fa on a.airporticao=fa.icao
left join HistoricalAirportWatchSwimFlightLeg h on ah.oid = h.airportwatchhistoricaldataid
where h.oid is null and aircraftpositiondatetimeutc BETWEEN @StartDate AND @EndDate

select OID
INTO #TempSwimFlightLegsID
from #TempAirportWatchHistoricalData ah
inner join flightdata.dbo.swimflightlegs sf on ah.TailNumber=sf.aircraftidentification
where sf.ATD BETWEEN dateadd(hh,-12,ah.aircraftpositiondatetimeutc) AND dateadd(hh,12,ah.aircraftpositiondatetimeutc) and isnull(isplaceholder,0)=0

select sf.eta, sf.atd, sf.departureicao, sf.arrivalicao, sf.oid, aircraftidentification
INTO #TempSwimFlightLegs
from flightdata.dbo.swimflightlegs sf
inner join #TempSwimFlightLegsID tsf on sf.OID=tsf.OID

INSERT INTO #TempSwimFlightLegsID (OID)
select sf.OID
from #TempAirportWatchHistoricalData ah
inner join flightdata.dbo.swimflightlegs sf on ah.AtcFlightNumber=sf.aircraftidentification
where sf.ATD BETWEEN dateadd(hh,-12,ah.aircraftpositiondatetimeutc) AND dateadd(hh,12,ah.aircraftpositiondatetimeutc) and isnull(isplaceholder,0)=0

insert into HistoricalAirportWatchSwimFlightLeg
select ah.AirportWatchHistoricalDataId, sf.oid as SwimFlightLegId
from #TempAirportWatchHistoricalData ah
inner join #TempSwimFlightLegs sf on (ah.TailNumber=sf.aircraftidentification or ah.atcflightnumber=sf.aircraftidentification)
left join HistoricalAirportWatchSwimFlightLeg hsf on sf.oid=hsf.swimflightlegid and ah.AirportWatchHistoricalDataId=hsf.AirportWatchHistoricalDataId
where hsf.oid is null and (ah.aircraftpositiondatetimeutc BETWEEN dateadd(hh,-1,sf.ATD) AND dateadd(hh,1,sf.ETA))
and (arrivalicao=ah.airporticao or arrivalicao=ah.IATA)
group by ah.AirportWatchHistoricalDataId, sf.oid

drop table #TempAirportWatchHistoricalData
drop table #TempSwimFlightLegsID
drop table #TempSwimFlightLegs
GO
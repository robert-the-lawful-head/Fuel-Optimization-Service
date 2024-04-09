use [paragon_test]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mike Mieglitz
-- Create date: 05/09/2023
-- Description:	Sync orders from FuelerLinx to FBOLinx and keep statistics up-to-date
-- =============================================
CREATE OR ALTER PROCEDURE up_ScheduledJob_SyncFuelerLinxOrdersAndStats
	
AS
BEGIN
	declare @CutoffDate datetime = dateadd(dd, -30, getdate())

	--The goal of this procedure is to ensure we keep FBOLinx orders up-to-date with info from the flight department.
	--If an order has changed and it's no longer a direct deal or the tail has changed locations/FBOs then we must remove it from FBOLinx.

	--First grab all orders in FBOLinx that still appear to be valid
	select frFBO.OID as FBOLinxOrderID,
	fr.OID as FuelerLinxID,
	fr.ICAO, 
	fr.FBO as FuelerLinxFBO,
	fr.eta as ArrivalDateTime,
	frFBO.FBOID,
	fr.FuelerID as FuelerLinxFuelerID,
	fr.etd as DepartureDateTime,
	fr.ActualPPG,
	fr.ActualVolume,
	fr.Cancelled,
	fr.tail_number as TailNumber,
	frFBO.CustomerID
	into #OrdersToConsider
	from paragon_test.dbo.fuelreq frFBO
	left join sqlfl.dbo.fuelreq fr on fr.oid = frFBO.SourceID
	left join sqlfl.dbo.Companies c on c.oid = fr.CompanyID
	where frFBO.ETA >= @CutoffDate
	and frFBO.SourceID > 0
	
	select *
	into #ValidOrders
	from
	(select orderInfo.FBOLinxOrderID,
		orderInfo.ICAO, 
		fbo.FBO as FBOLinxFBO,
		orderInfo.FuelerLinxFBO,
		case when isnull(afl.AcukwikFBOHandlerName, '') = '' then 'Other' else afl.AcukwikFBOHandlerName end as AcukwikFBOHandlerName, 
		orderInfo.ArrivalDateTime,	
		fbo.AcukwikFBOHandlerID as FBOHandlerID,
		afl.AcukwikFBOHandlerID as FuelerLinxHandlerID,
		orderInfo.DepartureDateTime,
		orderInfo.ActualPPG,
		orderInfo.ActualVolume,
		orderInfo.Cancelled,
		orderInfo.TailNumber,
		ca.OID as CustomerAircraftID
	from #OrdersToConsider orderInfo
	left join paragon_test.dbo.FBOs fbo on fbo.oid = orderInfo.FBOID
	left join paragon_test.dbo.FBOAirports fa on fa.FBOID = fbo.OID
	left join paragon_test.dbo.CustomerAircrafts ca on ca.CustomerID = orderInfo.CustomerID and ca.GroupID = fbo.GroupID and ca.TailNumber = orderInfo.TailNumber
	left join sqlfl.dbo.AcukwikFBOHandlerLookup afl on afl.icao = orderInfo.ICAO and afl.FboHandlerName = orderInfo.FuelerLinxFBO
	left join sqlfl.dbo.fuelerList fl on fl.oid = orderInfo.FuelerLinxFuelerID
	where (fl.OID is null or fl.processnm in ('dynamic', 'fbolinx') or fl.FBOLinxID > 1)
	and orderInfo.ICAO = fa.ICAO
	and fbo.AcukwikFBOHandlerID = afl.AcukwikFBOHandlerID) ordersToReview
	where ordersToReview.FBOHandlerID = ordersToReview.FuelerLinxHandlerID

	--Then grab all invalid orders from FBOLinx that are missing from our valid orders list
	select frFBO.FBOLinxOrderID
	into #InvalidOrders
	from #OrdersToConsider frFBO
	left join #ValidOrders validOrders on validOrders.FBOLinxOrderID = frFBO.FBOLinxOrderID
	where validOrders.FBOLinxOrderID is null

	--Update the valid orders with the new information from FuelerLinx
	update frFBO
	set frFBO.ETA = ValidOrders.ArrivalDateTime,
	frFBO.ETD = ValidOrders.DepartureDateTime,
	frFBO.ActualPPG = ValidOrders.ActualPPG,
	frFBO.ActualVolume = ValidOrders.ActualVolume,
	frFBO.CustomerAircraftID = ValidOrders.CustomerAircraftID,
	frFBO.Cancelled = ValidOrders.Cancelled
	from paragon_test.dbo.fuelreq frFBO
	inner join #ValidOrders ValidOrders on ValidOrders.FBOLinxOrderID = frFBO.OID

	--Mark all invalid orders as cancelled so we keep a record of it
	delete frFBO
	from paragon_test.dbo.fuelreq frFBO
	inner join #InvalidOrders invalidOrders on invalidOrders.FBOLinxOrderID = frFBO.OID

	drop table #ValidOrders
	drop table #InvalidOrders
	drop table #OrdersToConsider
END
GO

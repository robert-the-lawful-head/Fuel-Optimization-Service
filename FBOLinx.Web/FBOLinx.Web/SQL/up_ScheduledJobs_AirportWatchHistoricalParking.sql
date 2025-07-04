USE [paragon_test]
GO
/****** Object:  StoredProcedure [dbo].[up_ScheduledJobs_AirportWatchHistoricalParking]    Script Date: 12/1/2023 10:52:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mike Mieglitz
-- Create date: 06/01/2023
-- Description:	Update 
-- =============================================
ALTER   PROCEDURE [dbo].[up_ScheduledJobs_AirportWatchHistoricalParking]
	
AS
BEGIN
	declare @StartDateTime datetime = DATEADD(dd, -30, getdate())
	declare @EndDateTime datetime = getdate()
	exec up_AirportWatchHistoricalParking_Update @StartDateTime, @EndDateTime
END

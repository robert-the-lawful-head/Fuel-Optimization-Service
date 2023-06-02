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

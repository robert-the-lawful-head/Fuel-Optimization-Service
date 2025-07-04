USE [paragon_test]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[up_Clear_PullHistoryFlag]
as

begin
declare @CurrentTimeStamp datetime
declare @PastFiveMinutes datetime
SELECT @CurrentTimeStamp = GETUTCDATE(), @PastFiveMinutes = dateadd(MINUTE, -5, GETUTCDATE())



delete ph
from fboprices fp
inner join fboairports fa on fa.fboid = fp.fboid
inner join sqlfl.dbo.pullhistory ph on ph.icao = fa.icao
where effectivefrom >= @PastFiveMinutes and effectivefrom <= @CurrentTimeStamp and fp.product='JetA Retail'

end

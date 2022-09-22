USE [paragon_test]
GO

ALTER TABLE FBOPrices
ADD Source smallint;
GO

update fboprices set source=1 where EffectiveTo>='9999-12-31'
update fboprices set source=0 where EffectiveTo<'9999-12-31'
GO
/****** Object:  Table [dbo].[IntegrationStatus]    Script Date: 9/21/2022 10:53:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IntegrationStatus](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[IntegrationPartnerId] [int] NOT NULL,
	[FboId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_IntegrationStatus] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE AirportWatchDistinctBoxes
ADD AirportICAO varchar(10) null
GO
CREATE NONCLUSTERED INDEX [INX_CustomerAircrafts_GroupAndTail]
ON [dbo].[CustomerAircrafts] ([GroupID],[TailNumber])
INCLUDE ([CustomerID],[AircraftID],[Size],[BasedPAGLocation],[NetworkCode],[AddedFrom])
GO
/****** Object:  Table [dbo].[AirportWatchDistinctBoxes]    Script Date: 9/12/2022 11:25:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AirportWatchDistinctBoxes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[BoxName] [varchar](50) NULL,
	[LastLiveDateTime] [datetime] NULL,
	[LastHistoricDateTime] [datetime] NULL,
 CONSTRAINT [PK_AirportWatchDistinctBoxes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO AirportWatchDistinctBoxes (BoxName, LastHistoricDateTime)
SELECT [a].[BoxName], MAX([a].[AircraftPositionDateTimeUtc]) AS [AircraftPositionDateTimeUtc]
FROM [AirportWatchHistoricalData] AS [a]
GROUP BY [a].[BoxName]
GO
/****** Object:  StoredProcedure [dbo].[up_AirportWatchDistinctBoxes_Update]    Script Date: 9/15/2022 3:29:46 PM ******/
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
SET LastHistoricDateTime = [AircraftPositionDateTimeUtc],
	AirportICAO = a.AirportICAO
FROM AirportWatchDistinctBoxes ab
INNER JOIN 
(
    SELECT BoxName, AirportICAO, MAX([a].[AircraftPositionDateTimeUtc]) as [AircraftPositionDateTimeUtc]
    FROM [AirportWatchHistoricalData] AS [a]
    WHERE [a].[AircraftPositionDateTimeUtc] > @pastHalfHour
	GROUP BY BoxName, AirportICAO
) a ON a.BoxName = ab.BoxName
GO

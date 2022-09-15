USE [paragon_test]
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
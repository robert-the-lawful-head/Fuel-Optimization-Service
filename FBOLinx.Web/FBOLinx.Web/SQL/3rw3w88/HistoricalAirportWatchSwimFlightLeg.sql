USE [paragon_test]
GO

/****** Object:  Table [dbo].[HistoricalAirportWatchSwimFlightLeg]    Script Date: 4/15/2024 4:39:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HistoricalAirportWatchSwimFlightLeg](
	[OID] [bigint] NOT NULL IDENTITY(1,1),
	[AirportWatchHistoricalDataId] [int] NULL,
	[SwimFlightLegId] [bigint] NULL,
 CONSTRAINT [PK_HistoricalAirportWatchSwimFlightLeg] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE UNIQUE INDEX INX_HistoricalAirportWatchSwimFlightLeg_AirportWatchHistoricalDataIdSwimFlightLegId
ON HistoricalAirportWatchSwimFlightLeg (AirportWatchHistoricalDataId, SwimFlightLegId)
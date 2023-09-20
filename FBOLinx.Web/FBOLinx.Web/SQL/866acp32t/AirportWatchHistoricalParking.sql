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
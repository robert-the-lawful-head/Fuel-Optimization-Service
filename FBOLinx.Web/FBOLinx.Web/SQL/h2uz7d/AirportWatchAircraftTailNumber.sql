CREATE TABLE [dbo].[AirportWatchAircraftTailNumber](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AircraftHexCode] [varchar](10) NOT NULL,
	[AtcFlightNumber] [varchar](20) NULL,
 CONSTRAINT [PK_AirportWatchAircraftTailNumber] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

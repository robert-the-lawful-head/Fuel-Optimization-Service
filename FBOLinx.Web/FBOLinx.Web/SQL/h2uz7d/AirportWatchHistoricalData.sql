CREATE TABLE [dbo].[AirportWatchHistoricalData](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[BoxTransmissionDateTimeUtc] [datetime2](7) NOT NULL,
	[AircraftHexCode] [varchar](10) NOT NULL,
	[AtcFlightNumber] [varchar](20) NOT NULL,
	[AltitudeInStandardPressure] [int] NULL,
	[GroundSpeedKts] [int] NULL,
	[TrackingDegree] [float] NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[VerticalSpeedKts] [int] NULL,
	[TransponderCode] [int] NULL,
	[BoxName] [varchar](25) NULL,
	[AircraftPositionDateTimeUtc] [datetime2](7) NOT NULL,
	[AircraftTypeCode] [varchar](3) NULL,
	[GpsAltitude] [int] NULL,
	[IsAircraftOnGround] [bit] NOT NULL,
	[AircraftStatus] [smallint] NOT NULL,
 CONSTRAINT [PK_AirportWatchHistoricalData] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



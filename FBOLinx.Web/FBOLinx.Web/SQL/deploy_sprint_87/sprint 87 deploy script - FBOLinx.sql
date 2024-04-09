USE [paragon_test]
GO
CREATE TABLE [dbo].[AirportFBOGeoFenceClusters](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AcukwikAirportID] [int] NULL,
	[AcukwikFBOHandlerID] [int] NULL,
	[CenterLatitude] [float] NULL,
	[CenterLongitude] [float] NULL)
 GO
 CREATE TABLE [dbo].[AirportFBOGeoFenceClusterCoordinates](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[ClusterID] [int] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL)
GO
USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FuelReqConfirmation](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[SourceId] [int] NOT NULL,
	[IsConfirmed] [bit] NOT NULL
) ON [PRIMARY]
GO


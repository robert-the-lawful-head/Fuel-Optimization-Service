USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboCustomServicesAndFees](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceActionType] [smallint] NOT NULL,
	[FboId] [int] NOT NULL,
	[AcukwikServicesOfferedId] [int] NULL,
	[Service] [varchar](100) NULL,
	[ServiceType] [varchar](100) NULL,
 CONSTRAINT [PK_FboCustomServicesAndFees] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



USE [paragon_test]
GO

/****** Object:  Table [dbo].[ServiceOrderItems]    Script Date: 5/18/2023 6:58:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceOrderItems](
	[OID] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceOrderID] [int] NOT NULL,
	[ServiceName] [varchar](255) NOT NULL,
	[Quantity] int NOT NULL,
	[IsCompleted] [bit] NULL,
	[CompletionDateTimeUtc] [datetime] NULL,
	[CompletedByUserID] int NULL,
	[CompletedByName] varchar(255) NULL
 CONSTRAINT [PK_ServiceOrderItems] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

DROP INDEX [INX_ServiceOrderItems_ServiceOrderID] on [dbo].[ServiceOrderItems]
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrderItems_ServiceOrderID] on [dbo].[ServiceOrderItems]
(
	ServiceOrderID
)
INCLUDE ([ServiceName], [Quantity], [IsCompleted], [CompletionDateTimeUtc], [CompletedByUserID], [CompletedByName])
GO
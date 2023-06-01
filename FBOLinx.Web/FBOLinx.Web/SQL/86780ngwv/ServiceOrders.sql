USE [paragon_test]
GO

/****** Object:  Table [dbo].[ServiceOrders]    Script Date: 5/18/2023 6:53:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceOrders](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FBOID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[CustomerInfoByGroupID] [int] NOT NULL,
	[ArrivalDateTimeUTC] [datetime] NOT NULL,
	[DepartureDateTimeUTC] [datetime] NOT NULL,
	[CustomerAircraftID] [int] NOT NULL,
	[AssociatedFuelOrderID] [int] NULL,
	[FuelerLinxTransactionID] [int] NULL
 CONSTRAINT [PK_ServiceOrders] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_FBOID_ServiceDateTimeUTC] on [dbo].[ServiceOrders]
(
	FBOID,
	ArrivalDateTimeUTC
)
INCLUDE (GroupID, CustomerInfoByGroupID, CustomerAircraftID, AssociatedFuelOrderID, FuelerLinxTransactionId, DepartureDateTimeUTC)
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_CustomerInfoByGroupID_ServiceDateTimeUTC] on [dbo].[ServiceOrders]
(
	CustomerInfoByGroupID,
	ArrivalDateTimeUTC
)
INCLUDE (FBOID, GroupID, CustomerAircraftID, AssociatedFuelOrderID, FuelerLinxTransactionId, DepartureDateTimeUTC)
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_AssociatedFuelOrderID] on [dbo].[ServiceOrders]
(
	AssociatedFuelOrderID
)
GO

CREATE NONCLUSTERED INDEX [INX_ServiceOrders_FuelerLinxTransactionID] on [dbo].[ServiceOrders]
(
	FuelerLinxTransactionID
)
GO
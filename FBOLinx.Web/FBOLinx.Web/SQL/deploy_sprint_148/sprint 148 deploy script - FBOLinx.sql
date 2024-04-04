USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboCustomServicesAndFees](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceActionType] [smallint] NOT NULL,
	[FboId] [int] NULL,
	[AcukwikServicesOfferedId] [int] NULL,
	[Service] [varchar](100) NULL,
	[ServiceTypeId] [int] NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_FboCustomServicesAndFees] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FboCustomServicesAndFees]  WITH CHECK ADD  CONSTRAINT [FK_FboCustomServicesAndFees_FboCustomServiceTypes] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[User] ([OID])
GO

ALTER TABLE [dbo].[FboCustomServicesAndFees] CHECK CONSTRAINT [FK_FboCustomServicesAndFees_FboCustomServiceTypes]
GO
USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboCustomServiceTypes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[FboId] [int] NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_FboCustomServiceTypes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FboCustomServiceTypes]  WITH CHECK ADD  CONSTRAINT [FK_FboCustomServiceTypes_User] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[User] ([OID])
GO

ALTER TABLE [dbo].[FboCustomServiceTypes] CHECK CONSTRAINT [FK_FboCustomServiceTypes_User]
GO
USE [paragon_test]
GO

ALTER TABLE [paragon_test].[dbo].[FBOPreferences]
ADD DirectOrdersNotificationsEnabled bit;
GO
USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FuelReqConfirmation](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[SourceId] [int] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	AssociatedFuelOrderId int null
) ON [PRIMARY]
GO
USE [paragon_test]
GO

/****** Object:  Table [dbo].[OrderDetails]    Script Date: 6/26/2023 5:34:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderDetails](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FuelerLinxTransactionId] [int] NOT NULL,
	[ConfirmationEmail] [varchar](255) NOT NULL,
	[FuelVendor] [varchar](50) NOT NULL,
	[PaymentMethod] [varchar](255) NULL,
DateTimeUpdated datetime2(7) NULL,
IsEmailSent bit NULL,
DateTimeEmailSent datetime2(7) NULL,
QuotedVolume float NULL,
CustomerAircraftID int NULL,
ETA datetime2(7) NULL,
FboHandlerId int NULL,
IsCancelled bit NULL,
AssociatedFuelOrderId int null,
IsArchived bit null,
TimeStandard char(1) null,
IsOkToEmail bit null

 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [paragon_test]
GO

CREATE UNIQUE NONCLUSTERED INDEX [INX_OrderDetails_FuelerLinxTransactionIdFboHandlerIdAssociatedFuelOrderId] ON [dbo].[OrderDetails]
(
	[FuelerLinxTransactionId] ASC,
	[FboHandlerId] ASC,
	AssociatedFuelOrderId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

USE [paragon_test]
GO

/****** Object:  Table [dbo].[AcukwikServicesOfferedDefaults]    Script Date: 11/22/2023 5:02:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AcukwikServicesOfferedDefaults](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AcukwikServicesOfferedId] [int] NOT NULL,
	[Service] [nvarchar](255) NULL,
	[Omit] [bit] NULL
) ON [PRIMARY]
GO


SET IDENTITY_INSERT [dbo].[AcukwikServicesOfferedDefaults] ON 
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (1, 1, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (2, 2, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (3, 3, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (4, 4, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (5, 5, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (6, 6, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (7, 7, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (8, 8, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (9, 9, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (10, 10, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (11, 11, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (12, 12, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (13, 13, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (14, 14, N'(ASU) Air Start Unit', NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (15, 15, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (16, 16, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (17, 17, N'(GPU) Ground Power Unit', NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (18, 18, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (19, 19, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (20, 20, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (21, 21, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (22, 22, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (23, 23, N'(LAV) Toilet Service', NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (24, 24, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (25, 25, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (26, 26, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (27, 27, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (28, 28, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (29, 29, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (30, 30, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (31, 31, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (32, 32, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (33, 33, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (34, 34, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (35, 35, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (36, 36, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (37, 37, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (38, 38, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (39, 39, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (40, 40, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (41, 41, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (42, 42, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (43, 43, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (44, 44, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (45, 45, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (46, 46, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (47, 47, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (48, 48, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (49, 49, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (50, 50, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (51, 51, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (52, 52, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (53, 53, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (54, 54, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (55, 55, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (56, 56, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (57, 57, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (58, 58, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (59, 59, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (60, 60, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (61, 61, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (62, 63, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (63, 64, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (64, 65, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (65, 66, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (66, 67, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (67, 68, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (68, 69, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (69, 70, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (70, 71, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (71, 72, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (72, 73, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (73, 74, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (74, 75, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (75, 76, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (76, 77, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (77, 78, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (78, 79, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (79, 80, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (80, 81, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (81, 82, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (82, 83, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (83, 84, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (84, 85, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (85, 86, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (86, 87, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (87, 88, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (88, 89, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (89, 90, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (90, 91, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (91, 92, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (92, 93, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (93, 94, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (94, 95, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (95, 96, NULL, NULL)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (96, 97, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (97, 98, NULL, 1)
GO
INSERT [dbo].[AcukwikServicesOfferedDefaults] ([OID], [AcukwikServicesOfferedId], [Service], [Omit]) VALUES (98, 99, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[AcukwikServicesOfferedDefaults] OFF
GO





UPDATE AcukwikServicesOfferedDefaults
SET Service='(ASU) Air Start Unit'
WHERE AcukwikServicesOfferedId=14

UPDATE AcukwikServicesOfferedDefaults
SET Service='(GPU) Ground Power Unit'
WHERE AcukwikServicesOfferedId=17

UPDATE AcukwikServicesOfferedDefaults
SET Service='(LAV) Toilet Service'
WHERE AcukwikServicesOfferedId=23
GO
USE [paragon_Test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderNotes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AssociatedFuelOrderId] [int] NULL,
	[AssociatedServiceOrderId] [int] NULL,
	[AssociatedFuelerLinxTransactionId] [int] NULL,
	[DateAdded] [datetime2](7) NULL,
	[Note] [varchar](max) NULL,
	[AddedByUserID] [int] NULL,
	[AddedByName] [varchar](255) NULL,
	[TimeZone] [varchar](100) NULL,
 CONSTRAINT [PK_OrderNotes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [INX_OrderNotes_AssociatedFuelOrderID] ON [dbo].[OrderNotes]
(
	[AssociatedFuelOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [INX_OrderNotes_AssociatedServiceOrderId] ON [dbo].[OrderNotes]
(
	[AssociatedServiceOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [INX_OrderNotes_AssociatedFuelerLinxTransactionId] ON [dbo].[OrderNotes]
(
	[AssociatedFuelerLinxTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE ServiceOrderItems
ADD
AddedByUserID int NULL,
AddedByName varchar(255) NULL,
ServiceNotes varchar(MAX) null
GO
--USE [paragon_test]
--GO
--/****** Object:  StoredProcedure [dbo].[up_AirportWatchHistoricalParking_Update]    Script Date: 4/3/2024 3:33:28 PM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
---- =============================================
---- Author:		Mike Mieglitz
---- Create date: 06/01/2023
---- Description:	Insert/Update parking occurrences from AirportWatchHistoricalData
---- =============================================
--ALTER PROCEDURE [dbo].[up_AirportWatchHistoricalParking_Update]
--	@StartDateTime datetime,
--	@EndDateTime datetime,
--	@ICAO varchar(10) = null
--AS
--BEGIN

--	--Prepare FBO Geofencing results for review on parking location
--	select geography::STGeomFromText('POLYGON((' + ((STUFF((
--	select ',' + STR(ag.Longitude, 19, 14) + ' ' + STR(ag.Latitude, 19, 14)
--	from paragon_test.dbo.AirportFBOGeoFenceClusters a
--	inner join paragon_test.dbo.FBOs f on f.AcukwikFBOHandlerID = a.AcukwikFBOHandlerID
--	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates ag on ag.ClusterID = a.OID
--	where f.OID = fbo.OID and a.OID = clusterCoord.OID
--	order by ag.oid
--	FOR XML PATH('')), 1, 1, ''))+ ',' + STR(firstCoordRecord.Longitude, 19, 14) + ' ' + STR(firstCoordRecord.Latitude, 19, 14)) + '))', 4326) as GeoCoordinates, 
--	((STUFF((
--	select ',' + STR(ag.Longitude, 19, 14) + ' ' + STR(ag.Latitude, 19, 14)
--	from paragon_test.dbo.AirportFBOGeoFenceClusters a
--	inner join paragon_test.dbo.FBOs f on f.AcukwikFBOHandlerID = a.AcukwikFBOHandlerID
--	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates ag on ag.ClusterID = a.OID
--	where f.OID = fbo.OID and a.OID = clusterCoord.OID
--	order by ag.oid
--	FOR XML PATH('')), 1, 1, ''))+ ',' + STR(firstCoordRecord.Longitude, 19, 14) + ' ' + STR(firstCoordRecord.Latitude, 19, 14)) as CoordinatesAsString,
--	firstCoord.FBOID,
--	fbo.FBO,
--	fbo.AcukwikFBOHandlerID,
--	fa.ICAO,
--	clusterCoord.OID
--	into #FBOGeoPolygons
--	from paragon_test.dbo.FBOs fbo
--	inner join paragon_test.dbo.[Group] g on g.oid = FBO.GroupID
--	inner join paragon_test.dbo.FBOAirports fa on fa.FBOID = fbo.OID
--	inner join 
--	(
--	select f.oid as FBOID, f.FBO, min(ag.oid) as FirstCoordRecordID, a.oid as ClusterOID
--	from paragon_test.dbo.AirportFBOGeoFenceClusters a
--	inner join paragon_test.dbo.FBOs f on f.AcukwikFBOHandlerID = a.AcukwikFBOHandlerID
--	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates ag on ag.ClusterID = a.OID
--	group by f.oid, f.FBO, a.oid) firstCoord on firstCoord.FBOID = fbo.OID
--	inner join paragon_test.dbo.AirportFBOGeoFenceClusters clusterCoord on clusterCoord.OID = firstCoord.ClusterOID
--	inner join paragon_test.dbo.AirportFBOGeoFenceClusterCoordinates firstCoordRecord on firstCoordRecord.OID = firstCoord.FirstCoordRecordID
--	inner join paragon_test.dbo.AirportWatchDistinctBoxes ab on ab.AirportICAO = fa.ICAO
--	where fbo.GroupID <> 1

--	--We have to reverse the polygon direction if the area is too big.  This is due to the left-hand rule SQL uses to fill the polygon.
--	update fg
--	set fg.GeoCoordinates = fg.GeoCoordinates.MakeValid().ReorientObject()
--	from #FBOGeoPolygons fg
--	where fg.GeoCoordinates.MakeValid().STArea() > 10000000

--	select distinct (case when isnull(ab.AirportICAO, '') = '' then fa.ICAO else ab.AirportICAO end) as ICAO
--	into #DistinctAirportsForBoxes
--	from paragon_test.dbo.AirportWatchDistinctBoxes ab
--	left join paragon_test.dbo.fbos f on f.AntennaName = ab.BoxName
--	left join paragon_test.dbo.fboAirports fa on fa.fboid = f.oid
--	where (case when isnull(ab.AirportICAO, '') = '' then fa.ICAO else ab.AirportICAO end) is not null
--	and (@ICAO is null OR (case when isnull(ab.AirportICAO, '') = '' then fa.ICAO else ab.AirportICAO end) = @ICAO)

--	select awParking.OID,
--	awParking.Latitude,
--	awParking.Longitude,
--	aa.ICAO,
--	geography::STGeomFromText('POINT(' + convert(varchar, awParking.Longitude) + ' ' + convert(varchar, awParking.Latitude) + ')', 4326).MakeValid() as GeoLocation,
--	0 as AcukwikFBOHandlerID
--	INTO #AirportWatchHistoricalResults
--	from paragon_test.dbo.AirportWatchHistoricalData awParking with (nolock) 
--	inner join #DistinctAirportsForBoxes aa on aa.ICAO = awParking.AirportICAO 
--	where awParking.AircraftPositionDateTimeUTC > @StartDateTime and awParking.AircraftPositionDateTimeUTC < @EndDateTime
--	and awParking.AircraftStatus = 2
--	and awParking.Longitude is not null

--	drop table #DistinctAirportsForBoxes

--	update a
--	set AcukwikFBOHandlerID = fboGeo.AcukwikFBOHandlerID
--	from #AirportWatchHistoricalResults a
--	inner join #FBOGeoPolygons fboGeo on fboGeo.ICAO = a.ICAO and a.Longitude is not null 										
--										and fboGeo.GeoCoordinates.MakeValid().STContains(a.GeoLocation) = 1

--	update awp
--	set AcukwikFBOHandlerID = a.AcukwikFBOHandlerID,
--	DateCalculatedUTC = getutcdate()
--	from [AirportWatchHistoricalParking] awp with (nolock)	
--	inner join #AirportWatchHistoricalResults a on a.OID = awp.AirportWatchHistoricalDataID
--	where awp.IsConfirmed is null

--	insert into [AirportWatchHistoricalParking] (AirportWatchHistoricalDataID, AcukwikFBOHandlerID, DateCalculatedUTC)
--	select a.OID, a.AcukwikFBOHandlerID, getutcdate()
--	from #AirportWatchHistoricalResults a
--	left join [AirportWatchHistoricalParking] awp with (nolock) on awp.AirportWatchHistoricalDataID = a.OID
--	where awp.oid is null


--	drop table #AirportWatchHistoricalResults
--	drop table #FBOGeoPolygons
--END
--GO
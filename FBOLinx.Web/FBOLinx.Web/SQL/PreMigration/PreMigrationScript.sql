SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[airCrafts](
	[AircraftID] [int] IDENTITY(1,1) NOT NULL,
	[MAKE] [nvarchar](100) NULL,
	[MODEL] [nvarchar](100) NULL,
	[NORMAL CRUISE TAS] [float] NULL,
	[FUEL CAPACITY (gal)] [float] NULL,
	[LANDING PERF LENGTH] [float] NULL,
	[RANGE (nm)] [float] NULL,
	[RangePerGal] [float] NULL,
	[MaxRangeHours] [float] NULL,
	[MaxRangeMinutes] [float] NULL,
	[ReserveMinutes] [float] NULL,
	[ReserveNM] [float] NULL,
	[Size] [smallint] NULL,
	[MaxTakeoffWeight] [float] NULL,
	[MaxLandingWeight] [float] NULL,
	[AircraftCeiling] [float] NULL,
	[TakeoffPerfSL] [float] NULL,
	[TakeoffPerf] [float] NULL,
	[ZeroFuelWeight] [float] NULL,
	[BasicOperatingWeight] [float] NULL,
	[FuelType] [varchar](25) NULL,
	[TempSize] [smallint] NULL,
 CONSTRAINT [PK_airCrafts] PRIMARY KEY CLUSTERED 
(
	[AircraftID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET IDENTITY_INSERT aircrafts ON

insert into aircrafts ([AircraftID]
      ,[MAKE]
      ,[MODEL]
      ,[NORMAL CRUISE TAS]
      ,[FUEL CAPACITY (gal)]
      ,[LANDING PERF LENGTH]
      ,[RANGE (nm)]
      ,[RangePerGal]
      ,[MaxRangeHours]
      ,[MaxRangeMinutes]
      ,[ReserveMinutes]
      ,[ReserveNM]
      ,[Size]
      ,[MaxTakeoffWeight]
      ,[MaxLandingWeight]
      ,[AircraftCeiling]
      ,[TakeoffPerfSL]
      ,[TakeoffPerf]
      ,[ZeroFuelWeight]
      ,[BasicOperatingWeight]
      ,[FuelType]
      ,[TempSize])
select [AircraftID]
      ,[MAKE]
      ,[MODEL]
      ,[NORMAL CRUISE TAS]
      ,[FUEL CAPACITY (gal)]
      ,[LANDING PERF LENGTH]
      ,[RANGE (nm)]
      ,[RangePerGal]
      ,[MaxRangeHours]
      ,[MaxRangeMinutes]
      ,[ReserveMinutes]
      ,[ReserveNM]
      ,[Size]
      ,[MaxTakeoffWeight]
      ,[MaxLandingWeight]
      ,[AircraftCeiling]
      ,[TakeoffPerfSL]
      ,[TakeoffPerf]
      ,[ZeroFuelWeight]
      ,[BasicOperatingWeight]
      ,[FuelType]
      ,[TempSize]
from Dega.dbo.airCrafts ac

SET IDENTITY_INSERT aircrafts OFF


--FBOAirports cleanup and key changes
delete fa
from FBOAirports fa
left join FBOs f on f.OID = fa.FBOID
where f.OID is null

GO

ALTER TABLE [dbo].[FBOAirports]  WITH NOCHECK ADD  CONSTRAINT [FK_FBOAirports_FBOs] FOREIGN KEY([FBOID])
REFERENCES [dbo].[FBOs] ([OID])
GO

ALTER TABLE [dbo].[FBOAirports] NOCHECK CONSTRAINT [FK_FBOAirports_FBOs]
GO

--FBOContacts primary key addition
ALTER TABLE FBOContacts
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE FBOContacts
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE FuelReq
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomerInfoByGroup
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE CustomerInfoByGroup
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomCustomerTypes
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE CustomCustomerTypes
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomersViewedByFBO
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE CustomersViewedByFBO
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE FBOPrices
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE ContactInfoByGroup
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE ContactInfoByGroup
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE CompaniesByGroup
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE CompaniesByGroup
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE FBOFees
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomerMargins
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE CustomerMargins
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomerContacts
	ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE CustomerContacts
	ADD PRIMARY KEY (OID)
GO

ALTER TABLE RampFeeSettings
    ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE RampFeeSettings
    ADD PRIMARY KEY (OID)
GO

ALTER TABLE ReminderEmailsToFBOs
    ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE ReminderEmailsToFBOs
    ADD PRIMARY KEY (OID)
GO

ALTER TABLE FBOAircraftSizes
    ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE FBOAircraftSizes
    ADD PRIMARY KEY (OID)
GO

ALTER TABLE FBOPreferences
    ADD [OID] [int] IDENTITY(1,1) NOT NULL
GO

ALTER TABLE FBOPreferences
    ADD PRIMARY KEY (OID)
GO

ALTER TABLE RampFees
    ADD CategoryType smallint
GO

ALTER TABLE RampFees
    ADD CategoryMinValue int
GO

ALTER TABLE RampFees
    ADD CategoryMaxValue int
GO

ALTER TABLE RampFees
    ADD ExpirationDate datetime2
GO

/*Changes to CustomerAircrafts table - convert int to smallint*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CustomerAircrafts
	DROP CONSTRAINT DF_CustomerAircrafts_Size
GO
CREATE TABLE dbo.Tmp_CustomerAircrafts
	(
	OID int NOT NULL IDENTITY (1, 1),
	GroupID int NULL,
	CustomerID int NOT NULL,
	AircraftID int NOT NULL,
	TailNumber varchar(25) NULL,
	Size smallint NULL,
	BasedPAGLocation varchar(50) NULL,
	NetworkCode varchar(50) NULL,
	AddedFrom int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CustomerAircrafts SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_CustomerAircrafts ADD CONSTRAINT
	DF_CustomerAircrafts_Size DEFAULT (NULL) FOR Size
GO
SET IDENTITY_INSERT dbo.Tmp_CustomerAircrafts ON
GO
IF EXISTS(SELECT * FROM dbo.CustomerAircrafts)
	 EXEC('INSERT INTO dbo.Tmp_CustomerAircrafts (OID, GroupID, CustomerID, AircraftID, TailNumber, Size, BasedPAGLocation, NetworkCode, AddedFrom)
		SELECT OID, GroupID, CustomerID, AircraftID, TailNumber, CONVERT(smallint, Size), BasedPAGLocation, NetworkCode, AddedFrom FROM dbo.CustomerAircrafts WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CustomerAircrafts OFF
GO
DROP TABLE dbo.CustomerAircrafts
GO
EXECUTE sp_rename N'dbo.Tmp_CustomerAircrafts', N'CustomerAircrafts', 'OBJECT' 
GO
ALTER TABLE dbo.CustomerAircrafts ADD CONSTRAINT
	PK_CustomerAircrafts PRIMARY KEY CLUSTERED 
	(
	OID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX INX_CustomerAircrafts_TailNumber ON dbo.CustomerAircrafts
	(
	TailNumber
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX INX_CA_ID ON dbo.CustomerAircrafts
	(
	GroupID
	) INCLUDE (OID, CustomerID, AircraftID, TailNumber, Size, BasedPAGLocation, NetworkCode) 
 WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX INX_CA_CustomerID ON dbo.CustomerAircrafts
	(
	CustomerID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX INX_GroupID ON dbo.CustomerAircrafts
	(
	GroupID
	) INCLUDE (OID, CustomerID, AircraftID, TailNumber, Size, BasedPAGLocation, NetworkCode, AddedFrom) 
 WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT
GO

ALTER TABLE CustomerInfoByGroup
    ADD CertificateType smallint
GO
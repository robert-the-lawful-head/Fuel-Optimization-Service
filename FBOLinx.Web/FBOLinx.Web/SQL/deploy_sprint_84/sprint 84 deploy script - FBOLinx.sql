use [paragon_test]
GO
/****** Object:  Table [dbo].[ContactInfoByFbo]    Script Date: 10/4/2021 4:28:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactInfoByFbo](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NULL,
	[FboId] [int] NULL,
	[CopyAlerts] [bit] NULL,
 CONSTRAINT [PK_ContactInfoByFbo] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE PricingTemplate
	ADD DiscountType smallint NULL
GO
/****** Object:  Table [dbo].[CustomCustomerTypeLog]    Script Date: 20/10/2021 10:07:40 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomCustomerTypeLog](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [int] NULL,
	[Action] [int] NULL,
	[userId] [int] NULL,
	[Role] [int] NULL,
	[newcustomertypetId] [int] NULL,
	[oldcustomertypeId] [int] NULL,
	[Time] [datetime] NULL,
	[customerId] [int] NULL,
 CONSTRAINT [PK_CustomCustomerTypeLog] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomCustomerTypesLogData]    Script Date: 20/10/2021 10:07:41 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomCustomerTypesLogData](
	[CustomerID] [int] NOT NULL,
	[FBOID] [int] NOT NULL,
	[CustomerType] [int] NOT NULL,
	[OID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CustomCustomerTypesLogData] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerAircraftLog]    Script Date: 20/10/2021 10:07:41 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAircraftLog](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[Role] [int] NULL,
	[newcustomeraircraftId] [int] NULL,
	[oldcustomeraircraftId] [int] NULL,
	[Time] [datetime] NULL,
	[Location] [int] NULL,
	[Action] [int] NULL,
	[customerId] [int] NULL,
 CONSTRAINT [PK_CustomerAircraftLog] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerAircraftLogData]    Script Date: 20/10/2021 10:07:41 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAircraftLogData](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[GroupID] [int] NULL,
	[CustomerID] [int] NOT NULL,
	[AircraftID] [int] NOT NULL,
	[TailNumber] [varchar](25) NULL,
	[Size] [smallint] NULL,
	[BasedPAGLocation] [varchar](50) NULL,
	[NetworkCode] [varchar](50) NULL,
	[AddedFrom] [int] NULL,
 CONSTRAINT [PK_AircraftLogData] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerContactLog]    Script Date: 20/10/2021 10:07:41 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerContactLog](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [int] NULL,
	[Action] [int] NULL,
	[userId] [int] NULL,
	[Role] [int] NULL,
	[newcustomercontactId] [int] NULL,
	[oldcustomercontactId] [int] NULL,
	[Time] [datetime] NULL,
	[customerId] [int] NULL,
 CONSTRAINT [PK_CustomerContactLog] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerContactLogData]    Script Date: 20/10/2021 10:07:41 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerContactLogData](
	[CustomerID] [int] NOT NULL,
	[ContactID] [int] NOT NULL,
	[OID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CustomerContactLogData] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerInfoByGroupLog]    Script Date: 20/10/2021 10:07:41 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerInfoByGroupLog](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [int] NULL,
	[Action] [int] NULL,
	[userId] [int] NULL,
	[Role] [int] NULL,
	[newcustomerId] [int] NULL,
	[oldcustomerId] [int] NULL,
	[Time] [datetime] NULL,
	[customerId] [int] NULL,
 CONSTRAINT [PK_CustomerInfoByGroupLog] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerInfoByGroupLogData]    Script Date: 20/10/2021 10:07:41 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerInfoByGroupLogData](
	[GroupID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[Company] [varchar](255) NULL,
	[Username] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[Joined] [datetime] NULL,
	[Active] [bit] NULL,
	[Distribute] [bit] NULL,
	[Network] [bit] NULL,
	[MainPhone] [varchar](100) NULL,
	[Address] [varchar](255) NULL,
	[City] [varchar](255) NULL,
	[State] [varchar](10) NULL,
	[ZipCode] [varchar](11) NULL,
	[Country] [varchar](255) NULL,
	[Website] [varchar](255) NULL,
	[ShowJetA] [bit] NULL,
	[Show100LL] [bit] NULL,
	[Suspended] [bit] NULL,
	[DefaultTemplate] [int] NULL,
	[CustomerType] [int] NULL,
	[EmailSubscription] [bit] NULL,
	[SFID] [varchar](max) NULL,
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[CertificateType] [smallint] NULL,
	[CustomerCompanyType] [int] NULL,
	[PricingTemplateRemoved] [bit] NULL,
	[MergeRejected] [bit] NULL,
 CONSTRAINT [PK_CustomerInfoByGroupLogData] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerAircraftLogData] ADD  CONSTRAINT [DF_AircraftLogData_Size]  DEFAULT (NULL) FOR [Size]
GO
ALTER TABLE [dbo].[CustomerInfoByGroupLogData] ADD  CONSTRAINT [DF_CustomerInfoByGroupLogData_EmailSubscription]  DEFAULT ((1)) FOR [EmailSubscription]
GO

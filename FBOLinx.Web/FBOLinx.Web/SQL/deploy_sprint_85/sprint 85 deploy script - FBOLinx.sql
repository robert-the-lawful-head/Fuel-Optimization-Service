use [paragon_test]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Associations](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Association] [varchar](50) NULL,
 CONSTRAINT [PK_Associations] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerInfoByAssociation](
	[AssociationID] [int] NOT NULL,
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
PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditsLogs]    Script Date: 1/11/2021 3:21:16 ã ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditsLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Type] [nvarchar](max) NULL,
	[TableName] [nvarchar](max) NULL,
	[DateTime] [datetime] NULL,
	[OldValues] [nvarchar](max) NULL,
	[NewValues] [nvarchar](max) NULL,
	[AffectedColumns] [nvarchar](max) NULL,
	[PrimaryKey] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditsLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Drop TABLE [dbo].[CustomCustomerTypeLog]

Drop TABLE [dbo].[CustomCustomerTypes]

Drop TABLE [dbo].[CustomerAircraftLog]

Drop  TABLE [dbo].[CustomerAircraftLogData]

Drop TABLE [dbo].[CustomerContactLog]

Drop TABLE [dbo].[CustomerContactLogData]

Drop TABLE [dbo].[CustomerInfoByGroupLog]

Drop TABLE [dbo].[CustomerInfoByGroupLogData]
GO
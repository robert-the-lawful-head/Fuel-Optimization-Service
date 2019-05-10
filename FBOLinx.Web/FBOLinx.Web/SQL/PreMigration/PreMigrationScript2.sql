/****** Object:  Table [dbo].[User]    Script Date: 02/18/2019 16:37:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[User](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](255) NULL,
	[Token] [varchar](255) NULL,
	[Role] [smallint] NULL,
	[FBOID] [int] NULL,
	[GroupID] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[EmailContent]    Script Date: 02/18/2019 16:47:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EmailContent](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[EmailContentHTML] [varchar](max) NULL,
	[FBOID] [int] NULL,
	[EmailContentType] [smallint] NULL,
	[Name] varchar(255) NULL,
 CONSTRAINT [PK_EmailContent] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[DistributionLog]    Script Date: 2/18/2019 7:42:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DistributionLog](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[DateSent] [datetime2](7) NOT NULL,
	[FBOID] [int] NULL,
	[GroupID] [int] NULL,
	[UserID] [int] NULL,
 CONSTRAINT [PK_DistributionLog] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[CustomerCompanyTypes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[FBOID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[AllowMultiplePricingTemplates] [bit] NULL
 CONSTRAINT [PK_CustomerCompanyTypes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE DistributionLog
	ADD PricingTemplateID int
GO

ALTER TABLE DistributionLog
	ADD CustomerID int
GO

ALTER TABLE DistributionLog
	ADD CustomerCompanyType int
GO

ALTER TABLE CustomerInfoByGroup
	ADD CustomerCompanyType int
GO

CREATE TABLE [dbo].[DistributionQueue](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[DistributionLogID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[FBOID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[DateSent] [datetime2](7) NULL,
	[IsCompleted] [bit] NULL,
 CONSTRAINT [PK_DistributionQueue] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DistributionErrors](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[DistributionLogID] [int] NULL,
	[DistributionQueueID] [int] NULL,
	[Error] [varchar](max) NULL,
	[ErrorDateTime] [datetime2](7) NULL,
 CONSTRAINT [PK_DistributionErrors] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Group]
	ADD Active bit
GO

	UPDATE g
	set g.Active = 1
	from [Group] g
GO

ALTER TABLE EmailContent
    ADD [Subject] varchar(500)
GO    

ALTER TABLE FBOPreferences
    ADD [Omit100LLRetail] bit
GO

ALTER TABLE FBOPreferences
    ADD [Omit100LLCost] bit
GO
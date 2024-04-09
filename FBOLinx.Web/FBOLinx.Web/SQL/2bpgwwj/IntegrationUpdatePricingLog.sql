USE [paragon_test]
GO

/****** Object:  Table [dbo].[IntegrationUpdatePricingLog]    Script Date: 5/17/2022 1:09:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IntegrationUpdatePricingLog](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Request] [varchar](max) NULL,
	[Response] [varchar](max) NULL,
	[FboId] [int] NULL,
	[DateTimeRecorded] [datetime2](7) NULL,
 CONSTRAINT [PK_IntegrationUpdatePricingLog] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



CREATE NONCLUSTERED INDEX [INX_IntegrationUpdatePricingLog_FboId] ON [dbo].[IntegrationUpdatePricingLog]
(
	[FboId] ASC,
	[DateTimeRecorded] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO




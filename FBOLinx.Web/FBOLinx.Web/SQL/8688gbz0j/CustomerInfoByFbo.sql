USE [paragon_test]
GO

/****** Object:  Table [dbo].[CustomerInfoByFbo]    Script Date: 6/24/2024 2:32:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerInfoByFbo](
	[Oid] [int] IDENTITY(1,1) NOT NULL,
	[CustomerInfoByGroupId] [int] NOT NULL,
	[FboId] [int] NOT NULL,
	[CustomFboEmail] [varchar](max) NULL,
 CONSTRAINT [PK_CustomerInfoByFbo] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [INX_CustomerInfoByFbo_CustomerInfoByGroupId_FboId] ON [dbo].[CustomerInfoByFbo]
(
	[CustomerInfoByGroupId] ASC,
	[FboId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



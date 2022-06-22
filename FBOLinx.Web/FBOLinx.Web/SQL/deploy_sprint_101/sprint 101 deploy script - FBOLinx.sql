USE [paragon_test]
GO
/****** Object:  Table [dbo].[MissedQuoteLog]    Script Date: 5/5/2022 3:11:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MissedQuoteLog](
	[OID] [int] NOT NULL IDENTITY (1,1),
	[FboId] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CustomerId] [int] NULL,
	[Emailed] [bit] NULL,
 CONSTRAINT [PK_MissedQuoteLog] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [INX_MissedQuoteLog_FboIdCreatedDate] ON [dbo].[MissedQuoteLog]
(
	[FboId] ASC,
	[CreatedDate] ASC
)
INCLUDE([CustomerId],[Emailed]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
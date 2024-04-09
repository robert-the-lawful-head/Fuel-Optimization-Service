USE [paragon_test]
GO

/****** Object:  Table [dbo].[CustomerInfoByGroupNotes]    Script Date: 6/11/2023 11:17:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerInfoByGroupNotes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerInfoByGroupID] [int] NOT NULL,
	[FBOID] [int] NULL,
	[Notes] [varchar](max) NULL,
	[LastUpdatedUTC] [datetime] NULL,
	[LastUpdatedByUserID] [int] NULL,
 CONSTRAINT [PK_CustomerInfoByGroupNotes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE INDEX [INX_CustomerInfoByGroupNotes_CustomerInfoByGroupID] on [CustomerInfoByGroupNotes] (
	[CustomerInfoByGroupID]
)
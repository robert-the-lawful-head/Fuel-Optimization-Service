USE [paragon_test]
GO

/****** Object:  Table [dbo].[CustomerAircraftNotes]    Script Date: 6/11/2023 11:15:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerAircraftNotes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerAircraftID] [int] NOT NULL,
	[FBOID] [int] NULL,
	[Notes] [varchar](max) NULL,
	[LastUpdatedUTC] [datetime] NULL,
	[LastUpdatedByUserID] [int] NULL,
 CONSTRAINT [PK_CustomerAircraftNotes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE INDEX [INX_CustomerAircraftNotes_CustomerAircraftID] on CustomerAircraftNotes (
	[CustomerAircraftID]
)
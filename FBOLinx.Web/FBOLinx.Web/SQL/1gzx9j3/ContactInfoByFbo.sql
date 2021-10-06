/****** Object:  Table [dbo].[ContactInfoByFbo]    Script Date: 10/4/2021 4:28:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ContactInfoByFbo](
	[Oid] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NULL,
	[FboId] [int] NULL,
	[CopyAlerts] [bit] NULL,
 CONSTRAINT [PK_ContactInfoByFbo] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



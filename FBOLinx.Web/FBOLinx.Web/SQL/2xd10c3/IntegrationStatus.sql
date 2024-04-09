USE [paragon_test]
GO

/****** Object:  Table [dbo].[IntegrationStatus]    Script Date: 9/21/2022 10:53:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IntegrationStatus](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[IntegrationPartnerId] [int] NOT NULL,
	[FboId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_IntegrationStatus] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



USE [fileStorage]
GO

/****** Object:  Table [dbo].[FbolinxPricingTemplateAttachments]    Script Date: 2/17/2022 3:55:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FbolinxPricingTemplateAttachments](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FileData] [varbinary](max) NULL,
	[FileName] [varchar](200) NULL,
	[ContentType] [varchar](100) NULL,
	[PricingTemplateId] [int] NULL,
 CONSTRAINT [PK_FbolinxPricingTemplateAttachments] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



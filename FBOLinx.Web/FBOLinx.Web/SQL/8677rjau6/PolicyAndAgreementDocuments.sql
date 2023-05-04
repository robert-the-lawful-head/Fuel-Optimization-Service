USE [paragon_test_Local]
GO

/****** Object:  Table [dbo].[PolicyAndAgreementDocuments]    Script Date: 5/4/2023 5:17:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PolicyAndAgreementDocuments](
	[OID] [int] NOT NULL,
	[DocumentType] [varchar](50) NULL,
	[DocumentVersion] [varchar](50) NULL,
	[Document] [varchar](255) NULL,
	[AcceptanceFlag] [smallint] NULL,
	[isEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_PolicyAndAgreementDocuments] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PolicyAndAgreementDocuments] ADD  CONSTRAINT [DF_PolicyAndAgreementDocuments_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO



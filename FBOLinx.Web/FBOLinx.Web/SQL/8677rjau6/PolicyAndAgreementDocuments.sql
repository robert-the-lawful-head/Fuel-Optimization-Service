drop table [dbo].[PolicyAndAgreementGroupExemptions],[dbo].[UserAcceptedPolicyAndAgreements], [dbo].[PolicyAndAgreementDocuments] 

USE [paragon_test]
GO

ALTER TABLE [dbo].[PolicyAndAgreementDocuments] DROP CONSTRAINT [DF_PolicyAndAgreementDocuments_isEnabled]
GO

/****** Object:  Table [dbo].[PolicyAndAgreementDocuments]    Script Date: 5/24/2023 5:25:17 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PolicyAndAgreementDocuments]') AND type in (N'U'))
DROP TABLE [dbo].[PolicyAndAgreementDocuments]
GO

/****** Object:  Table [dbo].[PolicyAndAgreementDocuments]    Script Date: 5/24/2023 5:25:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PolicyAndAgreementDocuments](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentType] [smallint] NOT NULL,
	[DocumentVersion] [varchar](50) NOT NULL,
	[Document] [varchar](255) NOT NULL,
	[AcceptanceFlag] [smallint] NOT NULL,
	[isEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_PolicyAndAgreementDocuments] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PolicyAndAgreementDocuments] ADD  CONSTRAINT [DF_PolicyAndAgreementDocuments_isEnabled]  DEFAULT ((0)) FOR [isEnabled]
GO
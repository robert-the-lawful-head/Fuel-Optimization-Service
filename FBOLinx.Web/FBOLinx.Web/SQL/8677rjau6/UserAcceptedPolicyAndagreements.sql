USE [paragon_test_Local]
GO

/****** Object:  Table [dbo].[UserAcceptedPolicyAndAgreements]    Script Date: 5/4/2023 5:04:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserAcceptedPolicyAndAgreements](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DocumentId] [int] NOT NULL,
	[AcceptedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserAcceptedPolicyAndAgreements] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserAcceptedPolicyAndAgreements]  WITH CHECK ADD  CONSTRAINT [FK_UserAcceptedPolicyAndAgreements_PolicyAndAgreementDocuments] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[PolicyAndAgreementDocuments] ([OID])
GO

ALTER TABLE [dbo].[UserAcceptedPolicyAndAgreements] CHECK CONSTRAINT [FK_UserAcceptedPolicyAndAgreements_PolicyAndAgreementDocuments]
GO



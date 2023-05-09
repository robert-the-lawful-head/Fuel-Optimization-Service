USE [paragon_test_Local]
GO

/****** Object:  Table [dbo].[PolicyAndAgreementGroupExemptions]    Script Date: 5/4/2023 5:08:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PolicyAndAgreementGroupExemptions](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [int] NOT NULL,
	[DateTimeExempted] [datetime] NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_PolicyAndAgreementGroupExemptions] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PolicyAndAgreementGroupExemptions]  WITH CHECK ADD  CONSTRAINT [FK_PolicyAndAgreementGroupExemptions_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([OID])
GO

ALTER TABLE [dbo].[PolicyAndAgreementGroupExemptions] CHECK CONSTRAINT [FK_PolicyAndAgreementGroupExemptions_Group]
GO

ALTER TABLE [dbo].[PolicyAndAgreementGroupExemptions]  WITH CHECK ADD  CONSTRAINT [FK_PolicyAndAgreementGroupExemptions_PolicyAndAgreementDocuments] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[PolicyAndAgreementDocuments] ([OID])
GO

ALTER TABLE [dbo].[PolicyAndAgreementGroupExemptions] CHECK CONSTRAINT [FK_PolicyAndAgreementGroupExemptions_PolicyAndAgreementDocuments]
GO



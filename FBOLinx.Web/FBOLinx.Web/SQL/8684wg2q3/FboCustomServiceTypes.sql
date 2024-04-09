USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboCustomServiceTypes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[FboId] [int] NULL,
	[CreatedByUserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_FboCustomServiceTypes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FboCustomServiceTypes]  WITH CHECK ADD  CONSTRAINT [FK_FboCustomServiceTypes_User] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[User] ([OID])
GO

ALTER TABLE [dbo].[FboCustomServiceTypes] CHECK CONSTRAINT [FK_FboCustomServiceTypes_User]
GO



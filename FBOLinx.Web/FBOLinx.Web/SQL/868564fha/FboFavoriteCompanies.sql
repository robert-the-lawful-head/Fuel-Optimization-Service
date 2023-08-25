USE [paragon_test]
GO

/****** Object:  Table [dbo].[FboFavoriteCompanies]    Script Date: 8/24/2023 6:20:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboFavoriteCompanies](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerInfoByGroupId] [int] NOT NULL,
	[FboId] [int] NOT NULL,
 CONSTRAINT [PK_FboFavoriteCompanies] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FboFavoriteCompanies]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteCompanies_CustomerInfoByGroup] FOREIGN KEY([CustomerInfoByGroupId])
REFERENCES [dbo].[CustomerInfoByGroup] ([OID])
GO

ALTER TABLE [dbo].[FboFavoriteCompanies] CHECK CONSTRAINT [FK_FboFavoriteCompanies_CustomerInfoByGroup]
GO

ALTER TABLE [dbo].[FboFavoriteCompanies]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteCompanies_FBOs] FOREIGN KEY([FboId])
REFERENCES [dbo].[FBOs] ([OID])
GO

ALTER TABLE [dbo].[FboFavoriteCompanies] CHECK CONSTRAINT [FK_FboFavoriteCompanies_FBOs]
GO



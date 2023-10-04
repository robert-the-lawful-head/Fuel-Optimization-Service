USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboFavoriteAircraft](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FboId] [int] NOT NULL,
	[CustomerAircraftId] [int] NOT NULL,
 CONSTRAINT [PK_FboFavoriteAircraft] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FboFavoriteAircraft]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteAircraft_CustomerAircrafts] FOREIGN KEY([CustomerAircraftId])
REFERENCES [dbo].[CustomerAircrafts] ([OID])
GO

ALTER TABLE [dbo].[FboFavoriteAircraft] CHECK CONSTRAINT [FK_FboFavoriteAircraft_CustomerAircrafts]
GO

ALTER TABLE [dbo].[FboFavoriteAircraft]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteAircraft_FBOs] FOREIGN KEY([FboId])
REFERENCES [dbo].[FBOs] ([OID])
GO

ALTER TABLE [dbo].[FboFavoriteAircraft] CHECK CONSTRAINT [FK_FboFavoriteAircraft_FBOs]
GO



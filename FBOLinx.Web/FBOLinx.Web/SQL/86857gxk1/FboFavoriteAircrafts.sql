USE [paragon_test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboFavoriteAircrafts](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AircraftId] [int] NOT NULL,
	[FboId] [int] NOT NULL,
 CONSTRAINT [PK_FboFavoriteAircrafts] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FboFavoriteAircrafts]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteAircrafts_airCrafts] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[airCrafts] ([AircraftID])
GO

ALTER TABLE [dbo].[FboFavoriteAircrafts] CHECK CONSTRAINT [FK_FboFavoriteAircrafts_airCrafts]
GO

ALTER TABLE [dbo].[FboFavoriteAircrafts]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteAircrafts_FBOs] FOREIGN KEY([FboId])
REFERENCES [dbo].[FBOs] ([OID])
GO

ALTER TABLE [dbo].[FboFavoriteAircrafts] CHECK CONSTRAINT [FK_FboFavoriteAircrafts_FBOs]
GO



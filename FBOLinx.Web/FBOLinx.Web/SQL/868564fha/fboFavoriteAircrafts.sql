USE [paragon_test]
GO

/****** Object:  Table [dbo].[FboFavoriteAircraft]    Script Date: 8/24/2023 6:11:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FboFavoriteAircraft](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FboId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[TailNumber] [varchar](25) NOT NULL,
	[AircraftId] [int] NULL,
 CONSTRAINT [PK_FboFavoriteAircraft] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FboFavoriteAircraft]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteAircraft_FBOs] FOREIGN KEY([FboId])
REFERENCES [dbo].[FBOs] ([OID])
GO

ALTER TABLE [dbo].[FboFavoriteAircraft] CHECK CONSTRAINT [FK_FboFavoriteAircraft_FBOs]
GO

ALTER TABLE [dbo].[FboFavoriteAircraft]  WITH CHECK ADD  CONSTRAINT [FK_FboFavoriteAircraft_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([OID])
GO

ALTER TABLE [dbo].[FboFavoriteAircraft] CHECK CONSTRAINT [FK_FboFavoriteAircraft_Group]
GO



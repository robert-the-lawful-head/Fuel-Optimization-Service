

/****** Object:  Table [dbo].[FAAAircraftHexTailMapping]    Script Date: 12/13/2021 11:36:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FAAAircraftHexTailMapping](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[AircraftHexCode] [varchar](25) NOT NULL,
	[TailNumber] [varchar](25) NULL,
 CONSTRAINT [PK_FAAAircraftHexTailMapping] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO





/****** Object:  Table [dbo].[FBOFeesAndTaxes]    Script Date: 9/28/2020 11:37:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FBOFeesAndTaxes](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FBOID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CalculationType] [smallint] NULL,
	[Value] [float] NOT NULL,
	[FlightTypeClassification] [smallint] NULL,
	[DepartureType] [smallint] NULL,
 CONSTRAINT [PK_FBOFeesAndTaxes] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



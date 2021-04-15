use [paragon_test]
GO

/****** Object:  Table [dbo].[FBOFeeAndTaxOmitsByPricingTemplate]    Script Date: 11/19/2020 5:26:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FBOFeeAndTaxOmitsByPricingTemplate](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FBOFeeAndTaxID] [int] NOT NULL,
	[PricingTemplateID] [int] NOT NULL,
 CONSTRAINT [PK_FBOFeeAndTaxOmitsByPricingTemplate] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

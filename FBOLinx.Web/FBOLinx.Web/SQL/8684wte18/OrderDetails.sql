USE [paragon_test]
GO

/****** Object:  Table [dbo].[OrderDetails]    Script Date: 6/26/2023 5:34:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderDetails](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FuelerLinxTransactionId] [int] NOT NULL,
	[ConfirmationEmail] [varchar](255) NOT NULL,
	[FuelVendor] [varchar](50) NOT NULL,
	[PaymentMethod] [varchar](255) NULL,
DateTimeUpdated datetime2(7) NULL,
IsEmailSent bit NULL,
DateTimeEmailSent datetime2(7) NULL,
QuotedVolume float NULL,
CustomerAircraftID int NULL,
ETA datetime2(7) NULL
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



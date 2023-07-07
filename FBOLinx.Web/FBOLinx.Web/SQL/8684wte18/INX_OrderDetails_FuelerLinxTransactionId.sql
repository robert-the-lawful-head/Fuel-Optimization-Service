USE [paragon_test]
GO

/****** Object:  Index [INX_OrderDetails_FuelerLinxTransactionId]    Script Date: 6/26/2023 5:34:13 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [INX_OrderDetails_FuelerLinxTransactionId] ON [dbo].[OrderDetails]
(
	[FuelerLinxTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



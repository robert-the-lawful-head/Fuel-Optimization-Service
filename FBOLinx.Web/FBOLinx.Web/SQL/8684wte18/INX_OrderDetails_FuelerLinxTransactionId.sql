USE [paragon_test]
GO

CREATE UNIQUE NONCLUSTERED INDEX [INX_OrderDetails_FuelerLinxTransactionIdFboHandlerIdAssociatedFuelOrderId] ON [dbo].[OrderDetails]
(
	[FuelerLinxTransactionId] ASC,
	[FboHandlerId] ASC,
	AssociatedFuelOrderId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



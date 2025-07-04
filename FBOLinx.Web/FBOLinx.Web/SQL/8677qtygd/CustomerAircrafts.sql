use [paragon_test]

CREATE NONCLUSTERED INDEX [indx_DistributionQueue_GroupID_FBOID_CustomerID] ON [dbo].[DistributionQueue]
(
	[GroupID] ASC,
	[FBOID] ASC,
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

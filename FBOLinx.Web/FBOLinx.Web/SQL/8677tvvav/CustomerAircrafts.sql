use paragon_test


DROP INDEX [INX_CustomerAircrafts_GroupAndTail] ON [paragon_test_Local].[dbo].[CustomerAircrafts];
DROP INDEX [INX_CA_ID] ON [paragon_test_Local].[dbo].[CustomerAircrafts];




ALTER TABLE customeraircrafts
ALTER COLUMN groupid int not null





/****** Object:  Index [INX_CustomerAircrafts_GroupAndTail]    Script Date: 4/7/2023 1:13:57 PM ******/
CREATE NONCLUSTERED INDEX [INX_CustomerAircrafts_GroupAndTail] ON [dbo].[CustomerAircrafts]
(
	[GroupID] ASC,
	[TailNumber] ASC
)
INCLUDE([CustomerID],[AircraftID],[Size],[BasedPAGLocation],[NetworkCode],[AddedFrom]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]--, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
GO


/****** Object:  Index [INX_CA_ID]    Script Date: 4/7/2023 1:13:34 PM ******/
CREATE NONCLUSTERED INDEX [INX_CA_ID] ON [dbo].[CustomerAircrafts]
(
	[GroupID] ASC
)
INCLUDE([OID],[CustomerID],[AircraftID],[TailNumber],[Size],[BasedPAGLocation],[NetworkCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]--, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
GO


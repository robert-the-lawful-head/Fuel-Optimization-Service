USE [paragon_test]
GO
CREATE NONCLUSTERED INDEX [INX_CustomerAircrafts_GroupAndTail]
ON [dbo].[CustomerAircrafts] ([GroupID],[TailNumber])
INCLUDE ([CustomerID],[AircraftID],[Size],[BasedPAGLocation],[NetworkCode],[AddedFrom])
GO


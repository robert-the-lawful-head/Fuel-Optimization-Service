
CREATE NONCLUSTERED INDEX [INX_FBOPrices_EffectiveDates]
ON [dbo].[FBOPrices] ([EffectiveFrom],[EffectiveTo])
INCLUDE ([FBOID],[Product],[Price],[Expired])
GO

ALTER TABLE FuelReq
	ALTER COLUMN Email varchar(255)
GO
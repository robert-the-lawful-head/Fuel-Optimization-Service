ALTER TABLE RampFees
	ADD LastUpdated DateTime2(7) null
GO

Update RampFees
set LastUpdated = '9/2/2020'
where LastUpdated is null
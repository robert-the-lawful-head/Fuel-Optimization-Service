use [paragon_test]
ALTER TABLE RampFees
	ADD LastUpdated DateTime2(7) null
GO
Update RampFees
set LastUpdated = '9/2/2020'
where LastUpdated is null
GO
WITH CTE AS(
	SELECT *, RN = ROW_NUMBER() OVER (PARTITION BY CustomerID, FBOID ORDER BY CustomerID, FBOID)
	FROM [paragon_test].[dbo].[CustomCustomerTypes]
)
DELETE FROM CTE WHERE RN > 1;
GO

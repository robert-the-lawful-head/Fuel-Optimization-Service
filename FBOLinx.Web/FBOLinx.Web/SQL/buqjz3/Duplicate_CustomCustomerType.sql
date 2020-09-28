WITH CTE AS(
	SELECT *, RN = ROW_NUMBER() OVER (PARTITION BY CustomerID, FBOID ORDER BY CustomerID, FBOID)
	FROM [paragon_test].[dbo].[CustomCustomerTypes]
)
DELETE FROM CTE WHERE RN > 1;

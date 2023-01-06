USE [paragon_test]

WITH cte AS (
    SELECT 
      groupid, customerid, tailnumber,
        ROW_NUMBER() OVER (
            PARTITION BY 
                 groupid, customerid, tailnumber
            ORDER BY 
               groupid, customerid, tailnumber
        ) row_num
     FROM 
        customeraircrafts
)
DELETE FROM cte
WHERE row_num > 1;

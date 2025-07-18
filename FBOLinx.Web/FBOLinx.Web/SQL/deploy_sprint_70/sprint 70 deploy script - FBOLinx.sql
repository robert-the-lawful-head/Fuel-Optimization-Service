use [paragon_test]
GO
/****** Object:  StoredProcedure [dbo].[up_Load_PricesForFuelerLinxTransactions]    Script Date: 3/22/2021 12:55:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[up_Load_PricesForFuelerLinxTransactions]
@TransactionID int
as

--**********************************************
--                Updates
--  DATE	  By Who	Change Made
--**********************************************
--  09/09/14    CL      Created procedure

DECLARE @FuelerLinxCompanyID int = 0
DECLARE @TailNumber varchar(10) = ''
DECLARE @ICAO varchar(10) = ''

SELECT @FuelerLinxCompanyID = companyID, @TailNumber = tail_number, @ICAO = ICAO
FROM FuelerLinx_FuelReq
WHERE OID = @TransactionID

CREATE TABLE #temptable
(
ICAO varchar(10),
FBO varchar(100),
TailNumber varchar(10),
product varchar(50),
VolA float,
VolB float,
PriceA float,
PriceB float,
Note varchar(MAX),
Fueler varchar(50),
fuelerName varchar(50),
OID int,
CompanyID int,
FBOEmail varchar(50),
FuelInclusive bit,
PriceNote varchar(MAX),
member_ID int,
ContractID int,
Notes varchar(MAX)
)

INSERT INTO #temptable
exec up_Load_PricesForFuelerlinx @FuelerLinxCompanyID, @ICAO, @TailNumber

INSERT INTO #Tankering (RequestID, FuelerID, FBO, VolA, PriceA, VolB, PriceB, VolC, PriceC, VolD, PriceD, VolE, PriceE, VolF, PriceF, VolG, PriceG, VolH, PriceH, VolI, PriceI, VolJ, PriceJ, VolK, PriceK, VolL, PriceL, Fueler, product, FuelerDisplayName, FuelMasterID)
SELECT 
@TransactionID AS RequestID, 
FL.OID AS FuelerID,
FBO AS FBO, 
VolA, 
PriceA, 
VolB, 
PriceB, 
0 AS VolC, 
0 AS PriceC, 
0 AS VolD, 
0 AS PriceD, 
0 AS VolE, 
0 AS PriceE, 
0 AS VolF, 
0 AS PriceF, 
0 AS VolG, 
0 AS PriceG, 
0 AS VolH, 
0 AS PriceH, 
0 AS VolI,  
0 AS PriceI, 
0 AS VolJ, 
0 AS PriceJ, 
0 AS VolK, 
0 AS PriceK, 
0 AS VolL, 
0 AS PriceL,
'' AS Fueler,
'' AS product,
'' AS FuelerDisplayName,
0 AS FuelMasterID
FROM #temptable TT
INNER JOIN FUELERLINX_fuelerList FL ON FL.processNm = TT.Fueler

drop table #temptable
GO
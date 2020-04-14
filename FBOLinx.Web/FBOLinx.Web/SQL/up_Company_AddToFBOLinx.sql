USE [sqlfl]
GO
/****** Object:  StoredProcedure [dbo].[up_Company_AddToFBOLinx]    Script Date: 4/14/2020 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--*********************************************
--              Change History
--*********************************************
--  10/09/2013  CL	Changed Distribute flag from 0 to 1 when adding new FuelerLinx customers
--	01/16/2014	CL	Added in AddedFrom column for CustomerAircrafts query
-- =============================================
ALTER PROCEDURE [dbo].[up_Company_AddToFBOLinx](@CompanyID int, @Company varchar(255))
AS

BEGIN

SELECT @Company = coName
from Companies
WHERE OID = @CompanyID

declare @FBOLinxOID int
select @FBOLinxOID = C.OID 
from FBOLinx_Customers C
INNER JOIN FBOLinx_CustomerInfoByGroup CG ON CG.CustomerID = C.OID
                                                    AND (CG.Company = @Company OR ABS(C.FuelerlinxID) = @CompanyID)

if (@FBOLinxOID is null)
	BEGIN
		insert into FBOLinx_Customers
		(FuelerlinxID, Distribute, Company)
		values (@CompanyID, 0, @Company)

		set @FBOLinxOID = @@IDENTITY

		insert into FBOLinx_CustomerInfoByGroup
		(Company, CustomerID, Distribute, GroupID, Active, ShowJetA, Show100LL, Suspended, Network, CustomerType, CustomerCompanyType)
		select @Company, @FBOLinxOID, 1, OID, CASE WHEN OID = 1 THEN 0 ELSE 1 END, 1, 0, 0, 0, 1, 4
		FROM FBOLinx_Group

		insert into FBOLinx_CustomerAircrafts (CustomerID, GroupID, AircraftID, TailNumber, AddedFrom)
		select @FBOLinxOID, CIG.GroupID, ad.aircraftid, ad.tailno, 1
		FROM (SELECT DISTINCT companyID, TailNumID FROM UserAircraft) AS UA
		INNER JOIN acData2 AS AD ON AD.OID = UA.TailNumID AND UA.companyID = @CompanyID
		INNER JOIN FBOLinx_Customers AS C ON C.FuelerlinxID = @CompanyID
		INNER JOIN FBOLinx_CustomerInfoByGroup AS CIG ON CIG.CustomerID = C.OID
		order by ad.tailno
		
		INSERT INTO FBOLinx_VolumeScaleDiscount (CustomerID, FBOID, GroupMargin, LastUpdated, Margin, Margin100LL, MarginType, MarginType100LL, TemplateID, JetAVolumeDiscount, DefaultSettings)
        select @FBOLinxOID, F.OID, 0, GETDATE(), 0, 0, 0, 0, 0, pt.OID, 0
        FROM dbo.FBOLinx_FBOs AS F
        INNER JOIN FBOLinx_PricingTemplate pt on pt.FBOID = F.OID
                                          and pt.[Type] = 1
        WHERE GroupID > 1

	END
ELSE
	BEGIN
		update FBOLinx_Customers
		set FuelerlinxID = @CompanyID
		where OID = @FBOLinxOID
	END
END

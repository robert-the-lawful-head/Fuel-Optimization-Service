use [paragon_test]
GO
ALTER TABLE FBOs
ADD ExpirationDate datetime2 null
GO
--/****** Object:  UserDefinedFunction [dbo].[fn_Split]    Script Date: 4/15/2021 3:55:58 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE FUNCTION [dbo].[fn_Split]
--(
--@List varchar(max),
--@SplitOn varchar(5)
--) 
--RETURNS @RtnValue table 
--(
--ID int identity(1,1),
--Value varchar(500)
--) 
--AS 
--/******************************************************************************
--** Name: fn_Split
--** Desc: Returns table from comma-separated list
--**
--** Auth: Mike Mieglitz
--** Date: 06/4/08
--*******************************************************************************
--** Change History
--*******************************************************************************
--** Date: Author: Description:
--** -------- -------- ------------------------------------------- 
--**
--*******************************************************************************/
--BEGIN

--	WHILE (CHARINDEX(@SplitOn,@List)>0)
--	BEGIN 
--		INSERT INTO @RtnValue (value)
--		SELECT Value = LTrim(RTRIM(SUBSTRING(@List,1,CHARINDEX(@SplitOn, @List) - 1))) 
--		SET @List = SUBSTRING(@List,CHARINDEX(@SplitOn, @List) + LEN(@SplitOn), LEN(@List))
--	END 
--	INSERT INTO @RtnValue (Value)
--	SELECT Value = LTRIM(RTRIM(@List))

--	RETURN

--END
--GO
/****** Object:  StoredProcedure [dbo].[up_Load_VolumeDiscountsForFuelerlinx]    Script Date: 4/14/2021 5:15:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[up_Load_ParagonPricesForFuelerlinx]
	@FuelerlinxCompanyID int,
	@ICAOList varchar(max)
AS
BEGIN
	DECLARE @CompanyId int = 0
	SELECT @CompanyId = OID
	FROM Customers
	WHERE FuelerlinxID = @FuelerlinxCompanyID


	DECLARE @MaxAircraftPricesId int
	  DECLARE @MaxAircraftPricesNewId int

	  SELECT @MaxAircraftPricesId = MAX(OID)
	  FROM AircraftPrices

	INSERT INTO AircraftPrices (CustomerAircraftID, PriceTemplateID)
			select ca.oid, ISNULL(CDT.PricingTemplateID, VSD.JetAVolumeDiscount) AS PricingTemplateID
			FROM dbo.fn_Split(@ICAOList, ',') i 
			INNER JOIN FBOAirports FA on i.Value = FA.ICAO
			INNER JOIN FBOs F  on fa.FBOID = f.OID
			inner join PricingTemplate pt on pt.FBOID = F.OID and F.GroupID = 1
			inner join Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			inner join VolumeScaleDiscount vsd ON VSD.CustomerID = C.OID AND VSD.FBOID = pt.FBOID
			inner join CustomerInfoByGroup CIBG ON C.OID = CIBG.CustomerID AND CIBG.GroupID = F.GroupID
			inner join CustomerDefaultTemplates CDT ON CDT.PricingTemplateID = pt.OID 
			                                            and CDT.FBOID = F.OID 
			                                            and CDT.CustomerID = C.OID
			inner join CustomerAircrafts ca on ca.CustomerID = @CompanyID and ca.GroupID = f.GroupID
			left join
			(select ap2.CustomerAircraftID, pt2.FBOID
				from PricingTemplate pt2
				left join AircraftPrices ap2 on ap2.PriceTemplateID = pt2.OID) AP ON AP.CustomerAircraftID = ca.OID and AP.FBOID = F.OID
			where AP.FBOID is null		
			
		SELECT @MaxAircraftPricesNewId = MAX(OID)
	  FROM AircraftPrices

	  IF (@MaxAircraftPricesNewId > @MaxAircraftPricesId)
	  BEGIN
		INSERT INTO AircraftPricesInsertLog(DateTimeRecorded, SP, GroupId, FboId, AircraftPricesIdBefore, AircraftPricesIdAfter)
		VALUES (GETDATE(), 'up_Load_ParagonPricesForFuelerlinx ' + @ICAO, 1, 0, @MaxAircraftPricesId, @MaxAircraftPricesNewId)
	  END	

	INSERT INTO CompanyPricingLog (CompanyID, ICAO) VALUES(@FuelerlinxCompanyID, @ICAO)


DECLARE @date datetime
	--SELECT @date = CONVERT(VARCHAR(10), GETDATE(), 120)
	SELECT @date = GETDATE()
	
			SELECT
			FA.ICAO AS ICAO, 
			F.Fbo AS FBO,
			'paragon' as Fueler,
			'JetA' AS Product,
			ISNULL(P.[Min], 0) AS MinVolume,
			CASE WHEN P.[Max] = 0 THEN 99999 ELSE P.[Max] END AS MaxVolume,
			CASE WHEN ISNULL(VSD.MarginType, 0) = 0 THEN ((FP.Price + VSD.Margin+ ISNULL(P.Amount, 0)) * (1 + ISNULL(FP.SalesTax, 0))) 
															WHEN ISNULL(VSD.MarginType, 0) = 1 AND (VSD.Margin = 0)
															THEN FP.Price + ISNULL(P.Amount, 0)
															ELSE(((FP.Price/(1 + ISNULL(FP.SalesTax, 0))) - VSD.Margin+ ISNULL(P.Amount, 0)) * (1 + ISNULL(FP.SalesTax, 0)))
															END AS Price,
			'Paragon Net.' as FuelerDisplayName,
			T.[Notes] AS Notes,
			FP.EffectiveFrom,
			FP.EffectiveTo
	FROM dbo.fn_Split(@ICAOList, ',') i 
	INNER JOIN FBOAirports FA on i.Value = FA.ICAO
	INNER JOIN FBOs F ON F.OID = FA.FBOID
						AND F.Active = 1
						AND F.GroupID = 1
						AND F.Suspended = 0
	INNER JOIN PricingTemplate T ON T.FBOID = F.OID 
	INNER JOIN CustomerInfoByGroup CIBG ON CIBG.Distribute = 1
							AND ISNULL(CIBG.ShowJetA, 1) = 1
							AND CIBG.Active = 1
							AND CIBG.Suspended = 0
							AND CIBG.GroupID = 1
	INNER JOIN VolumeScaleDiscount VSD ON VSD.CustomerID = CIBG.CustomerID AND VSD.FBOID = F.OID
	INNER JOIN FBOPrices FP ON F.OID = FP.FBOID AND FP.Price > 0 AND
								(((VSD.MarginType = 0 AND VSD.Margin > 0 AND FP.Product = 'JetA Cost') OR
										(VSD.MarginType = 1 AND FP.Product = 'JetA Retail')) AND
										(FP.EffectiveFrom <= @date AND FP.EffectiveTo > @date))
	LEFT JOIN (select MAX(OID) AS OID FROM FBOPrices WHERE EffectiveFrom <= @date AND EffectiveTo > @date GROUP BY FBOID, Product) foid
	ON FP.OID = foid.OID 	
	LEFT JOIN
		(
			select PriceTierID, TemplateID, Amount, [Min], [Max] 
			FROM CustomerMargins CM
			INNER JOIN PriceTiers PT ON PT.OID = CM.PriceTierID
			
			UNION ALL
			
			SELECT 0 AS PriceTierID, CM.TemplateID, 0 AS Amount, 1 AS [Min], CPT.MinGal - 1 AS [Max]
			FROM CustomerMargins CM
			LEFT JOIN 
			(
				select CM.TemplateID, MIN(PT.[Min]) AS MinGal 
				FROM CustomerMargins CM
				LEFT JOIN PriceTiers PT ON PT.OID = CM.PriceTierID
											AND (PT.[Min] > 1)	
				WHERE PT.OID is not null
				Group By CM.TemplateID
			) CPT ON CPT.TemplateID = CM.TemplateID
			WHERE CM.TemplateID NOT IN
			(select CM.TemplateID 
			FROM CustomerMargins CM
			LEFT JOIN PriceTiers PT ON PT.OID = CM.PriceTierID
										AND (PT.[Min] > 1)	
			WHERE PT.OID is null)
			Group By CM.TemplateID, CPT.MinGal
		) P ON P.TemplateID = T.OID
	where ((vsd.JetAVolumeDiscount is not null and vsd.JetAVolumeDiscount = T.OID)
		OR
		(vsd.JetAVolumeDiscount is null and T.[Default] = 1))
	and FA.ICAO = @ICAO
	and CIBG.CustomerID = @CompanyID
	ORDER BY FBO, Product, ISNULL([Min], 1), EffectiveFrom
END
GO

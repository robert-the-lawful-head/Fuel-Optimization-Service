/****** Object:  StoredProcedure [dbo].[up_Load_VolumeDiscountsForFuelerlinx]    Script Date: 3/18/2020 2:20:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[up_Load_VolumeDiscountsForFuelerlinx]
	@FuelerlinxCompanyID int,
	@ICAO varchar(10),
	@TailNumber varchar(50)
AS
BEGIN
	DECLARE @date datetime
	SELECT @date = GETDATE()

	declare @CustomerAircraftID int = 0
	SELECT @CustomerAircraftID = CA.OID
	FROM CustomerAircrafts AS CA
	INNER JOIN Customers AS C ON CA.CustomerID = C.OID
	where TailNumber = @TailNumber AND C.FuelerlinxID = @FuelerlinxCompanyID

	IF (@CustomerAircraftID > 0)
	BEGIN
	INSERT INTO AircraftPrices (CustomerAircraftID, PriceTemplateID)
			select @CustomerAircraftID, ISNULL(CDT.PricingTemplateID, VSD.JetAVolumeDiscount) AS PricingTemplateID
			from FBOs F
			inner join PricingTemplate pt on pt.FBOID = F.OID and F.GroupID = 1
			inner join Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			inner join VolumeScaleDiscount vsd ON VSD.CustomerID = C.OID AND VSD.FBOID = pt.FBOID
			inner join CustomerInfoByGroup CIBG ON C.OID = CIBG.CustomerID AND CIBG.GroupID = F.GroupID
			left join CustomerDefaultTemplates CDT ON CDT.PricingTemplateID = pt.OID 
			                                            and CDT.FBOID = F.OID 
			                                            and CDT.CustomerID = C.OID
			left join
			(select ap2.CustomerAircraftID, pt2.FBOID
				from PricingTemplate pt2
				left join AircraftPrices ap2 on ap2.PriceTemplateID = pt2.OID) AP ON AP.CustomerAircraftID = @CustomerAircraftID and AP.FBOID = F.OID
			where AP.FBOID is null

	INSERT INTO CompanyPricingLog (CompanyID, ICAO) VALUES(@FuelerlinxCompanyID, @ICAO)

	SELECT F.Fbo AS FBO,
			FLF.Fueler AS Fueler,
			'JetA' AS Product,
			ISNULL(P.[Min], 1) AS MinVolume,
			CASE WHEN ISNULL(VSD.MarginType, 0) = 0 THEN ((FP.Price + VSD.Margin) * (1 + ISNULL(FP.SalesTax, 0))) + ISNULL(P.Amount, 0)
															WHEN ISNULL(VSD.MarginType, 0) = 1 AND (VSD.Margin = 0) AND FLF.GroupID = 1
															THEN FP.Price + ISNULL(P.Amount, 0)
															WHEN ISNULL(VSD.MarginType, 0) = 1 AND (VSD.Margin = 0)
															THEN FP.Price * (1 + ISNULL(FP.SalesTax, 0)) + ISNULL(P.Amount, 0)
															ELSE(((FP.Price/(1 + ISNULL(FP.SalesTax, 0))) - VSD.Margin) * (1 + ISNULL(FP.SalesTax, 0))) + ISNULL(P.Amount, 0)
															END AS Price,
			FLF.FuelerName AS FuelerDisplayName,			
			T.Notes
			FROM FBOAirports FA
			INNER JOIN FBOs F ON F.OID = FA.FBOID
								AND F.Active = 1
								AND FA.ICAO = @ICAO
								AND F.Suspended = 0
			INNER JOIN PricingTemplate T ON T.FBOID = F.OID
			INNER JOIN [Group] G ON G.OID = F.GroupID AND G.IsFBONetwork = 1
			INNER JOIN (SELECT DISTINCT FL.FBOLinxID AS GroupID, FL.processNm AS Fueler, FL.fuelerNm AS FuelerName
						FROM FUELERLINX_fuelerData FD
						INNER JOIN FUELERLINX_fuelerList FL ON FL.OID = FD.fuelerID
																AND ISNULL(FL.FBOLinxID, 0) > 0
																AND FD.active = 1
						where companyID = @FuelerlinxCompanyID) FLF on FLF.GroupID = G.OID
			INNER JOIN Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			INNER JOIN CustomerInfoByGroup CIBG ON CIBG.CustomerID = C.OID
													AND CIBG.GroupID = G.OID
													AND CIBG.Suspended = 0
													AND CIBG.Active = 1
			INNER JOIN CustomerAircrafts CA ON CA.CustomerID = C.OID
												AND CA.TailNumber = @TailNumber
												AND CA.GroupID = FLF.GroupID
												AND ISNULL(C.ShowJetA, 1) = 1
			INNER JOIN (SELECT top 1 A.CustomerAircraftID, A.PriceTemplateID
					FROM AircraftPrices A
					INNER JOIN PricingTemplate PT ON PT.OID = A.PriceTemplateID
					INNER JOIN FBOAirports FA ON FA.ICAO = @ICAO
					INNER JOIN FBOs F ON F.OID = FA.FBOID
								AND F.Active = 1
								AND F.Suspended = 0
					INNER JOIN [Group] G ON G.OID = F.GroupID AND G.IsFBONetwork = 1 AND PT.FBOID = F.OID
					INNER JOIN Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			INNER JOIN CustomerInfoByGroup CIBG ON CIBG.CustomerID = C.OID
													AND CIBG.GroupID = G.OID
													AND CIBG.Suspended = 0
													AND CIBG.Active = 1
			INNER JOIN CustomerAircrafts CA ON CA.CustomerID = C.OID
												AND CA.TailNumber = @TailNumber
												AND CA.GroupID = G.OID
												AND ISNULL(C.ShowJetA, 1) = 1
												AND A.CustomerAircraftID=CA.OID
				
			order by a.oid asc)AP ON AP.CustomerAircraftID = CA.OID AND AP.PriceTemplateID = T.OID
			INNER JOIN VolumeScaleDiscount VSD ON VSD.CustomerID = CA.CustomerID AND VSD.FBOID = F.OID AND VSD.DefaultSettings = 0
			INNER JOIN FBOPrices FP ON F.OID = FP.FBOID AND FP.Price > 0 AND
										(((VSD.MarginType = 0 AND VSD.Margin > 0 AND FP.Product = 'JetA Cost') OR
												(VSD.MarginType = 1 AND FP.Product = 'JetA Retail')) AND
												(EffectiveFrom <= @date AND EffectiveTo > @date) AND
												(Expired IS NULL))
			LEFT JOIN
				(select PT.[Min], PT.[Max], PT.MaxEntered, CM.* 
				FROM CustomerMargins CM
				INNER JOIN PriceTiers PT ON PT.OID = CM.PriceTierID) P ON P.TemplateID = T.OID
			
			UNION ALL
			
			SELECT F.Fbo AS FBO,
			FLF.Fueler AS Fueler,
			'JetA' AS Product,
			ISNULL(P.[Min], 1) AS MinVolume,
			CASE WHEN ISNULL(T.MarginType, 0) = 0 THEN ((FP.Price + ABS(ISNULL(P.Amount, 0))) * (1 + ISNULL(FF.FeeAmount, 0) / 100))
															WHEN ISNULL(T.MarginType, 0) = 1
															THEN FP.Price + -ABS(ISNULL(P.Amount, 0))
															WHEN ISNULL(T.MarginType, 0) = 2
															THEN ABS(ISNULL(P.Amount, 0))
															END AS Price,
			'Direct Pricing' AS FuelerDisplayName,
			T.Notes
			FROM FBOAirports FA
			INNER JOIN FBOs F ON F.OID = FA.FBOID
								AND F.Active = 1
								AND FA.ICAO = @ICAO
								AND F.Suspended = 0
			INNER JOIN PricingTemplate T ON T.FBOID = F.OID
			INNER JOIN [Group] G ON G.OID = F.GroupID AND G.IsFBONetwork = 0
			INNER JOIN (SELECT DISTINCT FL.FBOLinxID AS GroupID, FL.processNm AS Fueler, FL.fuelerNm AS FuelerName
						FROM FUELERLINX_fuelerData FD
						INNER JOIN FUELERLINX_fuelerList FL ON FL.OID = FD.fuelerID
																AND ISNULL(FL.FBOLinxID, 0) > 0
																AND FD.active = 1
						where companyID = @FuelerlinxCompanyID) FLF on FLF.GroupID = G.OID
			INNER JOIN Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			inner JOIN CustomerInfoByGroup CIBG ON CIBG.CustomerID = C.OID
													AND CIBG.GroupID = G.OID
													AND CIBG.Distribute = 1
			INNER JOIN CustomerAircrafts CA ON CA.CustomerID = C.OID
												AND CA.TailNumber = @TailNumber
												AND CA.GroupID = FLF.GroupID
												AND ISNULL(C.ShowJetA, 1) = 1
			--INNER JOIN VolumeScaleDiscount VSD ON VSD.CustomerID = CA.CustomerID AND VSD.FBOID = F.OID
			LEFT JOIN AircraftPrices AP ON AP.PriceTemplateID = T.OID
											AND AP.CustomerAircraftID = CA.OID
			inner join CustomCustomerTypes CCT ON CCT.CustomerID = CIBG.CustomerID AND CCT.FBOID = F.OID and (case when isnull(AP.PriceTemplateID, 0) = 0 then CCT.CustomerType else AP.PriceTemplateID end = T.OID)
			INNER JOIN FBOPrices FP ON F.OID = FP.FBOID AND FP.Price > 0 AND
										(((T.MarginType = 0 AND FP.Product = 'JetA Cost') OR
												(T.MarginType = 1 AND FP.Product = 'JetA Retail') OR (T.MarginType = 2 AND FP.Product = 'JetA Retail')) AND
												(EffectiveFrom <= GETDATE() AND EffectiveTo > GETDATE()) AND
												(Expired IS NULL))
			INNER JOIN CustomersViewedByFBO CVBF ON CVBF.CustomerID = CIBG.CustomerID AND CVBF.FBOID = F.OID
			left join FBOFees FF ON FF.FBOID = F.OID AND T.MarginType = 0 AND FF.FeeType = 8
			LEFT JOIN
				(select PT.[Min], PT.[Max], PT.MaxEntered, CM.* 
				FROM CustomerMargins CM
				INNER JOIN PriceTiers PT ON PT.OID = CM.PriceTierID) P ON P.TemplateID = T.OID
			LEFT JOIN TempAddOnMargin taom on taom.fboid = f.oid
											and taom.effectiveFrom <= getdate()
											and dateadd(dd, 1, taom.effectiveTo) > getdate()
			
			
			ORDER BY Fueler, FBO, Product, MinVolume
	END
END

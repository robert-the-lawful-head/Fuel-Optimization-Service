GO
use [paragon_test]
GO
ALTER TABLE FBOFeesAndTaxes 
	ADD WhenToApply smallint null
GO
/****** Object:  Table [dbo].[FBOFeeAndTaxOmitsByCustomer]    Script Date: 11/19/2020 5:26:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FBOFeeAndTaxOmitsByCustomer](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[FBOFeeAndTaxID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
 CONSTRAINT [PK_FBOFeeAndTaxOmitsByCustomer] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[up_Load_PricesForFuelerlinx]    Script Date: 11/17/2020 2:37:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mike Mieglitz
-- Create date: 8/6/2012
-- Description:	Select the correct pricing for a Fuelerlinx customer
--*********************************************
--              Change History
--*********************************************
--  08/23/2013  MM  #55731402: Replaced single quotes in FBO and Group names in the results
--  09/04/2013  CL Added in DefaultSettings in join for Paragon
--  09/11/2013  CL Changed sales tax formula for non-Paragon; changed inner join of FBOPrices for non-Paragon to include flat fees
--  10/09/2013  CL Added in FuelerlinxID check for non-Paragon to compensate for new Distribute functionality for FuelerLinx customers
--  11/05/2013  MM  #53484515: Added check for active flag in fuelerData
--  02/05/2014  CL  Added + 1 when return VolB
--  05/09/2014  CL	Fixed default template for Paragon
--  05/11/2014	CL	Undid default template for Paragon change
--  06/05/2014	CL	Insert TemplateID for AircraftPrices records
--  07/29/2014  MM  Added PriceB column to the results
--	09/22/2015	CL	Added FP.Price > 0 to FBOPrices ON clause
--	10/02/2015	CL	Hopefully selecting the correct AircraftPrices record for Paragon to the best my ability with SELECT 1 instead
--	11/05/2015	CL	Added in another query to add in custom customer types for the AircraftPrices table instead of using the default template
--	12/30/2015	CL	Added in FBOFees to get correct sales tax for Cost+
--	2/2/2018	CL	Added an Inner Join to CustomersViewedByFBO
-- =============================================
ALTER PROCEDURE [dbo].[up_Load_PricesForFuelerlinx] (
	@FuelerlinxCompanyID int,
	@ICAO varchar(10),
	@TailNumber varchar(50),
	@Volume float = null
)
AS
BEGIN
	DECLARE @date datetime
	SELECT @date = GETDATE()
	
	IF @Volume < 1
		SET @Volume = 1
	
	--If the tail number doesn't exist, then we will use the default pricing
	--Add the tail number to the account to keep it in sync with Fuelerlinx
	--ABOVE FUNCTIONALITY TAKEN OUT 3/15/13
	IF (EXISTS(SELECT * FROM Customers WHERE FuelerlinxID = @FuelerlinxCompanyID))
		BEGIN
			
			--Add all default pricing for the aircraft for each FBO that has not already added pricing for it
			INSERT INTO AircraftPrices (CustomerAircraftID, PriceTemplateID)
			select CA.OID, ISNULL(CDT.PricingTemplateID, VSD.JetAVolumeDiscount) AS PricingTemplateID
			from FBOs F
			inner join PricingTemplate pt on pt.FBOID = F.OID and F.GroupID = 1
			inner join Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			inner join VolumeScaleDiscount vsd ON VSD.CustomerID = C.OID AND VSD.FBOID = pt.FBOID
			inner join CustomerInfoByGroup CIBG ON C.OID = CIBG.CustomerID AND CIBG.GroupID = F.GroupID
			inner join CustomerAircrafts CA ON CA.CustomerID = C.OID AND CA.TailNumber = @TailNumber and CA.GroupID = F.GroupID
			left join CustomerDefaultTemplates CDT ON CDT.PricingTemplateID = pt.OID 
			                                            and CDT.FBOID = F.OID 
			                                            and CDT.CustomerID = C.OID
			left join
			(select ap2.CustomerAircraftID, pt2.FBOID
				from PricingTemplate pt2
				left join AircraftPrices ap2 on ap2.PriceTemplateID = pt2.OID) AP ON AP.CustomerAircraftID = CA.OID and AP.FBOID = F.OID
			where AP.FBOID is null
			

			SELECT FA.ICAO, 
			        REPLACE(F.Fbo, '''', '') AS FBO, 
			        CA.TailNumber, 
					'JetA' AS product,
					ISNULL(P.[Min], 1) AS [VolA],
					case when ISNULL(P.[Max], 0) = 0 THEN 99999 ELSE (P.[Max] + 1) END AS [VolB],
					CASE WHEN ISNULL(VSD.MarginType, 0) = 0 THEN ((FP.Price + VSD.Margin) * (1 + ISNULL(FP.SalesTax, 0))) + ISNULL(P.Amount, 0)
															WHEN ISNULL(VSD.MarginType, 0) = 1 AND (VSD.Margin = 0) AND FLF.GroupID = 1
															THEN FP.Price + ISNULL(P.Amount, 0)
															WHEN ISNULL(VSD.MarginType, 0) = 1 AND (VSD.Margin = 0)
															THEN FP.Price * (1 + ISNULL(FP.SalesTax, 0)) + ISNULL(P.Amount, 0)
															ELSE(((FP.Price/(1 + ISNULL(FP.SalesTax, 0))) - VSD.Margin) * (1 + ISNULL(FP.SalesTax, 0))) + ISNULL(P.Amount, 0)
															END AS PriceA,
                    CASE WHEN ISNULL(VSD.MarginType, 0) = 0 THEN ((FP.Price + VSD.Margin) * (1 + ISNULL(FP.SalesTax, 0))) + ISNULL(P.Amount, 0)
															WHEN ISNULL(VSD.MarginType, 0) = 1 AND (VSD.Margin = 0) AND FLF.GroupID = 1
															THEN FP.Price + ISNULL(P.Amount, 0)
															WHEN ISNULL(VSD.MarginType, 0) = 1 AND (VSD.Margin = 0)
															THEN FP.Price * (1 + ISNULL(FP.SalesTax, 0)) + ISNULL(P.Amount, 0)
															ELSE(((FP.Price/(1 + ISNULL(FP.SalesTax, 0))) - VSD.Margin) * (1 + ISNULL(FP.SalesTax, 0))) + ISNULL(P.Amount, 0)
															END AS PriceB,
					T.[Notes] AS Note,
					FLF.Fueler AS Fueler,
					REPLACE(G.[Group], '''', '') + ' Direct' AS fuelerName,
					T.OID AS OID,
					C.FuelerlinxID AS CompanyID,
					FuelDeskEmail AS FBOEmail,
					'false' AS FuelInclusive,
					T.Notes AS PriceNote,
					0 AS member_ID,
					'' AS ContractID,
					T.Notes
			FROM FBOAirports FA
			INNER JOIN FBOs F ON F.OID = FA.FBOID
								AND F.Active = 1
								AND FA.ICAO = @ICAO
								AND F.Suspended = 0
			INNER JOIN PricingTemplate T ON T.FBOID = F.OID
			INNER JOIN [Group] G ON G.OID = F.GroupID AND G.IsFBONetwork = 1
			INNER JOIN (SELECT DISTINCT FL.FBOLinxID AS GroupID, FL.processNm AS Fueler
						FROM FUELERLINX_fuelerData FD
						INNER JOIN FUELERLINX_fuelerList FL ON FL.OID = FD.fuelerID
																AND ISNULL(FL.FBOLinxID, 0) > 0
																AND FD.active = 1
						where companyID = @FuelerlinxCompanyID) FLF on FLF.GroupID = G.OID
			INNER JOIN Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			INNER JOIN FUELERLINX_Companies FLCo on FLCo.OID = C.FuelerlinxID AND ISNULL(FLCo.HideinFbolinx, 0) = 0
			INNER JOIN CustomerInfoByGroup CIBG ON CIBG.CustomerID = C.OID
													AND CIBG.GroupID = G.OID
													AND CIBG.Suspended = 0
													AND CIBG.Active = 1
			INNER JOIN CustomerAircrafts CA ON CA.CustomerID = C.OID
												AND CA.TailNumber = @TailNumber
												AND CA.GroupID = FLF.GroupID
												AND ISNULL(CIBG.ShowJetA, 1) = 1
			INNER JOIN AircraftPrices AP ON AP.PriceTemplateID = T.OID
											AND AP.CustomerAircraftID = CA.OID
			--INNER JOIN (SELECT top 1 A.CustomerAircraftID, A.PriceTemplateID
			--		FROM AircraftPrices A
			--		INNER JOIN PricingTemplate PT ON PT.OID = A.PriceTemplateID
			--		INNER JOIN FBOAirports FA ON FA.ICAO = @ICAO
			--		INNER JOIN FBOs F ON F.OID = FA.FBOID
			--					AND F.Active = 1
			--					AND F.Suspended = 0
			--		INNER JOIN [Group] G ON G.OID = F.GroupID AND G.IsFBONetwork = 1 AND PT.FBOID = F.OID
			--		INNER JOIN Customers C ON C.FuelerlinxID = @FuelerlinxCompanyID
			--INNER JOIN CustomerInfoByGroup CIBG ON CIBG.CustomerID = C.OID
			--										AND CIBG.GroupID = G.OID
			--										AND CIBG.Suspended = 0
			--										AND CIBG.Active = 1
			--INNER JOIN CustomerAircrafts CA ON CA.CustomerID = C.OID
			--									AND CA.TailNumber = @TailNumber
			--									AND CA.GroupID = G.OID
			--									AND ISNULL(C.ShowJetA, 1) = 1
			--									AND A.CustomerAircraftID=CA.OID
				
			--order by a.oid asc)AP ON AP.CustomerAircraftID = CA.OID AND AP.PriceTemplateID = T.OID
			INNER JOIN VolumeScaleDiscount VSD ON VSD.CustomerID = CA.CustomerID AND VSD.FBOID = F.OID AND VSD.DefaultSettings = 0
			INNER JOIN FBOPrices FP ON F.OID = FP.FBOID AND FP.Price > 0 AND
										(((VSD.MarginType = 0 AND VSD.Margin > 0 AND FP.Product = 'JetA Cost') OR
												(VSD.MarginType = 1 AND FP.Product = 'JetA Retail')) AND
												(EffectiveFrom <= @date AND EffectiveTo > @date))
			LEFT JOIN
				(select CM.*, PT.[Min], PT.[Max], PT.MaxEntered
				FROM CustomerMargins CM
				INNER JOIN PriceTiers PT ON PT.OID = CM.PriceTierID
										AND ((PT.[Min] <= @Volume
										AND (PT.[Max] >= @Volume OR PT.[Max] = 0))
											OR @Volume is null)) P ON P.TemplateID = T.OID
			
			UNION ALL
			
			SELECT FA.ICAO, 
			        REPLACE(F.Fbo, '''', '') AS FBO, 
			        CA.TailNumber, 
					'JetA' AS product,
					ISNULL(P.[Min], 1) AS [VolA],
					case when ISNULL(P.[Max], 0) = 0 THEN 99999 ELSE (P.[Max] + 1) END AS [VolB],
					CASE WHEN ISNULL(T.MarginType, 0) = 0 THEN ((FP.Price + ABS(ISNULL(P.Amount, 0))) * (1 + ISNULL(FF.FeeAmount, 0) / 100))
															WHEN ISNULL(T.MarginType, 0) = 1
															THEN FP.Price + -ABS(ISNULL(P.Amount, 0))
															WHEN ISNULL(T.MarginType, 0) = 2
															THEN ABS(ISNULL(P.Amount, 0))
															END AS PriceA,
					CASE WHEN ISNULL(T.MarginType, 0) = 0 THEN ((FP.Price + ABS(ISNULL(P.Amount, 0))) * (1 + ISNULL(FF.FeeAmount, 0) / 100))
															WHEN ISNULL(T.MarginType, 0) = 1
															THEN FP.Price + -ABS(ISNULL(P.Amount, 0))
															WHEN ISNULL(T.MarginType, 0) = 2
															THEN ABS(ISNULL(P.Amount, 0))
															END AS PriceB,
					T.[Notes] AS Note,
					FLF.Fueler AS Fueler,
					'Direct Pricing' AS fuelerName,
					T.OID AS OID,
					C.FuelerlinxID AS CompanyID,
					FuelDeskEmail AS FBOEmail,
					'false' AS FuelInclusive,
					T.Notes AS PriceNote,
					0 AS member_ID,
					'' AS ContractID,
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
												(EffectiveFrom <= GETDATE() AND EffectiveTo > GETDATE()))
			INNER JOIN CustomersViewedByFBO CVBF ON CVBF.CustomerID = CIBG.CustomerID AND CVBF.FBOID = F.OID
			left join FBOFees FF ON FF.FBOID = F.OID AND T.MarginType = 0 AND FF.FeeType = 8
			LEFT JOIN
				(select PT.[Min], PT.[Max], PT.MaxEntered, CM.* 
				FROM CustomerMargins CM
				INNER JOIN PriceTiers PT ON PT.OID = CM.PriceTierID) P ON P.TemplateID = T.OID
			LEFT JOIN TempAddOnMargin taom on taom.fboid = f.oid
											and taom.effectiveFrom <= getdate()
											and dateadd(dd, 1, taom.effectiveTo) > getdate()
			WHERE isnull(g.IsLegacyAccount, 0) = 1
			ORDER BY Fueler, Fbo, Product, VolA
		END
END
GO
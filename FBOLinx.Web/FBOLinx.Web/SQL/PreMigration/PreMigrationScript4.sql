IF OBJECT_ID('tempdb..#Temp') IS NOT NULL
    DROP TABLE #Temp

IF OBJECT_ID('tempdb..#NewMarginTemplate') IS NOT NULL
	DROP TABLE #NewMarginTemplate

IF OBJECT_ID('tempdb..#OriginalMarginTemplate') IS NOT NULL
	DROP TABLE #OriginalMarginTemplate


SELECT COUNT(*) As MarginCount, CM.TemplateID
INTO #Temp
FROM
(
	SELECT CustomerMargins.*, PriceTiers.Min, PriceTiers.Max
	FROM CustomerMargins
	LEFT JOIN PricingTemplate ON CustomerMargins.TemplateID = PricingTemplate.OID
	LEFT JOIN PriceTiers ON CustomerMargins.PriceTierID = PriceTiers.OID
	WHERE PricingTemplate.Name = 'Default Template' AND CustomerMargins.PriceTierID != 0
) CM
GROUP BY CM.TemplateID

CREATE TABLE #NewMarginTemplate
(
	OID int,
	Min float,
	Amount float
)

INSERT INTO #NewMarginTemplate VALUES (1, 0, 0)
INSERT INTO #NewMarginTemplate VALUES (2, 251, 0)
INSERT INTO #NewMarginTemplate VALUES (3, 501, 0)
INSERT INTO #NewMarginTemplate VALUES (4, 1001, 0)

SELECT OMT.MOID, OMT.PriceTierID, OMT.TemplateID, OMT.OID, #NewMarginTemplate.Min, #NewMarginTemplate.Amount
INTO #OriginalMarginTemplate
FROM
(
	SELECT ROW_NUMBER() OVER (ORDER BY PriceTierID) AS MOID, * 
	FROM
	(
		SELECT CustomerMargins.*, PriceTiers.Min, PriceTiers.Max
		FROM CustomerMargins
		LEFT JOIN PricingTemplate ON CustomerMargins.TemplateID = PricingTemplate.OID
		LEFT JOIN PriceTiers ON CustomerMargins.PriceTierID = PriceTiers.OID
		WHERE PricingTemplate.Name = 'Default Template' 
			AND CustomerMargins.PriceTierID != 0 
			AND CustomerMargins.TemplateID IN (SELECT TOP 1 TemplateID FROM #Temp WHERE MarginCount = 4)
	) CM
) OMT
LEFT JOIN #NewMarginTemplate ON OMT.MOID = #NewMarginTemplate.OID

UPDATE CustomerMargins SET CustomerMargins.Amount = #OriginalMarginTemplate.Amount
FROM #OriginalMarginTemplate
WHERE CustomerMargins.OID = #OriginalMarginTemplate.OID

UPDATE PriceTiers SET PriceTiers.Min = #OriginalMarginTemplate.Min
FROM #OriginalMarginTemplate
WHERE PriceTiers.OID = #OriginalMarginTemplate.PriceTierID

DROP TABLE #Temp
DROP TABLE #NewMarginTemplate
DROP TABLE #OriginalMarginTemplate

USE paragon_test
GO
ALTER TABLE FBOFeeAndTaxOmitsByCustomer
	ADD IsOmitted bit NULL

ALTER TABLE FBOFeeAndTaxOmitsByPricingTemplate
	ADD IsOmitted bit NULL
GO

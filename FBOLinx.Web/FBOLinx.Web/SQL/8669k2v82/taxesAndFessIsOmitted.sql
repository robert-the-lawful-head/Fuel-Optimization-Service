USE paragon_test

ALTER TABLE FBOFeeAndTaxOmitsByCustomer
	ADD IsOmitted bit NULL

ALTER TABLE FBOFeeAndTaxOmitsByPricingTemplate
	ADD IsOmitted bit NULL
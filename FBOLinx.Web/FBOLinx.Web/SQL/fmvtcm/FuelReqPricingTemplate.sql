CREATE TABLE FuelReqPricingTemplate (
	OID INT IDENTITY(1,1) NOT NULL,
	FuelReqId INT NULL,
	PricingTemplateId INT NULL,
	PricingTemplateName NVARCHAR(255) NULL,
	PricingTemplateRaw VARCHAR(MAX) NULL,
);

GO

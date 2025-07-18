ALTER TABLE [dbo].[FBOs]
DROP CONSTRAINT [FK_FBOs_Groups]

GO

ALTER TABLE [dbo].[FBOs] WITH CHECK ADD CONSTRAINT [FK_FBOs_Groups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([OID])
ON DELETE CASCADE
ON UPDATE CASCADE

GO

ALTER TABLE [dbo].[FBOs] CHECK CONSTRAINT [FK_FBOs_Groups]
GO

ALTER TABLE dbo.AdminEmails
ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomerAircraftViewedByGroup
ADD OID INT IDENTITY;
GO

ALTER TABLE CustomerAircraftViewedByGroup
ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomerNotes
ADD OID INT IDENTITY;
GO

ALTER TABLE CustomerNotes
ADD PRIMARY KEY (OID)
GO

ALTER TABLE CustomerSchedulingSoftwareByGroup
ADD OID INT IDENTITY;
GO

ALTER TABLE CustomerSchedulingSoftwareByGroup
ADD PRIMARY KEY (OID)
GO

ALTER TABLE Jobs
ADD PRIMARY KEY (OID)
GO

ALTER TABLE NetworkNotes
ADD PRIMARY KEY (OID)
GO

ALTER TABLE ContractFuelRelationships
ADD PRIMARY KEY (OID)
GO

ALTER TABLE DistributionEmailsBody
ADD PRIMARY KEY (OID)
GO

ALTER TABLE FBOCustomerPricing
ADD OID INT IDENTITY;
GO

ALTER TABLE FBOCustomerPricing
ADD PRIMARY KEY (OID)
GO

ALTER TABLE FBOLogos
ADD OID INT IDENTITY;
GO

ALTER TABLE FBOLogos
ADD PRIMARY KEY (OID)
GO

ALTER TABLE FBOSalesTax
ADD OID INT IDENTITY;
GO

ALTER TABLE FBOSalesTax
ADD PRIMARY KEY (OID)
GO

ALTER TABLE PriceHistory
ADD OID INT IDENTITY;
GO

ALTER TABLE PriceHistory
ADD PRIMARY KEY (OID)
GO

ALTER TABLE RequestPricingTracker
ADD PRIMARY KEY (OID)
GO

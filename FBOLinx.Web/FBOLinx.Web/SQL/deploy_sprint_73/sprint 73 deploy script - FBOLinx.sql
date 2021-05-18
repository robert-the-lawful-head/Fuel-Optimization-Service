use [paragon_test]
GO
ALTER TABLE Contacts
ADD CopyOrders bit
GO
ALTER TABLE [User]
ADD CopyOrders bit
GO
ALTER TABLE PricingTemplate
ADD EmailContentId int
GO
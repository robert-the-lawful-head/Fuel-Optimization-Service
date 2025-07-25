USE [paragon_test]
GO
/****** Object:  StoredProcedure [dbo].[up_Delete_Group]    Script Date: 10/11/2024 5:33:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[up_Delete_Group]
	@OID int
AS

SET NOCOUNT ON

DELETE FROM CompaniesByGroup WHERE GroupId = @OID

DELETE FROM ContactInfoByGroup WHERE GroupId = @OID

DELETE fa FROM FboFavoriteAircraft fa
INNER JOIN CustomerAircrafts ca on fa.customeraircraftid = ca.oid
WHERE ca.groupid = @OID

DELETE FROM CustomerAircrafts WHERE GroupId = @OID

--DELETE FROM CustomerCompanyTypes WHERE GroupId = @OID

DELETE FROM CustomerInfoByGroup WHERE GroupId = @OID

DELETE FROM DistributionLog WHERE GroupId = @OID

DELETE FROM DistributionQueue WHERE GroupId = @OID

--DELETE FROM MappingPrices WHERE GroupId = @OID

DELETE FROM [User] WHERE GroupId = @OID

DELETE FROM AdminEmails WHERE GroupId = @OID

--DELETE FROM CustomerAircraftViewedByGroup WHERE GroupId = @OID

--DELETE FROM CustomerNotes WHERE GroupId = @OID

--DELETE FROM CustomerSchedulingSoftwareByGroup WHERE GroupId = @OID

--DELETE FROM Jobs WHERE GroupId = @OID

--DELETE FROM NetworkNotes WHERE GroupId = @OID

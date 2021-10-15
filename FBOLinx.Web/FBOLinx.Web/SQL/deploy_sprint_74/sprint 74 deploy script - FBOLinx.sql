use [paragon_test]
GO
ALTER TABLE FBOs
ADD AccountType smallint
GO
UPDATE FBOs
SET AccountType=0
GO
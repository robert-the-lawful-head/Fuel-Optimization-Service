USE [paragon_test]
GO

ALTER TABLE FBOPrices
ADD Source smallint;
GO

update fboprices set source=1 where EffectiveTo>='9999-12-31'
update fboprices set source=0 where EffectiveTo<'9999-12-31'
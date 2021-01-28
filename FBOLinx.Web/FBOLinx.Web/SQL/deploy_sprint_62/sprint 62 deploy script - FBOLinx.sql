use [paragon_test]
GO
delete from AircraftPrices WHERE oid IN (
  select distinct ap.oid
  from CustomerAircrafts ca
  inner join AircraftPrices ap on ap.CustomerAircraftID = ca.oid
  inner join [Group] g on g.oid = ca.GroupID
  inner join PricingTemplate pt on pt.oid = ap.PriceTemplateID
  inner join FBOs f on pt.fboid = f.oid
  where ca.groupid != f.groupid
)
GO
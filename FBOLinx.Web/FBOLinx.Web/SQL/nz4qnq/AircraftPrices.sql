delete a
FROM [CustomerAircrafts] AS [c]
INNER JOIN [AircraftPrices] AS [a] ON [c].[OID] = [a].[CustomerAircraftID]
inner join PricingTemplate pt on pt.oid = a.PriceTemplateID
left join FBOs f on f.oid = pt.fboid and f.groupid = c.Groupid
where f.OID is null
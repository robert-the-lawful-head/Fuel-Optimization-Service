[use] paragon_Test
go

select ca.* 
into #AircraftWithAssignedPricing
from customeraircrafts ca
inner join aircraftprices ap on ap.customeraircraftid=ca.oid and ca.groupid>1
inner join pricingtemplate pt on pt.oid=ap.PriceTemplateID


--Add all customer aircraft to the #ResultsForEvaluation temp table.  
--The MinimumValidCustomerAircraftID column should be the assigned AircraftPrices CustomerAircraftID if available.  Otherwise, the minimum OID of the customerAircrafts record.
select min(ap.oid) as MinAssignedPricingOID,
	max(ap.oid) as MaxAssignedPricingOID,
	min(case when ap.oid > 0 then ap.oid else ca.oid end) as MinimumValidCustomerAircraftID,
	ca.GroupID,
	ca.CustomerID,
	ca.TailNumber	
into #ResultsForEvaluation
from CustomerAircrafts ca
left join #AircraftWithAssignedPricing ap on ap.OID = ca.oid
where ca.GroupID > 1
group by ca.GroupID, ca.customerid, ca.tailnumber


--Results that require an evaluation because a pricing was somehow assigned to multiple copies (CustomerAircraft records) of the tail.  Should be handled first and corrected manually.
--select *
--from #ResultsForEvaluation re
--where re.MinAssignedPricingOID <> MaxAssignedPricingOID



--Results that should be OK to delete after the ABOVE is cleaned up - PLEASE DOUBLE-CHECK SOME EXAMPLES FROM THIS LIST.  
delete ca
from CustomerAircrafts ca
left join #ResultsForEvaluation re on re.MinimumValidCustomerAircraftID = ca.oid
where ca.GroupID > 1
and re.MinimumValidCustomerAircraftID is null



drop table #AircraftWithAssignedPricing
drop table #ResultsForEvaluation



update ca
set customerid=0
from customeraircrafts ca
inner join customers c on c.oid=ca.customerid
inner join sqlfl.dbo.companies co on co.oid=c.fuelerlinxid
left join sqlfl.dbo.acdata2 ac on ac.tailno=ca.tailnumber
left join sqlfl.dbo.useraircraft ua on ua.tailnumid=ac.oid
where (ac.oid is null or ua.oid is null) and ca.groupid>1
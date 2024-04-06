declare @maxserviceorders int
select @maxserviceorders = max(oid)
from ServiceOrders

declare @fbolinxvendorid int

select @fbolinxvendorid = oid
from FuelerLinx_FuelerList fl
where fl.processnm = 'fbolinx'

insert into ServiceOrders
select
f.oid as fboid, f.groupid, cibg.oid as customerinfobygroupid, 
case when isnull(d.FuelOn, 'departure') = 'arrival' then isnull(fr.eta, GETDATE()) else isnull(fr.etd, GETDATE()) end as servicedatetimeutc, 
fr.eta as arrivaldatetimeutc, fr.etd as departuredatetimeutc, ca.oid as customeraircraftid, 
case when fr.fuelerid=@fbolinxvendorid then d.VendorServiceID else 0 end as associatedfuelorderid, fr.oid as fuelerlinxtransactionid, 
case when isnull(d.FuelOn, 'departure') ='Arrival' then 0 else 1 end as serviceon
FROM FuelerLinx_FuelReq fr
inner join sqlfl.dbo.transactiondetails d on fr.oid=d.requestid
left join sqlfl.dbo.AcukwikFBOHandlerLookup afl on afl.ICAO = fr.ICAO and afl.FboHandlerName = fr.FBO
left join fbos f on f.acukwikfbohandlerid=afl.acukwikfbohandlerid
inner join customers c on fr.companyid=c.fuelerlinxid
inner join customerinfobygroup cibg on c.oid=cibg.customerid and f.groupid=cibg.groupid
inner join customeraircrafts ca on ca.groupid=f.groupid and ca.customerid=c.oid and fr.tail_number=ca.tailnumber
WHERE f.oid>0 and fr.fuel_est_weight > 0 
order by fr.oid

insert into ServiceOrderItems
select
s.oid as serviceorderid, 'Fuel: ' + CAST(fr.fuel_est_weight as varchar(10)) + case when fuel_est_weight = 1 and fr.fuelerid = @fbolinxvendorid then ' gal @ ' + FORMAT(fr.fuel_est_cost, 'c', 'en-US') when fuel_est_weight > 1 and fuelerid = @fbolinxvendorid then ' gals @ ' + FORMAT(fr.fuel_est_cost, 'c', 'en-US') when fuel_est_weight = 1 and fuelerid <> @fbolinxvendorid then ' gal'  else ' gals' end as servicename, 0 as quantity, null as iscompleted, null as completiondatetimeutc, null as completedbyuserid, null as completedbyname, null as addedbyuserid, null as addedbyname, null as servicenote
from ServiceOrders s
inner join FuelerLinx_FuelReq fr on fr.oid=s.fuelerlinxtransactionid
where s.oid>@maxserviceorders



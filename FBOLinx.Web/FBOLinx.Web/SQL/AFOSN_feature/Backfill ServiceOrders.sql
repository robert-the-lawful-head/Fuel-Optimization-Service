declare @maxserviceorders int
select @maxserviceorders = max(oid)
from ServiceOrders

declare @fbolinxvendorid int

select @fbolinxvendorid = oid
from FuelerLinx_FuelerList fl
where fl.processnm = 'fbolinx'

-- insert into ServiceOrders
select
f.oid as fboid, f.groupid, cibg.oid as customerinfobygroupid, 
case when isnull(d.FuelOn, 'departure') = 'arrival' then isnull(fr.eta, GETDATE()) else isnull(fr.etd, GETDATE()) end as servicedatetimeutc, 
fr.eta as arrivaldatetimeutc, fr.etd as departuredatetimeutc, ca.oid as customeraircraftid, 
frf.oid as associatedfuelorderid, 
fr.oid as fuelerlinxtransactionid, 
case when isnull(d.FuelOn, 'departure') ='Arrival' then 0 else 1 end as serviceon
FROM FuelerLinx_FuelReq fr
inner join sqlfl.dbo.transactiondetails d on fr.oid = d.requestid
left join fuelreq frf on fr.oid = frf.SourceID
left join sqlfl.dbo.AcukwikFBOHandlerLookup afl on afl.ICAO = fr.ICAO and afl.FboHandlerName = fr.FBO
left join fbos f on f.acukwikfbohandlerid=afl.acukwikfbohandlerid
inner join customers c on fr.companyid=c.fuelerlinxid
inner join customerinfobygroup cibg on c.oid=cibg.customerid and f.groupid=cibg.groupid
inner join customeraircrafts ca on ca.groupid=f.groupid and ca.customerid=c.oid and fr.tail_number=ca.tailnumber
WHERE f.oid>0 and isnull(fr.isplaceholder,0)=0 and fr.fuel_est_weight > 0 and ((fr.fuelerid <> @fbolinxvendorid and isnull(fr.cancelled,0)=0) or (fr.fuelerid=@fbolinxvendorid))
order by fr.oid

-- If tail doesn't belong to that customer anymore
--insert into ServiceOrders
select
f.oid as fboid, f.groupid, cibg.oid as customerinfobygroupid, 
case when isnull(d.FuelOn, 'departure') = 'arrival' then isnull(fr.eta, GETDATE()) else isnull(fr.etd, GETDATE()) end as servicedatetimeutc, 
fr.eta as arrivaldatetimeutc, fr.etd as departuredatetimeutc, isnull(frf.customeraircraftid,0) as customeraircraftid,
frf.oid as associatedfuelorderid, 
fr.oid as fuelerlinxtransactionid, 
case when isnull(d.FuelOn, 'departure') ='Arrival' then 0 else 1 end as serviceon
FROM FuelerLinx_FuelReq fr
inner join sqlfl.dbo.transactiondetails d on fr.oid = d.requestid
inner join fuelreq frf on fr.oid = frf.SourceID
left join sqlfl.dbo.AcukwikFBOHandlerLookup afl on afl.ICAO = fr.ICAO and afl.FboHandlerName = fr.FBO
left join fbos f on f.acukwikfbohandlerid=afl.acukwikfbohandlerid
inner join customers c on fr.companyid=c.fuelerlinxid
inner join customerinfobygroup cibg on c.oid=cibg.customerid and f.groupid=cibg.groupid
left join customeraircrafts ca on ca.groupid=f.groupid and ca.customerid=c.oid and fr.tail_number=ca.tailnumber
WHERE ca.oid is null and f.oid>0 and isnull(fr.isplaceholder,0)=0 and fr.fuel_est_weight > 0 and ((fr.fuelerid <> @fbolinxvendorid and isnull(fr.cancelled,0)=0) or (fr.fuelerid=@fbolinxvendorid))
order by fr.oid

-- Clay Lacy changes from 1-> 0 after dispatch
--insert into ServiceOrders
select
f.oid as fboid, f.groupid, cibg.oid as customerinfobygroupid, 
case when isnull(d.FuelOn, 'departure') = 'arrival' then isnull(fr.eta, GETDATE()) else isnull(fr.etd, GETDATE()) end as servicedatetimeutc, 
fr.eta as arrivaldatetimeutc, fr.etd as departuredatetimeutc, ca.oid as customeraircraftid, 
frf.oid as associatedfuelorderid, 
fr.oid as fuelerlinxtransactionid, 
case when isnull(d.FuelOn, 'departure') ='Arrival' then 0 else 1 end as serviceon
FROM FuelerLinx_FuelReq fr
inner join sqlfl.dbo.transactiondetails d on fr.oid = d.requestid
left join fuelreq frf on fr.oid = frf.SourceID
left join sqlfl.dbo.AcukwikFBOHandlerLookup afl on afl.ICAO = fr.ICAO and afl.FboHandlerName = fr.FBO
left join fbos f on f.acukwikfbohandlerid=afl.acukwikfbohandlerid
inner join customers c on fr.companyid=c.fuelerlinxid
inner join customerinfobygroup cibg on c.oid=cibg.customerid and f.groupid=cibg.groupid
inner join customeraircrafts ca on ca.groupid=f.groupid and ca.customerid=c.oid and fr.tail_number=ca.tailnumber
WHERE f.oid>0 and isnull(fr.isplaceholder,0)=0 and fr.fuel_est_weight = 0 and fr.CompanyId = 91
and fr.FuelerID = @fbolinxvendorid
order by fr.oid

-- Other flight depts changes from 1-> 0 after dispatch
--insert into ServiceOrders
select
f.oid as fboid, f.groupid, cibg.oid as customerinfobygroupid, 
case when isnull(d.FuelOn, 'departure') = 'arrival' then isnull(fr.eta, GETDATE()) else isnull(fr.etd, GETDATE()) end as servicedatetimeutc, 
fr.eta as arrivaldatetimeutc, fr.etd as departuredatetimeutc, ca.oid as customeraircraftid, 
frf.oid as associatedfuelorderid, 
fr.oid as fuelerlinxtransactionid, 
case when isnull(d.FuelOn, 'departure') ='Arrival' then 0 else 1 end as serviceon
FROM FuelerLinx_FuelReq fr
inner join sqlfl.dbo.transactiondetails d on fr.oid = d.requestid
inner join fuelreq frf on fr.oid = frf.SourceID
left join sqlfl.dbo.AcukwikFBOHandlerLookup afl on afl.ICAO = fr.ICAO and afl.FboHandlerName = fr.FBO
left join fbos f on f.acukwikfbohandlerid=afl.acukwikfbohandlerid
inner join customers c on fr.companyid=c.fuelerlinxid
inner join customerinfobygroup cibg on c.oid=cibg.customerid and f.groupid=cibg.groupid
inner join customeraircrafts ca on ca.groupid=f.groupid and ca.customerid=c.oid and fr.tail_number=ca.tailnumber
WHERE f.oid>0 and isnull(fr.isplaceholder,0)=0 and fr.fuel_est_weight = 0 and fr.companyid<>91
and fr.FuelerID = @fbolinxvendorid
order by fr.oid


insert into ServiceOrderItems
select
s.oid as serviceorderid, 'Fuel: ' + CAST(case when fuel_est_weight = 0 then 1 else fr.fuel_est_weight end as varchar(10)) + case when (fuel_est_weight = 1 or fuel_est_weight = 0) and fr.fuelerid = @fbolinxvendorid then ' gal @ ' + FORMAT(fr.fuel_est_cost, 'c', 'en-US') when fuel_est_weight > 1 and fuelerid = @fbolinxvendorid then ' gals @ ' + FORMAT(fr.fuel_est_cost, 'c', 'en-US') when (fuel_est_weight = 1 or fuel_est_weight = 0) and fuelerid <> @fbolinxvendorid then ' gal'  else ' gals' end as servicename, 0 as quantity, null as iscompleted, null as completiondatetimeutc, null as completedbyuserid, null as completedbyname, null as addedbyuserid, null as addedbyname, null as servicenote
from ServiceOrders s
inner join FuelerLinx_FuelReq fr on fr.oid=s.fuelerlinxtransactionid
--where s.oid>@maxserviceorders



--- delete 2 OrderDetails duplicates from clay lacy's account
select * from serviceorders where fuelerlinxtransactionid=4941454

delete from serviceorderitems where serviceorderid=
delete from serviceorders where oid=





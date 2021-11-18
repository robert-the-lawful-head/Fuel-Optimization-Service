use [paragon_test]
GO
update frp
set frp.cancelled=fr.cancelled
from sqlfl.dbo.fuelreq fr
inner join sqlfl.dbo.transactiondetails td on td.requestid=fr.oid and fuelerid=331 and td.vendorserviceid!='' and fr.cancelled=1
inner join paragon_test.dbo.fuelreq frp on TRY_CAST(td.vendorserviceid AS INT)=frp.oid and frp.cancelled=0
GO
delete allca
from CustomerAircrafts allCA
left join (
select min(OID) as CorrectOID, GroupID, CustomerID, TailNumber, AircraftID
from CustomerAircrafts CA where groupid>1
group by GroupID, CustomerID, tailnumber, aircraftid) correctCA on correctCA.correctOID = allCA.OID
where correctCA.correctOID is null and allCA.groupid>1
GO
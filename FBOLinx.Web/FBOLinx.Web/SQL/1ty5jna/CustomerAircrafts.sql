delete allca
from CustomerAircrafts allCA
left join (
select min(OID) as CorrectOID, GroupID, CustomerID, TailNumber, AircraftID
from CustomerAircrafts CA where groupid>1
group by GroupID, CustomerID, tailnumber, aircraftid) correctCA on correctCA.correctOID = allCA.OID
where correctCA.correctOID is null and allCA.groupid>1
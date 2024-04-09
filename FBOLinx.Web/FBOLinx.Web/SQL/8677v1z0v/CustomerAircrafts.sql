use [paragon_Test]


--select *
delete caDelete
from customeraircrafts caDelete
left join (

select min(ca.oid) as minOID, ca.GroupID, ca.TailNumber, ca.CustomerId

from customeraircrafts ca
group by ca.GroupID, ca.TailNumber, ca.CustomerId
) validRecords on validRecords.minOID = caDelete.OID

where validRecords.minOID is null
--order by caDelete.groupid
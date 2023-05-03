use paragon_test
delete cDelete
from customerinfobygroup cDelete
left join (

select min(c.oid) as minOID, c.GroupID, c.CustomerId

from customerinfobygroup c
group by c.GroupID, c.CustomerId
) validRecords on validRecords.minOID = cDelete.OID

where validRecords.minOID is null
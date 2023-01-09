[use] paragon_Test

delete caDelete
from CustomerAircrafts caDelete
WHERE OID NOT IN
(

select min(ca.oid) 

from CustomerAircrafts ca 
group by ca.GroupID, ca.customerid, tailnumber
) 



update ca
set customerid=0
from customeraircrafts ca
inner join customers c on c.oid=ca.customerid
inner join sqlfl.dbo.companies co on co.oid=c.fuelerlinxid
left join sqlfl.dbo.acdata2 ac on ac.tailno=ca.tailnumber
where ac.oid is null and ca.groupid>1 and ca.addedfrom=1

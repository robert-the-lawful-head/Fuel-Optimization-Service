use paragon_test
GO

select co.oid,ac.tailno
INTO #TempUserAircrafts
from sqlfl.dbo.companies co
inner join sqlfl.dbo.custdata cd on co.oid=cd.companyid
inner join sqlfl.dbo.useraircraft ua on ua.custID=cd.oid
inner join sqlfl.dbo.acdata2 ac on ac.oid=ua.tailnumid
where cd.primaryaccount=1
GO

update ca
set ca.AddedFrom = 1
from customeraircrafts ca 
inner join customers c on ca.customerid=c.oid
inner join [group] g on g.oid=ca.groupid
inner join #TempUserAircrafts tu on tu.oid=c.fuelerlinxid
where ca.tailnumber=tu.tailno and isnull(ca.addedfrom, 0)=0
GO

drop table #TempUserAircrafts
GO
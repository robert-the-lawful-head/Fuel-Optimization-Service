update frp
set frp.cancelled=fr.cancelled
from fuelreq fr
inner join transactiondetails td on td.requestid=fr.oid and fuelerid=331 and td.vendorserviceid!='' and fr.cancelled=1
inner join paragon_test.dbo.fuelreq frp on TRY_CAST(td.vendorserviceid AS INT)=frp.oid and frp.cancelled=0
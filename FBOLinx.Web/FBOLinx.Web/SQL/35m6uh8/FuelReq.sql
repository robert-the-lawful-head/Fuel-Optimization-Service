[use] paragon_Test

--Duplicates to delete that will have a remaining record with the same SourceID
delete fr
from fuelreq fr
inner join fbos f on f.oid=fr.fboid and f.groupid>1
inner join fuelreq frKeep on frKeep.SourceID = fr.SourceID and frKeep.OID <> fr.OID
inner join CustomerAircrafts caKeep on caKeep.oid = frKeep.CustomerAircraftID
left join customeraircrafts ca on ca.oid=fr.customeraircraftid
where fr.ETA > '01-01-2020'
and ca.oid is null
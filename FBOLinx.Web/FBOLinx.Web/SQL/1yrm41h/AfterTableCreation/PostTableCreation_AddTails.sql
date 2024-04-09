insert into AircraftHexTailMapping (AircraftHexCode, TailNumber)
select f.AircraftHexCode, f.TailNumber
from FAAAircraftHexTailMapping f
left join AircraftHexTailMapping a on a.AircraftHexCode = f.AircraftHexCode
where a.oid is null
and isnull(f.TailNumber, '') <> ''
and isnull(f.AircraftHexCode, '') <> ''
group by f.AircraftHexCode, f.TailNumber

update a
set a.TailNumber = a.TailNumber
from AircraftHexTailMapping a
inner join FAAAircraftHexTailMapping f on f.AircraftHexCode = a.aircraftHexCode
where isnull(f.TailNumber, '') <> ''
and isnull(f.AircraftHexCode, '') <> ''
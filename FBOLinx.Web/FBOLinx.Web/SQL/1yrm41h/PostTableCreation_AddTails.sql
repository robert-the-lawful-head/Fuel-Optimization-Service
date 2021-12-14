insert into AircraftHexTailMapping (AircraftHexCode, TailNumber)
select f.AircraftHexCode, f.TailNumber
from FAAAircraftHexTailMapping f
inner join AirportWatchAircraftTailNumber aw on aw.AtcFlightNumber = f.TailNumber and aw.AircraftHexCode = f.AircraftHexCode
left join AircraftHexTailMapping a on a.AircraftHexCode = f.AircraftHexCode and a.TailNumber = f.TailNumber
where a.oid is null
group by f.AircraftHexCode, f.TailNumber
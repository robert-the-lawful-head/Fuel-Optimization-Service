USE [paragon_test]

GO

DECLARE @FAAAircraftHexTailMapping TABLE ([AircraftHexCode] varchar(25),  [TailNumber] varchar(25));

INSERT @FAAAircraftHexTailMapping SELECT AircraftHexCode,TailNumber FROM Dega.[dbo].[AircraftHexTailMapping];

UPDATE a
SET a.TailNumber = f.TailNumber
FROM AirportWatchHistoricalData a
INNER JOIN @FAAAircraftHexTailMapping f ON f.AircraftHexCode = a.AircraftHexCode
WHERE isnull(f.TailNumber, '') <> ''

UPDATE a
SET a.TailNumber = f.TailNumber
FROM AirportWatchLiveData a
INNER JOIN @FAAAircraftHexTailMapping f ON f.AircraftHexCode = a.AircraftHexCode
WHERE isnull(f.TailNumber, '') <> ''

GO
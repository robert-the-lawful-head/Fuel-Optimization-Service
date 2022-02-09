USE [paragon_test]

GO

ALTER TABLE [AirportWatchHistoricalData]
ADD [TailNumber] [varchar](25) NULL

GO
USE [paragon_test]

GO

ALTER TABLE [AirportWatchLiveData]
ADD [TailNumber] [varchar](25) NULL

GO
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
use [paragon_test]
GO
ALTER TABLE fbopreferences
ADD EnableJetA bit;

ALTER TABLE fbopreferences
ADD EnableSaf bit;


update fp
set fp.enablejeta=1, fp.enablesaf=1
from fbos f
inner join fbopreferences fp on f.oid=fp.fboid and f.groupid>1
GO

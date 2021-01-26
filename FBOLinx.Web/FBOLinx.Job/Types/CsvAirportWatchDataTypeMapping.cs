using System;
using System.Collections.Generic;
using System.Text;
using TinyCsvParser.Mapping;

namespace FBOLinx.Job.Types
{
    class CsvAirportWatchDataTypeMapping : CsvMapping<AirportWatchDataType>
    {
        public CsvAirportWatchDataTypeMapping()
            : base()
        {
            MapProperty(0, x => x.BoxTransmissionDateTimeUtc);
            MapProperty(1, x => x.AircraftHexCode);
            MapProperty(2, x => x.AtcFlightNumber);
            MapProperty(3, x => x.AltitudeInStandardPressure);
            MapProperty(4, x => x.GroundSpeedKts);
            MapProperty(5, x => x.TrackingDegree);
            MapProperty(6, x => x.Latitude);
            MapProperty(7, x => x.Longitude);
            MapProperty(8, x => x.VerticalSpeedKts);
            MapProperty(9, x => x.TransponderCode);
            MapProperty(10, x => x.BoxName);
            MapProperty(11, x => x.AircraftPositionDateTimeUtc);
            MapProperty(12, x => x.AircraftTypeCode);
            MapProperty(13, x => x.GpsAltitude);
            MapProperty(14, x => x.IsAircraftOnGround);
        }
    }
}

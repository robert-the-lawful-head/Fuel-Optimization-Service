using FBOLinx.Job.Interfaces;

namespace FBOLinx.Job.AirportWatch
{
    public class AirportWatchCsvParser : ICsvReader<AirportWatchDataType>
    {
        public AirportWatchCsvParser(string filePath) : base(filePath)
        {
        }

        public override AirportWatchDataType ParseCsvLineToEntity(string line)
        {
            var fields = line.Split(",");
            if (fields.Length < 15)
            {
                return null;
            }

            AirportWatchDataType record = new AirportWatchDataType();
            record.BoxTransmissionDateTimeUtc = GetSafeField(fields, 0);
            record.AircraftHexCode = GetSafeField(fields, 1);
            record.AtcFlightNumber = GetSafeField(fields, 2);
            record.AltitudeInStandardPressure = GetSafeField(fields, 3);
            record.GroundSpeedKts = GetSafeField(fields, 4);
            record.TrackingDegree = GetSafeField(fields, 5);
            record.Latitude = GetSafeField(fields, 6);
            record.Longitude = GetSafeField(fields, 7);
            record.VerticalSpeedKts = GetSafeField(fields, 8);
            record.TransponderCode = GetSafeField(fields, 9);
            record.BoxName = GetSafeField(fields, 10);
            record.AircraftPositionDateTimeUtc = GetSafeField(fields, 11);
            record.AircraftTypeCode = GetSafeField(fields, 12);
            record.GpsAltitude = GetSafeField(fields, 13);
            record.IsAircraftOnGround = GetSafeField(fields, 14);

            return record;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.TableStorage.Entities
{   
    public class AirportWatchLiveDataTableEntity: BaseTableEntity
    {
        public string BoxName { get; set; }
        public DateTime BoxTransmissionDateTimeUtc { get; set; }
        public string AtcFlightNumber { get; set; }
        public int? AltitudeInStandardPressure { get; set; }
        public int? GroundSpeedKts { get; set; }
        public double? TrackingDegree { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? VerticalSpeedKts { get; set; }
        public int? TransponderCode { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        public string AircraftTypeCode { get; set; }
        public int? GpsAltitude { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public string AircraftHexCode { get; set; }
    }
}

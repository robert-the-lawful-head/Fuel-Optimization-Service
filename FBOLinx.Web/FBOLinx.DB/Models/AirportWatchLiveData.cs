using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class AirportWatchLiveData
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public DateTime BoxTransmissionDateTimeUtc { get; set; }
        [Required]
        [StringLength(10)]
        public string AircraftHexCode { get; set; }
        [StringLength(20)]
        public string AtcFlightNumber { get; set; }
        public int? AltitudeInStandardPressure { get; set; }
        public int? GroundSpeedKts { get; set; }
        public double? TrackingDegree { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? VerticalSpeedKts { get; set; }
        public int? TransponderCode { get; set; }
        [StringLength(25)]
        public string BoxName { get; set; }
        public DateTime? AircraftPositionDateTimeUtc { get; set; }
        [StringLength(3)]
        public string AircraftTypeCode { get; set; }
        public int? GpsAltitude { get; set; }
        public bool? IsAircraftOnGround { get; set; }
    }
}
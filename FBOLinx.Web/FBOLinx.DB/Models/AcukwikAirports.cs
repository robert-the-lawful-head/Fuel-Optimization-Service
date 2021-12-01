using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Web;

namespace FBOLinx.DB.Models
{
    [Table("AcukwikAirports")]

    public partial class AcukwikAirports
    {
        [Key]
        [Column("Airport_ID")]
        public int AirportId { get; set; }
        [Column("ICAO")]
        [StringLength(255)]
        public string Icao { get; set; }
        [Column("IATA")]
        [StringLength(255)]
        public string Iata { get; set; }
        [Column("FAA")]
        [StringLength(255)]
        public string Faa { get; set; }
        [StringLength(255)]
        public string FullAirportName { get; set; }
        [StringLength(255)]
        public string AirportCity { get; set; }
        [Column("State/Subdivision")]
        [StringLength(255)]
        public string StateSubdivision { get; set; }
        [StringLength(255)]
        public string Country { get; set; }
        [StringLength(255)]
        public string AirportType { get; set; }
        [Column("Distance_From_City")]
        [StringLength(255)]
        public string DistanceFromCity { get; set; }
        [StringLength(255)]
        public string Latitude { get; set; }
        [StringLength(255)]
        public string Longitude { get; set; }
        public double? Elevation { get; set; }
        [StringLength(255)]
        public string Variation { get; set; }
        public double? IntlTimeZone { get; set; }
        [Column("DaylightSavingsYN")]
        [StringLength(255)]
        public string DaylightSavingsYn { get; set; }
        [StringLength(255)]
        public string FuelType { get; set; }
        [StringLength(255)]
        public string AirportOfEntry { get; set; }
        [StringLength(255)]
        public string Customs { get; set; }
        [StringLength(255)]
        public string HandlingMandatory { get; set; }
        [StringLength(255)]
        public string SlotsRequired { get; set; }
        [StringLength(255)]
        public string Open24Hours { get; set; }
        [StringLength(255)]
        public string ControlTowerHours { get; set; }
        [StringLength(255)]
        public string ApproachList { get; set; }
        [Column("PrimaryRunwayID")]
        [StringLength(255)]
        public string PrimaryRunwayId { get; set; }
        public double? RunwayLength { get; set; }
        public double? RunwayWidth { get; set; }
        [StringLength(255)]
        public string Lighting { get; set; }
        [StringLength(255)]
        public string AirportNameShort { get; set; }

        #region Relationships
        [InverseProperty("AcukwikAirport")]
        public ICollection<AcukwikFbohandlerDetail> AcukwikFbohandlerDetailCollection { get; set; }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("AFSAircraft")]
    public class AFSAircraft
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [Column("ICAO")]
        public string Icao { get; set; }

        public string AircraftTypeName { get; set; }

        public string AircraftTypeEngineName { get; set; }

        public int DegaAircraftID { get; set; }

        [ForeignKey("DegaAircraftID")]
        [InverseProperty("AFSAircraft")]
        public AirCrafts AirCrafts { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class AirportWatchAircraftTailNumber
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Required]
        [StringLength(10)]
        public string AircraftHexCode { get; set; }
        [StringLength(20)]
        public string AtcFlightNumber { get; set; }
    }
}
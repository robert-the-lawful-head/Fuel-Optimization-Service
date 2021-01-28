using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("FBOAirports")]
    public partial class Fboairports
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("IATA")]
        [StringLength(10)]
        public string Iata { get; set; }
        [Column("ICAO")]
        [StringLength(10)]
        public string Icao { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        public bool? DefaultTemplate { get; set; }

        [ForeignKey("Fboid")]
        [InverseProperty("fboAirport")]
        public Fbos Fbo { get; set; }
    }
}

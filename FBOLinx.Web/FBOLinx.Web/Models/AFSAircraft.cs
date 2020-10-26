using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models
{
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
    }
}

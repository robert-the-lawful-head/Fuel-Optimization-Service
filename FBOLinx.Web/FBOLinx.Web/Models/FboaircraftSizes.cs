using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    [Table("FBOAircraftSizes")]
    public partial class FboaircraftSizes
    {
        [Column("FBOID")]
        public int Fboid { get; set; }
        [Column("AircraftID")]
        public int AircraftId { get; set; }
        public short? Size { get; set; }
        [Column("OID")]
        public int Oid { get; set; }
    }
}

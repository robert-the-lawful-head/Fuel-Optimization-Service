using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class PriceTiers
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? MaxEntered { get; set; }

        [InverseProperty("PriceTier")]
        public CustomerMargins CustomerMargin { get; set; }
    }
}

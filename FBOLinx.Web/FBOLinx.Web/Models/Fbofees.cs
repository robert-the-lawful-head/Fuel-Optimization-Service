using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    [Table("FBOFees")]
    public partial class Fbofees
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        public int FeeType { get; set; }
        [StringLength(50)]
        public string DisplayText { get; set; }
        public double? FeeAmount { get; set; }
        public bool? Omit { get; set; }
        public bool? NoSalesTax { get; set; }
    }
}

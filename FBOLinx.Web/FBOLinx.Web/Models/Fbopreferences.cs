using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    [Table("FBOPreferences")]
    public partial class Fbopreferences
    {
        [Column("FBOID")]
        public int Fboid { get; set; }
        public bool? CostCalculator { get; set; }
        [Column("OmitJetARetail")]
        public bool? OmitJetARetail { get; set; }
        [Column("OmitJetACost")]
        public bool? OmitJetACost { get; set; }
        [Column("Omit100LLRetail")]
        public bool? Omit100LLRetail { get; set; }
        [Column("Omit100LLCost")]
        public bool? Omit100LLCost { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [InverseProperty("Preferences")]
        [ForeignKey("Fboid")]
        public Models.Fbos Fbo { get; set; }
    }
}

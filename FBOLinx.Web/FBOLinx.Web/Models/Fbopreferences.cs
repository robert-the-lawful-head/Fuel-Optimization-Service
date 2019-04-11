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
        public bool? OmitJetAretail { get; set; }
        [Column("OmitJetACost")]
        public bool? OmitJetAcost { get; set; }
        [Column("OID")]
        public int Oid { get; set; }
    }
}

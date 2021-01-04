using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class RampFeeSettings
    {
        [Column("FBOID")]
        public int Fboid { get; set; }
        public bool? HasRampFees { get; set; }
        [Column("OID")]
        public int Oid { get; set; }
    }
}

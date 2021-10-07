using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Web;
using Newtonsoft.Json;

namespace FBOLinx.DB.Models
{
    [Table("FBOFeeAndTaxOmitsByCustomer")]
    public class FboFeeAndTaxOmitsByCustomer
    {
        [Column("OID")]
        public int Oid { get; set; }
        [Column("FBOFeeAndTaxID")]
        [Required]
        public int FboFeeAndTaxId { get; set; }
        [Column("CustomerID")]
        [Required]
        public int CustomerId { get; set; }

        #region Relationships

        [JsonIgnore]
        [ForeignKey("FboFeeAndTaxId")]
        [InverseProperty("OmitsByCustomer")]
        public FboFeesAndTaxes FboFeeAndTax { get; set; }
        #endregion
    }
}

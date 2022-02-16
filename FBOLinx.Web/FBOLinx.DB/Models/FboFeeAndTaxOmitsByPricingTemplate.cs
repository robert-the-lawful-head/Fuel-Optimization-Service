using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FBOLinx.DB.Models
{
    [Table("FBOFeeAndTaxOmitsByPricingTemplate")]
    public class FboFeeAndTaxOmitsByPricingTemplate
    {
        [Column("OID")]
        public int Oid { get; set; }
        [Column("FBOFeeAndTaxID")]
        [Required]
        public int FboFeeAndTaxId { get; set; }
        [Column("PricingTemplateID")]
        [Required]
        public int PricingTemplateId { get; set; }

        #region Relationships
        [JsonIgnore]
        [ForeignKey("FboFeeAndTaxId")]
        [InverseProperty("OmitsByPricingTemplate")]
        public FboFeesAndTaxes FboFeeAndTax { get; set; }
        #endregion
    }
}

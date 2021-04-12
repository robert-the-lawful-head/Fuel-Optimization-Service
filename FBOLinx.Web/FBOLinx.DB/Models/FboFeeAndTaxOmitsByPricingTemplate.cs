using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Web;

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
        [ForeignKey("FboFeeAndTaxId")]
        [InverseProperty("OmitsByPricingTemplate")]
        public FboFeesAndTaxes FboFeeAndTax { get; set; }
        #endregion
    }
}

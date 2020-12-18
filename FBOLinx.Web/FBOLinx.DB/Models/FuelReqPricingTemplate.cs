using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class FuelReqPricingTemplate
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public int FuelReqId { get; set; }
        public int PricingTemplateId { get; set; }
        public string PricingTemplateName { get; set; }
        public string PricingTemplateRaw { get; set; }

        [ForeignKey("FuelReqId")]
        [InverseProperty("FuelReqPricingTemplate")]
        public FuelReq FuelReq { get; set; }
    }
}

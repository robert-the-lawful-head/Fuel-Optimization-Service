using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models
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

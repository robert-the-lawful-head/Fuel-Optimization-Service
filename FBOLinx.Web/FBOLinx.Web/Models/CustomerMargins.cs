using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class CustomerMargins
    {
        [Column("PriceTierID")]
        public int PriceTierId { get; set; }
        [Column("TemplateID")]
        public int TemplateId { get; set; }
        public double? Amount { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [ForeignKey("PriceTierId")]
        [InverseProperty("CustomerMargin")]
        public PriceTiers PriceTier { get; set; }

        [ForeignKey("TemplateId")]
        [InverseProperty("CustomerMargins")]
        public PricingTemplate PricingTemplate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    [Table("FBOPrices")]
    public partial class Fboprices
    {
        public enum FuelProductPriceTypes
        {
            [Description("JetA Cost")]
            FuelJetACost = 0,
            [Description("JetA Retail")]
            FuelJetARetail = 1,
            [Description("100LL Cost")]
            Fuel100LLCost = 2,
            [Description("100LL Retail")]
            Fuel100LRetail = 3
        }
        public enum ProductPriceType
        {
            [Description("JetA")]
            FuelJetA = 0,
            [Description("100LL")]
            Fuel100LL = 2
        }

        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("FBOID")]
        public int? Fboid { get; set; }
        [StringLength(50)]
        public string Product { get; set; }
        public double? Price { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EffectiveTo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Timestamp { get; set; }
        public double? SalesTax { get; set; }
        [StringLength(50)]
        public string Currency { get; set; }
    }
}

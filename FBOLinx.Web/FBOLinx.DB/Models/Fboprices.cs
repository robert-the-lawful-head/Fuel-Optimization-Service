using FBOLinx.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("FBOPrices")]
    public partial class Fboprices
    {
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
        public bool? Expired { get; set; }
        [NotMapped]
        public int? Id { get; set; }

        [NotMapped]
        public string GenericProduct
        {
            get
            {
                if (Product.Contains("Cost"))
                    return "Cost";
                if (Product.Contains("Retail"))
                    return "Retail";
                return "";
            }
        }

        [ForeignKey("Fboid")]
        [InverseProperty("Fboprices")]
        public Fbos Fbo { get; set; }

        public FboPricesSource Source { get; set; } = FboPricesSource.FboLinx;
    }
}

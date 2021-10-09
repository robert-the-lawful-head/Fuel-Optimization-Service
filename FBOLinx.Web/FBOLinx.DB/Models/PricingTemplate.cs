using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class PricingTemplate
    {
        public enum MarginTypes : short
        {
            [Description("Cost +")]
            CostPlus = 0,
            [Description("Retail -")]
            RetailMinus = 1,
            [Description("Flat Fee")]
            FlatFee = 2,
            [Description("Inactive")]
            Inactive = 3
        }

        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        [Column("CustomerID")]
        public int? CustomerId { get; set; }
        public bool? Default { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public short? Type { get; set; }
       
       //  public int discountType { get; set; }
        public MarginTypes? MarginType { get; set; }
        public int? EmailContentId { get; set; }
        [NotMapped]
        public List<string> TailNumbers { get; set; }

        public string MarginTypeProduct
        {
            get
            {
                if (!MarginType.HasValue)
                    return "";
                if (MarginType.Value == MarginTypes.CostPlus)
                    return "JetA Cost";
                if (MarginType.Value == MarginTypes.RetailMinus)
                    return "JetA Retail";
                return "";
            }
        }

        [InverseProperty("PricingTemplate")]
        public ICollection<CustomerMargins> CustomerMargins { get; set; }

        [InverseProperty("PricingTemplate")]
        public ICollection<AircraftPrices> AircraftPrices { get; set; }

        [InverseProperty("PricingTemplates")]
        [ForeignKey("Fboid")]
        public Models.Fbos Fbo { get; set; }
        
        [InverseProperty("PricingTemplate")]
        public ICollection<CustomCustomerTypes> CustomCustomerTypes { get; set; }
    }
}

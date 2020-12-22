using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class CustomCustomerTypes
    {
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        public int CustomerType { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [InverseProperty("CustomCustomerType")]
        [ForeignKey("CustomerId")]
        public Customers Customer { get; set; }
        
        [InverseProperty("CustomCustomerTypes")]
        [ForeignKey("CustomerType")]
        public PricingTemplate PricingTemplate { get; set; }
    }
}

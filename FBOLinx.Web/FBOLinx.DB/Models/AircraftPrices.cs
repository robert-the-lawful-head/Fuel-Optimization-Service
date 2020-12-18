using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class AircraftPrices
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("CustomerAircraftID")]
        public int CustomerAircraftId { get; set; }
        [Column("PriceTemplateID")]
        public int? PriceTemplateId { get; set; }
        public bool? CustomTemplate { get; set; }

        [InverseProperty("AircraftPrices")]
        [ForeignKey("CustomerAircraftId")]
        public CustomerAircrafts CustomerAircraft { get; set; }

        [InverseProperty("AircraftPrices")]
        [ForeignKey("PriceTemplateId")]
        public PricingTemplate PricingTemplate { get; set; }

    }
}

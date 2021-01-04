using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class CompanyPricingLog
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [Column("CompanyID")]
        public int CompanyId { get; set; }

        [Column("ICAO")]
        public string ICAO { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("CompanyPricingLogs")]
        public Customers Customer { get; set; }
    }
}

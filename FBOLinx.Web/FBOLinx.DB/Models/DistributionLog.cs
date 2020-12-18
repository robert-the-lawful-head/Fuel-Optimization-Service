using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class DistributionLog
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public DateTime DateSent { get; set; }
        [Column("FBOID")]
        public int? Fboid { get; set; }
        [Column("GroupID")]
        public int? GroupId { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        [Column("PricingTemplateID")]
        public int? PricingTemplateId { get; set; }
        [Column("CustomerID")]
        public int? CustomerId { get; set; }
        public int CustomerCompanyType { get; set; }
    }
}

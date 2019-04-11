using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class DistributionQueue
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("DistributionLogID")]
        public int DistributionLogId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        [Column("GroupID")]
        public int GroupId { get; set; }
        public DateTime? DateSent { get; set; }
        public bool? IsCompleted { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class DistributionErrors
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("DistributionLogID")]
        public int? DistributionLogId { get; set; }
        [Column("DistributionQueueID")]
        public int? DistributionQueueId { get; set; }
        public string Error { get; set; }
        public DateTime? ErrorDateTime { get; set; }
    }
}

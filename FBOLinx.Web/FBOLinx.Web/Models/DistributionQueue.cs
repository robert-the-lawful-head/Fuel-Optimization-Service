using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class DistributionQueue
    {
        public int Oid { get; set; }
        public int DistributionLogId { get; set; }
        public int CustomerId { get; set; }
        public int Fboid { get; set; }
        public int GroupId { get; set; }
        public DateTime? DateSent { get; set; }
        public bool? IsCompleted { get; set; }
    }
}

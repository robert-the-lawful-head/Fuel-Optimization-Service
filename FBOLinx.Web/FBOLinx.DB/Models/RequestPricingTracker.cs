using System;

namespace FBOLinx.DB.Models
{
    public partial class RequestPricingTracker
    {
        public int Oid { get; set; }
        public int? CustomerId { get; set; }
        public int? GroupId { get; set; }
        public int? Fboid { get; set; }
        public DateTime? Date { get; set; }
    }
}

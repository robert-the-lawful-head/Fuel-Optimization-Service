using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class RequestPricingTrackerDto
    {
        public int Oid { get; set; }
        public int? CustomerId { get; set; }
        public int? GroupId { get; set; }
        public int? Fboid { get; set; }
        public DateTime? Date { get; set; }
    }
}
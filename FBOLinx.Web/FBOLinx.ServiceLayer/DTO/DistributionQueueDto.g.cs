using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class DistributionQueueDto
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
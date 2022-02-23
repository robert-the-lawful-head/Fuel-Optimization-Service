using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class DistributionErrorsDto
    {
        public int Oid { get; set; }
        public int? DistributionLogId { get; set; }
        public int? DistributionQueueId { get; set; }
        public string Error { get; set; }
        public DateTime? ErrorDateTime { get; set; }
    }
}
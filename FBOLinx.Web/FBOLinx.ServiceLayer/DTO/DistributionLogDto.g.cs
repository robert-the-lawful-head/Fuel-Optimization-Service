using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class DistributionLogDto
    {
        public int Oid { get; set; }
        public DateTime DateSent { get; set; }
        public int? Fboid { get; set; }
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
        public int? PricingTemplateId { get; set; }
        public int? CustomerId { get; set; }
        public int CustomerCompanyType { get; set; }
    }
}
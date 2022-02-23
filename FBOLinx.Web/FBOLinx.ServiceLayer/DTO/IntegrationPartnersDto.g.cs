using System;
using FBOLinx.Core.Enums;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class IntegrationPartnersDto
    {
        public int Oid { get; set; }
        public string PartnerName { get; set; }
        public IntegrationPartnerTypes PartnerType { get; set; }
        public short Affiliation { get; set; }
        public Guid Apikey { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? PartnerId { get; set; }
        public TrustLevels? TrustLevel { get; set; }
    }
}
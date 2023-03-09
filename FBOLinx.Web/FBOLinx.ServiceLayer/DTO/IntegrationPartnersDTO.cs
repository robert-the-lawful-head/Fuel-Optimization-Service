using System;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class IntegrationPartnersDTO : FBOLinxBaseEntityModel<int>
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
using System;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class IntegrationStatusDTO : FBOLinxBaseEntityModel<int>
    {
        public int IntegrationPartnerId { get; set; }
        public int FboId { get; set; }
        public bool IsActive { get; set; }
    }
}
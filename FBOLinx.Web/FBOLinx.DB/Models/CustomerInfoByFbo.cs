using System;

namespace FBOLinx.DB.Models
{
    public partial class CustomerInfoByFbo : FBOLinxBaseEntityModel<int>
    {
        public int CustomerInfoByGroupId { get; set; }
        public int FboId { get; set; }
        public string? CustomFboEmail { get; set; }
    }
}

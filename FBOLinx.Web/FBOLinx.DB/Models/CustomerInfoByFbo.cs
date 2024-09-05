using System;

namespace FBOLinx.DB.Models
{
    public partial class CustomerInfoByFbo
    {
        public int Oid { get; set; }
        public int CustomerInfoByGroupId { get; set; }
        public int FboId { get; set; }
        public string? CustomFboEmail { get; set; }
    }
}

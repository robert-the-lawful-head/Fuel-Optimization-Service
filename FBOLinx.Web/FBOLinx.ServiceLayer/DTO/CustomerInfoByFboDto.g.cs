using System;

namespace FBOLinx.DB.Models
{
    public class CustomerInfoByFboDto
    {
        public int Oid { get; set; }
        public int CustomerInfoByGroupId { get; set; }
        public int FboId { get; set; }
        public string? CustomFboEmail { get; set; }
    }
}

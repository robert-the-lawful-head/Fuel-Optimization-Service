using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class RefreshTokens
    {
        public int Oid { get; set; }
        public string Token { get; set; }
        public DateTime? Expired { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

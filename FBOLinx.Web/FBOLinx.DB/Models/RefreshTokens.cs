using System;

namespace FBOLinx.DB.Models
{
    public partial class RefreshTokens
    {
        public int Oid { get; set; }
        public int AccessTokenId { get; set; }
        public string Token { get; set; }
        public DateTime? Expired { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

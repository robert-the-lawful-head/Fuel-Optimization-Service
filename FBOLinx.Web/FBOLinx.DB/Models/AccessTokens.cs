using System;

namespace FBOLinx.DB.Models
{
    public partial class AccessTokens
    {
        public int Oid { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expired { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
    }
}

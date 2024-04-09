using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class RefreshTokensDto
    {
        public int Oid { get; set; }
        public int AccessTokenId { get; set; }
        public string Token { get; set; }
        public DateTime? Expired { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
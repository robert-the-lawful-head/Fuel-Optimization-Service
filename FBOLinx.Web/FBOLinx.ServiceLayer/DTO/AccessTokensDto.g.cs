using System;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AccessTokensDto
    {
        public int Oid { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expired { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO User { get; set; }
    }
}
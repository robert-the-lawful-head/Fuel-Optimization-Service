using System;
using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class UserDto
    {
        public int Oid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? CopyAlerts { get; set; }
        public long? LoginCount { get; set; }
        public int? AddOnMarginTries { get; set; }
        public bool? GoOverTutorial { get; set; }
        public bool? Active { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiration { get; set; }
        public UserRoles Role { get; set; }
        public int FboId { get; set; }
        public bool? CopyOrders { get; set; }
        public FbosDto Fbo { get; set; }
        public int? GroupId { get; set; }
        public GroupDto Group { get; set; }
        public ICollection<AccessTokensDto> AccessTokens { get; set; }
        public string RoleDescription { get; set; }
    }
}
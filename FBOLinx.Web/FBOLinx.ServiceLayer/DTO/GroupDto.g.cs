using System.Collections.Generic;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class GroupDto
    {
        public string GroupName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Isfbonetwork { get; set; }
        public string Domain { get; set; }
        public string LoggedInHomePage { get; set; }
        public bool Active { get; set; }
        public bool? IsLegacyAccount { get; set; }
        public ICollection<FbosDto> Fbos { get; set; }
        public ICollection<UserDto> Users { get; set; }
        public ICollection<ContactInfoByGroupDto> ContactInfoByGroup { get; set; }
        public int Oid { get; set; }
    }
}
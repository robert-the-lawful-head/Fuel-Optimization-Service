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
        public int Oid { get; set; }
    }
}
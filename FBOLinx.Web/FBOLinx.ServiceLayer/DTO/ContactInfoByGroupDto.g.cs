using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class ContactInfoByGroupDto
    {
        public int GroupId { get; set; }
        public int ContactId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Fax { get; set; }
        public string Title { get; set; }
        public bool? Primary { get; set; }
        public bool? CopyAlerts { get; set; }
        public string Extension { get; set; }
        public int Oid { get; set; }
        public ContactsDto Contact { get; set; }
        public GroupDto Group { get; set; }
    }
}
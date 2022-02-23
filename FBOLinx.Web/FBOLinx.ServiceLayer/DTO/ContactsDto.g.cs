using System.Collections.Generic;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class ContactsDto
    {
        public int Oid { get; set; }
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
        public bool? CopyOrders { get; set; }
        public FbocontactsDto FboContact { get; set; }
        public ICollection<CustomerContactsDto> CustomerContacts { get; set; }
        public ICollection<ContactInfoByGroupDto> ContactInfoByGroups { get; set; }
    }
}
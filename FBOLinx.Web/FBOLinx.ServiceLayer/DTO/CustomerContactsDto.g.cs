using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomerContactsDto
    {
        public int CustomerId { get; set; }
        public int ContactId { get; set; }
        public int Oid { get; set; }
        public ContactsDto Contact { get; set; }
        public CustomersDto Customer { get; set; }
    }
}
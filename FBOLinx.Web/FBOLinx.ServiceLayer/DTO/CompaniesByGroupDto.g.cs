using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CompaniesByGroupDto
    {
        public int GroupId { get; set; }
        public int CustomerId { get; set; }
        public int Oid { get; set; }
        public CustomersDto Customer { get; set; }
    }
}
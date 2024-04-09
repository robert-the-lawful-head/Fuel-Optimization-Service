using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomersViewedByFboDto
    {
        public int CustomerId { get; set; }
        public int Fboid { get; set; }
        public DateTime? ViewDate { get; set; }
        public int Oid { get; set; }
    }
}
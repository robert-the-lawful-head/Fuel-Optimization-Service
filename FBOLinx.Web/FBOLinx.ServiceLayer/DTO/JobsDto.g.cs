using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class JobsDto
    {
        public int Oid { get; set; }
        public int? GroupId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
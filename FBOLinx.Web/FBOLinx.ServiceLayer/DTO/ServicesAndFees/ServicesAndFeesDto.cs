using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
using System;

namespace FBOLinx.ServiceLayer.DTO.ServicesAndFees
{
    public class ServicesAndFeesDto
    {
        public int Oid { get; set; }
        public int? HandlerId { get; set; }
        public int? ServiceOfferedId { get; set; }
        public string Service { get; set; }
        public int? ServiceTypeId { get; set; }
        public bool IsActive { get;set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

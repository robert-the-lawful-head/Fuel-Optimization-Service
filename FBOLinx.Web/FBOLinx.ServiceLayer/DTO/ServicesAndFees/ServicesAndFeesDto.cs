using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;

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
    }
}

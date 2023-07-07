namespace FBOLinx.ServiceLayer.DTO.ServicesAndFees
{
    public class ServicesAndFeesDto
    {
        public int Oid { get; set; }
        public int? HandlerId { get; set; }
        public int? ServiceOfferedId { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
        public bool isActive { get;set; }
    }
}

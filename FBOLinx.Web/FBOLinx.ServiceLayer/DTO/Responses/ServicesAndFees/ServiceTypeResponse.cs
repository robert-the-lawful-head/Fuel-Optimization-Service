using FBOLinx.DB.Models.ServicesAndFees;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees
{
    public class ServiceTypeResponse : FboCustomServiceType
    {
        public bool IsCustom { get; set; }
        public string createdByUser { get; set; }
    }
}

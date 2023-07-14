using System.Collections.Generic;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees
{
    public class FbosServicesAndFeesResponse
    {
        public ServiceTypeResponse ServiceType { get; set; }
        public List<ServicesAndFeesResponse> ServicesAndFees { get;set; }
    }
}

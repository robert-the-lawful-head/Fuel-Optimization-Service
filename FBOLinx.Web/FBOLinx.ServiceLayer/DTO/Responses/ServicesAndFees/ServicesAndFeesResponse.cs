using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees
{
    public class ServicesAndFeesResponse : ServicesAndFeesDto
    {
        public bool IsCustom { get; set; }
        public string CreatedByUser { get; set; }
    }
}

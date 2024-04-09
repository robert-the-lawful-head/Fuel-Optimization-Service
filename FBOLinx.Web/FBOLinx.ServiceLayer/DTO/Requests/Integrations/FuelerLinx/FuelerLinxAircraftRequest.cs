using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.DTO.Requests.Integrations.FuelerLinx
{
    public class FuelerLinxAircraftRequest
    {
        [Required]
        public int FuelerlinxCompanyID { get; set; }

        public string TailNumbers { get; set; }
    }
}

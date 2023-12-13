using FBOLinx.DB.Models;

namespace FBOLinx.Web.DTO
{
    public class CustomerWithAicraftDto
    {
        public CustomerInfoByGroup CustomerInfoByGroup { get; set; }
        public CustomerAircrafts CustomerAircraft { get; set; }
    }
}

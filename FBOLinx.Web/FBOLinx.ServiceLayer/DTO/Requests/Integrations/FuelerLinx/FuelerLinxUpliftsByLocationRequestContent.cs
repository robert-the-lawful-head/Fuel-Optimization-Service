using System;

namespace FBOLinx.ServiceLayer.DTO.Requests.Integrations.FuelerLinx
{
    public class FuelerLinxUpliftsByLocationRequestContent
    {
        public string UserServiceKey { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string ICAO { get; set; }
        public int FboId { get; set; }
    }
}

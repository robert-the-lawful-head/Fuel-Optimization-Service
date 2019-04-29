using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
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

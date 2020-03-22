using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class RampFeesImportVM
    {
        public string icao { get; set; }
        public string fbo { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string rampfee { get; set; }
        public string waivedat { get; set; }
        public string landing { get; set; }
        public string overnight { get; set; }
    }
}

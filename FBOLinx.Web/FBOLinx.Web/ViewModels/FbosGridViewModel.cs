using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class FbosGridViewModel
    {
        public int Oid { get; set; }
        public string Fbo { get; set; }
        public bool? Active { get; set; }
        public string Icao { get; set; }
        public string Iata { get; set; }
    }
}

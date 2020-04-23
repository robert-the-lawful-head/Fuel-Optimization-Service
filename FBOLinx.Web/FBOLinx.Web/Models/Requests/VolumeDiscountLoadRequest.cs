using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class VolumeDiscountLoadRequest
    {
        public int FuelerlinxCompanyID { get; set; }
        public string ICAO { get; set; }
        public string TailNumber { get; set; }
    }
}

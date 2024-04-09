using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelReqUpdateDateTimesRequest
    {
        public DateTime Eta { get; set; }
        public DateTime Etd { get; set; }
    }
}

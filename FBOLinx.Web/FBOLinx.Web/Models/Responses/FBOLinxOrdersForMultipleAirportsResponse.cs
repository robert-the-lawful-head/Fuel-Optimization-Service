using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fuelerlinx.SDK;

namespace FBOLinx.Web.Models.Responses
{
    public class FBOLinxOrdersForMultipleAirportsResponse
    {
        public string Icao { get; set; }
        public int DirectOrders { get; set; }
        public List<FbolinxContractFuelVendorTransactionsCountByAirport> VendorOrders { get; set; }
    }
}

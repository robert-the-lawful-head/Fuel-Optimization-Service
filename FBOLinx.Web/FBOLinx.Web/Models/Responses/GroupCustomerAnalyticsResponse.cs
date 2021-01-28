using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class GroupCustomerAnalyticsResponse
    {
        public string Company { get; set; }

        public List<GroupedFboPrices> GroupCustomerFbos { get; set; }
    }

    public class GroupedFboPrices
    {
        public string Icao { get; set; }
        public List<Prices> Prices { get; set; }
    }

    public class Prices
    {
        public string VolumeTier { get; set; }
        public double? IntComm { get; set; }
        public double? IntPrivate { get; set; }
        public double? DomComm { get; set; }
        public double? DomPrivate { get; set; }
    }
}

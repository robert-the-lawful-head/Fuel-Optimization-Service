using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class GroupCustomerAnalyticsResponse
    {
        public string Company { get; set; }

        public List<GroupCustomerFbos> GroupCustomerFbos { get; set; }
    }

    public class GroupCustomerFbos
    {
        public string Icao { get; set; }
        public double? AllIn { get; set; }
        public string Notes { get; set; }
        public double? Retail { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}

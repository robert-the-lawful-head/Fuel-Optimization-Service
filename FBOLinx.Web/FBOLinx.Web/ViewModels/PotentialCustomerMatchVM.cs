using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class PotentialCustomerMatchVM
    {
        public int CurrentCustomerId { get; set; }
        public string CurrentCustomerName { get; set; }
        public int MatchCustomerId { get; set; }
        public int MatchCustomerOid { get; set; }
        public string MatchCustomerName { get; set; }
        public bool? IsAircraftMatch { get; set; }
        public List<string> AircraftTails { get; set; }
        public bool? IsNameMatch { get; set; }
        public string MatchNameCustomer { get; set; }
        public int MatchNameCustomerId { get; set; }
        public int MatchNameCustomerOid { get; set; }

        public bool? IsContactMatch { get; set; }
        public string MatchContactCustomer { get; set; }
        public int MatchContactCustomerId { get; set; }
        public int MatchContactCustomerOid { get; set; }
    }
}

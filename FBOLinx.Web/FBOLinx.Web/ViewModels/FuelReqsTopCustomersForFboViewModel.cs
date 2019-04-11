using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class FuelReqsTopCustomersForFboViewModel
    {
        public string CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public int TotalOrders { get; set; }
    }
}

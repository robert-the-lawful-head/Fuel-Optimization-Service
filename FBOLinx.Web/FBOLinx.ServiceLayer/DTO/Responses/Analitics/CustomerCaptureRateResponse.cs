using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.Analitics
{
    public class CustomerCaptureRateResponse
    {
        public int Oid { get; set; }
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public int TotalOrders { get; set; }
        public int AirportOrders { get; set; }
        public int? PercentCustomerBusiness { get; set; }
    }
}

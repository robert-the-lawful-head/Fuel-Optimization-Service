using System;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelReqsTopCustomersByFboRequest
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int NumberOfResults { get; set; } = 10;
    }
}
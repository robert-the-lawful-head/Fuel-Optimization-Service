using System;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelReqsTotalOrdersByMonthForFboRequest
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}

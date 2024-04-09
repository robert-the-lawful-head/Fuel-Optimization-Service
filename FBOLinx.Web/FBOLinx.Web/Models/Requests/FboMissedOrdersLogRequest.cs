using System;

namespace FBOLinx.Web.Models.Requests
{
    public class FboMissedOrdersLogRequest
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}

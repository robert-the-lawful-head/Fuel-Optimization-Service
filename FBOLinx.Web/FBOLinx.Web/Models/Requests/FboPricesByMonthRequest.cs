using System;

namespace FBOLinx.Web.Models.Requests
{
    public class FboPricesByMonthRequest
    {
        public string Product { get; set; } = "JetA Retail";
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}

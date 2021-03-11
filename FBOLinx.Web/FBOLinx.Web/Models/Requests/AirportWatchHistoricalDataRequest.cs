using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.Web.Models.Requests
{
    public class AirportWatchHistoricalDataRequest
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}

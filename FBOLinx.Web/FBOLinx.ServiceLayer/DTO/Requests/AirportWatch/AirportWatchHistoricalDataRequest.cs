using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.DTO.Requests.AirportWatch
{
    public class AirportWatchHistoricalDataRequest
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public List<string> FlightOrTailNumbers { get; set; }
    }
}

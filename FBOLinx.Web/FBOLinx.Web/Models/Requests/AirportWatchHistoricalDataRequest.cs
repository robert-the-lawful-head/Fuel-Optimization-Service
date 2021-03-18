using System;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.Web.Models.Requests
{
    public class AirportWatchHistoricalDataRequest
    {
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.SWIM
{
    public class FlightLegDTO
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureICAO { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalICAO { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime ATD { get; set; }
        public DateTime ETA { get; set; }
        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}

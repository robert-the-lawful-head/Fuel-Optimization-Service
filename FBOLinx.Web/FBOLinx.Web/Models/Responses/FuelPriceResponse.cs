using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class FuelPriceResponse
    {
        public string Icao { get; set; }
        public string Iata { get; set; }
        public int FboId { get; set; }
        public string Fbo { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public string Product { get; set; }
        public double MinVolume { get; set; }
        public string Notes { get;set;}
        public bool Default { get; set; }
        public double Price { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string TailNumberList { get; set; }
        public int CustomerId { get; set; }
        public int AircraftId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport
{
    public class AirportPosition
    {
        public string Icao { get; set; }
        public string Iata { get; set; }
        public string Faa { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft
{
    public class AirportPosition
    {
        public string Icao { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

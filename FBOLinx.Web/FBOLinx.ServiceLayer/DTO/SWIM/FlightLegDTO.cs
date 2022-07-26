using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.SWIM
{
    public class FlightLegDTO
    {
        public int Id { get; set; }
        public string TailNumber { get; set; }
        public string FlightDepartment { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double? FuelCapacityGal { get; set; }
        public string Origin { get; set; }
        public string City { get; set; }
        public string DepartureICAO { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalICAO { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime ATDLocal { get; set; }
        public DateTime ATDZulu { get; set; }
        public DateTime ETALocal { get; set; }
        public DateTime ETAZulu { get; set; }
        public TimeSpan ETE { get; set; }
        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public string ITPMarginTemplate { get; set; }
        public FlightLegStatus Status { get; set; }
        public string Phone { get; set; }
    }
}

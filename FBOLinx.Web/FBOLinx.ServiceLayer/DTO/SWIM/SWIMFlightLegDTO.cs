using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.DTO.SWIM
{
    public class SWIMFlightLegDTO : BaseEntityModelDTO<SWIMFlightLeg>, IEntityModelDTO<SWIMFlightLeg, int>
    {
        public int Oid { get; set; }
        public string AircraftIdentification { get; set; }
        public string DepartureICAO { get; set; }
        public string ArrivalICAO { get; set; }
        public DateTime ATD { get; set; }
        public DateTime? ETA { get; set; }
        
        public string DepartureCity { get; set; }
        
        public string ArrivalCity { get; set; }
        
        public DateTime? ATDLocal { get; set; }
        
        public DateTime? ETALocal { get; set; }

        public bool IsPlaceholder { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public FlightLegStatus? Status { get; set; }
        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string FlightDepartment { get; set; }
        public string Make { get; set; }
        public string FAAMake { get; set; }
        public string Model { get; set; }
        public string FAAModel { get; set; }
        public double? FuelCapacityGal { get; set; }
        public string Phone { get; set; }
        public string ICAOAircraftCode { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string FAARegisteredOwner { get; set; }

        public virtual List<SWIMFlightLegDataDTO> SWIMFlightLegDataMessages { get; set; }

        public int GetDepartureLegSortPriority()
        {
            if (Status == FlightLegStatus.TaxiingDestination)
                return 1;
            if (Status == FlightLegStatus.Departing)
                return 2;
            if (Status == FlightLegStatus.EnRoute)
                return 3;
            return 4;
        }
    }
}

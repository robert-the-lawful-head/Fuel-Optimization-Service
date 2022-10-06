using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch
{
    public class FlightWatchLegAdditionalDetailsModel
    {
        public AirportPosition ArrivalAirportPositionInfo { get; set; }
        public AirportPosition DepartureAirportPositionInfo { get; set; }
        public SWIMFlightLegDTO SWIMFlightLeg { get; set; }
    }
}

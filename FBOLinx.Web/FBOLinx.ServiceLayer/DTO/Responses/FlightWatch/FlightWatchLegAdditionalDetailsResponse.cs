using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;

namespace FBOLinx.ServiceLayer.DTO.Responses.FlightWatch
{
    public class FlightWatchLegAdditionalDetailsResponse : EntityResponseMessage<FlightWatchLegAdditionalDetailsModel>
    {
        public FlightWatchLegAdditionalDetailsResponse(bool success, string message) : base(success, message)
        {
        }

        public FlightWatchLegAdditionalDetailsResponse(FlightWatchLegAdditionalDetailsModel result) : base(result)
        {
        }
    }
}

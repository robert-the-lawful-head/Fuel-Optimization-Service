using System.Collections.Generic;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.Web.Models.Responses.SWIM
{
    public class FlightLegsResponse : EntityResponseMessage<IEnumerable<FlightLegDTO>>
    {
        public FlightLegsResponse(bool success = false, string message = null) : base(success, message)
        {
        }

        public FlightLegsResponse(IEnumerable<FlightLegDTO> result) : base(result)
        {
        }
    }
}
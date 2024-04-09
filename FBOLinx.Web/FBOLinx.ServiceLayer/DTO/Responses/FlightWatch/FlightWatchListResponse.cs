using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;

namespace FBOLinx.ServiceLayer.DTO.Responses.FlightWatch
{
    public class FlightWatchListResponse : EntityResponseMessage<List<FlightWatchModel>>
    {
        public FlightWatchListResponse(bool success, string message) : base(success, message)
        {
        }

        public FlightWatchListResponse(List<FlightWatchModel> result) : base(result)
        {
        }
    }
}

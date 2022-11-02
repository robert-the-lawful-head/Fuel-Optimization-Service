using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;

namespace FBOLinx.ServiceLayer.DTO.Responses.AirportWatch
{
    public class AirportWatchLiveDataResponse : EntityResponseMessage<List<AirportWatchLiveDataDto>>
    {
        public AirportWatchLiveDataResponse(bool success, string message) : base(success, message)
        {
        }

        public AirportWatchLiveDataResponse(List<AirportWatchLiveDataDto> result) : base(result)
        {
        }
    }
}
